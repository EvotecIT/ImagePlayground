using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using ChartForgeX.Typography;
using ImagePlayground.PowerShell;
using Xunit;

namespace ImagePlayground.Tests;

public class TableColumnSelectorTests {
    [Fact]
    public void NamedColumnsTrimNamesAndReadObjectProperties() {
        var selector = TableColumnSelector.Create(" Name ");
        var row = PSObject.AsPSObject(new { Name = "API" });

        Assert.Equal("Name", selector.Header);
        Assert.Equal("Name", selector.PropertyName);
        Assert.Equal("API", selector.Read(row));
        Assert.True(selector.CanRead(row));
        Assert.Null(selector.Read(PSObject.AsPSObject(new { Other = "value" })));
        Assert.False(selector.CanRead(PSObject.AsPSObject(new { Other = "value" })));
    }

    [Fact]
    public void CalculatedColumnsApplyAliasesAndDisplayOptions() {
        var selector = TableColumnSelector.Create(new Hashtable {
            ["Label"] = "Item count",
            ["E"] = ScriptBlock.Create("$_.Count"),
            ["F"] = "N0",
            ["Align"] = "centre",
            ["W"] = 18
        });

        Assert.Equal("Item count", selector.Header);
        Assert.Null(selector.PropertyName);
        Assert.NotNull(selector.Expression);
        Assert.Equal("N0", selector.Format);
        Assert.Equal(TextAlignment.Center, selector.Alignment);
        Assert.Equal(18d, selector.Width);
        Assert.True(selector.CanRead(PSObject.AsPSObject(new { Count = 3 })));
        Assert.Equal(3, InRunspace(() => selector.Read(PSObject.AsPSObject(new { Count = 3 }))));
    }

    [Fact]
    public void CalculatedColumnsJoinMultipleExpressionResults() {
        var selector = TableColumnSelector.Create(new Hashtable {
            ["Name"] = "Pair",
            ["Expression"] = ScriptBlock.Create("'first'; 'second'")
        });

        Assert.Equal("first, second", InRunspace(() => selector.Read(PSObject.AsPSObject(new { Value = 1 }))));
    }

    [Fact]
    public void NamedCalculatedColumnsReadDictionaryKeysWithoutCaseSensitivity() {
        var selector = TableColumnSelector.Create(new Hashtable {
            ["N"] = "Requests",
            ["Expression"] = "requests",
            ["Alignment"] = "right",
            ["Width"] = 12d
        });
        IDictionary row = new Hashtable { ["REQUESTS"] = 42 };

        Assert.Equal("Requests", selector.Header);
        Assert.Equal("requests", selector.PropertyName);
        Assert.Equal(42, selector.Read(PSObject.AsPSObject(row)));
        Assert.True(selector.CanRead(PSObject.AsPSObject(row)));
        Assert.False(selector.CanRead(PSObject.AsPSObject(new Hashtable { ["Other"] = 1 })));
        Assert.Equal(TextAlignment.Right, selector.Alignment);
        Assert.Equal(12d, selector.Width);
    }

    [Fact]
    public void FromRowUsesDictionaryKeysAndObjectProperties() {
        var dictionaryColumns = TableColumnSelector.FromRow(PSObject.AsPSObject(new Hashtable {
            ["Name"] = "API",
            ["State"] = "Healthy"
        }));
        var objectColumns = TableColumnSelector.FromRow(PSObject.AsPSObject(new { Name = "API", State = "Healthy" }));

        Assert.Equal(new[] { "Name", "State" }, dictionaryColumns.Select(column => column.Header).OrderBy(name => name));
        Assert.Equal(new[] { "Name", "State" }, objectColumns.Select(column => column.Header).OrderBy(name => name));
    }

    [Theory]
    [InlineData(0d)]
    [InlineData(-1d)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    public void CalculatedColumnsRejectInvalidWidths(double width) {
        var properties = new Hashtable {
            ["Name"] = "Value",
            ["Expression"] = "Value",
            ["Width"] = width
        };

        Assert.Throws<PSArgumentException>(() => TableColumnSelector.Create(properties));
    }

    [Fact]
    public void CalculatedColumnsRejectMissingDefinitionsUnknownKeysAndInvalidAlignment() {
        Assert.Throws<PSArgumentException>(() => TableColumnSelector.Create("  "));
        Assert.Throws<PSArgumentException>(() => TableColumnSelector.Create(new Hashtable()));
        Assert.Throws<PSArgumentException>(() => TableColumnSelector.Create(new Hashtable {
            ["Name"] = "Value", ["Expression"] = "Value", ["Colour"] = "Red"
        }));
        Assert.Throws<PSArgumentException>(() => TableColumnSelector.Create(new Hashtable {
            ["Name"] = "Value", ["Expression"] = "Value", ["Alignment"] = "Justify"
        }));
    }

    private static T InRunspace<T>(Func<T> action) {
        var previous = Runspace.DefaultRunspace;
        using var runspace = RunspaceFactory.CreateRunspace();
        runspace.Open();
        Runspace.DefaultRunspace = runspace;
        try {
            return action();
        } finally {
            Runspace.DefaultRunspace = previous;
        }
    }
}

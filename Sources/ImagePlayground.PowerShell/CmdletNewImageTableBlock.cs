using ChartForgeX.VisualBlocks;
using ImagePlayground;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a visual table block.</summary>
/// <para>Rows can be arrays, hashtables, or PowerShell objects whose property names match the requested columns.</para>
/// <example>
///   <summary>Create a service status table</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageTableBlock -Title 'Services' -Column Name,Status -Row @{Name='API';Status='Healthy'},@{Name='Database';Status='Warning'} -Dense</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageTableBlock")]
[OutputType(typeof(ChartTable))]
public sealed class NewImageTableBlockCmdlet : PSCmdlet {
    /// <summary>Column headers.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public string[] Column { get; set; } = Array.Empty<string>();

    /// <summary>Rows represented as arrays, dictionaries, or PowerShell objects.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public object[] Row { get; set; } = Array.Empty<object>();

    /// <summary>Optional block title.</summary>
    [Parameter]
    public string Title { get; set; } = string.Empty;

    /// <summary>Optional block subtitle.</summary>
    [Parameter]
    public string Subtitle { get; set; } = string.Empty;

    /// <summary>Column that contains semantic status values.</summary>
    [Parameter]
    public string StatusColumn { get; set; } = string.Empty;

    /// <summary>Use compact row spacing.</summary>
    [Parameter]
    public SwitchParameter Dense { get; set; }

    /// <summary>Hide the table header.</summary>
    [Parameter]
    public SwitchParameter NoHeader { get; set; }

    /// <summary>Disable alternating row surfaces.</summary>
    [Parameter]
    public SwitchParameter NoRowStriping { get; set; }

    /// <summary>ChartForgeX theme.</summary>
    [Parameter]
    public ChartTheme Theme { get; set; } = ChartTheme.Default;

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var table = ChartTable.Create()
            .WithColumns(Column)
            .WithDenseMode(Dense.IsPresent)
            .WithHeader(!NoHeader.IsPresent)
            .WithRowStriping(!NoRowStriping.IsPresent)
            .WithTheme(ChartThemeResolver.Resolve(Theme));
        if (!string.IsNullOrWhiteSpace(Title)) table.WithTitle(Title);
        if (!string.IsNullOrWhiteSpace(Subtitle)) table.WithSubtitle(Subtitle);
        foreach (var row in Row) table.AddRow(ResolveValues(row));
        if (!string.IsNullOrWhiteSpace(StatusColumn)) table.WithStatusColumn(StatusColumn);
        WriteObject(table);
    }

    private object?[] ResolveValues(object row) {
        var value = row is PSObject wrapper ? wrapper.BaseObject : row;
        if (value is IDictionary dictionary) {
            return Column.Select(column => DictionaryValue(dictionary, column)).ToArray();
        }
        var psObject = PSObject.AsPSObject(value);
        if (Column.All(column => psObject.Properties[column] != null)) {
            return Column.Select(column => psObject.Properties[column].Value).ToArray();
        }
        if (value is IEnumerable enumerable && value is not string) {
            var values = new List<object?>();
            foreach (var item in enumerable) values.Add(item is PSObject itemWrapper ? itemWrapper.BaseObject : item);
            return values.ToArray();
        }
        return new object?[] { value };
    }

    private static object? DictionaryValue(IDictionary dictionary, string column) {
        foreach (DictionaryEntry entry in dictionary) {
            if (string.Equals(entry.Key?.ToString(), column, StringComparison.OrdinalIgnoreCase)) return entry.Value;
        }
        return null;
    }
}

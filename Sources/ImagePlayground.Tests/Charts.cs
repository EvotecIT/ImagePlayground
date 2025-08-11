using global::ImagePlayground;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for Charts.
/// </summary>
public partial class ImagePlayground {
    [Fact]
    public void Test_GenerateBarChart() {
        string file = Path.Combine(_directoryWithTests, "chart_bar.png");
        if (File.Exists(file)) File.Delete(file);

        var defs = new List<ChartDefinition> {
                new ChartBar("A", new List<double> { 1, 2 }),
                new ChartBar("B", new List<double> { 3, 4 })
            };

        Charts.Generate(defs, file, 300, 200);

        Assert.True(File.Exists(file));
        using var stream = File.Open(file, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
    }

    [Fact]
    public void Test_GenerateBarChart_WithAxisTitles() {
        string file = Path.Combine(_directoryWithTests, "chart_bar_titles.png");
        if (File.Exists(file)) File.Delete(file);

        var defs = new List<ChartDefinition> {
                new ChartBar("A", new List<double> { 1, 2 }),
                new ChartBar("B", new List<double> { 3, 4 })
            };

        Charts.Generate(defs, file, 300, 200, null, "X", "Y");

        Assert.True(File.Exists(file));
        using var stream = File.Open(file, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
    }

    [Fact]
    public void Test_GenerateBarChart_ShowGrid() {
        string file = Path.Combine(_directoryWithTests, "chart_bar_grid.png");
        if (File.Exists(file)) File.Delete(file);

        var defs = new List<ChartDefinition> {
                new ChartBar("A", new List<double> { 1, 2 }),
                new ChartBar("B", new List<double> { 3, 4 })
            };

        Charts.Generate(defs, file, 300, 200, null, null, null, true);

        Assert.True(File.Exists(file));
        using var stream = File.Open(file, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
    }

    [Fact]
    public void Test_GenerateContactQr() {
        string file = Path.Combine(_directoryWithTests, "contact.png");
        if (File.Exists(file)) File.Delete(file);

        QrCode.GenerateContact(file, QRCoder.PayloadGenerator.ContactData.ContactOutputType.VCard4, "John", "Doe");

        Assert.True(File.Exists(file));
        var read = QrCode.Read(file);
        Assert.Contains("BEGIN", read.Message);
    }

    [Fact]
    public void Test_GenerateMixedChartsThrows() {
        var defs = new List<ChartDefinition> {
                new ChartBar("A", new List<double> { 1 }),
                new ChartLine("B", new List<double> { 2 })
            };

        Assert.Throws<ArgumentException>(() => Charts.Generate(defs, Path.Combine(_directoryWithTests, "mixed.png")));
    }

    [Fact]
    public void Test_GenerateNullDefinitionsThrows() {
        Assert.Throws<ArgumentNullException>(() => Charts.Generate(null!, Path.Combine(_directoryWithTests, "null.png")));
    }

    [Fact]
    public void Test_GenerateEmptyDefinitionsThrows() {
        Assert.Throws<ArgumentException>(() => Charts.Generate(new List<ChartDefinition>(), Path.Combine(_directoryWithTests, "empty.png")));
    }

    [Fact]
    public void Test_GenerateWithAnnotations() {
        string file = Path.Combine(_directoryWithTests, "chart_annotation.png");
        if (File.Exists(file)) File.Delete(file);

        var defs = new List<ChartDefinition> {
                new ChartBar("A", new List<double> { 1 })
            };
        var anns = new List<ChartAnnotation> {
                new ChartAnnotation(0, 1, "first", true)
            };

        Charts.Generate(defs, file, 300, 200, null, null, null, false, ChartTheme.Default, anns);

        Assert.True(File.Exists(file));
        using var stream = File.Open(file, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
    }
}

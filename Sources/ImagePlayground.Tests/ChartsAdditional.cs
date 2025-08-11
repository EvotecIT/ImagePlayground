using System.Collections.Generic;
using System.IO;
using Xunit;
using ImagePlayground;
using SixLabors.ImageSharp;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for ChartsAdditional.
/// </summary>
public partial class ImagePlayground {
    [Fact]
    public void Test_GenerateHistogram() {
        string file = Path.Combine(_directoryWithTests, "chart_histogram.png");
        if (File.Exists(file)) File.Delete(file);
        var defs = new List<ChartDefinition> {
                new ChartHistogram("H", new double[] {1,2,3,4}, 1)
            };
        Charts.Generate(defs, file, 200, 150);
        Assert.True(File.Exists(file));
        using var stream = File.Open(file, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
    }

    [Fact]
    public void Test_GenerateBarChart_DarkTheme() {
        string file = Path.Combine(_directoryWithTests, "chart_dark.png");
        if (File.Exists(file)) File.Delete(file);
        var defs = new List<ChartDefinition> {
                new ChartBar("A", new List<double> {1,2})
            };
        Charts.Generate(defs, file, 200, 150, null, null, null, false, ChartTheme.Dark);
        Assert.True(File.Exists(file));
        using var streamDark = File.Open(file, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
    }

    [Fact]
    public void Test_GenerateBarChart_LightTheme() {
        string file = Path.Combine(_directoryWithTests, "chart_light.png");
        if (File.Exists(file)) File.Delete(file);
        var defs = new List<ChartDefinition> {
                new ChartBar("A", new List<double> {1,2})
            };
        Charts.Generate(defs, file, 200, 150, null, null, null, false, ChartTheme.Light);
        Assert.True(File.Exists(file));
        using var streamLight = File.Open(file, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
    }

    [Fact]
    public void Test_GenerateBarChart_WithBackground() {
        string file = Path.Combine(_directoryWithTests, "chart_background.png");
        if (File.Exists(file)) File.Delete(file);
        var defs = new List<ChartDefinition> {
                new ChartBar("A", new List<double> {1,2})
            };
        Charts.Generate(defs, file, 200, 150, null, null, null, false, ChartTheme.Default, null, Color.Aqua);
        Assert.True(File.Exists(file));
        using var streamBg = File.Open(file, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
    }
}

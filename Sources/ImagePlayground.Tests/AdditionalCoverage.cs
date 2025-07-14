using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for AdditionalCoverage.
/// </summary>
public partial class ImagePlayground {
    [Fact]
    public void Test_AddImage_Overlay() {
        string baseFile = Path.Combine(_directoryWithImages, "QRCode1.png");
        string overlay = Path.Combine(_directoryWithImages, "LogoEvotec.png");
        string dest = Path.Combine(_directoryWithTests, "addimage.png");
        if (File.Exists(dest)) File.Delete(dest);

        using (var img = Image.Load(baseFile)) {
            img.AddImage(overlay, 0, 0, 1f);
            img.Save(dest);
        }

        Assert.True(File.Exists(dest));
        using var baseImg = Image.Load(baseFile);
        using var modified = Image.Load(dest);
        Assert.True(baseImg.Compare(modified).PixelErrorCount > 0);
    }

    [Fact]
    public void Test_WatermarkImage_RotateFlip() {
        string src = Path.Combine(_directoryWithImages, "PrzemyslawKlysAndKulkozaurr.jpg");
        string watermark = Path.Combine(_directoryWithImages, "LogoEvotec.png");
        string dest = Path.Combine(_directoryWithTests, "watermark.png");
        if (File.Exists(dest)) File.Delete(dest);

        using (var img = Image.Load(src)) {
            img.WatermarkImage(watermark, WatermarkPlacement.TopRight, 0.8f, 10f, 90, FlipMode.Horizontal, 50);
            img.Save(dest);
        }

        Assert.True(File.Exists(dest));
        using var orig = Image.Load(src);
        using var result = Image.Load(dest);
        Assert.True(orig.Compare(result).PixelErrorCount > 0);
    }

    [Fact]
    public void Test_Resize_Stretch() {
        string src = Path.Combine(_directoryWithImages, "QRCode1.png");
        using var img = Image.Load(src);
        img.Resize(100, 50, false);
        Assert.Equal(100, img.Width);
        Assert.Equal(50, img.Height);
    }

    [Fact]
    public void Test_Resize_Percentage() {
        string src = Path.Combine(_directoryWithImages, "QRCode1.png");
        using var img = Image.Load(src);
        img.Resize(50);
        Assert.Equal(330, img.Width);
        Assert.Equal(330, img.Height);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void Test_Resize_Percentage_Invalid(int percentage) {
        string src = Path.Combine(_directoryWithImages, "QRCode1.png");
        using var img = Image.Load(src);
        Assert.Throws<ArgumentOutOfRangeException>(() => img.Resize(percentage));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-20)]
    public void Test_ImageHelper_Resize_Percentage_Invalid(int percentage) {
        string src = Path.Combine(_directoryWithImages, "QRCode1.png");
        string dest = Path.Combine(_directoryWithTests, $"percent_invalid_{percentage}.png");
        Assert.Throws<ArgumentOutOfRangeException>(() => ImageHelper.Resize(src, dest, percentage));
    }

    [Fact]
    public void Test_Combine_LeftResize() {
        string file1 = Path.Combine(_directoryWithImages, "LogoEvotec.png");
        string file2 = Path.Combine(_directoryWithImages, "QRCode1.png");
        string dest = Path.Combine(_directoryWithTests, "combine_left.png");
        if (File.Exists(dest)) File.Delete(dest);

        ImageHelper.Combine(file1, file2, dest, true, ImagePlacement.Left);
        Assert.True(File.Exists(dest));
        using var img = Image.Load(dest);
        Assert.Equal(2335, img.Width);
        Assert.Equal(660, img.Height);
    }

    [Fact]
    public void Test_Combine_InvalidPlacement() {
        string file1 = Path.Combine(_directoryWithImages, "LogoEvotec.png");
        string file2 = Path.Combine(_directoryWithImages, "QRCode1.png");
        string dest = Path.Combine(_directoryWithTests, "invalid.png");
        Assert.Throws<ArgumentException>(() => ImageHelper.Combine(file1, file2, dest, false, (ImagePlacement)999));
    }

    [Fact]
    public void Test_GetResampler_AllMapped() {
        foreach (Sampler sampler in Enum.GetValues(typeof(Sampler))) {
            Assert.NotNull(Helpers.GetResampler(sampler));
        }
    }

    [Fact]
    public void Test_IsFileLocked() {
        string file = Path.Combine(_directoryWithTests, "lock.txt");
        File.WriteAllText(file, "test");
        var info = new FileInfo(file);
        using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.None)) {
            Assert.True(info.IsFileLocked());
        }
        Assert.False(info.IsFileLocked());
    }

    [Fact]
    public void Test_LineChart() {
        string file = Path.Combine(_directoryWithTests, "chart_line.png");
        if (File.Exists(file)) File.Delete(file);
        var defs = new List<Charts.ChartDefinition> {
                new Charts.ChartLine("First", new List<double>{1,2,3}),
                new Charts.ChartLine("Second", new List<double>{2,3,4})
            };
        Charts.Generate(defs, file, 300, 200);
        Assert.True(File.Exists(file));
        using var stream = File.Open(file, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
    }

    [Fact]
    public void Test_LineChart_WithMarkers() {
        string file = Path.Combine(_directoryWithTests, "chart_line_marker.png");
        if (File.Exists(file)) File.Delete(file);
        var defs = new List<Charts.ChartDefinition> {
                new Charts.ChartLine(
                    "First",
                    new List<double>{1,2,3},
                    null,
                    ScottPlot.MarkerShape.FilledCircle,
                    5)
            };
        Charts.Generate(defs, file, 300, 200);
        Assert.True(File.Exists(file));
        using var stream = File.Open(file, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
    }

    [Fact]
    public void Test_ScatterChart() {
        string file = Path.Combine(_directoryWithTests, "chart_scatter.png");
        if (File.Exists(file)) File.Delete(file);
        var defs = new List<Charts.ChartDefinition> {
                new Charts.ChartScatter("First", new List<double>{1,2,3}, new List<double>{4,5,6}),
                new Charts.ChartScatter("Second", new List<double>{1,2,3}, new List<double>{3,2,1})
            };
        Charts.Generate(defs, file, 300, 200);
        Assert.True(File.Exists(file));
        using var stream = File.Open(file, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
    }

    [Fact]
    public void Test_PieAndRadialCharts() {
        string pie = Path.Combine(_directoryWithTests, "chart_pie.png");
        if (File.Exists(pie)) File.Delete(pie);
        var pies = new List<Charts.ChartDefinition> {
                new Charts.ChartPie("A", 1),
                new Charts.ChartPie("B", 2)
            };
        Charts.Generate(pies, pie);
        Assert.True(File.Exists(pie));
        using var streamPie = File.Open(pie, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

        string radial = Path.Combine(_directoryWithTests, "chart_radial.png");
        if (File.Exists(radial)) File.Delete(radial);
        var radials = new List<Charts.ChartDefinition> {
                new Charts.ChartRadial("A", 0.2),
                new Charts.ChartRadial("B", 0.4),
                new Charts.ChartRadial("C", 0.6)
            };
        Charts.Generate(radials, radial, 300, 200);
        Assert.True(File.Exists(radial));
        using var streamRadial = File.Open(radial, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
    }

    [Fact]
    public void Test_HeatmapChart() {
        string file = Path.Combine(_directoryWithTests, "chart_heatmap.png");
        if (File.Exists(file)) File.Delete(file);
        var map = new double[,] { { 1, 2 }, { 3, 4 } };
        var defs = new List<Charts.ChartDefinition> {
                new Charts.ChartHeatmap("H", map)
            };
        Charts.Generate(defs, file, 100, 100);
        Assert.True(File.Exists(file));
        using var stream = File.Open(file, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
    }
}

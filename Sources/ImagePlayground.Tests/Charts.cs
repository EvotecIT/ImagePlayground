using System;
using System.IO;
using System.Linq;
using ChartForgeX;
using ChartForgeX.Core;
using CodeGlyphX.Payloads;
using Xunit;

namespace ImagePlayground.Tests;

public partial class ImagePlayground {
    [Theory]
    [InlineData(global::ImagePlayground.ChartTheme.Default, false)]
    [InlineData(global::ImagePlayground.ChartTheme.Light, false)]
    [InlineData(global::ImagePlayground.ChartTheme.Dark, true)]
    public void Test_ChartDefaultsUseCanonicalThemeWithoutReplacingExplicitPalette(global::ImagePlayground.ChartTheme theme, bool dark) {
        var expected = dark ? ChartForgeX.Themes.ChartTheme.GraphiteDark() : ChartForgeX.Themes.ChartTheme.GraphiteLight();
        var chart = global::ImagePlayground.Charts.Create(theme: theme);

        Assert.True(chart.Options.Theme.UseGraphiteLayout);
        Assert.Equal(expected.FontFamily, chart.Options.Theme.FontFamily);
        Assert.Equal(expected.Text, chart.Options.Theme.Text);
        Assert.Equal(expected.TitleFontSize, chart.Options.Theme.TitleFontSize);
        Assert.Equal(expected.Palette, chart.Options.Theme.Palette);

        var explicitColors = new[] { ChartForgeX.Primitives.ChartColor.FromHex("#B91C1C"), ChartForgeX.Primitives.ChartColor.FromHex("#047857") };
        var customized = global::ImagePlayground.Charts.Create(theme: theme, options: new global::ImagePlayground.ChartRenderOptions { Palette = explicitColors });
        Assert.Equal(explicitColors, customized.Options.Theme.Palette);
        Assert.True(customized.Options.Theme.UseGraphiteLayout);
    }

    [Fact]
    public void Test_NativeChartForgeXChartRendersWithoutImagePlaygroundModels() {
        var file = Path.Combine(_directoryWithTests, "chart-native.png");
        if (File.Exists(file)) File.Delete(file);

        Chart.Create()
            .WithSize(360, 220)
            .WithTitle("Native ChartForgeX")
            .WithGrid()
            .AddBar("A", ChartPoints.FromValues(1, 2, 3))
            .AddBar("B", ChartPoints.FromValues(3, 4, 5))
            .Save(file);

        Assert.True(File.Exists(file));
        using var stream = File.Open(file, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        Assert.True(stream.Length > 64);
    }

    [Fact]
    public void Test_LineDefinitionsPreserveMarkerPolicy() {
        var markerless = global::ImagePlayground.Charts.Build(new global::ImagePlayground.ChartDefinition[] {
            new global::ImagePlayground.ChartLine("Without markers", new[] { 10d, 30d, 20d })
        });

        Assert.Equal(0d, markerless.Options.Theme.MarkerRadius);
        Assert.DoesNotContain("data-cfx-role=\"line-marker\"", markerless.ToSvg());

        var mixed = new global::ImagePlayground.ChartDefinition[] {
            new global::ImagePlayground.ChartLine("Without markers", new[] { 10d, 30d, 20d }, markerSize: 0),
            new global::ImagePlayground.ChartLine("With markers", new[] { 20d, 15d, 25d }, markerSize: 6)
        };
        var exception = Assert.Throws<ArgumentException>(() => global::ImagePlayground.Charts.Build(mixed));
        Assert.Contains("shared marker size", exception.Message);
    }

    [Fact]
    public void Test_HistogramBinSizePreservesRequestedWidth() {
        var chart = global::ImagePlayground.Charts.Build(new global::ImagePlayground.ChartDefinition[] {
            new global::ImagePlayground.ChartHistogram("Requested width", new[] { 0d, 1d, 3d, 5d, 6d, 9d, 10d }, 3)
        });

        Assert.Equal(new[] { "0-3", "3-6", "6-9", "9-12" }, chart.Options.XAxisLabels.Select(label => label.Text));
        Assert.Equal(new[] { 2d, 2d, 1d, 2d }, chart.Series[0].Points.Select(point => point.Y));
    }

    [Fact]
    public void Test_GenerateContactQr() {
        var file = Path.Combine(_directoryWithTests, "contact.png");
        if (File.Exists(file)) File.Delete(file);

        QrCode.GenerateContact(file, QrContactOutputType.VCard4, "John", "Doe");

        Assert.True(File.Exists(file));
        var read = QrCode.Read(file);
        Assert.NotNull(read);
        Assert.Contains("BEGIN", read.Text);
    }
}

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
        var expected = ChartForgeX.Themes.VisualTheme.Graphite();
        var mode = dark ? ChartForgeX.Themes.VisualThemeMode.Dark : ChartForgeX.Themes.VisualThemeMode.Light;
        var chart = global::ImagePlayground.Charts.Create(theme: theme);

        Assert.Equal(mode, chart.Options.ThemeMode);
        Assert.Equal(expected.Typography.Family, chart.Options.Theme.Typography.Family);
        Assert.Equal(expected.Resolve(mode).Foreground, chart.Options.Theme.Resolve(mode).Foreground);
        Assert.Equal(expected.Typography.TitleSize, chart.Options.Theme.Typography.TitleSize);
        Assert.Equal(expected.Resolve(mode).Palette, chart.Options.Theme.Resolve(mode).Palette);

        var explicitColors = new[] { ChartForgeX.Primitives.ChartColor.FromHex("#B91C1C"), ChartForgeX.Primitives.ChartColor.FromHex("#047857") };
        var customized = global::ImagePlayground.Charts.Create(theme: theme, options: new global::ImagePlayground.ChartRenderOptions { Palette = explicitColors });
        Assert.Equal(explicitColors, customized.Options.Theme.Resolve(mode).Palette);
        Assert.Equal(expected.Resolve(mode).Palette, chart.Options.Theme.Resolve(mode).Palette);
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

        Assert.False(markerless.Series[0].Markers.Enabled);
        Assert.DoesNotContain("data-cfx-role=\"line-marker\"", markerless.ToSvg());

        var mixed = global::ImagePlayground.Charts.Build(new global::ImagePlayground.ChartDefinition[] {
            new global::ImagePlayground.ChartLine("Without markers", new[] { 10d, 30d, 20d }, markerSize: 0),
            new global::ImagePlayground.ChartLine("With markers", new[] { 20d, 15d, 25d }, markerSize: 8, markerShape: global::ImagePlayground.ChartMarkerShape.Diamond)
        });
        Assert.False(mixed.Series[0].Markers.Enabled);
        Assert.True(mixed.Series[1].Markers.Enabled);
        Assert.Equal(ChartForgeX.Core.ChartMarkerShape.Diamond, mixed.Series[1].Markers.Shape);
        Assert.Equal(4d, mixed.Series[1].Markers.Radius);
    }

    [Fact]
    public void Test_CartesianDefinitionsCombineInDeclarationOrder() {
        var bar = new global::ImagePlayground.ChartBar("Requests", new[] { 410d, 380d, 455d });
        var line = new global::ImagePlayground.ChartLine("Error rate", new[] { 1.2d, 0.8d, 2.4d });
        var chart = global::ImagePlayground.Charts.Build(new global::ImagePlayground.ChartDefinition[] { bar, line });

        Assert.Equal(new[] { ChartSeriesKind.Bar, ChartSeriesKind.Line }, chart.Series.Select(series => series.Kind));
        Assert.Equal(new[] { "1", "2", "3" }, chart.Options.XAxisLabels.Select(label => label.Text));
        Assert.Equal(new[] { "series-1", "series-2" }, chart.Series.Select(series => series.Id.Value));
        var retained = chart.Series[1];
        retained.WithName("Errors");
        Assert.Same(retained, chart.Series[1]);
        Assert.Equal("series-2", retained.Id.Value);
        Assert.Equal("Errors", retained.Name);
    }

    [Fact]
    public void Test_RadialDefinitionsRetainPercentProgressRings() {
        var chart = global::ImagePlayground.Charts.Build(new global::ImagePlayground.ChartDefinition[] {
            new global::ImagePlayground.ChartRadial("CPU", 25),
            new global::ImagePlayground.ChartRadial("Memory", 75)
        });

        Assert.Equal(ChartSeriesKind.ProgressRing, chart.Series[0].Kind);
        Assert.Equal(new[] { 25d, 75d }, chart.Series[0].Points.Select(point => point.Y));
        Assert.Contains("progress-ring", chart.ToSvg());
    }

    [Fact]
    public void Test_ExplicitBackgroundDoesNotMutateSharedTheme() {
        var background = ChartForgeX.Primitives.ChartColor.FromHex("#334455");
        var original = ChartForgeX.Themes.VisualTheme.Graphite();
        var chart = global::ImagePlayground.Charts.Create(theme: global::ImagePlayground.ChartTheme.Dark, background: background);

        Assert.Equal(background, chart.Options.Theme.Resolve(chart.Options.ThemeMode).Background);
        Assert.Equal(original.Resolve(ChartForgeX.Themes.VisualThemeMode.Light).Background,
            chart.Options.Theme.Resolve(ChartForgeX.Themes.VisualThemeMode.Light).Background);
        Assert.NotEqual(background, original.Resolve(ChartForgeX.Themes.VisualThemeMode.Dark).Background);
    }

    [Fact]
    public void Test_ExclusiveDefinitionsCannotBeCombined() {
        var definitions = new global::ImagePlayground.ChartDefinition[] {
            new global::ImagePlayground.ChartPie("Web", 10),
            new global::ImagePlayground.ChartBar("Requests", new[] { 1d, 2d })
        };

        var exception = Assert.Throws<ArgumentException>(() => global::ImagePlayground.Charts.Build(definitions));
        Assert.Contains("cannot share one chart", exception.Message);
    }

    [Fact]
    public void Test_BarLayoutOptionStacksBars() {
        var chart = global::ImagePlayground.Charts.Build(new global::ImagePlayground.ChartDefinition[] {
            new global::ImagePlayground.ChartBar("Passed", new[] { 41d, 38d }),
            new global::ImagePlayground.ChartBar("Failed", new[] { 3d, 6d })
        }, options: new global::ImagePlayground.ChartRenderOptions { BarLayout = global::ImagePlayground.ChartBarLayout.Stacked });

        Assert.Equal(ChartBarMode.Stacked, chart.Options.BarMode);
    }

    [Fact]
    public void Test_HistogramBinSizePreservesRequestedWidth() {
        var chart = global::ImagePlayground.Charts.Build(new global::ImagePlayground.ChartDefinition[] {
            new global::ImagePlayground.ChartHistogram("Requested width", new[] { 0d, 1d, 3d, 5d, 6d, 9d, 10d }, 3)
        });

        var bins = chart.Series[0].HistogramBins;
        Assert.Equal(new[] { 0d, 3d, 6d, 9d }, bins.Select(bin => bin.LowerBound));
        Assert.Equal(new[] { 3d, 6d, 9d, 12d }, bins.Select(bin => bin.UpperBound));
        Assert.All(bins, bin => Assert.Equal(3d, bin.Width));
        Assert.Equal(new[] { 2, 2, 1, 2 }, bins.Select(bin => bin.Count));
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

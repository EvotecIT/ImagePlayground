#if NET8_0
using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Xml.Linq;
using ChartForgeX;
using ChartForgeX.Composition;
using ChartForgeX.Core;
using ChartForgeX.Primitives;
using ChartForgeX.Stories;
using ChartForgeX.Terminal;
using ChartForgeX.Themes;
using ChartForgeX.Topology;
using Xunit;

namespace ImagePlayground.Tests;

public partial class ImagePlayground {
    // These public invocations run inside the existing coverage host, including PowerShell parameter binding.
    [Theory]
    [InlineData("Light")]
    [InlineData("Dark")]
    public void Test_PowerShell_CanvasThemeBindingsPreserveInheritedAndExplicitText(string theme) {
        using var fixture = new PowerShellPathFixture();
        var mode = theme == "Dark" ? VisualThemeMode.Dark : VisualThemeMode.Light;
        var tokens = VisualTheme.Graphite().Resolve(mode);
        var inherited = Assert.IsType<VisualCanvasTextLayer>(fixture.Invoke("New-ImageCanvasText",
            ("X", 12), ("Y", 8), ("Width", 320), ("Text", "Service health"), ("FontSize", 20)).Single().BaseObject);
        var white = Assert.IsType<VisualCanvasTextLayer>(fixture.Invoke("New-ImageCanvasText",
            ("X", 12), ("Y", 40), ("Width", 320), ("Text", "Explicit white"), ("FontSize", 20), ("Color", "White")).Single().BaseObject);
        var transparent = Assert.IsType<VisualCanvasTextLayer>(fixture.Invoke("New-ImageCanvasText",
            ("X", 12), ("Y", 72), ("Width", 320), ("Text", "Transparent"), ("FontSize", 20), ("Color", "Transparent")).Single().BaseObject);
        var canvas = Assert.IsType<VisualCanvas>(fixture.Invoke("New-ImageCanvas",
            ("Layer", new VisualCanvasLayer[] { inherited, white, transparent }), ("Width", 368), ("Height", 120),
            ("Theme", theme), ("BackgroundTop", "#334455"), ("FilePath", "./canvas.svg"), ("PassThru", true)).Single().BaseObject);

        Assert.Equal(mode, canvas.ThemeMode);
        Assert.Equal(tokens.Foreground, canvas.Theme.Resolve(canvas.ThemeMode).Foreground);
        Assert.Equal(VisualTheme.Graphite().Typography.Family, canvas.Theme.Typography.Family);
        Assert.Equal("#334455", canvas.BackgroundTop.ToCss());
        var text = XDocument.Load(Path.Combine(fixture.Location, "canvas.svg")).Descendants()
            .Where(element => (string?)element.Attribute("data-cfx-role") == "visual-canvas-text").ToArray();
        Assert.Equal(new[] { "Service health", "Explicit white", "Transparent" }, text.Select(element => element.Value));
        Assert.Equal(new[] { tokens.Foreground.ToCss(), ChartColor.White.ToCss(), ChartColor.Transparent.ToCss() },
            text.Select(element => (string?)element.Attribute("fill")));

        fixture.Invoke("New-ImageCanvas", ("Canvas", canvas), ("FilePath", "./canvas.png"));
        fixture.AssertImageOutput("canvas.png", 368, 120);
        using var actual = global::ImagePlayground.Image.Load(Path.Combine(fixture.Location, "canvas.png"));
        var expected = VisualCanvas.Create(368, 120).WithTheme(VisualTheme.Graphite(), mode).WithBackground(ChartColor.FromHex("#334455"));
        expected.AddLayer(new VisualCanvasTextLayer(12, 8, 320, "Service health", 20, tokens.Foreground));
        expected.AddLayer(white).AddLayer(transparent);
        Assert.Equal(expected.ToRgbaImage().Pixels, actual.Raster.GetPixels());
    }

    [Fact]
    public void Test_PowerShell_CanvasKeepsCallerThemeUntilExplicitlyOverridden() {
        using var fixture = new PowerShellPathFixture();
        var canvas = VisualCanvas.Create(368, 120).WithBackground(ChartColor.FromHex("#123456"));
        canvas.Theme = canvas.Theme.Configure(builder => builder.Dark.Foreground = ChartColor.FromHex("#ABCDEF"));
        canvas.AddLayer(new VisualCanvasTextLayer(12, 8, 320, "Caller theme", 20));
        var preserved = fixture.Invoke("New-ImageCanvas", ("Canvas", canvas), ("FilePath", "./preserved.svg"), ("PassThru", true)).Single().BaseObject;
        Assert.Same(canvas, preserved);
        Assert.Equal("#ABCDEF", canvas.Theme.Resolve(canvas.ThemeMode).Foreground.ToCss());
        Assert.Equal("#123456", canvas.BackgroundTop.ToCss());
        fixture.Invoke("New-ImageCanvas", ("Canvas", canvas), ("Theme", "Light"), ("FilePath", "./overridden.svg"));
        Assert.Equal(VisualThemeMode.Light, canvas.ThemeMode);
        Assert.Equal(VisualTheme.Graphite().Resolve(VisualThemeMode.Light).Foreground, canvas.Theme.Resolve(canvas.ThemeMode).Foreground);
    }

    [Fact]
    public void Test_PowerShell_ChartHeadingsArePreservedOverriddenAndExplicitlyCleared() {
        using var fixture = new PowerShellPathFixture();
        var chart = Chart.Create().WithSize(368, 280).WithTitle("Weekly builds").WithSubtitle("Completed pipeline runs")
            .AddBar("Builds", ChartPoints.FromValues(1, 2));
        var preserved = fixture.Invoke("New-ImageChart", ("Chart", chart), ("FilePath", "./preserved.svg"), ("PassThru", true)).Single().BaseObject;
        Assert.Same(chart, preserved);
        Assert.Equal("Weekly builds", chart.Title);
        Assert.Equal("Completed pipeline runs", chart.Subtitle);
        fixture.Invoke("New-ImageChart", ("Chart", chart), ("Title", "Build history"), ("Subtitle", ""), ("FilePath", "./changed.svg"));
        Assert.Equal("Build history", chart.Title);
        Assert.True(string.IsNullOrEmpty(chart.Subtitle));
        var svg = File.ReadAllText(Path.Combine(fixture.Location, "changed.svg"));
        Assert.Contains("Build history", svg);
        Assert.DoesNotContain("Completed pipeline runs", svg);
    }

    [Theory]
    [InlineData("Light", false)]
    [InlineData("Dark", false)]
    [InlineData("Light", true)]
    [InlineData("Dark", true)]
    public void Test_PowerShell_TopologyUsesCanonicalThemeAndSemanticStatus(string theme, bool transparent) {
        using var fixture = new PowerShellPathFixture();
        var mode = theme == "Dark" ? VisualThemeMode.Dark : VisualThemeMode.Light;
        var tokens = VisualTheme.Graphite().Resolve(mode);
        var chart = Assert.IsType<TopologyChart>(fixture.Invoke("New-ImageTopology",
            ("TopologyDefinition", ScriptBlock.Create("New-ImageTopologyNode -Id api -Label API -Kind Service -Status Healthy")),
            ("Theme", theme), ("Transparent", transparent), ("FilePath", "./topology.svg"), ("PassThru", true)).Single().BaseObject);
        Assert.NotNull(chart.Theme);
        Assert.Equal(mode, chart.ThemeMode);
        var actual = chart.Theme.Resolve(chart.ThemeMode);
        Assert.Equal(transparent ? tokens.Background.WithOpacity(0) : tokens.Background, actual.Background);
        Assert.Equal(tokens.Foreground, actual.Foreground);
        Assert.Equal(tokens.Positive, actual.Positive);
        Assert.Equal(VisualTheme.Graphite().Typography.Family, chart.Theme.Typography.Family);
        Assert.Equal("api", Assert.IsType<TopologyNode>(Assert.Single(chart.Nodes)).Id);
        Assert.Contains("API", File.ReadAllText(Path.Combine(fixture.Location, "topology.svg")));
    }

    [Fact]
    public void Test_PowerShell_TransparentTopologyKeepsCallerModeAndPaletteWithoutChangingSharedTheme() {
        using var fixture = new PowerShellPathFixture();
        var theme = VisualTheme.Graphite().Configure(builder => builder.Dark.Foreground = ChartColor.FromHex("#ABCDEF"));
        var chart = TopologyChart.Create().WithTheme(theme, VisualThemeMode.Dark);
        chart.Nodes.Add(new TopologyNode { Id = "api", Label = "API" });
        fixture.Invoke("New-ImageTopology", ("Chart", chart), ("Transparent", true), ("FilePath", "./transparent.svg"));

        Assert.Equal(VisualThemeMode.Dark, chart.ThemeMode);
        Assert.Equal(ChartColor.FromHex("#ABCDEF"), chart.Theme.Resolve(chart.ThemeMode).Foreground);
        Assert.Equal(theme.Resolve(VisualThemeMode.Dark).Background.WithOpacity(0), chart.Theme.Resolve(chart.ThemeMode).Background);
        Assert.Equal(VisualTheme.Graphite().Resolve(VisualThemeMode.Dark).Background, theme.Resolve(VisualThemeMode.Dark).Background);
    }

    [Theory]
    [InlineData("Light")]
    [InlineData("Dark")]
    public void Test_PowerShell_TerminalThemePreservesExplicitPaletteOverrides(string theme) {
        using var fixture = new PowerShellPathFixture();
        var mode = theme == "Dark" ? VisualThemeMode.Dark : VisualThemeMode.Light;
        var expected = VisualTheme.Graphite().Resolve(mode);
        var content = ScriptBlock.Create("New-ImageConsoleStoryCommand -Text Get-Date; New-ImageConsoleStoryOutput -Text Ready -Style Success");
        var story = Assert.IsType<TerminalStory>(fixture.Invoke("New-ImageConsoleStory", ("Theme", theme), ("Content", content)).Single().BaseObject);
        Assert.Equal(mode, story.ThemeMode);
        Assert.Equal(expected.ElevatedSurface, story.Theme.Resolve(story.ThemeMode).ElevatedSurface);
        Assert.Equal(VisualTheme.Graphite().Typography.MonospaceFamily, story.Theme.Typography.MonospaceFamily);
        var palette = Assert.IsType<VisualTheme>(fixture.Invoke("New-ImageConsoleStoryPalette", ("Preset", theme), ("Accent", "#9F1239")).Single().BaseObject);
        var customized = Assert.IsType<TerminalStory>(fixture.Invoke("New-ImageConsoleStory", ("Theme", theme), ("Palette", palette), ("Content", content)).Single().BaseObject);
        Assert.Equal("#9F1239", customized.Theme.Resolve(customized.ThemeMode).Accent.ToCss());
        Assert.Equal(expected.ElevatedSurface, customized.Theme.Resolve(customized.ThemeMode).ElevatedSurface);
        Assert.Equal("#9F1239", palette.Resolve(mode == VisualThemeMode.Light ? VisualThemeMode.Dark : VisualThemeMode.Light).Accent.ToCss());
        Assert.Contains("Ready", customized.ToSvg());
    }

    [Fact]
    public void Test_PowerShell_AuthoredTerminalStartsCanonicalAndKeepsScriptOverrides() {
        using var fixture = new PowerShellPathFixture();
        var story = Assert.IsType<TerminalStory>(fixture.Invoke("New-ImageConsoleStory",
            ("StoryScript", ScriptBlock.Create("param($Console) [void]$Console.Command('Get-Date').Output('Ready')"))).Single().BaseObject);
        Assert.Equal(VisualThemeMode.Dark, story.ThemeMode);
        Assert.Equal(VisualTheme.Graphite().Resolve(VisualThemeMode.Dark).ElevatedSurface, story.Theme.Resolve(story.ThemeMode).ElevatedSurface);
        var customized = Assert.IsType<TerminalStory>(fixture.Invoke("New-ImageConsoleStory",
            ("StoryScript", ScriptBlock.Create("param($Console) [void]$Console.WithTheme([ChartForgeX.Terminal.TerminalThemes]::Ubuntu()).Output('Ready')"))).Single().BaseObject);
        Assert.Equal(TerminalThemes.Ubuntu().Resolve(VisualThemeMode.Dark).ElevatedSurface, customized.Theme.Resolve(customized.ThemeMode).ElevatedSurface);
    }

    [Fact]
    public void Test_PowerShell_TerminalTabCarriesExplicitModeAndDetachedStyle() {
        using var fixture = new PowerShellPathFixture();
        var style = new TerminalStyleOptions { Cursor = ChartColor.FromHex("#9F1239"), HeaderBackground = ChartColor.FromHex("#CCDDEE") };
        var tab = fixture.Invoke("New-ImageConsoleStoryTab", ("Id", "initial"), ("Active", true),
            ("Profile", "PowerShell"), ("ThemeMode", VisualThemeMode.Light), ("Style", style)).Single().BaseObject;
        var output = fixture.Invoke("New-ImageConsoleStoryOutput", ("Text", "Ready")).Single().BaseObject;
        style.Cursor = ChartColor.Black;
        var story = Assert.IsType<TerminalStory>(fixture.Invoke("New-ImageConsoleStory", ("Step", new[] { tab, output })).Single().BaseObject);

        Assert.Equal(VisualThemeMode.Light, story.ThemeMode);
        Assert.Equal(TerminalThemes.PowerShell().Resolve(VisualThemeMode.Light).ElevatedSurface, story.Theme.Resolve(story.ThemeMode).ElevatedSurface);
        Assert.Equal(ChartColor.FromHex("#9F1239"), story.Style.Cursor);
        Assert.Equal(ChartColor.FromHex("#CCDDEE"), story.Style.HeaderBackground);
        Assert.Contains("#9F1239", story.ToSvg());
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Test_PowerShell_VisualGridStoryKeepsAuthoredTheme(bool custom) {
        using var fixture = new PowerShellPathFixture();
        var script = "param($Story) " + (custom ? "[void]$Story.WithTheme([ChartForgeX.Themes.VisualTheme]::Graphite(), [ChartForgeX.Themes.VisualThemeMode]::Dark); " : "")
            + "[void]$Story.Add([ChartForgeX.VisualBlocks.MetricCard]::Create().WithMetric('Ready','Yes'))";
        var grid = Assert.IsType<ChartForgeX.VisualBlocks.VisualGrid>(fixture.Invoke("New-ImageVisualStory",
            ("StoryScript", ScriptBlock.Create(script)), ("FilePath", "./story.svg"), ("PassThru", true)).Single().BaseObject);
        var mode = custom ? VisualThemeMode.Dark : VisualThemeMode.Light;
        var expected = VisualTheme.Graphite().Resolve(mode);
        Assert.NotNull(grid.Theme);
        Assert.Equal(mode, grid.ThemeMode);
        Assert.Equal(expected.Foreground, grid.Theme.Resolve(grid.ThemeMode).Foreground);
        Assert.Contains("Ready", File.ReadAllText(Path.Combine(fixture.Location, "story.svg")));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Test_PowerShell_AuthoredStoryDefaultsAndExplicitTheme(bool light) {
        using var fixture = new PowerShellPathFixture();
        var panel = fixture.Invoke("New-ImageStoryPanel", ("Id", "ready"), ("Text", "Ready")).Single().BaseObject;
        var scene = fixture.Invoke("New-ImageStoryScene", ("Id", "complete"), ("Title", "Completed"), ("Panels", new[] { panel })).Single().BaseObject;
        var outcome = fixture.Invoke("New-ImageStoryOutcome", ("Id", "ready"), ("Label", "Ready"), ("PanelId", "ready")).Single().BaseObject;
        var parameters = new System.Collections.Generic.List<(string, object)> {
            ("Title", "Service health"), ("Scenes", new[] { scene }), ("Outcomes", new[] { outcome }),
            ("FilePath", "./authored.svg"), ("PassThru", true)
        };
        if (light) {
            parameters.Add(("Theme", VisualTheme.Graphite()));
            parameters.Add(("ThemeMode", VisualThemeMode.Light));
        }
        var story = Assert.IsType<VisualStory>(fixture.Invoke("New-ImageStory", parameters.ToArray()).Single().BaseObject);
        var mode = light ? VisualThemeMode.Light : VisualThemeMode.Dark;
        Assert.Equal(mode, story.ThemeMode);
        Assert.Equal(VisualTheme.Graphite().Resolve(mode).Background, story.Theme.Resolve(story.ThemeMode).Background);
        Assert.Contains("Ready", File.ReadAllText(Path.Combine(fixture.Location, "authored.svg")));
    }
}
#endif

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
        var tokens = theme == "Dark" ? VisualDesignTokens.GraphiteDark() : VisualDesignTokens.GraphiteLight();
        var inherited = Assert.IsType<VisualCanvasTextLayer>(fixture.Invoke("New-ImageCanvasText",
            ("X", 12), ("Y", 8), ("Width", 320), ("Text", "Service health"), ("FontSize", 20)).Single().BaseObject);
        var white = Assert.IsType<VisualCanvasTextLayer>(fixture.Invoke("New-ImageCanvasText",
            ("X", 12), ("Y", 40), ("Width", 320), ("Text", "Explicit white"), ("FontSize", 20), ("Color", "White")).Single().BaseObject);
        var transparent = Assert.IsType<VisualCanvasTextLayer>(fixture.Invoke("New-ImageCanvasText",
            ("X", 12), ("Y", 72), ("Width", 320), ("Text", "Transparent"), ("FontSize", 20), ("Color", "Transparent")).Single().BaseObject);
        var canvas = Assert.IsType<VisualCanvas>(fixture.Invoke("New-ImageCanvas",
            ("Layer", new VisualCanvasLayer[] { inherited, white, transparent }), ("Width", 368), ("Height", 120),
            ("Theme", theme), ("BackgroundTop", "#334455"), ("FilePath", "./canvas.svg"), ("PassThru", true)).Single().BaseObject);

        Assert.Equal(tokens.Foreground, canvas.Theme.TileValueColor);
        Assert.Equal(tokens.FontFamily, canvas.Theme.FontFamily);
        Assert.Equal("#334455", canvas.BackgroundTop.ToCss());
        var text = XDocument.Load(Path.Combine(fixture.Location, "canvas.svg")).Descendants()
            .Where(element => (string?)element.Attribute("data-cfx-role") == "visual-canvas-text").ToArray();
        Assert.Equal(new[] { "Service health", "Explicit white", "Transparent" }, text.Select(element => element.Value));
        Assert.Equal(new[] { tokens.Foreground.ToCss(), ChartColor.White.ToCss(), ChartColor.Transparent.ToCss() },
            text.Select(element => (string?)element.Attribute("fill")));

        fixture.Invoke("New-ImageCanvas", ("Canvas", canvas), ("FilePath", "./canvas.png"));
        fixture.AssertImageOutput("canvas.png", 368, 120);
        using var actual = global::ImagePlayground.Image.Load(Path.Combine(fixture.Location, "canvas.png"));
        var expected = VisualCanvas.Create(368, 120).WithDesignTokens(tokens).WithBackground(ChartColor.FromHex("#334455"));
        expected.AddLayer(new VisualCanvasTextLayer(12, 8, 320, "Service health", 20, tokens.Foreground));
        expected.AddLayer(white).AddLayer(transparent);
        Assert.Equal(expected.ToRgbaImage().Pixels, actual.Raster.GetPixels());
    }

    [Fact]
    public void Test_PowerShell_CanvasKeepsCallerThemeUntilExplicitlyOverridden() {
        using var fixture = new PowerShellPathFixture();
        var canvas = VisualCanvas.Create(368, 120).WithBackground(ChartColor.FromHex("#123456"));
        canvas.Theme.TileValueColor = ChartColor.FromHex("#ABCDEF");
        canvas.AddLayer(new VisualCanvasTextLayer(12, 8, 320, "Caller theme", 20));
        var preserved = fixture.Invoke("New-ImageCanvas", ("Canvas", canvas), ("FilePath", "./preserved.svg"), ("PassThru", true)).Single().BaseObject;
        Assert.Same(canvas, preserved);
        Assert.Equal("#ABCDEF", canvas.Theme.TileValueColor.ToCss());
        Assert.Equal("#123456", canvas.BackgroundTop.ToCss());
        fixture.Invoke("New-ImageCanvas", ("Canvas", canvas), ("Theme", "Light"), ("FilePath", "./overridden.svg"));
        Assert.Equal(VisualDesignTokens.GraphiteLight().Foreground, canvas.Theme.TileValueColor);
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
        var tokens = theme == "Dark" ? VisualDesignTokens.GraphiteDark() : VisualDesignTokens.GraphiteLight();
        var chart = Assert.IsType<TopologyChart>(fixture.Invoke("New-ImageTopology",
            ("TopologyDefinition", ScriptBlock.Create("New-ImageTopologyNode -Id api -Label API -Kind Service -Status Healthy")),
            ("Theme", theme), ("Transparent", transparent), ("FilePath", "./topology.svg"), ("PassThru", true)).Single().BaseObject);
        Assert.NotNull(chart.Theme);
        Assert.Equal((transparent ? tokens.Background.WithOpacity(0) : tokens.Background).ToCss(), chart.Theme.Background);
        Assert.Equal(tokens.Foreground.ToCss(), chart.Theme.Foreground);
        Assert.Equal(tokens.Positive.ToCss(), chart.Theme.Healthy);
        Assert.Equal(tokens.FontFamily, chart.Theme.FontFamily);
        Assert.Equal("api", Assert.IsType<TopologyNode>(Assert.Single(chart.Nodes)).Id);
        Assert.Contains("API", File.ReadAllText(Path.Combine(fixture.Location, "topology.svg")));
    }

    [Theory]
    [InlineData("Light")]
    [InlineData("Dark")]
    public void Test_PowerShell_TerminalThemePreservesExplicitPaletteOverrides(string theme) {
        using var fixture = new PowerShellPathFixture();
        var expected = theme == "Dark" ? TerminalTheme.GraphiteDark() : TerminalTheme.GraphiteLight();
        var content = ScriptBlock.Create("New-ImageConsoleStoryCommand -Text Get-Date; New-ImageConsoleStoryOutput -Text Ready -Style Success");
        var story = Assert.IsType<TerminalStory>(fixture.Invoke("New-ImageConsoleStory", ("Theme", theme), ("Content", content)).Single().BaseObject);
        Assert.Equal(expected.Background, story.Theme.Background);
        Assert.Equal(expected.FontFamily, story.Theme.FontFamily);
        var palette = Assert.IsType<TerminalTheme>(fixture.Invoke("New-ImageConsoleStoryPalette", ("Preset", theme), ("Accent", "#9F1239")).Single().BaseObject);
        var customized = Assert.IsType<TerminalStory>(fixture.Invoke("New-ImageConsoleStory", ("Theme", theme), ("Palette", palette), ("Content", content)).Single().BaseObject);
        Assert.Equal("#9F1239", customized.Theme.Accent.ToCss());
        Assert.Equal(expected.Background, customized.Theme.Background);
        Assert.Contains("Ready", customized.ToSvg());
    }

    [Fact]
    public void Test_PowerShell_AuthoredTerminalStartsCanonicalAndKeepsScriptOverrides() {
        using var fixture = new PowerShellPathFixture();
        var story = Assert.IsType<TerminalStory>(fixture.Invoke("New-ImageConsoleStory",
            ("StoryScript", ScriptBlock.Create("param($Console) [void]$Console.Command('Get-Date').Output('Ready')"))).Single().BaseObject);
        Assert.Equal(TerminalTheme.GraphiteDark().Background, story.Theme.Background);
        var customized = Assert.IsType<TerminalStory>(fixture.Invoke("New-ImageConsoleStory",
            ("StoryScript", ScriptBlock.Create("param($Console) [void]$Console.WithTheme([ChartForgeX.Terminal.TerminalTheme]::Ubuntu()).Output('Ready')"))).Single().BaseObject);
        Assert.Equal(TerminalTheme.Ubuntu().Background, customized.Theme.Background);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Test_PowerShell_VisualGridStoryKeepsAuthoredTheme(bool custom) {
        using var fixture = new PowerShellPathFixture();
        var script = "param($Story) " + (custom ? "[void]$Story.WithTheme([ChartForgeX.Themes.ChartTheme]::Aurora()); " : "")
            + "[void]$Story.Add([ChartForgeX.VisualBlocks.MetricCard]::Create().WithMetric('Ready','Yes'))";
        var grid = Assert.IsType<ChartForgeX.VisualBlocks.VisualGrid>(fixture.Invoke("New-ImageVisualStory",
            ("StoryScript", ScriptBlock.Create(script)), ("FilePath", "./story.svg"), ("PassThru", true)).Single().BaseObject);
        var expected = custom ? ChartForgeX.Themes.ChartTheme.Aurora() : ChartForgeX.Themes.ChartTheme.GraphiteLight();
        Assert.NotNull(grid.Theme);
        Assert.Equal(expected.Text, grid.Theme.Text);
        Assert.Equal(expected.UseGraphiteLayout, grid.Theme.UseGraphiteLayout);
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
        if (light) parameters.Add(("Theme", VisualStoryTheme.GraphiteLight()));
        var story = Assert.IsType<VisualStory>(fixture.Invoke("New-ImageStory", parameters.ToArray()).Single().BaseObject);
        var expected = light ? VisualStoryTheme.GraphiteLight() : VisualStoryTheme.GraphiteDark();
        Assert.Equal(expected.Background, story.Theme.Background);
        Assert.Contains("Ready", File.ReadAllText(Path.Combine(fixture.Location, "authored.svg")));
    }
}
#endif

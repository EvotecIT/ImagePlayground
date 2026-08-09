using ChartForgeX.Topology;
using ChartForgeX.Interactivity.Html;
using ChartForgeX.Core;
using ChartForgeX.VisualArtifacts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a topology diagram image.</summary>
/// <para>Renders ChartForgeX topology definitions to PNG, SVG, HTML, GIF, or APNG output. GIF and APNG sample the route configured by <c>Motion</c>.</para>
/// <example>
///   <summary>Create a service topology diagram</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageTopology -TopologyDefinition {
///     New-ImageTopologyNode -Id api -Label API -Kind Service -Status Healthy -Symbol API
///     New-ImageTopologyNode -Id db -Label Database -Kind Database -Status Warning -Symbol SQL
///     New-ImageTopologyEdge -SourceNodeId api -TargetNodeId db -Label '32 ms' -Kind Dependency -Status Warning -Direction Forward
/// } -Title 'Service map' -Layout Layered -Direction LeftToRight -FilePath service-map.png</code>
///   <para>Creates a transparent-ready PNG topology diagram from a PowerShell DSL.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageTopology", DefaultParameterSetName = ScriptBlockSet)]
[OutputType(typeof(TopologyChart))]
public sealed class NewImageTopologyCmdlet : ImageCmdlet {
    private const string ScriptBlockSet = "ScriptBlock";
    private const string DefinitionSet = "Definition";
    private readonly List<TopologyGroup> _groups = new();
    private readonly List<TopologyNode> _nodes = new();
    private readonly List<TopologyEdge> _edges = new();
    private readonly List<TopologyScenario> _scenarios = new();
    private TopologyChart? _chart;

    /// <para>Script block that emits topology groups, nodes, edges, or a topology chart.</para>
    [Parameter(Mandatory = true, Position = 0, ParameterSetName = ScriptBlockSet)]
    public ScriptBlock? TopologyDefinition { get; set; }

    /// <para>Topology objects provided directly or from the pipeline.</para>
    [Parameter(ValueFromPipeline = true, ParameterSetName = DefinitionSet)]
    public object[]? Definition { get; set; }

    /// <para>Topology chart provided directly.</para>
    [Parameter(ParameterSetName = DefinitionSet)]
    public TopologyChart? Chart { get; set; }

    /// <para>Topology nodes provided directly.</para>
    [Parameter(ParameterSetName = DefinitionSet)]
    public TopologyNode[] Node { get; set; } = Array.Empty<TopologyNode>();

    /// <para>Topology edges provided directly.</para>
    [Parameter(ParameterSetName = DefinitionSet)]
    public TopologyEdge[] Edge { get; set; } = Array.Empty<TopologyEdge>();

    /// <para>Topology groups provided directly.</para>
    [Parameter(ParameterSetName = DefinitionSet)]
    public TopologyGroup[] Group { get; set; } = Array.Empty<TopologyGroup>();

    /// <para>Topology scenarios provided directly.</para>
    [Parameter(ParameterSetName = DefinitionSet)]
    public TopologyScenario[] Scenario { get; set; } = Array.Empty<TopologyScenario>();

    /// <para>Script block that emits topology scenarios.</para>
    [Parameter]
    public ScriptBlock? ScenarioDefinition { get; set; }

    /// <para>Topology title.</para>
    [Parameter]
    public string Title { get; set; } = string.Empty;

    /// <para>Topology subtitle.</para>
    [Parameter]
    public string Subtitle { get; set; } = string.Empty;

    /// <para>Viewport width in pixels.</para>
    [Parameter]
    [ValidateRange(1, 10000)]
    public int Width { get; set; } = 1200;

    /// <para>Viewport height in pixels.</para>
    [Parameter]
    [ValidateRange(1, 10000)]
    public int Height { get; set; } = 700;

    /// <para>Viewport padding in pixels.</para>
    [Parameter]
    [ValidateRange(0, 1000)]
    public int Padding { get; set; } = 24;

    /// <para>Topology layout mode.</para>
    [Parameter]
    public TopologyLayoutMode Layout { get; set; } = TopologyLayoutMode.Layered;

    /// <para>Topology layout direction.</para>
    [Parameter]
    public TopologyLayoutDirection Direction { get; set; } = TopologyLayoutDirection.TopToBottom;

    /// <para>Node presentation mode.</para>
    [Parameter]
    public TopologyNodeDisplayMode NodeDisplayMode { get; set; } = TopologyNodeDisplayMode.Card;

    /// <para>Reusable visual style.</para>
    [Parameter]
    public TopologyVisualStyle VisualStyle { get; set; } = TopologyVisualStyle.Default;

    /// <para>Canvas surface style.</para>
    [Parameter]
    public TopologyCanvasSurfaceStyle CanvasSurfaceStyle { get; set; } = TopologyCanvasSurfaceStyle.Plain;

    /// <para>Topology theme name.</para>
    [Parameter]
    [ValidateSet("Light", "Dark")]
    public string Theme { get; set; } = "Light";

    /// <para>Use a transparent diagram canvas.</para>
    [Parameter]
    public SwitchParameter Transparent { get; set; }

    /// <para>Hide the topology title.</para>
    [Parameter]
    public SwitchParameter NoTitle { get; set; }

    /// <para>Hide the topology legend.</para>
    [Parameter]
    public SwitchParameter NoLegend { get; set; }

    /// <para>Hide topology groups.</para>
    [Parameter]
    public SwitchParameter NoGroups { get; set; }

    /// <para>Hide edge labels.</para>
    [Parameter]
    public SwitchParameter NoEdgeLabels { get; set; }

    /// <para>Hide status badges.</para>
    [Parameter]
    public SwitchParameter NoStatusBadges { get; set; }

    /// <para>Scale topology content down to remain inside the requested viewport.</para>
    [Parameter]
    public SwitchParameter FitContentToViewport { get; set; }

    /// <para>Enable lightweight interactions for HTML output.</para>
    [Parameter]
    public SwitchParameter InteractiveHtml { get; set; }

    /// <para>Scenario activated for static highlighting or when interactive HTML first loads.</para>
    [Parameter]
    public string ActiveScenarioId { get; set; } = string.Empty;

    /// <para>Script-free route motion used by SVG, GIF, APNG, and sampled PNG output.</para>
    [Parameter]
    public TopologyMotionOptions? Motion { get; set; }

    /// <para>Reusable topology spacing and presentation profile.</para>
    [Parameter]
    public TopologyLayoutPreset LayoutPreset { get; set; } = TopologyLayoutPreset.Automatic;

    /// <para>Draw a developer-oriented layout diagnostic overlay in SVG, HTML, or PNG output.</para>
    [Parameter]
    public SwitchParameter IncludeLayoutDiagnostics { get; set; }

    /// <para>Watermarks applied to static SVG, HTML, or PNG output.</para>
    [Parameter]
    public VisualWatermark[] Watermark { get; set; } = Array.Empty<VisualWatermark>();

    /// <para>PNG physical resolution metadata in dots per inch.</para>
    [Parameter]
    [ValidateRange(1D, 100000D)]
    public double Dpi { get; set; } = 96D;

    /// <para>Hide scenario controls in interactive HTML output.</para>
    [Parameter]
    public SwitchParameter NoScenarioControls { get; set; }

    /// <para>Scenario control presentation used by interactive HTML output.</para>
    [Parameter]
    public TopologyHtmlScenarioControlMode ScenarioControlMode { get; set; } = TopologyHtmlScenarioControlMode.Buttons;

    /// <para>Hide the compact scenario detail panel in interactive HTML output.</para>
    [Parameter]
    public SwitchParameter NoScenarioPanel { get; set; }

    /// <para>Synchronize the active scenario with the HTML page query string.</para>
    [Parameter]
    public SwitchParameter ScenarioUrlState { get; set; }

    /// <para>Output file path. The extension selects PNG, SVG, HTML, GIF, or APNG output.</para>
    [Parameter(Mandatory = true, ParameterSetName = ScriptBlockSet)]
    [Parameter(Mandatory = true, ParameterSetName = DefinitionSet)]
    public string FilePath { get; set; } = string.Empty;

    /// <para>Open the generated file after creation.</para>
    [Parameter]
    public SwitchParameter Show { get; set; }

    /// <para>Write the generated topology chart to the pipeline after rendering.</para>
    [Parameter]
    public SwitchParameter PassThru { get; set; }

    /// <inheritdoc />
    protected override void BeginProcessing() {
        if (Chart != null) {
            _chart = Chart;
        }

        AddDefinitions(Group);
        AddDefinitions(Node);
        AddDefinitions(Edge);
        AddDefinitions(Scenario);
    }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        AddDefinitions(Definition);
    }

    /// <inheritdoc />
    protected override void EndProcessing() {
        var output = Helpers.ResolvePath(FilePath);
        var extension = Path.GetExtension(output);
        ValidateExtension(extension, output);
        if (TopologyDefinition != null) {
            AddDefinitions(TopologyDefinition.Invoke());
        }
        if (ScenarioDefinition != null) {
            AddDefinitions(ScenarioDefinition.Invoke());
        }

        var chart = BuildChart();
        var options = BuildRenderOptions();
        var directory = Path.GetDirectoryName(output);
        if (!string.IsNullOrWhiteSpace(directory)) {
            Directory.CreateDirectory(directory!);
        }

        if (Watermark.Length > 0 && (extension.Equals(".gif", StringComparison.OrdinalIgnoreCase) || extension.Equals(".apng", StringComparison.OrdinalIgnoreCase))) {
            throw new PSArgumentException("Visual artifact watermarks are supported for static SVG, HTML, and PNG topology output.", nameof(Watermark));
        }

        if (extension.Equals(".svg", StringComparison.OrdinalIgnoreCase)) {
            SaveStaticArtifact(chart, output, options, StaticArtifactFormat.Svg);
        } else if (extension.Equals(".html", StringComparison.OrdinalIgnoreCase) || extension.Equals(".htm", StringComparison.OrdinalIgnoreCase)) {
            if (InteractiveHtml.IsPresent && Watermark.Length == 0) chart.SaveInteractiveHtml(output, options);
            else SaveStaticArtifact(chart, output, options, StaticArtifactFormat.Html);
        } else if (extension.Equals(".png", StringComparison.OrdinalIgnoreCase)) {
            SaveStaticArtifact(chart, output, options, StaticArtifactFormat.Png);
        } else if (extension.Equals(".gif", StringComparison.OrdinalIgnoreCase)) {
            chart.SaveGif(output, options);
        } else if (extension.Equals(".apng", StringComparison.OrdinalIgnoreCase)) {
            chart.SaveApng(output, options);
        }

        if (Show.IsPresent) {
            ImagePlayground.Helpers.Open(output, true);
        }
        if (PassThru.IsPresent) {
            WriteObject(chart);
        }
    }

    private void ValidateExtension(string extension, string output) {
        if (extension.Equals(".svg", StringComparison.OrdinalIgnoreCase) ||
            extension.Equals(".html", StringComparison.OrdinalIgnoreCase) ||
            extension.Equals(".htm", StringComparison.OrdinalIgnoreCase) ||
            extension.Equals(".png", StringComparison.OrdinalIgnoreCase) ||
            extension.Equals(".gif", StringComparison.OrdinalIgnoreCase) ||
            extension.Equals(".apng", StringComparison.OrdinalIgnoreCase)) return;
        var exception = new PSArgumentException("Topology output supports only .png, .svg, .html, .htm, .gif, or .apng file extensions.");
        ThrowTerminatingError(new ErrorRecord(exception, "NewImageTopologyUnsupportedExtension", ErrorCategory.InvalidArgument, output));
    }

    private void AddDefinitions(IEnumerable<PSObject>? items) {
        if (items == null) {
            return;
        }

        foreach (var item in items) {
            AddDefinition(item);
        }
    }

    private void AddDefinitions(IEnumerable<object>? items) {
        if (items == null) {
            return;
        }

        foreach (var item in items) {
            AddDefinition(item);
        }
    }

    private void AddDefinition(object? item) {
        var value = item is PSObject psObject ? psObject.BaseObject : item;
        switch (value) {
            case TopologyChart chart:
                _chart = chart;
                break;
            case TopologyGroup group:
                _groups.Add(group);
                break;
            case TopologyNode node:
                _nodes.Add(node);
                break;
            case TopologyEdge edge:
                _edges.Add(edge);
                break;
            case TopologyScenario scenario:
                _scenarios.Add(scenario);
                break;
        }
    }

    private TopologyChart BuildChart() {
        var chartWasProvided = _chart != null;
        var chart = _chart ?? TopologyChart.Create();
        if (IsParameterBound(nameof(Title))) {
            chart.Title = Title;
        } else if (string.IsNullOrWhiteSpace(chart.Title)) {
            chart.Title = null;
        }
        if (IsParameterBound(nameof(Subtitle))) {
            chart.Subtitle = Subtitle;
        }

        if (!chartWasProvided || IsParameterBound(nameof(Layout))) {
            chart.LayoutMode = Layout;
        }
        if (!chartWasProvided || IsParameterBound(nameof(Direction))) {
            chart.LayoutDirection = Direction;
        }
        if (!chartWasProvided || IsParameterBound(nameof(Width))) {
            chart.Viewport.Width = Width;
        }
        if (!chartWasProvided || IsParameterBound(nameof(Height))) {
            chart.Viewport.Height = Height;
        }
        if (!chartWasProvided || IsParameterBound(nameof(Padding))) {
            chart.Viewport.Padding = Padding;
        }
        if (!chartWasProvided || IsParameterBound(nameof(Theme))) {
            chart.Theme = CreateTheme();
        } else if (Transparent.IsPresent) {
            chart.Theme ??= TopologyTheme.Light();
            chart.Theme.Background = "#FFFFFF00";
        }

        foreach (var group in _groups) {
            chart.Groups.Add(group);
        }
        foreach (var node in _nodes) {
            chart.Nodes.Add(node);
        }
        foreach (var edge in _edges) {
            chart.Edges.Add(edge);
        }
        foreach (var scenario in _scenarios) {
            chart.Scenarios.Add(scenario);
        }

        if (chart.Nodes.Count == 0) {
            var exception = new PSArgumentException("At least one topology node must be specified.");
            ThrowTerminatingError(new ErrorRecord(exception, "NewImageTopologyMissingNodes", ErrorCategory.InvalidArgument, null));
        }

        return chart;
    }

    private TopologyRenderOptions BuildRenderOptions() {
        return new TopologyRenderOptions {
            IncludeTitle = !NoTitle.IsPresent,
            IncludeLegend = !NoLegend.IsPresent,
            IncludeGroups = !NoGroups.IsPresent,
            IncludeEdgeLabels = !NoEdgeLabels.IsPresent,
            IncludeStatusBadges = !NoStatusBadges.IsPresent,
            FitContentToViewport = FitContentToViewport.IsPresent,
            EnableHtmlInteractions = InteractiveHtml.IsPresent,
            EnableHtmlViewportControls = InteractiveHtml.IsPresent,
            EnableHtmlExportControls = InteractiveHtml.IsPresent,
            EnableHtmlScenarioControls = InteractiveHtml.IsPresent && !NoScenarioControls.IsPresent,
            HtmlScenarioControlMode = ScenarioControlMode,
            EnableHtmlScenarioPanel = InteractiveHtml.IsPresent && !NoScenarioPanel.IsPresent,
            EnableHtmlScenarioUrlState = InteractiveHtml.IsPresent && ScenarioUrlState.IsPresent,
            ActiveScenarioId = string.IsNullOrWhiteSpace(ActiveScenarioId) ? null : ActiveScenarioId,
            Motion = Motion,
            LayoutPreset = LayoutPreset,
            IncludeLayoutDiagnosticOverlay = IncludeLayoutDiagnostics.IsPresent,
            NodeDisplayMode = NodeDisplayMode,
            VisualStyle = VisualStyle,
            CanvasSurfaceStyle = Transparent.IsPresent ? TopologyCanvasSurfaceStyle.Plain : CanvasSurfaceStyle
        };
    }

    private void SaveStaticArtifact(TopologyChart chart, string output, TopologyRenderOptions topologyOptions, StaticArtifactFormat format) {
        var artifactOptions = new VisualArtifactRenderOptions { Topology = topologyOptions };
        foreach (VisualWatermark watermark in Watermark) {
            if (watermark == null) throw new PSArgumentException("Watermark cannot contain null entries.", nameof(Watermark));
            artifactOptions.Watermarks.Add(watermark);
        }
        if (MyInvocation.BoundParameters.ContainsKey(nameof(Dpi))) artifactOptions.Raster = new RasterImageOptions { Dpi = Dpi };
        VisualArtifact artifact = chart.ToVisualArtifact();
        if (format == StaticArtifactFormat.Svg) artifact.SaveSvg(output, artifactOptions);
        else if (format == StaticArtifactFormat.Html) artifact.SaveHtml(output, artifactOptions);
        else artifact.SavePng(output, artifactOptions);
    }

    private enum StaticArtifactFormat {
        Svg,
        Html,
        Png
    }

    private TopologyTheme CreateTheme() {
        var theme = Theme.Equals("Dark", StringComparison.OrdinalIgnoreCase)
            ? TopologyTheme.Dark()
            : TopologyTheme.Light();

        if (Transparent.IsPresent) {
            theme.Background = Theme.Equals("Dark", StringComparison.OrdinalIgnoreCase) ? "#0B112000" : "#FFFFFF00";
        }

        return theme;
    }

    private bool IsParameterBound(string parameterName) => MyInvocation.BoundParameters.ContainsKey(parameterName);
}

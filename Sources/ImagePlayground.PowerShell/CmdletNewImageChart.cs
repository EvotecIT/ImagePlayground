using System.Collections.Generic;
using System.Management.Automation;
using ChartForgeX.Core;
using ChartForgeX.Primitives;
using ChartForgeX.VisualArtifacts;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates an image chart from definitions.</summary>
/// <para>Use this cmdlet to render one or more chart definitions into a final image file.</para>
/// <example>
///   <summary>Create a simple bar chart and save it</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageChart -ChartsDefinition {
///     New-ImageChartBar -Name 'Q1' -Value 12,18,25 -Color CornflowerBlue
///     New-ImageChartBar -Name 'Q2' -Value 14,20,28 -Color Orange
/// } -FilePath chart.png -XTitle 'Month' -YTitle 'Revenue'</code>
///   <para>Builds chart definitions inside a script block and renders them into a PNG file.</para>
/// </example>
/// <example>
///   <summary>Create a line chart with annotations and preview it</summary>
///   <prefix>PS&gt; </prefix>
///   <code>$defs = @(
///     New-ImageChartLine -Name 'CPU' -Value 35,42,58,61,49 -Color LimeGreen -Smooth
/// )
/// $ann = @(
///     New-ImageChartAnnotation -X 3 -Y 61 -Text 'Peak' -Arrow
/// )
/// New-ImageChart -Definition $defs -Annotation $ann -FilePath cpu-usage.png -Theme Dark -ShowGrid -Show</code>
///   <para>Renders a themed line chart and overlays an annotation highlighting the peak value.</para>
/// </example>
/// <example>
///   <summary>Title, category labels, and stacked bars from CSV data</summary>
///   <prefix>PS&gt; </prefix>
///   <code>$builds = Import-Csv builds.csv   # Day, Passed, Failed
/// New-ImageChart -Title 'Builds by result' -Subtitle 'Week 41' -XLabel $builds.Day -FilePath builds.png -Options (New-ImageChartOptions -BarLayout Stacked -ShowLegend) {
///     New-ImageChartBar -Name Passed -Value $builds.Passed
///     New-ImageChartBar -Name Failed -Value $builds.Failed
/// }</code>
///   <para>Titles, category labels, and the bar layout are parameters, so the chart never needs to be modified through .NET methods.</para>
/// </example>
/// <example>
///   <summary>Bars and a target line on one chart, saved as WebP</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageChart -XLabel Mon, Tue, Wed, Thu, Fri -FilePath latency.webp {
///     New-ImageChartBar -Name Requests -Value 410, 380, 455, 520, 470
///     New-ImageChartLine -Name 'p95 (ms)' -Value 182, 175, 240, 310, 205 -Marker Circle -SecondaryAxis
/// } -SecondaryYTitle 'p95 (ms)'</code>
///   <para>Bar, line, area, scatter, and similar cartesian definitions can be mixed; markers and axes are set per series.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageChart", DefaultParameterSetName = ScriptBlockSet)]
[OutputType(typeof(Chart))]
public sealed class NewImageChartCmdlet : ImageCmdlet {
    private const string ScriptBlockSet = "ScriptBlock";
    private const string DefinitionSet = "Definition";
    private const string ChartSet = "Chart";
    private const string ChartScriptSet = "ChartScript";
    private readonly List<ChartDefinition> _definitions = new();
    private readonly List<ChartAnnotationDefinition> _annotations = new();
    private readonly List<Chart> _charts = new();

    /// <summary>ScriptBlock producing chart definitions.</summary>
    [Parameter(Position = 0, Mandatory = true, ParameterSetName = ScriptBlockSet)]
    public ScriptBlock? ChartsDefinition { get; set; }

    /// <summary>ScriptBlock that receives and configures a ChartForgeX chart.</summary>
    /// <para>The script block receives the chart as its first argument and can mutate it directly or return a replacement chart.</para>
    [Parameter(Mandatory = true, ParameterSetName = ChartScriptSet)]
    public ScriptBlock? ChartScript { get; set; }

    /// <summary>ChartForgeX chart object to render.</summary>
    [Parameter(ValueFromPipeline = true, Mandatory = true, ParameterSetName = ChartSet)]
    public Chart? Chart { get; set; }

    /// <summary>Typed definitions produced by New-ImageChart data cmdlets, supplied directly or through the pipeline.</summary>
    [Parameter(ValueFromPipeline = true, Mandatory = true, ParameterSetName = DefinitionSet)]
    public ChartDefinition[]? Definition { get; set; }

    /// <summary>ScriptBlock producing annotations.</summary>
    [Parameter]
    public ScriptBlock? AnnotationsDefinition { get; set; }

    /// <summary>Annotations for the chart.</summary>
    [Parameter]
    public object[]? Annotation { get; set; }

    /// <summary>Width of the chart in pixels.</summary>
    [Parameter]
    [ValidateRange(1, 10000)]
    public int Width { get; set; } = 600;

    /// <summary>Height of the chart in pixels.</summary>
    [Parameter]
    [ValidateRange(1, 10000)]
    public int Height { get; set; } = 400;

    /// <summary>Chart heading. A supplied native chart keeps its heading unless this parameter is supplied.</summary>
    [Parameter]
    [AllowEmptyString]
    public string Title { get; set; } = string.Empty;

    /// <summary>Supporting chart heading. Supply an empty string to clear a supplied chart's subtitle.</summary>
    [Parameter]
    [AllowEmptyString]
    public string Subtitle { get; set; } = string.Empty;

    /// <summary>X axis title.</summary>
    [Parameter]
    public string? XTitle { get; set; }

    /// <summary>Y axis title.</summary>
    [Parameter]
    public string? YTitle { get; set; }

    /// <summary>Category labels along the X axis, one per value position (for example days or months).</summary>
    /// <para>Values at positions 1, 2, 3... of each series are labelled with these entries in order.</para>
    [Parameter]
    [Alias("Category", "Categories", "XLabels")]
    public string[]? XLabel { get; set; }

    /// <summary>Title of the secondary (right) Y axis used by series created with -SecondaryAxis.</summary>
    [Parameter]
    [Alias("Y2Title")]
    public string? SecondaryYTitle { get; set; }

    /// <summary>Output file path.</summary>
    /// <para>The extension picks the format: .png, .svg, .html, or .htm use the ChartForgeX renderers; .jpg, .jpeg, .webp, .gif, .bmp, .tif, .tiff, and .tga encode the same pixels through ImagePlayground. Relative paths resolve against the current PowerShell location.</para>
    [Parameter(Mandatory = true, ParameterSetName = ScriptBlockSet)]
    [Parameter(Mandatory = true, ParameterSetName = ChartScriptSet)]
    [Parameter(Mandatory = true, ParameterSetName = ChartSet)]
    [Parameter(Mandatory = true, ParameterSetName = DefinitionSet)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Open the image after creation.</summary>
    [Parameter]
    public SwitchParameter Show { get; set; }

    /// <summary>Display grid lines.</summary>
    [Parameter]
    public SwitchParameter ShowGrid { get; set; }

    /// <summary>Chart theme.</summary>
    [Parameter]
    public ChartTheme Theme { get; set; } = ChartTheme.Default;

    /// <summary>Chart background color.</summary>
    [Parameter]
    [ChartColorArgumentTransformation]
    public ChartColor? Background { get; set; }

    /// <summary>Renderer options created by New-ImageChartOptions.</summary>
    [Parameter]
    public ChartRenderOptions? Options { get; set; }

    private VisualWatermark[] _watermark = System.Array.Empty<VisualWatermark>();

    /// <summary>Watermarks applied to static SVG, HTML, or PNG output.</summary>
    [Parameter]
    [AllowNull]
    public VisualWatermark[] Watermark { get => _watermark; set => _watermark = VisualWatermarkParameter.Normalize(value); }

    /// <summary>PNG physical resolution metadata in dots per inch.</summary>
    [Parameter]
    [ValidateRange(1D, 100000D)]
    public double Dpi { get; set; } = 96D;

    /// <summary>Write the ChartForgeX chart to the pipeline after rendering.</summary>
    [Parameter]
    public SwitchParameter PassThru { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        if (Definition is not null) {
            AddDefinitions(Definition);
        }

        if (Annotation is not null) {
            AddAnnotations(Annotation);
        }

        if (Chart is not null) {
            _charts.Add(Chart);
        }
    }

    /// <inheritdoc />
    protected override void EndProcessing() {
        AddAnnotationsFromScriptBlock();

        if (ChartScript != null) {
            var chart = Charts.Create(Width, Height, XTitle, YTitle, ShowGrid.IsPresent, Theme, Background, Options);
            var results = ChartScript.Invoke(chart);
            foreach (var o in results) {
                var obj = o is PSObject ps ? ps.BaseObject : o;
                if (obj is Chart returnedChart) {
                    chart = returnedChart;
                    break;
                }
            }

            ApplyChartSettings(chart);
            ApplyAnnotations(chart);
            SaveChart(chart);
            return;
        }

        if (_charts.Count > 0) {
            if (_charts.Count > 1) {
                var exception = new PSArgumentException("New-ImageChart accepts one ChartForgeX chart when -FilePath is a single output path.");
                ThrowTerminatingError(new ErrorRecord(exception, "NewImageChartMultipleCharts", ErrorCategory.InvalidArgument, null));
                return;
            }

            var chart = _charts[0];
            ApplyChartSettings(chart);
            ApplyAnnotations(chart);
            SaveChart(chart);
            return;
        }

        if (ChartsDefinition != null) {
            var results = ChartsDefinition.Invoke();
            foreach (var o in results) {
                var obj = o is PSObject ps ? ps.BaseObject : o;
                if (obj is ChartDefinition def) {
                    _definitions.Add(def);
                }
            }
        }

        if (_definitions.Count == 0) {
            var exception = new PSArgumentException("At least one chart definition must be specified.");
            ThrowTerminatingError(new ErrorRecord(exception, "NewImageChartMissingDefinitions", ErrorCategory.InvalidArgument, null));
            return;
        }

        var built = Charts.Build(_definitions, Width, Height, XTitle, YTitle, ShowGrid.IsPresent, Theme, _annotations, Background, Options);
        SaveChart(built);
    }

    private void SaveChart(Chart chart) {
        if (MyInvocation.BoundParameters.ContainsKey(nameof(Title))) {
            chart.WithTitle(Title);
        }
        if (MyInvocation.BoundParameters.ContainsKey(nameof(Subtitle))) {
            chart.WithSubtitle(Subtitle);
        }
        if (XLabel != null && XLabel.Length > 0) {
            chart.WithXLabels(XLabel);
        }
        if (SecondaryYTitle != null) {
            chart.WithSecondaryYAxis(SecondaryYTitle);
        }
        VisualWatermark[] watermarks = Watermark ?? System.Array.Empty<VisualWatermark>();
        var output = PowerShellPathResolver.ResolveFileSystemPath(this, FilePath);
        var outputExtension = System.IO.Path.GetExtension(output);
        if (!VisualArtifactOutput.IsSupported(outputExtension)) {
            throw new PSArgumentException("Chart output supports " + VisualArtifactOutput.SupportedExtensions + " file extensions.", nameof(FilePath));
        }

        string? directory = System.IO.Path.GetDirectoryName(output);
        if (!string.IsNullOrWhiteSpace(directory)) {
            System.IO.Directory.CreateDirectory(directory!);
        }

        if (watermarks.Length == 0 && !MyInvocation.BoundParameters.ContainsKey(nameof(Dpi)) && VisualArtifactOutput.IsNative(outputExtension)) {
            Charts.Save(chart, output);
        } else {
            var render = new VisualArtifactRenderOptions();
            foreach (VisualWatermark watermark in watermarks) {
                if (watermark == null) {
                    throw new PSArgumentException("Watermark cannot contain null entries.", nameof(Watermark));
                }
            }
            if (MyInvocation.BoundParameters.ContainsKey(nameof(Dpi))) {
                render.Raster = new RasterImageOptions { Dpi = Dpi };
            }

            VisualArtifact artifact = chart.ToVisualArtifact();
            if (watermarks.Length > 0) {
                artifact = artifact.ToWatermarkedArtifact(render, watermarks);
            }
            VisualArtifactOutput.Save(artifact, output, render);
        }

        if (Show.IsPresent) {
            ImagePlayground.Helpers.Open(output, true);
        }
        if (PassThru.IsPresent) {
            WriteObject(chart);
        }
    }

    private void AddAnnotationsFromScriptBlock() {
        if (AnnotationsDefinition == null) {
            return;
        }

        var results = AnnotationsDefinition.Invoke();
        foreach (var o in results) {
            var obj = o is PSObject ps ? ps.BaseObject : o;
            if (obj is ChartAnnotationDefinition annotation) {
                _annotations.Add(annotation);
            }
        }
    }

    private void ApplyChartSettings(Chart chart) {
        if (MyInvocation.BoundParameters.ContainsKey(nameof(Width)) || MyInvocation.BoundParameters.ContainsKey(nameof(Height))) {
            var size = chart.Options.Size;
            var width = MyInvocation.BoundParameters.ContainsKey(nameof(Width)) ? Width : size.Width;
            var height = MyInvocation.BoundParameters.ContainsKey(nameof(Height)) ? Height : size.Height;
            chart.WithSize(width, height);
        }

        var showGrid = MyInvocation.BoundParameters.ContainsKey(nameof(ShowGrid)) ? ShowGrid.IsPresent : (bool?)null;
        var theme = MyInvocation.BoundParameters.ContainsKey(nameof(Theme)) ? Theme : (ChartTheme?)null;
        Charts.ApplySettings(chart, XTitle, YTitle, Background, Options, showGrid, theme);
    }

    private void ApplyAnnotations(Chart chart) {
        if (_annotations.Count == 0) {
            return;
        }

        foreach (var annotation in _annotations) {
            if (annotation.Arrow) {
                if (Charts.HasExclusiveSeries(chart)) {
                    var exception = new PSArgumentException("Point-callout annotations cannot be used with exclusive chart kinds because they add a scatter series.");
                    ThrowTerminatingError(new ErrorRecord(exception, "NewImageChartAnnotationUnsupported", ErrorCategory.InvalidArgument, annotation));
                    return;
                }

                chart.AddPointCallout(annotation.Text, annotation.X, annotation.Y);
            } else {
                chart.AddVerticalLine(annotation.X, annotation.Text);
            }
        }
    }

    private void AddDefinitions(IEnumerable<object> definitions) {
        foreach (var item in definitions) {
            var obj = item is PSObject ps ? ps.BaseObject : item;
            if (obj is ChartDefinition definition) {
                _definitions.Add(definition);
                continue;
            }

            var exception = new PSArgumentException("Definition must contain objects produced by New-ImageChart* data cmdlets or use -Chart with a native ChartForgeX.Core.Chart.");
            ThrowTerminatingError(new ErrorRecord(exception, "NewImageChartInvalidDefinition", ErrorCategory.InvalidArgument, obj));
        }
    }

    private void AddAnnotations(IEnumerable<object> annotations) {
        foreach (var item in annotations) {
            var obj = item is PSObject ps ? ps.BaseObject : item;
            if (obj is ChartAnnotationDefinition annotation) {
                _annotations.Add(annotation);
                continue;
            }

            var exception = new PSArgumentException("Annotation must contain objects produced by New-ImageChartAnnotation.");
            ThrowTerminatingError(new ErrorRecord(exception, "NewImageChartInvalidAnnotation", ErrorCategory.InvalidArgument, obj));
        }
    }
}

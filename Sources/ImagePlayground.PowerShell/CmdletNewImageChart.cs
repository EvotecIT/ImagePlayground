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

    /// <summary>Chart definitions provided directly.</summary>
    [Parameter(ValueFromPipeline = true, Mandatory = true, ParameterSetName = DefinitionSet)]
    public object[]? Definition { get; set; }

    /// <summary>ScriptBlock producing annotations.</summary>
    [Parameter]
    public ScriptBlock? AnnotationsDefinition { get; set; }

    /// <summary>Annotations for the chart.</summary>
    [Parameter]
    public object[]? Annotation { get; set; }

    /// <summary>Width of the chart.</summary>
    [Parameter]
    [ValidateRange(1, 1000)]
    public int Width { get; set; } = 600;

    /// <summary>Height of the chart.</summary>
    [Parameter]
    [ValidateRange(1, 1000)]
    public int Height { get; set; } = 400;

    /// <summary>X axis title.</summary>
    [Parameter]
    public string? XTitle { get; set; }

    /// <summary>Y axis title.</summary>
    [Parameter]
    public string? YTitle { get; set; }

    /// <summary>Output file path.</summary>
    /// <para>The image format is inferred from the file extension.</para>
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
        VisualWatermark[] watermarks = Watermark ?? System.Array.Empty<VisualWatermark>();
        var output = Helpers.ResolvePath(FilePath);
        string? directory = System.IO.Path.GetDirectoryName(output);
        if (!string.IsNullOrWhiteSpace(directory)) {
            System.IO.Directory.CreateDirectory(directory!);
        }

        if (watermarks.Length == 0 && !MyInvocation.BoundParameters.ContainsKey(nameof(Dpi))) {
            Charts.Save(chart, output);
        } else {
            var render = new VisualArtifactRenderOptions();
            foreach (VisualWatermark watermark in watermarks) {
                if (watermark == null) {
                    throw new PSArgumentException("Watermark cannot contain null entries.", nameof(Watermark));
                }

                render.Watermarks.Add(watermark);
            }
            if (MyInvocation.BoundParameters.ContainsKey(nameof(Dpi))) {
                render.Raster = new RasterImageOptions { Dpi = Dpi };
            }

            VisualArtifact artifact = chart.ToVisualArtifact();
            var extension = System.IO.Path.GetExtension(output);
            if (extension.Equals(".svg", System.StringComparison.OrdinalIgnoreCase)) {
                artifact.SaveSvg(output, render);
            } else if (extension.Equals(".html", System.StringComparison.OrdinalIgnoreCase) || extension.Equals(".htm", System.StringComparison.OrdinalIgnoreCase)) {
                artifact.SaveHtml(output, render);
            } else if (extension.Equals(".png", System.StringComparison.OrdinalIgnoreCase)) {
                artifact.SavePng(output, render);
            } else {
                throw new PSArgumentException("Watermarked chart output supports only .png, .svg, .html, or .htm file extensions.", nameof(FilePath));
            }
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

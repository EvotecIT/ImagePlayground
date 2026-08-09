using ChartForgeX;
using ChartForgeX.Core;
using ChartForgeX.Motion;
using ChartForgeX.Primitives;
using ChartForgeX.VisualBlocks;
using ImagePlayground;
using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a reusable dashboard grid from charts and visual blocks.</summary>
/// <para>Render the grid directly, pass it to <c>New-ImageVisualStory</c>, or compose it inside a ChartForgeX visual canvas.</para>
/// <example>
///   <summary>Create a status dashboard</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageVisualGrid -Title 'Service health' -Columns 2 -ContentDefinition { New-ImageMetricCard -Label 'Requests' -Value 12840 -Status Positive; New-ImageListBlock -Title 'Checks' -Item API,Database -Status Positive,Warning } -FilePath dashboard.svg</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageVisualGrid", DefaultParameterSetName = DefinitionSet)]
[OutputType(typeof(VisualGrid))]
public sealed class NewImageVisualGridCmdlet : PSCmdlet {
    private const string DefinitionSet = "Definition";
    private const string ContentSet = "Content";
    private readonly List<object> _content = new();

    /// <summary>Script block that emits charts, visual blocks, or <c>New-ImageVisualGridItem</c> results.</summary>
    [Parameter(Mandatory = true, Position = 0, ParameterSetName = DefinitionSet)]
    public ScriptBlock? ContentDefinition { get; set; }

    /// <summary>Charts, visual blocks, or grid items supplied directly or through the pipeline.</summary>
    [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = ContentSet)]
    public object[] Content { get; set; } = Array.Empty<object>();

    /// <summary>Optional dashboard title.</summary>
    [Parameter]
    public string Title { get; set; } = string.Empty;

    /// <summary>Optional dashboard subtitle.</summary>
    [Parameter]
    public string Subtitle { get; set; } = string.Empty;

    /// <summary>Preferred column count.</summary>
    [Parameter]
    [ValidateRange(1, 100)]
    public int Columns { get; set; } = 2;

    /// <summary>Gap between panels in pixels.</summary>
    [Parameter]
    [ValidateRange(0, 1000)]
    public int Gap { get; set; } = 18;

    /// <summary>Outer grid padding in pixels.</summary>
    [Parameter]
    [ValidateRange(0, 1000)]
    public int Padding { get; set; } = 24;

    /// <summary>Optional fixed panel width.</summary>
    [Parameter]
    [ValidateRange(1, 10000)]
    public int PanelWidth { get; set; }

    /// <summary>Optional fixed panel height.</summary>
    [Parameter]
    [ValidateRange(1, 10000)]
    public int PanelHeight { get; set; }

    /// <summary>How child content fits fixed panels.</summary>
    [Parameter]
    public VisualPanelFit PanelFit { get; set; } = VisualPanelFit.Contain;

    /// <summary>Use each row's natural item height.</summary>
    [Parameter]
    public SwitchParameter AdaptiveRowHeights { get; set; }

    /// <summary>Render a subtle outer frame.</summary>
    [Parameter]
    public SwitchParameter Frame { get; set; }

    /// <summary>ChartForgeX theme.</summary>
    [Parameter]
    public ChartTheme Theme { get; set; } = ChartTheme.Default;

    /// <summary>Optional script-free SVG and HTML motion timeline.</summary>
    [Parameter]
    public VisualMotionTimeline? Motion { get; set; }

    /// <summary>Optional output path. Omit it to return the grid without rendering.</summary>
    [Parameter]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Open the rendered dashboard.</summary>
    [Parameter]
    public SwitchParameter Show { get; set; }

    /// <summary>Return the grid when an output file is also written.</summary>
    [Parameter]
    public SwitchParameter PassThru { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        foreach (var item in Content) AddContent(item);
    }

    /// <inheritdoc />
    protected override void EndProcessing() {
        string? output = null;
        if (!string.IsNullOrWhiteSpace(FilePath)) {
            output = PowerShellPathResolver.ResolveFileSystemPath(this, FilePath);
            ValidateExtension(Path.GetExtension(output), output);
            PowerShellPathResolver.ValidateFileDestination(output, nameof(FilePath), nameof(FilePath));
        }
        if (ContentDefinition != null) {
            foreach (var result in ContentDefinition.Invoke()) AddContent(result);
        }
        if (_content.Count == 0) {
            var exception = new PSArgumentException("A visual grid requires at least one chart or visual block.");
            ThrowTerminatingError(new ErrorRecord(exception, "NewImageVisualGridMissingContent", ErrorCategory.InvalidArgument, null));
        }
        var grid = VisualGrid.Create()
            .WithColumns(Columns)
            .WithGap(Gap)
            .WithPadding(Padding)
            .WithPanelFit(PanelFit)
            .WithAdaptiveRowHeights(AdaptiveRowHeights.IsPresent)
            .WithFrame(Frame.IsPresent)
            .WithTheme(ChartThemeResolver.Resolve(Theme));
        if (!string.IsNullOrWhiteSpace(Title)) grid.WithTitle(Title);
        if (!string.IsNullOrWhiteSpace(Subtitle)) grid.WithSubtitle(Subtitle);
        if (MyInvocation.BoundParameters.ContainsKey(nameof(PanelWidth)) != MyInvocation.BoundParameters.ContainsKey(nameof(PanelHeight))) {
            var exception = new PSArgumentException("PanelWidth and PanelHeight must be supplied together.");
            ThrowTerminatingError(new ErrorRecord(exception, "NewImageVisualGridIncompletePanelSize", ErrorCategory.InvalidArgument, null));
        }
        if (MyInvocation.BoundParameters.ContainsKey(nameof(PanelWidth))) grid.WithPanelSize(PanelWidth, PanelHeight);
        if (Motion != null) grid.WithMotion(Motion);
        foreach (var item in _content) AddToGrid(grid, item);
        if (output != null) {
            var directory = Path.GetDirectoryName(output);
            if (!string.IsNullOrWhiteSpace(directory)) Directory.CreateDirectory(directory!);
            grid.Save(output);
            if (Show.IsPresent) ImagePlayground.Helpers.Open(output, true);
        }
        if (output == null || PassThru.IsPresent) WriteObject(grid);
    }

    private void ValidateExtension(string extension, string output) {
        switch (extension.ToLowerInvariant()) {
            case ".svg":
            case ".html":
            case ".htm":
            case ".png":
            case ".jpg":
            case ".jpeg":
            case ".bmp":
            case ".ppm":
            case ".tif":
            case ".tiff":
                return;
            default:
                var exception = new PSArgumentException("Visual grid output supports .svg, .html, .htm, .png, .jpg, .jpeg, .bmp, .ppm, .tif, or .tiff file extensions.");
                ThrowTerminatingError(new ErrorRecord(exception, "NewImageVisualGridUnsupportedExtension", ErrorCategory.InvalidArgument, output));
                return;
        }
    }

    private void AddContent(object? input) {
        var value = input is PSObject psObject ? psObject.BaseObject : input;
        if (value is VisualGridItem || value is Chart || value is IVisualBlock) {
            _content.Add(value);
        } else if (value != null) {
            var exception = new PSArgumentException("Visual grid content must contain ChartForgeX charts, visual blocks, or New-ImageVisualGridItem results.");
            ThrowTerminatingError(new ErrorRecord(exception, "NewImageVisualGridUnsupportedContent", ErrorCategory.InvalidArgument, value));
        }
    }

    private static void AddToGrid(VisualGrid grid, object item) {
        if (item is Chart chart) grid.Add(chart);
        else if (item is IVisualBlock block) grid.Add(block);
        else if (item is VisualGridItem placement) {
            if (placement.Chart != null) {
                if (string.IsNullOrWhiteSpace(placement.MotionTargetId)) grid.Add(placement.Chart, placement.ColumnSpan, placement.RowSpan);
                else grid.Add(placement.MotionTargetId!, placement.Chart, placement.ColumnSpan, placement.RowSpan);
            } else if (string.IsNullOrWhiteSpace(placement.MotionTargetId)) grid.Add(placement.Block!, placement.ColumnSpan, placement.RowSpan);
            else grid.Add(placement.MotionTargetId!, placement.Block!, placement.ColumnSpan, placement.RowSpan);
        }
    }
}

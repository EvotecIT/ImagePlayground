using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using ChartForgeX;
using ChartForgeX.Motion;
using ChartForgeX.VisualBlocks;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a script-free animated visual story from a ChartForgeX visual grid.</summary>
/// <para>Use a native ChartForgeX VisualGrid or configure one in StoryScript. SVG and HTML preserve motion, while PNG renders the completed static state.</para>
/// <para>A VisualMotionPresentation returned by New-ImageVisualGrid -Motion can also be piped directly into this command.</para>
/// <example>
///   <summary>Create an animated engineering profile card</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageVisualStory -StoryScript {
///   param($Story)
///   [void] $Story.WithTitle('Engineering signal').WithColumns(1)
///   $metric = [ChartForgeX.VisualBlocks.MetricCard]::Create().WithMetric('Projects', '126')
///   [void] $Story.Add('projects', $metric)
/// } -MotionDefinition {
///   New-ImageVisualMotionCue -TargetId title -Effect Reveal -DurationSeconds 0.9
///   New-ImageVisualMotionCue -TargetId projects -Effect Rise -DelaySeconds 0.3
/// } -FilePath profile.svg</code>
///   <para>Builds a dependency-free SVG whose one-shot motion honors reduced-motion preferences.</para>
/// </example>
/// <example>
///   <summary>Export a detached motion grid through the pipeline</summary>
///   <prefix>PS&gt; </prefix>
///   <code>$motion = [ChartForgeX.Motion.VisualMotionTimeline]::Create().Rise('requests')
/// New-ImageVisualGrid -ContentDefinition {
///   New-ImageVisualGridItem -TargetId requests -Block (New-ImageMetricCard -Label Requests -Value 12840)
/// } -Motion $motion | New-ImageVisualStory -FilePath requests.svg</code>
///   <para>Reuses the typed presentation captured by the grid command without changing its source grid or timeline.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageVisualStory", DefaultParameterSetName = StoryScriptSet)]
[OutputType(typeof(VisualGrid), typeof(VisualMotionPresentation))]
public sealed class NewImageVisualStoryCmdlet : PSCmdlet {
    private const string StoryScriptSet = "StoryScript";
    private const string GridSet = "Grid";
    private const string PresentationSet = "Presentation";
    private readonly List<VisualGrid> _grids = new();
    private readonly List<VisualMotionPresentation> _presentations = new();

    /// <summary>Script block that receives and configures a new ChartForgeX VisualGrid.</summary>
    [Parameter(Mandatory = true, Position = 0, ParameterSetName = StoryScriptSet)]
    public ScriptBlock? StoryScript { get; set; }

    /// <summary>Native ChartForgeX visual grid to render.</summary>
    [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = GridSet)]
    public VisualGrid? Grid { get; set; }

    /// <summary>Detached motion presentation to export, including output from New-ImageVisualGrid -Motion.</summary>
    [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = PresentationSet)]
    public VisualMotionPresentation? Presentation { get; set; }

    /// <summary>Ready-to-use ChartForgeX motion timeline.</summary>
    [Parameter(ParameterSetName = StoryScriptSet)]
    [Parameter(ParameterSetName = GridSet)]
    public VisualMotionTimeline? Motion { get; set; }

    /// <summary>Script block that emits New-ImageVisualMotionCue results or one VisualMotionTimeline.</summary>
    [Parameter(ParameterSetName = StoryScriptSet)]
    [Parameter(ParameterSetName = GridSet)]
    public ScriptBlock? MotionDefinition { get; set; }

    /// <summary>Output file path. Supported extensions are SVG, HTML, HTM, and PNG.</summary>
    [Parameter(Mandatory = true)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Open the generated visual after creation.</summary>
    [Parameter]
    public SwitchParameter Show { get; set; }

    /// <summary>Write the detached motion presentation, or the static grid when no motion is supplied.</summary>
    [Parameter]
    public SwitchParameter PassThru { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        if (Grid != null) {
            _grids.Add(Grid);
        }
        if (Presentation != null) {
            _presentations.Add(Presentation);
        }
    }

    /// <inheritdoc />
    protected override void EndProcessing() {
        ValidateMotionSources();
        var output = PowerShellPathResolver.ResolveFileSystemPath(this, FilePath);
        var extension = Path.GetExtension(output);
        ValidateExtension(extension, output);
        PowerShellPathResolver.ValidateFileDestination(output, nameof(FilePath), nameof(FilePath));
        VisualGrid? grid = null;
        VisualMotionPresentation? presentation;
        if (ParameterSetName == PresentationSet) {
            if (_presentations.Count != 1) {
                throw new PSArgumentException("New-ImageVisualStory accepts one VisualMotionPresentation per output path.", nameof(Presentation));
            }
            presentation = _presentations[0];
        } else {
            ValidateGridInput();
            var motion = BuildMotion();
            grid = BuildGrid();
            presentation = motion == null ? null : VisualMotionPresentation.Create(grid, motion);
        }

        var directory = Path.GetDirectoryName(output);
        if (!string.IsNullOrWhiteSpace(directory)) {
            Directory.CreateDirectory(directory!);
        }

        if (extension.Equals(".svg", StringComparison.OrdinalIgnoreCase)) {
            if (presentation == null) grid!.SaveSvg(output);
            else File.WriteAllText(output, presentation.ToSvg());
        } else if (extension.Equals(".html", StringComparison.OrdinalIgnoreCase) || extension.Equals(".htm", StringComparison.OrdinalIgnoreCase)) {
            if (presentation == null) grid!.SaveHtml(output);
            else File.WriteAllText(output, presentation.ToHtmlPage());
        } else {
            if (presentation == null) grid!.SavePng(output);
            else OfficeIMO.Drawing.OfficeImageFileWriter.WriteAllBytes(output, presentation.ToPng());
        }

        if (Show.IsPresent) {
            ImagePlayground.Helpers.Open(output, true);
        }
        if (PassThru.IsPresent) {
            WriteObject(presentation == null ? (object)grid! : presentation);
        }
    }

    private void ValidateMotionSources() {
        if (Motion == null || MotionDefinition == null) {
            return;
        }

        var exception = new PSArgumentException("Use either Motion or MotionDefinition, not both.");
        ThrowTerminatingError(new ErrorRecord(exception, "NewImageVisualStoryMultipleMotionSources", ErrorCategory.InvalidArgument, null));
    }

    private void ValidateGridInput() {
        if (StoryScript != null) {
            return;
        }
        if (_grids.Count == 0) {
            var exception = new PSArgumentException("New-ImageVisualStory requires one ChartForgeX VisualGrid.");
            ThrowTerminatingError(new ErrorRecord(exception, "NewImageVisualStoryMissingGrid", ErrorCategory.InvalidArgument, null));
        }
        if (_grids.Count > 1) {
            var exception = new PSArgumentException("New-ImageVisualStory accepts one ChartForgeX VisualGrid per output path.");
            ThrowTerminatingError(new ErrorRecord(exception, "NewImageVisualStoryMultipleGrids", ErrorCategory.InvalidArgument, null));
        }
    }

    private VisualGrid BuildGrid() {
        if (StoryScript != null) {
            var grid = VisualGrid.Create();
            foreach (var result in StoryScript.Invoke(grid)) {
                var value = result is PSObject psObject ? psObject.BaseObject : result;
                if (value is VisualGrid returnedGrid) {
                    grid = returnedGrid;
                }
            }

            return grid;
        }

        return _grids[0];
    }

    private VisualMotionTimeline? BuildMotion() {
        if (Motion != null || MotionDefinition == null) {
            return Motion;
        }

        VisualMotionTimeline? returnedTimeline = null;
        var returnedTimelineCount = 0;
        var cues = new List<VisualMotionCue>();
        foreach (var result in MotionDefinition.Invoke()) {
            var value = result is PSObject psObject ? psObject.BaseObject : result;
            if (value is VisualMotionTimeline timeline) {
                returnedTimeline = timeline;
                returnedTimelineCount++;
            } else if (value is VisualMotionCue cue) {
                cues.Add(cue);
            } else if (value != null) {
                var exception = new PSArgumentException(
                    $"MotionDefinition emitted unsupported output of type '{value.GetType().FullName}'. Emit only VisualMotionCue objects or one VisualMotionTimeline.");
                ThrowTerminatingError(new ErrorRecord(exception, "NewImageVisualStoryUnsupportedMotionOutput", ErrorCategory.InvalidArgument, value));
            }
        }

        if (returnedTimelineCount > 1) {
            var exception = new PSArgumentException("MotionDefinition must emit at most one VisualMotionTimeline.");
            ThrowTerminatingError(new ErrorRecord(exception, "NewImageVisualStoryMultipleMotionTimelines", ErrorCategory.InvalidArgument, null));
        }
        if (returnedTimeline != null && cues.Count > 0) {
            var exception = new PSArgumentException("MotionDefinition must emit either one VisualMotionTimeline or motion cues, not both.");
            ThrowTerminatingError(new ErrorRecord(exception, "NewImageVisualStoryMixedMotionDefinition", ErrorCategory.InvalidArgument, null));
        }
        if (returnedTimeline != null) {
            return returnedTimeline;
        }
        if (cues.Count == 0) {
            var exception = new PSArgumentException("MotionDefinition must emit at least one object from New-ImageVisualMotionCue.");
            ThrowTerminatingError(new ErrorRecord(exception, "NewImageVisualStoryMissingMotionCues", ErrorCategory.InvalidArgument, null));
        }

        var motion = VisualMotionTimeline.Create();
        foreach (var cue in cues) {
            motion.Add(cue);
        }
        return motion;
    }

    private void ValidateExtension(string extension, string output) {
        if (extension.Equals(".svg", StringComparison.OrdinalIgnoreCase) ||
            extension.Equals(".html", StringComparison.OrdinalIgnoreCase) ||
            extension.Equals(".htm", StringComparison.OrdinalIgnoreCase) ||
            extension.Equals(".png", StringComparison.OrdinalIgnoreCase)) {
            return;
        }

        var exception = new PSArgumentException("Visual story output supports only .svg, .html, .htm, or .png file extensions.");
        ThrowTerminatingError(new ErrorRecord(exception, "NewImageVisualStoryUnsupportedExtension", ErrorCategory.InvalidArgument, output));
    }
}

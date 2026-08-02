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
[Cmdlet(VerbsCommon.New, "ImageVisualStory", DefaultParameterSetName = StoryScriptSet)]
[OutputType(typeof(VisualGrid))]
public sealed class NewImageVisualStoryCmdlet : PSCmdlet {
    private const string StoryScriptSet = "StoryScript";
    private const string GridSet = "Grid";
    private readonly List<VisualGrid> _grids = new();

    /// <summary>Script block that receives and configures a new ChartForgeX VisualGrid.</summary>
    [Parameter(Mandatory = true, Position = 0, ParameterSetName = StoryScriptSet)]
    public ScriptBlock? StoryScript { get; set; }

    /// <summary>Native ChartForgeX visual grid to render.</summary>
    [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = GridSet)]
    public VisualGrid? Grid { get; set; }

    /// <summary>Ready-to-use ChartForgeX motion timeline.</summary>
    [Parameter]
    public VisualMotionTimeline? Motion { get; set; }

    /// <summary>Script block that emits New-ImageVisualMotionCue results or one VisualMotionTimeline.</summary>
    [Parameter]
    public ScriptBlock? MotionDefinition { get; set; }

    /// <summary>Output file path. Supported extensions are SVG, HTML, HTM, and PNG.</summary>
    [Parameter(Mandatory = true)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Open the generated visual after creation.</summary>
    [Parameter]
    public SwitchParameter Show { get; set; }

    /// <summary>Write the configured ChartForgeX VisualGrid to the pipeline.</summary>
    [Parameter]
    public SwitchParameter PassThru { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        if (Grid != null) {
            _grids.Add(Grid);
        }
    }

    /// <inheritdoc />
    protected override void EndProcessing() {
        ValidateMotionSources();
        var output = PowerShellPathResolver.ResolveFileSystemPath(this, FilePath);
        var extension = Path.GetExtension(output);
        ValidateExtension(extension, output);
        PowerShellPathResolver.ValidateFileDestination(output, nameof(FilePath), nameof(FilePath));

        var motion = BuildMotion();
        var grid = BuildGrid();
        if (motion != null) {
            grid.WithMotion(motion);
        }

        var directory = Path.GetDirectoryName(output);
        if (!string.IsNullOrWhiteSpace(directory)) {
            Directory.CreateDirectory(directory!);
        }

        if (extension.Equals(".svg", StringComparison.OrdinalIgnoreCase)) {
            grid.SaveSvg(output);
        } else if (extension.Equals(".html", StringComparison.OrdinalIgnoreCase) || extension.Equals(".htm", StringComparison.OrdinalIgnoreCase)) {
            grid.SaveHtml(output);
        } else {
            grid.SavePng(output);
        }

        if (Show.IsPresent) {
            ImagePlayground.Helpers.Open(output, true);
        }
        if (PassThru.IsPresent) {
            WriteObject(grid);
        }
    }

    private void ValidateMotionSources() {
        if (Motion == null || MotionDefinition == null) {
            return;
        }

        var exception = new PSArgumentException("Use either Motion or MotionDefinition, not both.");
        ThrowTerminatingError(new ErrorRecord(exception, "NewImageVisualStoryMultipleMotionSources", ErrorCategory.InvalidArgument, null));
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

        if (_grids.Count == 0) {
            var exception = new PSArgumentException("New-ImageVisualStory requires one ChartForgeX VisualGrid.");
            ThrowTerminatingError(new ErrorRecord(exception, "NewImageVisualStoryMissingGrid", ErrorCategory.InvalidArgument, null));
        }
        if (_grids.Count > 1) {
            var exception = new PSArgumentException("New-ImageVisualStory accepts one ChartForgeX VisualGrid per output path.");
            ThrowTerminatingError(new ErrorRecord(exception, "NewImageVisualStoryMultipleGrids", ErrorCategory.InvalidArgument, null));
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using ChartForgeX;
using ChartForgeX.Terminal;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a script-free animated console presentation from authored steps, captured transcript lines, or a native ChartForgeX terminal story.</summary>
/// <para>The cmdlet renders deterministic SVG or HTML motion, animated GIF or APNG motion, and a completed PNG state. It never executes the displayed command: callers run scripts themselves and pipe captured output when they want a real execution transcript.</para>
/// <example>
///   <summary>Author a PowerShell console presentation</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageConsoleStory -StoryScript {
///   param($Console)
///   [void] $Console.WithTitle('pwsh - C:\OpenSource').WithWorkingDirectory('C:\OpenSource')
///   [void] $Console.Command('Get-Service -Name WinRM')
///   [void] $Console.Output('Status   Name               DisplayName', [ChartForgeX.Terminal.TerminalTextTone]::Accent)
///   [void] $Console.Output('Running  WinRM              Windows Remote Management', [ChartForgeX.Terminal.TerminalTextTone]::Success)
/// } -FilePath '.\service-demo.svg'</code>
///   <para>Creates a self-contained SVG with command typing, output reveals, a blinking cursor, and a completed reduced-motion state.</para>
/// </example>
/// <example>
///   <summary>Render output captured from an actual script run</summary>
///   <prefix>PS&gt; </prefix>
///   <code>$output = &amp; .\Invoke-EnvironmentAudit.ps1 2&gt;&amp;1 | Out-String -Stream -Width 110
/// $output | New-ImageConsoleStory -CommandText '.\Invoke-EnvironmentAudit.ps1' -Dialect PowerShell -FilePath '.\audit-demo.svg'</code>
///   <para>The caller controls execution; the cmdlet only turns the captured lines into a deterministic presentation.</para>
/// </example>
/// <example>
///   <summary>Export a portable animated GIF for chat or documentation</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageConsoleStory -StoryScript {
///   param($Console)
///   [void] $Console.WithDialect([ChartForgeX.Terminal.TerminalDialect]::CSharp)
///   [void] $Console.Command('var chart = Chart.Create().WithTitle("Weekly builds");')
///   [void] $Console.Command('chart.SavePng("weekly-builds.png");')
///   [void] $Console.Output('Saved weekly-builds.png', [ChartForgeX.Terminal.TerminalTextTone]::Success)
/// } -FilePath '.\chart-demo.gif' -FramesPerSecond 10 -EndHoldSeconds 1.5</code>
///   <para>GIF and APNG export sample the same deterministic terminal timeline used by SVG and HTML.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageConsoleStory", DefaultParameterSetName = StoryScriptSet)]
[OutputType(typeof(TerminalStory))]
public sealed class NewImageConsoleStoryCmdlet : PSCmdlet {
    private const string StoryScriptSet = "StoryScript";
    private const string StorySet = "Story";
    private const string TranscriptSet = "Transcript";
    private readonly List<TerminalStory> _stories = new();
    private readonly List<string> _transcript = new();

    /// <summary>Script block that receives and configures a new ChartForgeX TerminalStory.</summary>
    [Parameter(Mandatory = true, Position = 0, ParameterSetName = StoryScriptSet)]
    public ScriptBlock? StoryScript { get; set; }

    /// <summary>Native ChartForgeX terminal story to render.</summary>
    [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = StorySet)]
    public TerminalStory? Story { get; set; }

    /// <summary>One captured output line. Accepts pipeline input and never executes the displayed command.</summary>
    [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = TranscriptSet)]
    [AllowEmptyString]
    public string? InputObject { get; set; }

    /// <summary>Command text shown before captured transcript lines.</summary>
    [Parameter(Mandatory = true, ParameterSetName = TranscriptSet)]
    public string CommandText { get; set; } = string.Empty;

    /// <summary>Prompt dialect used for captured transcript presentations.</summary>
    [Parameter(ParameterSetName = TranscriptSet)]
    public TerminalDialect Dialect { get; set; } = TerminalDialect.PowerShell;

    /// <summary>Prompt text used when Dialect is Custom.</summary>
    [Parameter(ParameterSetName = TranscriptSet)]
    public string? CustomPrompt { get; set; }

    /// <summary>Terminal title shown for captured transcript presentations.</summary>
    [Parameter(ParameterSetName = TranscriptSet)]
    public string Title { get; set; } = "PowerShell";

    /// <summary>Working directory shown in shell prompts for captured transcript presentations.</summary>
    [Parameter(ParameterSetName = TranscriptSet)]
    public string WorkingDirectory { get; set; } = @"C:\";

    /// <summary>Output file path. Supported extensions are SVG, HTML, HTM, PNG, GIF, and APNG.</summary>
    [Parameter(Mandatory = true)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Frame rate used for animated GIF and APNG output.</summary>
    [Parameter]
    [ValidateRange(2, 30)]
    public int FramesPerSecond { get; set; } = 10;

    /// <summary>Completed-state hold time used for animated GIF and APNG output.</summary>
    [Parameter]
    [ValidateRange(0, 10)]
    public double EndHoldSeconds { get; set; } = 1.2;

    /// <summary>Raster density multiplier used for animated GIF and APNG output.</summary>
    [Parameter]
    [ValidateRange(1, 4)]
    public int AnimationScale { get; set; } = 1;

    /// <summary>Maximum frame budget used for animated GIF and APNG output.</summary>
    [Parameter]
    [ValidateRange(2, 600)]
    public int MaximumFrames { get; set; } = 240;

    /// <summary>Produce a single-play animated GIF or APNG instead of a repeating animation.</summary>
    [Parameter]
    public SwitchParameter NoLoop { get; set; }

    /// <summary>Open the generated presentation after creation.</summary>
    [Parameter]
    public SwitchParameter Show { get; set; }

    /// <summary>Write the configured ChartForgeX TerminalStory to the pipeline.</summary>
    [Parameter]
    public SwitchParameter PassThru { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        if (Story != null) {
            _stories.Add(Story);
        }
        if (ParameterSetName == TranscriptSet) {
            _transcript.Add(InputObject ?? string.Empty);
        }
    }

    /// <inheritdoc />
    protected override void EndProcessing() {
        var story = BuildStory();
        var output = Helpers.ResolvePath(FilePath);
        var extension = Path.GetExtension(output);
        ValidateExtension(extension, output);
        var directory = Path.GetDirectoryName(output);
        if (!string.IsNullOrWhiteSpace(directory)) {
            Directory.CreateDirectory(directory!);
        }

        if (extension.Equals(".svg", StringComparison.OrdinalIgnoreCase)) {
            story.SaveSvg(output);
        } else if (extension.Equals(".html", StringComparison.OrdinalIgnoreCase) || extension.Equals(".htm", StringComparison.OrdinalIgnoreCase)) {
            story.SaveHtml(output);
        } else if (extension.Equals(".png", StringComparison.OrdinalIgnoreCase)) {
            story.SavePng(output);
        } else if (extension.Equals(".gif", StringComparison.OrdinalIgnoreCase)) {
            story.SaveGif(output, BuildAnimationOptions());
        } else {
            story.SaveApng(output, BuildAnimationOptions());
        }

        if (Show.IsPresent) {
            ImagePlayground.Helpers.Open(output, true);
        }
        if (PassThru.IsPresent) {
            WriteObject(story);
        }
    }

    private TerminalStory BuildStory() {
        if (ParameterSetName == StoryScriptSet) {
            var story = TerminalStory.Create();
            foreach (var result in StoryScript!.Invoke(story)) {
                var value = result is PSObject psObject ? psObject.BaseObject : result;
                if (value is TerminalStory returnedStory) {
                    story = returnedStory;
                }
            }

            return story;
        }

        if (ParameterSetName == TranscriptSet) {
            return TerminalStory.Create()
                .WithTitle(Title)
                .WithDialect(Dialect, CustomPrompt)
                .WithWorkingDirectory(WorkingDirectory)
                .Command(CommandText)
                .Transcript(_transcript);
        }

        if (_stories.Count == 0) {
            var exception = new PSArgumentException("New-ImageConsoleStory requires one ChartForgeX TerminalStory.");
            ThrowTerminatingError(new ErrorRecord(exception, "NewImageConsoleStoryMissingStory", ErrorCategory.InvalidArgument, null));
        }
        if (_stories.Count > 1) {
            var exception = new PSArgumentException("New-ImageConsoleStory accepts one ChartForgeX TerminalStory per output path.");
            ThrowTerminatingError(new ErrorRecord(exception, "NewImageConsoleStoryMultipleStories", ErrorCategory.InvalidArgument, null));
        }

        return _stories[0];
    }

    private TerminalStoryAnimationOptions BuildAnimationOptions() {
        return TerminalStoryAnimationOptions.Create()
            .WithFramesPerSecond(FramesPerSecond)
            .WithEndHold(EndHoldSeconds)
            .WithOutputScale(AnimationScale)
            .WithMaximumFrames(MaximumFrames)
            .WithLoop(!NoLoop.IsPresent);
    }

    private void ValidateExtension(string extension, string output) {
        if (extension.Equals(".svg", StringComparison.OrdinalIgnoreCase) ||
            extension.Equals(".html", StringComparison.OrdinalIgnoreCase) ||
            extension.Equals(".htm", StringComparison.OrdinalIgnoreCase) ||
            extension.Equals(".png", StringComparison.OrdinalIgnoreCase) ||
            extension.Equals(".gif", StringComparison.OrdinalIgnoreCase) ||
            extension.Equals(".apng", StringComparison.OrdinalIgnoreCase)) {
            return;
        }

        var exception = new PSArgumentException("Console story output supports only .svg, .html, .htm, .png, .gif, or .apng file extensions.");
        ThrowTerminatingError(new ErrorRecord(exception, "NewImageConsoleStoryUnsupportedExtension", ErrorCategory.InvalidArgument, output));
    }
}

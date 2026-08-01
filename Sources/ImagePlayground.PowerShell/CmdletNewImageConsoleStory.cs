using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using ChartForgeX;
using ChartForgeX.Terminal;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a script-free animated console presentation from PowerShell-native steps, captured transcript lines, or a native ChartForgeX terminal story.</summary>
/// <para>The recommended Content and Step parameter sets compose objects created by the New-ImageConsoleStoryCommand, Output, Table, BlankLine, and Pause cmdlets. StoryScript remains available as the low-level ChartForgeX builder escape hatch.</para>
/// <para>The cmdlet renders deterministic SVG or HTML motion, animated GIF or APNG motion, and a completed PNG state. It never executes the displayed command: callers run scripts themselves and pipe captured output when they want a real execution transcript.</para>
/// <example>
///   <summary>Author a PowerShell console presentation</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageConsoleStory -Title 'pwsh - C:\OpenSource' -WorkingDirectory 'C:\OpenSource' -Theme PowerShell -WindowStyle WindowsTerminal -Content {
///   New-ImageConsoleStoryCommand -Text 'Get-Service -Name WinRM'
///   New-ImageConsoleStoryOutput -Text 'Status   Name               DisplayName' -Tone Accent
///   New-ImageConsoleStoryOutput -Text 'Running  WinRM              Windows Remote Management' -Tone Success
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
///   <code>New-ImageConsoleStory -Dialect CSharp -Title 'dotnet run - ChartForgeX' -Content {
///   New-ImageConsoleStoryCommand -Text 'var chart = Chart.Create().WithTitle("Weekly builds");'
///   New-ImageConsoleStoryCommand -Text 'chart.SavePng("weekly-builds.png");'
///   New-ImageConsoleStoryOutput -Text 'Saved weekly-builds.png' -Tone Success
/// } -FilePath '.\chart-demo.gif' -FramesPerSecond 10 -EndHoldSeconds 1.5</code>
///   <para>GIF and APNG export sample the same deterministic terminal timeline used by SVG and HTML.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageConsoleStory", DefaultParameterSetName = StoryScriptSet)]
[OutputType(typeof(TerminalStory))]
public sealed class NewImageConsoleStoryCmdlet : PSCmdlet {
    private const string StoryScriptSet = "StoryScript";
    private const string ContentSet = "Content";
    private const string StepSet = "Step";
    private const string StorySet = "Story";
    private const string TranscriptSet = "Transcript";
    private readonly List<TerminalStory> _stories = new();
    private readonly List<ImageConsoleStoryStep> _steps = new();
    private readonly List<string> _transcript = new();

    /// <summary>Script block that receives and configures a new ChartForgeX TerminalStory.</summary>
    [Parameter(Mandatory = true, Position = 0, ParameterSetName = StoryScriptSet)]
    public ScriptBlock? StoryScript { get; set; }

    /// <summary>PowerShell-native authoring block that emits steps created by the New-ImageConsoleStoryCommand, Output, Table, BlankLine, and Pause cmdlets.</summary>
    [Parameter(Mandatory = true, ParameterSetName = ContentSet)]
    public ScriptBlock? Content { get; set; }

    /// <summary>Typed console story steps. Accepts pipeline input and arrays created by the New-ImageConsoleStoryCommand, Output, Table, BlankLine, and Pause cmdlets.</summary>
    [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = StepSet)]
    public ImageConsoleStoryStep[]? Step { get; set; }

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
    [Parameter(ParameterSetName = ContentSet)]
    [Parameter(ParameterSetName = StepSet)]
    public TerminalDialect Dialect { get; set; } = TerminalDialect.PowerShell;

    /// <summary>Prompt text used when Dialect is Custom.</summary>
    [Parameter(ParameterSetName = TranscriptSet)]
    [Parameter(ParameterSetName = ContentSet)]
    [Parameter(ParameterSetName = StepSet)]
    public string? CustomPrompt { get; set; }

    /// <summary>Terminal title shown for captured transcript presentations.</summary>
    [Parameter(ParameterSetName = TranscriptSet)]
    [Parameter(ParameterSetName = ContentSet)]
    [Parameter(ParameterSetName = StepSet)]
    public string Title { get; set; } = "PowerShell";

    /// <summary>Working directory shown in shell prompts for captured transcript presentations.</summary>
    [Parameter(ParameterSetName = TranscriptSet)]
    [Parameter(ParameterSetName = ContentSet)]
    [Parameter(ParameterSetName = StepSet)]
    public string WorkingDirectory { get; set; } = @"C:\";

    /// <summary>Built-in terminal color palette used by composed and captured stories.</summary>
    [Parameter(ParameterSetName = TranscriptSet)]
    [Parameter(ParameterSetName = ContentSet)]
    [Parameter(ParameterSetName = StepSet)]
    [ValidateSet("Dark", "PowerShell", "Classic", "Light")]
    public string Theme { get; set; } = "Dark";

    /// <summary>Visible terminal window chrome, independent of the color palette and prompt dialect.</summary>
    [Parameter(ParameterSetName = TranscriptSet)]
    [Parameter(ParameterSetName = ContentSet)]
    [Parameter(ParameterSetName = StepSet)]
    public TerminalWindowStyle WindowStyle { get; set; } = TerminalWindowStyle.MacOS;

    /// <summary>Logical terminal width used by composed and captured stories.</summary>
    [Parameter(ParameterSetName = TranscriptSet)]
    [Parameter(ParameterSetName = ContentSet)]
    [Parameter(ParameterSetName = StepSet)]
    [ValidateRange(480, 1800)]
    public int Width { get; set; } = 886;

    /// <summary>Terminal font size used by composed and captured stories.</summary>
    [Parameter(ParameterSetName = TranscriptSet)]
    [Parameter(ParameterSetName = ContentSet)]
    [Parameter(ParameterSetName = StepSet)]
    [ValidateRange(10, 24)]
    public double FontSize { get; set; } = 14;

    /// <summary>Terminal line height used by composed and captured stories.</summary>
    [Parameter(ParameterSetName = TranscriptSet)]
    [Parameter(ParameterSetName = ContentSet)]
    [Parameter(ParameterSetName = StepSet)]
    [ValidateRange(13, 40)]
    public double LineHeight { get; set; } = 22;

    /// <summary>Delay before the first animated step.</summary>
    [Parameter(ParameterSetName = TranscriptSet)]
    [Parameter(ParameterSetName = ContentSet)]
    [Parameter(ParameterSetName = StepSet)]
    [ValidateRange(0, 10)]
    public double InitialDelaySeconds { get; set; } = 0.35;

    /// <summary>Simulated command typing speed.</summary>
    [Parameter(ParameterSetName = TranscriptSet)]
    [Parameter(ParameterSetName = ContentSet)]
    [Parameter(ParameterSetName = StepSet)]
    [ValidateRange(5, 200)]
    public double CharactersPerSecond { get; set; } = 42;

    /// <summary>Delay between output lines.</summary>
    [Parameter(ParameterSetName = TranscriptSet)]
    [Parameter(ParameterSetName = ContentSet)]
    [Parameter(ParameterSetName = StepSet)]
    [ValidateRange(0, 3)]
    public double LineDelaySeconds { get; set; } = 0.08;

    /// <summary>Hide the final prompt and cursor in the completed story.</summary>
    [Parameter(ParameterSetName = TranscriptSet)]
    [Parameter(ParameterSetName = ContentSet)]
    [Parameter(ParameterSetName = StepSet)]
    public SwitchParameter NoFinalPrompt { get; set; }

    /// <summary>PNG output density multiplier used by composed and captured stories.</summary>
    [Parameter(ParameterSetName = TranscriptSet)]
    [Parameter(ParameterSetName = ContentSet)]
    [Parameter(ParameterSetName = StepSet)]
    [ValidateRange(1, 4)]
    public int PngOutputScale { get; set; } = 2;

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
        if (ParameterSetName == StepSet && Step != null) {
            _steps.AddRange(Step);
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

        if (ParameterSetName == ContentSet) {
            foreach (var result in Content!.Invoke()) {
                AddContentResult(result);
            }
            return BuildComposedStory();
        }

        if (ParameterSetName == StepSet) {
            return BuildComposedStory();
        }

        if (ParameterSetName == TranscriptSet) {
            return ConfigureStory()
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

    private TerminalStory BuildComposedStory() {
        if (_steps.Count == 0) {
            var exception = new PSArgumentException("New-ImageConsoleStory requires at least one ImageConsoleStoryStep from -Content or -Step.");
            ThrowTerminatingError(new ErrorRecord(exception, "NewImageConsoleStoryMissingStep", ErrorCategory.InvalidArgument, null));
        }

        var story = ConfigureStory();
        foreach (var step in _steps) {
            step.ApplyTo(story);
        }
        return story;
    }

    private TerminalStory ConfigureStory() {
        return TerminalStory.Create()
            .WithTitle(Title)
            .WithDialect(Dialect, CustomPrompt)
            .WithWorkingDirectory(WorkingDirectory)
            .WithTheme(ResolveTheme())
            .WithWindowStyle(WindowStyle)
            .WithWidth(Width)
            .WithTypography(FontSize, LineHeight)
            .WithTiming(InitialDelaySeconds, CharactersPerSecond, LineDelaySeconds)
            .WithFinalPrompt(!NoFinalPrompt.IsPresent)
            .WithPngOutputScale(PngOutputScale);
    }

    private TerminalTheme ResolveTheme() {
        switch (Theme.ToUpperInvariant()) {
            case "DARK": return TerminalTheme.Dark();
            case "POWERSHELL": return TerminalTheme.PowerShell();
            case "CLASSIC": return TerminalTheme.Classic();
            case "LIGHT": return TerminalTheme.Light();
            default: throw new InvalidOperationException("Unknown console story theme.");
        }
    }

    private void AddContentResult(object? result) {
        var value = Unwrap(result);
        if (value == null) {
            return;
        }
        if (value is ImageConsoleStoryStep step) {
            _steps.Add(step);
            return;
        }
        if (value is ImageConsoleStoryStep[] steps) {
            _steps.AddRange(steps);
            return;
        }

        var exception = new PSArgumentException(
            "New-ImageConsoleStory -Content accepts only steps created by New-ImageConsoleStoryCommand, New-ImageConsoleStoryOutput, New-ImageConsoleStoryTable, New-ImageConsoleStoryBlankLine, or New-ImageConsoleStoryPause. Received: " + value.GetType().FullName + ".");
        ThrowTerminatingError(new ErrorRecord(exception, "NewImageConsoleStoryInvalidContent", ErrorCategory.InvalidData, value));
    }

    private static object? Unwrap(object? value) {
        while (value is PSObject psObject) {
            value = psObject.BaseObject;
        }
        return value;
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

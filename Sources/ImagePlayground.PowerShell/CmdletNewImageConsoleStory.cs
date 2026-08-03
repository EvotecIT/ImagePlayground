using System;
using System.Collections.Generic;
using System.Management.Automation;
using ChartForgeX;
using ChartForgeX.Terminal;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a reusable script-free console story from PowerShell-native steps, captured transcript lines, or a native ChartForgeX terminal story.</summary>
/// <para>The recommended Content and Step parameter sets compose objects created by the New-ImageConsoleStoryCommand, Output, Table, BlankLine, Pause, Tab, and Select-ImageConsoleStoryTab cmdlets. StoryScript remains available as the low-level ChartForgeX builder escape hatch.</para>
/// <para>The cmdlet renders deterministic SVG or HTML motion, animated GIF or APNG motion, and a completed PNG state. It never executes the displayed command: callers run scripts themselves and pipe captured output when they want a real execution transcript.</para>
/// <example>
///   <summary>Author a paced multi-tab Windows Terminal presentation</summary>
///   <prefix>PS&gt; </prefix>
///   <code>$story = New-ImageConsoleStory -WindowStyle WindowsTerminal -Width 1100 -Speed Slow -Content {
///   New-ImageConsoleStoryTab -Id PowerShell -Title 'PowerShell' -Profile PowerShell -Active
///   New-ImageConsoleStoryCommand -Text 'dotnet build'
///   New-ImageConsoleStoryOutput -Text 'Build succeeded.' -Style Success
///
///   New-ImageConsoleStoryTab -Id WindowsPowerShell -Title 'Windows PowerShell' -Profile WindowsPowerShell
///   New-ImageConsoleStoryCommand -Text '.\Invoke-LegacyTests.ps1'
///   New-ImageConsoleStoryOutput -Text 'PS 5.1 compatibility passed.' -Style Success
///
///   New-ImageConsoleStoryTab -Id Ubuntu -Title 'Ubuntu' -Profile Ubuntu
///   New-ImageConsoleStoryCommand -Text './build.sh'
///   New-ImageConsoleStoryOutput -Text 'Linux package ready.' -Style Success
/// }
/// $story | Export-ImageConsoleStory -Path '.\demo.gif'</code>
///   <para>Creates three persistent tab buffers. Each new tab opens atomically after the previous tab's reading dwell, then the shared story is exported through Export-ImageConsoleStory.</para>
/// </example>
/// <example>
///   <summary>Jump between retained sessions intentionally</summary>
///   <prefix>PS&gt; </prefix>
///   <code>$story = New-ImageConsoleStory -WindowStyle WindowsTerminal -Speed Slow -Content {
///   New-ImageConsoleStoryTab -Id PowerShell -Profile PowerShell -Active
///   New-ImageConsoleStoryTab -Id Logs -Title 'Build logs' -Profile PowerShell -Background
///   New-ImageConsoleStoryCommand -Text 'dotnet build'
///   Select-ImageConsoleStoryTab -Id Logs
///   New-ImageConsoleStoryOutput -Text 'Waiting for integration tests...' -Style Muted
///   New-ImageConsoleStoryPause -Seconds 1.5
///   Select-ImageConsoleStoryTab -Id PowerShell
///   New-ImageConsoleStoryCommand -Text 'Get-ChildItem .\artifacts'
/// }
/// $story | Export-ImageConsoleStory -Path '.\navigation.gif'</code>
///   <para>Background prepares an inactive tab. Each Select step deliberately switches to a retained buffer; a pause can show that session ready and a later command continues it.</para>
/// </example>
/// <example>
///   <summary>Render output captured from an actual script run</summary>
///   <prefix>PS&gt; </prefix>
///   <code>$output = &amp; .\Invoke-EnvironmentAudit.ps1 2&gt;&amp;1 | Out-String -Stream -Width 110
/// $story = $output | New-ImageConsoleStory -CommandText '.\Invoke-EnvironmentAudit.ps1' -Dialect PowerShell
/// $story | Export-ImageConsoleStory -Path '.\audit-demo.svg'</code>
///   <para>The caller controls execution; the cmdlet only turns the captured lines into a deterministic presentation.</para>
/// </example>
/// <example>
///   <summary>Export a portable animated GIF for chat or documentation</summary>
///   <prefix>PS&gt; </prefix>
///   <code>$story = New-ImageConsoleStory -Dialect CSharp -Title 'dotnet run - ChartForgeX' -Content {
///   New-ImageConsoleStoryCommand -Text 'var chart = Chart.Create().WithTitle("Weekly builds");'
///   New-ImageConsoleStoryCommand -Text 'chart.SavePng("weekly-builds.png");'
///   New-ImageConsoleStoryOutput -Text 'Saved weekly-builds.png' -Style Success
/// }
/// $story | Export-ImageConsoleStory -Path '.\chart-demo.gif' -FramesPerSecond 10 -EndHoldSeconds 1.5</code>
///   <para>GIF and APNG export sample the same deterministic terminal timeline used by SVG and HTML.</para>
/// </example>
/// <example>
///   <summary>Tune typing and tab reading time independently</summary>
///   <prefix>PS&gt; </prefix>
///   <code>$story = New-ImageConsoleStory -Speed Normal -TypingSpeed 36 -TabHoldSeconds 2.5 -Content {
///   New-ImageConsoleStoryCommand -Text 'Invoke-ProjectBuild'
///   New-ImageConsoleStoryOutput -Text 'Build completed.' -Style Success
/// }</code>
///   <para>TypingSpeed is measured in visible characters per second. A command-level DurationSeconds value remains the most specific override.</para>
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

    /// <summary>PowerShell-native authoring block that emits command, output, table, pause, tab declaration, and tab-selection steps.</summary>
    [Parameter(Mandatory = true, ParameterSetName = ContentSet)]
    public ScriptBlock? Content { get; set; }

    /// <summary>Typed console story steps. Accepts pipeline input and arrays created by the console story step cmdlets.</summary>
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
    [ValidateSet("Dark", "PowerShell", "WindowsPowerShell", "Ubuntu", "Campbell", "Classic", "Light")]
    public string Theme { get; set; } = "Dark";

    /// <summary>Optional custom palette, normally created by New-ImageConsoleStoryPalette.</summary>
    [Parameter(ParameterSetName = TranscriptSet)]
    [Parameter(ParameterSetName = ContentSet)]
    [Parameter(ParameterSetName = StepSet)]
    public TerminalTheme? Palette { get; set; }

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
    [PSDefaultValue(Value = 0.35, Help = "0.35")]
    [ValidateRange(0, 10)]
    public double InitialDelaySeconds { get; set; } = 0.35;

    /// <summary>Optional simulated command typing speed in visible characters per second. When omitted, Speed selects the rate: Slow 28, Normal 42, or Fast 72 characters per second.</summary>
    [Parameter(ParameterSetName = TranscriptSet)]
    [Parameter(ParameterSetName = ContentSet)]
    [Parameter(ParameterSetName = StepSet)]
    [Alias("CharactersPerSecond")]
    [PSDefaultValue(Value = "Derived from Speed", Help = "Slow: 28 characters/second; Normal: 42 characters/second; Fast: 72 characters/second")]
    [ValidateRange(5, 200)]
    public double TypingSpeed { get; set; } = 42;

    /// <summary>Delay between output lines.</summary>
    [Parameter(ParameterSetName = TranscriptSet)]
    [Parameter(ParameterSetName = ContentSet)]
    [Parameter(ParameterSetName = StepSet)]
    [PSDefaultValue(Value = 0.08, Help = "0.08")]
    [ValidateRange(0, 3)]
    public double LineDelaySeconds { get; set; } = 0.08;

    /// <summary>Reusable playback pace. Slow leaves the most reading time, Normal is balanced, and Fast is intended for short demos.</summary>
    [Parameter(ParameterSetName = TranscriptSet)]
    [Parameter(ParameterSetName = ContentSet)]
    [Parameter(ParameterSetName = StepSet)]
    public TerminalStoryPlaybackSpeed Speed { get; set; } = TerminalStoryPlaybackSpeed.Normal;

    /// <summary>Optional minimum reading time after content appears and before the active tab changes. When omitted, Speed selects the hold: Slow 2 seconds, Normal 0.9 seconds, or Fast 0.35 seconds.</summary>
    [Parameter(ParameterSetName = TranscriptSet)]
    [Parameter(ParameterSetName = ContentSet)]
    [Parameter(ParameterSetName = StepSet)]
    [PSDefaultValue(Value = "Derived from Speed", Help = "Slow: 2 seconds; Normal: 0.9 seconds; Fast: 0.35 seconds")]
    [ValidateRange(0, 10)]
    public double TabHoldSeconds { get; set; }

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
    [Parameter]
    public string? FilePath { get; set; }

    /// <summary>Frame rate used for animated GIF and APNG output.</summary>
    [Parameter]
    [ValidateRange(2, 30)]
    public int FramesPerSecond { get; set; } = 10;

    /// <summary>Completed-state hold time used for animated GIF and APNG output.</summary>
    [Parameter]
    [PSDefaultValue(Value = 1.2, Help = "1.2")]
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
        if (string.IsNullOrWhiteSpace(FilePath)) {
            if (Show.IsPresent) {
                throw new PSArgumentException("New-ImageConsoleStory -Show requires -FilePath, or pipe the story to Export-ImageConsoleStory -Show.", nameof(Show));
            }

            var story = BuildStory();
            WriteObject(story);
            return;
        }

        var output = ConsoleStoryExporter.ResolveOutputPath(this, FilePath!);
        var storyWithOutput = BuildStory();
        ConsoleStoryExporter.Write(this, storyWithOutput, output, BuildAnimationOptions());

        if (Show.IsPresent) {
            ImagePlayground.Helpers.Open(output, true);
        }
        if (PassThru.IsPresent) {
            WriteObject(storyWithOutput);
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

        var initialTabIndex = -1;
        for (var index = 0; index < _steps.Count; index++) {
            if (!_steps[index].IsInitialTab) continue;
            if (initialTabIndex >= 0) {
                var exception = new PSArgumentException("New-ImageConsoleStory accepts one -Active tab declaration.");
                ThrowTerminatingError(new ErrorRecord(exception, "NewImageConsoleStoryMultipleInitialTabs", ErrorCategory.InvalidData, _steps[index]));
            }
            initialTabIndex = index;
        }
        if (initialTabIndex > 0) {
            var exception = new PSArgumentException("New-ImageConsoleStory requires the -Active tab declaration to be the first content step.");
            ThrowTerminatingError(new ErrorRecord(exception, "NewImageConsoleStoryLateInitialTab", ErrorCategory.InvalidData, _steps[initialTabIndex]));
        }

        var story = ConfigureStory();
        foreach (var step in _steps) {
            step.ApplyTo(story);
        }
        return story;
    }

    private TerminalStory ConfigureStory() {
        var story = TerminalStory.Create()
            .WithTitle(Title)
            .WithDialect(Dialect, CustomPrompt)
            .WithWorkingDirectory(WorkingDirectory)
            .WithTheme(Palette ?? ResolveTheme())
            .WithWindowStyle(WindowStyle)
            .WithWidth(Width)
            .WithTypography(FontSize, LineHeight)
            .WithPlaybackSpeed(Speed)
            .WithFinalPrompt(!NoFinalPrompt.IsPresent)
            .WithPngOutputScale(PngOutputScale);

        var initialDelay = MyInvocation.BoundParameters.ContainsKey(nameof(InitialDelaySeconds)) ? InitialDelaySeconds : story.InitialDelaySeconds;
        var charactersPerSecond = MyInvocation.BoundParameters.ContainsKey(nameof(TypingSpeed)) ? TypingSpeed : story.CharactersPerSecond;
        var lineDelay = MyInvocation.BoundParameters.ContainsKey(nameof(LineDelaySeconds)) ? LineDelaySeconds : story.LineDelaySeconds;
        story.WithTiming(initialDelay, charactersPerSecond, lineDelay);
        if (MyInvocation.BoundParameters.ContainsKey(nameof(TabHoldSeconds))) {
            story.WithTabHold(TabHoldSeconds);
        }
        return story;
    }

    private TerminalTheme ResolveTheme() {
        return ConsoleStoryPaletteResolver.Resolve(Theme);
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
            "New-ImageConsoleStory -Content accepts only steps created by the ImageConsoleStory command, output, table, blank-line, pause, tab, and tab-selection cmdlets. Received: " + value.GetType().FullName + ".");
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

}

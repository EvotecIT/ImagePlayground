using System.Management.Automation;
using ChartForgeX.Terminal;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Exports an authored ImagePlayground console story to SVG, HTML, PNG, GIF, or APNG.</summary>
/// <para>Use New-ImageConsoleStory to compose the reusable story once, then pipe it to this cmdlet for each required format. Export never executes displayed commands.</para>
/// <example>
///   <summary>Export a reusable console story to GIF</summary>
///   <prefix>PS&gt; </prefix>
///   <code>$story = New-ImageConsoleStory -WindowStyle WindowsTerminal -Width 1100 -Speed Slow -Content {
///   New-ImageConsoleStoryTab -Id PowerShell -Title 'PowerShell' -Profile PowerShell -Active
///   New-ImageConsoleStoryCommand -Text 'dotnet build'
///   New-ImageConsoleStoryOutput -Text 'Build succeeded.' -Style Success
///   New-ImageConsoleStoryTab -Id Ubuntu -Title 'Ubuntu' -Profile Ubuntu
///   Select-ImageConsoleStoryTab -Id Ubuntu
///   New-ImageConsoleStoryCommand -Text './build.sh'
///   New-ImageConsoleStoryOutput -Text 'Linux package ready.' -Style Success
/// }
/// $story | Export-ImageConsoleStory -Path '.\demo.gif'</code>
///   <para>Creates a Discord-friendly animated GIF while retaining the story object for other exports.</para>
/// </example>
[Cmdlet(VerbsData.Export, "ImageConsoleStory")]
[OutputType(typeof(TerminalStory))]
public sealed class ExportImageConsoleStoryCmdlet : PSCmdlet {
    /// <summary>Authored terminal story to export. Accepts pipeline input from New-ImageConsoleStory.</summary>
    [Parameter(Mandatory = true, ValueFromPipeline = true)]
    public TerminalStory? Story { get; set; }

    /// <summary>Output file path. Supported extensions are SVG, HTML, HTM, PNG, GIF, and APNG.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    [Alias("FilePath")]
    public string Path { get; set; } = string.Empty;

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

    /// <summary>Open the generated presentation after export.</summary>
    [Parameter]
    public SwitchParameter Show { get; set; }

    /// <summary>Write the exported story back to the pipeline.</summary>
    [Parameter]
    public SwitchParameter PassThru { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var story = Story ?? throw new PSArgumentException("Export-ImageConsoleStory requires a terminal story.", nameof(Story));
        var output = ConsoleStoryExporter.Write(this, story, Path, BuildAnimationOptions());
        if (Show.IsPresent) {
            Helpers.Open(output, true);
        }
        if (PassThru.IsPresent) {
            WriteObject(story);
        }
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

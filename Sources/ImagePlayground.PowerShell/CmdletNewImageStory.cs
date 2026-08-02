using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Text.Json;
using System.Text.Json.Serialization;
using ChartForgeX;
using ChartForgeX.Stories;
using ImagePlayground.PowerShell.Stories;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a generic source-to-result visual story from resolved scenes and declared outcomes.</summary>
/// <para>The cmdlet never executes displayed code. Callers or trusted build tooling capture real outputs first, then pass the resolved source, transcript, and media into the story.</para>
/// <example>
///   <summary>Show PowerShell source and the chart it produced</summary>
///   <prefix>PS&gt; </prefix>
///   <code>$source = ConvertTo-ImageStorySource -Text $code -Language PowerShell
/// $codePanel = New-ImageStoryPanel -Id source -Source $source -Title 'PowerShell'
/// $chartPanel = New-ImageStoryPanel -Id chart -MediaPath .\chart.png -AccessibleText 'Weekly builds chart' -Title 'Result'
/// $scenes = @(
///   New-ImageStoryScene -Id source -Title 'Write the script' -Panels $codePanel
///   New-ImageStoryScene -Id result -Title 'See the chart' -Layout Split -Panels @($codePanel, $chartPanel)
/// )
/// $outcome = New-ImageStoryOutcome -Id chart-created -Label 'The chart is visible' -PanelId chart
/// New-ImageStory -Title 'Chart in five lines' -Scenes $scenes -Outcomes $outcome -FilePath .\chart-story.gif</code>
///   <para>Produces a portable animated story whose final scene contains the promised chart.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageStory", DefaultParameterSetName = PartsSet)]
[OutputType(typeof(VisualStory))]
public sealed class NewImageStoryCmdlet : PSCmdlet {
    private const string PartsSet = "Parts";
    private const string StorySet = "Story";
    private readonly List<VisualStory> _stories = new();

    /// <summary>Native resolved ChartForgeX visual story.</summary>
    [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = StorySet)]
    public VisualStory? Story { get; set; }

    /// <summary>Story title.</summary>
    [Parameter(Mandatory = true, ParameterSetName = PartsSet)]
    public string Title { get; set; } = string.Empty;

    /// <summary>Accessible story description.</summary>
    [Parameter(ParameterSetName = PartsSet)]
    public string Description { get; set; } = string.Empty;

    /// <summary>Resolved scenes in display order.</summary>
    [Parameter(Mandatory = true, ParameterSetName = PartsSet)]
    public ImageStorySceneSpec[] Scenes { get; set; } = System.Array.Empty<ImageStorySceneSpec>();

    /// <summary>Outcomes that the completed scene must reveal.</summary>
    [Parameter(Mandatory = true, ParameterSetName = PartsSet)]
    public ImageStoryOutcomeSpec[] Outcomes { get; set; } = System.Array.Empty<ImageStoryOutcomeSpec>();

    /// <summary>Logical output width.</summary>
    [Parameter(ParameterSetName = PartsSet)]
    [ValidateRange(480, 3840)]
    public int Width { get; set; } = 1200;

    /// <summary>Logical output height.</summary>
    [Parameter(ParameterSetName = PartsSet)]
    [ValidateRange(320, 2160)]
    public int Height { get; set; } = 675;

    /// <summary>Optional visual-story theme.</summary>
    [Parameter(ParameterSetName = PartsSet)]
    public VisualStoryTheme? Theme { get; set; }

    /// <summary>Output path. Supports SVG, HTML, PNG, GIF, APNG, and TXT.</summary>
    [Parameter(Mandatory = true)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Optional directory that receives a portable PowerForge-compatible story bundle and manifest.</summary>
    [Parameter]
    public string? BundlePath { get; set; }

    /// <summary>Formats emitted into BundlePath. The completed PNG is always included.</summary>
    [Parameter]
    [ValidateSet("Svg", "Html", "Png", "Gif", "Apng", "Transcript")]
    public string[] BundleFormats { get; set; } = new[] { "Svg", "Png", "Gif", "Transcript" };

    /// <summary>Optional capture time recorded in the portable bundle. Omit it for deterministic bundle manifests.</summary>
    [Parameter]
    public System.DateTimeOffset? CapturedAtUtc { get; set; }

    /// <summary>Frame rate for GIF and APNG output.</summary>
    [Parameter]
    [ValidateRange(2, 30)]
    public int FramesPerSecond { get; set; } = 6;

    /// <summary>Completed-scene hold time for GIF and APNG output.</summary>
    [Parameter]
    [PSDefaultValue(Value = 1.5, Help = "1.5")]
    [ValidateRange(0, 10)]
    public double EndHoldSeconds { get; set; } = 1.5;

    /// <summary>Cross-fade duration between scenes.</summary>
    [Parameter]
    [PSDefaultValue(Value = 0.24, Help = "0.24")]
    [ValidateRange(0, 1)]
    public double TransitionSeconds { get; set; } = 0.24;

    /// <summary>Raster density multiplier for GIF and APNG output.</summary>
    [Parameter]
    [ValidateRange(1, 4)]
    public int AnimationScale { get; set; } = 1;

    /// <summary>Maximum animated frame budget.</summary>
    [Parameter]
    [ValidateRange(2, 600)]
    public int MaximumFrames { get; set; } = 240;

    /// <summary>Produce a single-play GIF or APNG.</summary>
    [Parameter]
    public SwitchParameter NoLoop { get; set; }

    /// <summary>Open the generated story after creation.</summary>
    [Parameter]
    public SwitchParameter Show { get; set; }

    /// <summary>Write the resolved native story to the pipeline.</summary>
    [Parameter]
    public SwitchParameter PassThru { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        if (Story != null) _stories.Add(Story);
    }

    /// <inheritdoc />
    protected override void EndProcessing() {
        var output = PowerShellPathResolver.ResolveFileSystemPath(this, FilePath);
        var extension = Path.GetExtension(output);
        ValidateExtension(extension, output);
        var bundle = string.IsNullOrWhiteSpace(BundlePath)
            ? null
            : PowerShellPathResolver.ResolveFileSystemPath(this, BundlePath!);
        var story = BuildStory();
        var directory = Path.GetDirectoryName(output);
        if (!string.IsNullOrWhiteSpace(directory)) Directory.CreateDirectory(directory!);

        if (extension.Equals(".svg", System.StringComparison.OrdinalIgnoreCase)) story.SaveSvg(output);
        else if (extension.Equals(".html", System.StringComparison.OrdinalIgnoreCase) || extension.Equals(".htm", System.StringComparison.OrdinalIgnoreCase)) File.WriteAllText(output, story.ToHtmlPage());
        else if (extension.Equals(".png", System.StringComparison.OrdinalIgnoreCase)) story.SavePng(output);
        else if (extension.Equals(".gif", System.StringComparison.OrdinalIgnoreCase)) story.SaveGif(output, BuildAnimationOptions());
        else if (extension.Equals(".apng", System.StringComparison.OrdinalIgnoreCase)) story.SaveApng(output, BuildAnimationOptions());
        else story.SaveTranscript(output);

        if (bundle != null) {
            WriteBundle(story, bundle, output);
        }
        if (Show.IsPresent) ImagePlayground.Helpers.Open(output, true);
        if (PassThru.IsPresent) WriteObject(story);
    }

    private VisualStory BuildStory() {
        if (ParameterSetName == StorySet) {
            if (_stories.Count != 1) throw new PSArgumentException("New-ImageStory accepts exactly one ChartForgeX VisualStory per output path.");
            return _stories[0];
        }
        var story = VisualStory.Create(Title).WithSize(Width, Height);
        if (Description.Length > 0) story.WithDescription(Description);
        if (Theme != null) story.WithTheme(Theme);
        foreach (var sceneSpec in Scenes) {
            var scene = story.Scene(sceneSpec.Id, sceneSpec.Title, sceneSpec.DurationSeconds, sceneSpec.Layout);
            foreach (var panel in sceneSpec.Panels) scene.Panel(panel.Id, panel.Surface, panel.Title, panel.Weight);
        }
        foreach (var outcome in Outcomes) story.Outcome(outcome.Id, outcome.Label, outcome.PanelId);
        return story;
    }

    private VisualStoryAnimationOptions BuildAnimationOptions() => VisualStoryAnimationOptions.Create()
        .WithFramesPerSecond(FramesPerSecond)
        .WithEndHold(EndHoldSeconds)
        .WithTransition(TransitionSeconds)
        .WithOutputScale(AnimationScale)
        .WithMaximumFrames(MaximumFrames)
        .WithLoop(!NoLoop.IsPresent);

    private void WriteBundle(VisualStory story, string bundlePath, string resolvedOutputPath) {
        Directory.CreateDirectory(bundlePath);
        var baseName = Path.GetFileNameWithoutExtension(resolvedOutputPath);
        if (string.IsNullOrWhiteSpace(baseName)) baseName = "visual-story";
        var requested = new HashSet<string>(BundleFormats ?? System.Array.Empty<string>(), System.StringComparer.OrdinalIgnoreCase) {
            "Png"
        };
        var artifacts = new List<object>();
        foreach (var format in new[] { "Svg", "Html", "Png", "Gif", "Apng", "Transcript" }) {
            if (!requested.Contains(format)) continue;
            var extension = format switch {
                "Svg" => ".svg",
                "Html" => ".html",
                "Png" => ".png",
                "Gif" => ".gif",
                "Apng" => ".apng",
                _ => ".txt"
            };
            var path = Path.Combine(bundlePath, baseName + extension);
            if (format == "Svg") story.SaveSvg(path);
            else if (format == "Html") File.WriteAllText(path, story.ToHtmlPage());
            else if (format == "Png") story.SavePng(path);
            else if (format == "Gif") story.SaveGif(path, BuildAnimationOptions());
            else if (format == "Apng") story.SaveApng(path, BuildAnimationOptions());
            else story.SaveTranscript(path);
            artifacts.Add(new {
                role = format == "Png" ? "completed" : format == "Transcript" ? "transcript" : format == "Html" ? "html" : "animated",
                format = format == "Transcript" ? "text" : format.ToLowerInvariant(),
                path = Path.GetFileName(path)
            });
        }

        var outcome = string.Join(" ", story.Outcomes.Select(item => item.Label));
        var manifest = new {
            schemaVersion = 1,
            id = ToStoryId(baseName),
            title = story.Title,
            alt = story.Description.Length == 0 ? outcome : story.Description,
            caption = story.Description.Length == 0 ? null : story.Description,
            outcome,
            generatedAtUtc = CapturedAtUtc?.ToUniversalTime(),
            producer = BuildProducerName(),
            artifacts
        };
        File.WriteAllText(
            Path.Combine(bundlePath, "story.json"),
            JsonSerializer.Serialize(manifest, new JsonSerializerOptions {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            }));
    }

    private static string BuildProducerName() {
        var version = typeof(NewImageStoryCmdlet).Assembly.GetName().Version;
        return version == null ? "ImagePlayground" : $"ImagePlayground {version.Major}.{version.Minor}.{version.Build}";
    }

    private static string ToStoryId(string value) {
        var output = new StringBuilder(value.Length);
        var separator = false;
        foreach (var character in value.ToLowerInvariant()) {
            if (char.IsLetterOrDigit(character)) {
                output.Append(character);
                separator = false;
            } else if (!separator && output.Length > 0) {
                output.Append('-');
                separator = true;
            }
        }
        var id = output.ToString().Trim('-');
        return id.Length == 0 ? "visual-story" : id;
    }

    private void ValidateExtension(string extension, string output) {
        if (extension.Equals(".svg", System.StringComparison.OrdinalIgnoreCase) ||
            extension.Equals(".html", System.StringComparison.OrdinalIgnoreCase) ||
            extension.Equals(".htm", System.StringComparison.OrdinalIgnoreCase) ||
            extension.Equals(".png", System.StringComparison.OrdinalIgnoreCase) ||
            extension.Equals(".gif", System.StringComparison.OrdinalIgnoreCase) ||
            extension.Equals(".apng", System.StringComparison.OrdinalIgnoreCase) ||
            extension.Equals(".txt", System.StringComparison.OrdinalIgnoreCase)) return;
        var exception = new PSArgumentException("Image story output supports only .svg, .html, .htm, .png, .gif, .apng, or .txt file extensions.");
        ThrowTerminatingError(new ErrorRecord(exception, "NewImageStoryUnsupportedExtension", ErrorCategory.InvalidArgument, output));
    }
}

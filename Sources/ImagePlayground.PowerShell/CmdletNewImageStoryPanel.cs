using System.IO;
using System.Management.Automation;
using ChartForgeX.Raster;
using ChartForgeX.Stories;
using ChartForgeX.Terminal;
using ImagePlayground.PowerShell.Stories;

namespace ImagePlayground.PowerShell;

/// <summary>Creates one resolved source, terminal, media, or text panel for a generic visual story.</summary>
/// <example>
///   <summary>Create an emphasized result panel</summary>
///   <code>New-ImageStoryPanel -Id 'summary' -Title 'Result' -Text 'Deployment completed.' -Emphasized</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageStoryPanel", DefaultParameterSetName = SourceSet)]
[OutputType(typeof(ImageStoryPanelSpec))]
public sealed class NewImageStoryPanelCmdlet : PSCmdlet {
    private const string SourceSet = "Source";
    private const string SourceTextSet = "SourceText";
    private const string TerminalSet = "Terminal";
    private const string MediaPathSet = "MediaPath";
    private const string MediaBytesSet = "MediaBytes";
    private const string MediaImageSet = "MediaImage";
    private const string TextSet = "Text";

    /// <summary>Stable panel identifier referenced by completed outcomes.</summary>
    [Parameter(Mandatory = true)]
    public string Id { get; set; } = string.Empty;

    /// <summary>Optional visible panel title.</summary>
    [Parameter]
    public string Title { get; set; } = string.Empty;

    /// <summary>Relative size in split or stacked scenes.</summary>
    [Parameter]
    [ValidateRange(0.1, 10)]
    public double Weight { get; set; } = 1;

    /// <summary>Exact source text with optional renderer-neutral syntax spans.</summary>
    [Parameter(Mandatory = true, ParameterSetName = SourceSet, ValueFromPipeline = true)]
    public StorySourceText? Source { get; set; }

    /// <summary>Exact source text to tokenize or preserve.</summary>
    [Parameter(Mandatory = true, ParameterSetName = SourceTextSet)]
    [AllowEmptyString]
    public string SourceText { get; set; } = string.Empty;

    /// <summary>Language identifier used for SourceText.</summary>
    [Parameter(ParameterSetName = SourceTextSet)]
    public string Language { get; set; } = "PowerShell";

    /// <summary>Optional language tokenizer for SourceText.</summary>
    [Parameter(ParameterSetName = SourceTextSet)]
    public IStorySourceTokenizer? Tokenizer { get; set; }

    /// <summary>Resolved deterministic terminal presentation.</summary>
    [Parameter(Mandatory = true, ParameterSetName = TerminalSet, ValueFromPipeline = true)]
    public TerminalStory? Terminal { get; set; }

    /// <summary>Raster image file to reveal.</summary>
    [Parameter(Mandatory = true, ParameterSetName = MediaPathSet)]
    public string MediaPath { get; set; } = string.Empty;

    /// <summary>Resolved raster image bytes to reveal.</summary>
    [Parameter(Mandatory = true, ParameterSetName = MediaBytesSet)]
    public byte[]? MediaBytes { get; set; }

    /// <summary>Resolved RGBA image to reveal.</summary>
    [Parameter(Mandatory = true, ParameterSetName = MediaImageSet)]
    public RgbaImage MediaImage { get; set; }

    /// <summary>Optional resolved SVG representation paired with MediaBytes or MediaImage.</summary>
    [Parameter(ParameterSetName = MediaBytesSet)]
    [Parameter(ParameterSetName = MediaImageSet)]
    public string? MediaSvg { get; set; }

    /// <summary>Accessible alternative for terminal or media content.</summary>
    [Parameter(ParameterSetName = TerminalSet, Mandatory = true)]
    [Parameter(ParameterSetName = MediaPathSet, Mandatory = true)]
    [Parameter(ParameterSetName = MediaBytesSet, Mandatory = true)]
    [Parameter(ParameterSetName = MediaImageSet, Mandatory = true)]
    public string AccessibleText { get; set; } = string.Empty;

    /// <summary>Explanatory prose or a short status.</summary>
    [Parameter(Mandatory = true, ParameterSetName = TextSet)]
    public string Text { get; set; } = string.Empty;

    /// <summary>Give text content stronger visual emphasis.</summary>
    [Parameter(ParameterSetName = TextSet)]
    public SwitchParameter Emphasized { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ImageStoryPanelSpec(Id, Title, BuildSurface(), Weight));
    }

    private VisualStorySurface BuildSurface() {
        switch (ParameterSetName) {
            case SourceSet:
                return new VisualStorySourceSurface(Source!, string.IsNullOrWhiteSpace(Title) ? null : Title);
            case SourceTextSet:
                return new VisualStorySourceSurface(
                    Tokenize(SourceText),
                    string.IsNullOrWhiteSpace(Title) ? null : Title);
            case TerminalSet:
                return new VisualStoryTerminalSurface(Terminal!, AccessibleText);
            case MediaPathSet:
                return new VisualStoryMediaSurface(
                    File.ReadAllBytes(PowerShellPathResolver.ResolveFileSystemPath(this, MediaPath)),
                    AccessibleText);
            case MediaBytesSet:
                return new VisualStoryMediaSurface(MediaBytes!, AccessibleText, MediaSvg);
            case MediaImageSet:
                return new VisualStoryMediaSurface(MediaImage, AccessibleText, MediaSvg);
            case TextSet:
                return new VisualStoryTextSurface(Text, Emphasized.IsPresent);
            default:
                throw new PSInvalidOperationException("Unknown visual-story panel parameter set.");
        }
    }

    private StorySourceText Tokenize(string source) {
        if (Tokenizer != null) return Tokenizer.Tokenize(source);
        if (Language.Equals("powershell", System.StringComparison.OrdinalIgnoreCase) ||
            Language.Equals("pwsh", System.StringComparison.OrdinalIgnoreCase) ||
            Language.Equals("ps1", System.StringComparison.OrdinalIgnoreCase)) {
            return new PowerShellStorySourceTokenizer().Tokenize(source);
        }
        if (Language.Equals("plain", System.StringComparison.OrdinalIgnoreCase) ||
            Language.Equals("text", System.StringComparison.OrdinalIgnoreCase)) {
            return StorySourceText.Create(source, Language);
        }
        throw new PSArgumentException("Language '" + Language + "' requires an IStorySourceTokenizer adapter.");
    }
}

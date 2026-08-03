using System.Management.Automation;
using ChartForgeX.Primitives;
using ChartForgeX.Terminal;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a reusable terminal color palette for ImagePlayground console stories and tabs.</summary>
/// <para>Start from a built-in palette and override only the colors that belong to the demo. Window chrome and shell dialect remain separate choices.</para>
/// <example>
///   <summary>Create a branded Ubuntu-derived palette</summary>
///   <prefix>PS&gt; </prefix>
///   <code>$palette = New-ImageConsoleStoryPalette -Preset Ubuntu -Background '#24071B' -Accent '#FF6A2B'</code>
///   <para>Returns a ChartForgeX TerminalTheme that can be supplied through the Palette parameter.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageConsoleStoryPalette")]
[OutputType(typeof(TerminalTheme))]
public sealed class NewImageConsoleStoryPaletteCmdlet : PSCmdlet {
    /// <summary>Built-in palette used as the customization baseline.</summary>
    [Parameter]
    [ValidateSet("Dark", "PowerShell", "WindowsPowerShell", "Ubuntu", "Campbell", "Classic", "Light")]
    public string Preset { get; set; } = "Dark";

    /// <summary>Outer page color in hexadecimal notation.</summary>
    [Parameter]
    public string? PageBackground { get; set; }

    /// <summary>Terminal content color in hexadecimal notation.</summary>
    [Parameter]
    public string? Background { get; set; }

    /// <summary>Title-bar color in hexadecimal notation.</summary>
    [Parameter]
    public string? HeaderBackground { get; set; }

    /// <summary>Frame and divider color in hexadecimal notation.</summary>
    [Parameter]
    public string? Border { get; set; }

    /// <summary>Normal text color in hexadecimal notation.</summary>
    [Parameter]
    public string? Text { get; set; }

    /// <summary>Muted text color in hexadecimal notation.</summary>
    [Parameter]
    public string? Muted { get; set; }

    /// <summary>Prompt and command accent color in hexadecimal notation.</summary>
    [Parameter]
    public string? Accent { get; set; }

    /// <summary>Success text color in hexadecimal notation.</summary>
    [Parameter]
    public string? Success { get; set; }

    /// <summary>Warning text color in hexadecimal notation.</summary>
    [Parameter]
    public string? Warning { get; set; }

    /// <summary>Error text color in hexadecimal notation.</summary>
    [Parameter]
    public string? Error { get; set; }

    /// <summary>Cursor color in hexadecimal notation.</summary>
    [Parameter]
    public string? Cursor { get; set; }

    /// <summary>CSS font-family stack shared by every tab in the story.</summary>
    [Parameter]
    public string? FontFamily { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var palette = ConsoleStoryPaletteResolver.Resolve(Preset);
        if (!string.IsNullOrWhiteSpace(PageBackground)) {
            palette.PageBackground = Parse(PageBackground!);
        }
        if (!string.IsNullOrWhiteSpace(Background)) {
            palette.Background = Parse(Background!);
        }
        if (!string.IsNullOrWhiteSpace(HeaderBackground)) {
            palette.HeaderBackground = Parse(HeaderBackground!);
        }
        if (!string.IsNullOrWhiteSpace(Border)) {
            palette.Border = Parse(Border!);
        }
        if (!string.IsNullOrWhiteSpace(Text)) {
            palette.Text = Parse(Text!);
        }
        if (!string.IsNullOrWhiteSpace(Muted)) {
            palette.Muted = Parse(Muted!);
        }
        if (!string.IsNullOrWhiteSpace(Accent)) {
            palette.Accent = Parse(Accent!);
        }
        if (!string.IsNullOrWhiteSpace(Success)) {
            palette.Success = Parse(Success!);
        }
        if (!string.IsNullOrWhiteSpace(Warning)) {
            palette.Warning = Parse(Warning!);
        }
        if (!string.IsNullOrWhiteSpace(Error)) {
            palette.Error = Parse(Error!);
        }
        if (!string.IsNullOrWhiteSpace(Cursor)) {
            palette.Cursor = Parse(Cursor!);
        }
        if (!string.IsNullOrWhiteSpace(FontFamily)) {
            palette.FontFamily = FontFamily!;
        }
        WriteObject(palette);
    }

    private static ChartColor Parse(string value) => ChartColor.FromHex(value);
}

using System.Management.Automation;
using ChartForgeX.Primitives;
using ChartForgeX.Themes;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a reusable terminal color palette for ImagePlayground console stories and tabs.</summary>
/// <para>Start from a built-in palette and override only the colors that belong to the demo. Window chrome and shell dialect remain separate choices.</para>
/// <example>
///   <summary>Create a branded Ubuntu-derived palette</summary>
///   <prefix>PS&gt; </prefix>
///   <code>$palette = New-ImageConsoleStoryPalette -Preset Ubuntu -Background '#24071B' -Accent '#FF6A2B'</code>
///   <para>Returns a paired immutable VisualTheme that can be supplied through Palette. Select its appearance with ThemeMode on the story or tab.</para>
/// </example>
/// <example>
///   <summary>Keep cursor styling separate from the palette</summary>
///   <code>$palette = New-ImageConsoleStoryPalette -Preset Ubuntu -Accent '#FF6A2B'
/// $style = [ChartForgeX.Terminal.TerminalStyleOptions]::new()
/// $style.Cursor = [ChartForgeX.Primitives.ChartColor]::FromHex('#FF6A2B')
/// New-ImageConsoleStory -Palette $palette -Style $style -Content { New-ImageConsoleStoryOutput -Text Ready }</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageConsoleStoryPalette")]
[OutputType(typeof(VisualTheme))]
public sealed class NewImageConsoleStoryPaletteCmdlet : PSCmdlet {
    /// <summary>Built-in palette used as the customization baseline. Light and Dark use the canonical ChartForgeX tokens; shell-specific presets remain available.</summary>
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

    /// <summary>CSS font-family stack shared by every tab in the story.</summary>
    [Parameter]
    public string? FontFamily { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var palette = ConsoleStoryPaletteResolver.Resolve(Preset).Configure(builder => {
            ApplyOverrides(builder.Light);
            ApplyOverrides(builder.Dark);
            if (!string.IsNullOrWhiteSpace(FontFamily)) {
                var fonts = builder.Typography;
                builder.Typography = new VisualTypography(fonts.Family, fonts.TitleSize, fonts.SubtitleSize,
                    fonts.AxisSize, fonts.LegendSize, fonts.DataLabelSize, fonts.ScalarValueSize,
                    fonts.CenterValueSize, fonts.AxisTitleSize, FontFamily!);
            }
        });
        WriteObject(palette);
    }

    private void ApplyOverrides(VisualDesignTokens colors) {
        if (!string.IsNullOrWhiteSpace(PageBackground)) colors.Background = Parse(PageBackground!);
        if (!string.IsNullOrWhiteSpace(Background)) colors.ElevatedSurface = Parse(Background!);
        if (!string.IsNullOrWhiteSpace(HeaderBackground)) colors.Surface = Parse(HeaderBackground!);
        if (!string.IsNullOrWhiteSpace(Border)) colors.Border = Parse(Border!);
        if (!string.IsNullOrWhiteSpace(Text)) colors.Foreground = Parse(Text!);
        if (!string.IsNullOrWhiteSpace(Muted)) colors.MutedForeground = Parse(Muted!);
        if (!string.IsNullOrWhiteSpace(Accent)) colors.Accent = Parse(Accent!);
        if (!string.IsNullOrWhiteSpace(Success)) {
            var color = Parse(Success!);
            colors.Status.Pass = new VisualTokenColor(color, color);
        }
        if (!string.IsNullOrWhiteSpace(Warning)) {
            var color = Parse(Warning!);
            colors.Status.Medium = new VisualTokenColor(color, color);
        }
        if (!string.IsNullOrWhiteSpace(Error)) {
            var color = Parse(Error!);
            colors.Status.Critical = new VisualTokenColor(color, color);
        }
    }

    private static ChartColor Parse(string value) => ChartColor.FromHex(value);
}

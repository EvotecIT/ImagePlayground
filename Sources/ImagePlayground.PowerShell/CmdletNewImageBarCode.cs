using ImagePlayground;
using CodeGlyphX;
using System.Management.Automation;
using System.Threading.Tasks;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a barcode image.</summary>
/// <para>The file extension picks the format (.png, .svg, .jpg, .webp and others). PNG output is compressed.</para>
/// <example>
///   <summary>Create barcode</summary>
///   <code>New-ImageBarCode -Type EAN -Value 9012341234571 -FilePath barcode.png</code>
/// </example>
/// <example>
///   <summary>Asset tag with the value printed underneath</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageBarCode -Type Code128 -Value 'EVO-LAP-0142' -FilePath tag.png -ShowText -ModuleSize 3 -Height 30</code>
///   <para>-ShowText prints the encoded value under linear symbols; -Text prints custom text instead.</para>
/// </example>
/// <example>
///   <summary>Brand colours for a 2D code</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageBarCode -Type DataMatrix -Value 'LOT-2026-0042' -FilePath lot.png -ModuleSize 8 -ForegroundColor '#1e1b4b'</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageBarCode")]
public sealed class NewImageBarCodeCmdlet : AsyncImageCmdlet {
    /// <summary>Physical barcode format from the CodeGlyphX catalogue.</summary>
    /// <para>QR formats use New-ImageQRCode. MaxiCode requires specialized rendering, and GS1 Composite requires separate payloads; neither is supported by this single-value command.</para>
    [Parameter(Mandatory = true, Position = 0)]
    public SymbolFormat Type { get; set; }

    /// <summary>Value encoded in the barcode.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public string Value { get; set; } = string.Empty;

    /// <summary>Output path for the barcode image.</summary>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 2)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Pixel width of one module: the narrowest bar for linear symbols, one cell for 2D symbols.</summary>
    [Parameter]
    [ValidateRange(1, 100)]
    public int ModuleSize { get; set; }

    /// <summary>Quiet zone (blank border) in modules.</summary>
    [Parameter]
    [ValidateRange(0, 100)]
    public int QuietZone { get; set; }

    /// <summary>Bar height in modules. Linear symbols only.</summary>
    [Parameter]
    [ValidateRange(1, 1000)]
    public int Height { get; set; }

    /// <summary>Print the encoded value under the bars. Linear symbols only.</summary>
    [Parameter]
    public SwitchParameter ShowText { get; set; }

    /// <summary>Custom human-readable text printed under the bars. Linear symbols only.</summary>
    [Parameter]
    public string? Text { get; set; }

    /// <summary>Bar colour.</summary>
    [Parameter]
    public OfficeIMO.Drawing.OfficeColor ForegroundColor { get; set; } = OfficeIMO.Drawing.OfficeColor.Black;

    /// <summary>Background colour.</summary>
    [Parameter]
    public OfficeIMO.Drawing.OfficeColor BackgroundColor { get; set; } = OfficeIMO.Drawing.OfficeColor.White;

    /// <inheritdoc />
    protected override async Task ProcessRecordAsync() {
        var output = PowerShellPathResolver.ResolveFileSystemPath(this, FilePath);
        var bound = MyInvocation.BoundParameters;
        var style = new BarCodeStyle {
            ModuleSize = bound.ContainsKey(nameof(ModuleSize)) ? ModuleSize : null,
            QuietZone = bound.ContainsKey(nameof(QuietZone)) ? QuietZone : null,
            Height = bound.ContainsKey(nameof(Height)) ? Height : null,
            ShowText = ShowText.IsPresent,
            Text = Text,
            ForegroundColor = bound.ContainsKey(nameof(ForegroundColor)) ? ForegroundColor : null,
            BackgroundColor = bound.ContainsKey(nameof(BackgroundColor)) ? BackgroundColor : null
        };
        await BarCode.GenerateAsync(Type, Value, output, style, CancelToken).ConfigureAwait(false);
    }
}

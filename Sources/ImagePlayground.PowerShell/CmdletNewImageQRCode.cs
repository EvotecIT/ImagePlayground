using System;
using ImagePlayground;
using CodeGlyphX;
using CodeGlyphX.Rendering;
using System.Management.Automation;
using System.Threading.Tasks;

namespace ImagePlayground.PowerShell;

/// <summary>Generates a QR code image from plain text content.</summary>
/// <para>Use this cmdlet when the QR payload is already available as a raw string.</para>
/// <para>Styled artwork and budgeted logos must pass the native CodeGlyphX guardrails. The limited decoder used by Windows PowerShell 5.1 can reject artwork accepted by PowerShell 7. Rendering then throws rather than writing unverified output; simplify the design or validate it with a modern runtime.</para>
/// <example>
///   <summary>Create QR code</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageQRCode -Content 'https://evotec.xyz' -FilePath qr.png</code>
/// </example>
/// <example>
///   <summary>Show QR code after generation</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageQRCode -Content 'text' -FilePath qr.png -Show</code>
/// </example>
/// <example>
///   <summary>Create branded QR code</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageQRCode -Content 'https://evotec.xyz' -FilePath qr-logo.png -LogoPath logo.png</code>
/// </example>
/// <example>
///   <summary>Style modules and eyes, and size the image for print</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageQRCode -Content 'https://evotec.xyz' -FilePath styled.png -ModuleShape Rounded -EyeShape Circle -EyeColor '#4c1d95' -ForegroundColor '#1e1b4b' -Size 600 -ErrorCorrection H</code>
///   <para>Rounded data modules, circular eyes in an accent colour, and an image at most 600 px wide. High error correction leaves room for styling and logos.</para>
/// </example>
/// <example>
///   <summary>Generate one label per asset from the pipeline</summary>
///   <prefix>PS&gt; </prefix>
///   <code>'EVO-LAP-0142', 'EVO-LAP-0177' | ForEach-Object { New-ImageQRCode -Content "https://assets.example.com/$_" -FilePath "$_.png" -QuietZone 2 -Size 300 }</code>
///   <para>PNG output is compressed; a typical URL code at 300 px is about 1 KB.</para>
/// </example>
/// <example>
///   <summary>Use a coordinated look with a caption</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageQRCode -Content 'https://example.com/menu' -FilePath menu.png -Preset Sunset -Frame ScanMePill -Caption 'VIEW THE MENU' -ErrorCorrection H</code>
/// </example>
/// <example>
///   <summary>Export styled SVG with separate finder shapes</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageQRCode -Content 'https://example.com' -FilePath styled.svg -Preset Ink -EyeShape Rounded -EyeBall Circle -ErrorCorrection H</code>
/// </example>
/// <example>
///   <summary>Apply two colours and a budgeted logo</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageQRCode -Content 'https://example.com' -FilePath brand.png -Preset Brand -Gradient '#431b1b', '#8c2059' -Logo logo.png -LogoScale 0.3 -ErrorCorrection H</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageQRCode")]
[Alias("New-QRCode")]
public sealed class NewImageQrCodeCmdlet : AsyncQrCodeCmdlet {
    /// <summary>Content encoded in the QR code.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public string Content { get; set; } = string.Empty;

    /// <summary>Output path for the QR code image.</summary>
    /// <para>The image format is inferred from the file extension.</para>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 1)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Open the image after creation.</summary>
    [Parameter]
    public SwitchParameter Show { get; set; }

    /// <summary>Optional logo placed near the centre while preserving functional patterns.</summary>
    /// <para>CodeGlyphX sizes the padded logo against each affected correction block. The default size limit is one eighth of the image width; -LogoScale changes the limit. Unreadable or invisible artwork fails before replacing the output. Raster and SVG formats use the same placement.</para>
    [Parameter]
    [Alias("Logo")]
    public string? LogoPath { get; set; }

    /// <summary>Coordinated QR appearance, such as Ink, Sunset, Ocean or Brand.</summary>
    [Parameter]
    public QrHeroPreset Preset { get; set; }

    /// <summary>Shape of the finder centres, independently of -EyeShape.</summary>
    [Parameter]
    public QrModuleShape EyeBall { get; set; }

    /// <summary>Two foreground colours, from left to right, for example '#9c3411', '#8c2059'.</summary>
    [Parameter]
    [ValidateCount(2, 2)]
    public OfficeIMO.Drawing.OfficeColor[]? Gradient { get; set; }

    /// <summary>Frame template outside the complete QR quiet zone.</summary>
    [Parameter]
    public QrCanvasTemplate Frame { get; set; }

    /// <summary>Caption below the code. A frame uses SCAN ME when no caption is supplied.</summary>
    [Parameter]
    public string? Caption { get; set; }

    /// <summary>Create the QR code with a transparent background.</summary>
    [Parameter]
    public SwitchParameter Transparent { get; set; }

    /// <summary>Foreground color of QR modules.</summary>
    [Parameter]
    public OfficeIMO.Drawing.OfficeColor ForegroundColor { get; set; } = OfficeIMO.Drawing.OfficeColor.Black;

    /// <summary>Background color of the QR code.</summary>
    [Parameter]
    public OfficeIMO.Drawing.OfficeColor BackgroundColor { get; set; } = OfficeIMO.Drawing.OfficeColor.White;

    /// <summary>Pixel size for each QR module. Ignored when -Size is supplied.</summary>
    [Parameter]
    [ValidateRange(1, 1000)]
    public int PixelSize { get; set; } = 20;

    /// <summary>Target image width and height in pixels, including the quiet zone.</summary>
    /// <para>The module size is the largest whole number of pixels that fits, so the image can be a few pixels smaller than requested.</para>
    [Parameter]
    [ValidateRange(21, 20000)]
    public int Size { get; set; }

    /// <summary>Quiet zone (blank border) in modules. Defaults to 4, the size scanners expect.</summary>
    [Parameter]
    [ValidateRange(0, 40)]
    public int QuietZone { get; set; } = 4;

    /// <summary>Error correction level: L (7%), M (15%), Q (25%, the default), or H (30%).</summary>
    /// <para>Higher levels survive more damage, logos, and styling at the cost of a denser code.</para>
    [Parameter]
    public QrErrorCorrectionLevel ErrorCorrection { get; set; } = QrErrorCorrectionLevel.Q;

    /// <summary>Shape of the data modules, for example Square, Rounded, Circle, Dot, or Diamond.</summary>
    /// <para>Finder and timing patterns stay readable. SVG supports the coordinated shapes through vector coverage contours.</para>
    [Parameter]
    public QrModuleShape ModuleShape { get; set; } = QrModuleShape.Square;

    /// <summary>Shape of the three finder patterns (eyes), for example Square, Rounded, or Circle.</summary>
    [Parameter]
    public QrModuleShape EyeShape { get; set; } = QrModuleShape.Square;

    /// <summary>Colour of the three finder patterns (eyes).</summary>
    [Parameter]
    public OfficeIMO.Drawing.OfficeColor EyeColor { get; set; } = OfficeIMO.Drawing.OfficeColor.Black;

    /// <summary>Logo width as a fraction of the symbol width (0.05 to 0.4). Defaults to one eighth of the image. Use -ErrorCorrection H for larger logos.</summary>
    [Parameter]
    [ValidateRange(0.05D, 0.4D)]
    public double LogoScale { get; set; }

    /// <inheritdoc />
    protected override async Task ProcessRecordAsync() {
        ValidatePixelSize(PixelSize);
        FilePath = EnsureQrOutputPath(FilePath);

        string? logoPath = string.IsNullOrWhiteSpace(LogoPath) ? null : LogoPath;
        if (logoPath is not null) {
            logoPath = ResolveExistingFilePath(logoPath, "NewImageQrCodeLogoFileNotFound", logoPath);
            ValidateLogoImage(logoPath);
        }

        var bound = MyInvocation.BoundParameters;
        var style = new QrImageStyle {
            Transparent = Transparent.IsPresent,
            ForegroundColor = bound.ContainsKey(nameof(ForegroundColor)) ? ForegroundColor : null,
            BackgroundColor = bound.ContainsKey(nameof(BackgroundColor)) ? BackgroundColor : null,
            PixelSize = PixelSize,
            Size = bound.ContainsKey(nameof(Size)) ? Size : null,
            QuietZone = bound.ContainsKey(nameof(QuietZone)) ? QuietZone : null,
            ModuleShape = bound.ContainsKey(nameof(ModuleShape)) ? ModuleShape : null,
            EyeShape = bound.ContainsKey(nameof(EyeShape)) ? EyeShape : null,
            EyeColor = bound.ContainsKey(nameof(EyeColor)) ? EyeColor : null,
            LogoScale = bound.ContainsKey(nameof(LogoScale)) ? LogoScale : null,
            Preset = bound.ContainsKey(nameof(Preset)) ? Preset : null,
            EyeBall = bound.ContainsKey(nameof(EyeBall)) ? EyeBall : null,
            Gradient = Gradient,
            Frame = bound.ContainsKey(nameof(Frame)) ? Frame : null,
            Caption = Caption
        };

        await ImagePlayground.QrCode.GenerateAsync(Content, FilePath, style, ErrorCorrection, logoPath, CancelToken).ConfigureAwait(false);

        ShowGeneratedQrCode(FilePath, Show);
    }

    private void ValidateLogoImage(string logoPath) {
        try {
            using var _ = ImagePlayground.Image.Load(logoPath);
        } catch (Exception ex) {
            var exception = new InvalidDataException($"Logo file is not a readable image: {logoPath}", ex);
            ThrowTerminatingError(new ErrorRecord(exception, "NewImageQrCodeLogoFileInvalid", ErrorCategory.InvalidData, LogoPath));
        }
    }
}

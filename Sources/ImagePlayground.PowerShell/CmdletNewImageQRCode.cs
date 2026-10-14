using System;
using ImagePlayground;
using CodeGlyphX;
using CodeGlyphX.Rendering;
using System.Management.Automation;
using System.Threading.Tasks;

namespace ImagePlayground.PowerShell;

/// <summary>Generates a QR code image from plain text content.</summary>
/// <para>Use this cmdlet when the QR payload is already available as a raw string.</para>
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

    /// <summary>Optional logo image to place at the center of the QR code.</summary>
    /// <para>The logo fits within one eighth of the image width, retaining its aspect ratio. It is normalized before CodeGlyphX embeds it in raster or SVG output, without a background plate. Invalid logos leave an existing destination intact.</para>
    [Parameter]
    public string? LogoPath { get; set; }

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
    /// <para>Finder and timing patterns stay readable. ConnectedRounded and ConnectedSquircle are raster-only and cannot be written to .svg or .html.</para>
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
            ForegroundColor = ForegroundColor,
            BackgroundColor = BackgroundColor,
            PixelSize = PixelSize,
            Size = bound.ContainsKey(nameof(Size)) ? Size : null,
            QuietZone = bound.ContainsKey(nameof(QuietZone)) ? QuietZone : null,
            ModuleShape = bound.ContainsKey(nameof(ModuleShape)) ? ModuleShape : null,
            EyeShape = bound.ContainsKey(nameof(EyeShape)) ? EyeShape : null,
            EyeColor = bound.ContainsKey(nameof(EyeColor)) ? EyeColor : null,
            LogoScale = bound.ContainsKey(nameof(LogoScale)) ? LogoScale : null
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

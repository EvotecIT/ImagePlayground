using System;
using ImagePlayground;
using CodeGlyphX;
using System.IO;
using System.Management.Automation;

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
[Cmdlet(VerbsCommon.New, "ImageQRCode")]
[Alias("New-QRCode")]
public sealed class NewImageQrCodeCmdlet : PSCmdlet {
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

    /// <summary>Create the QR code with a transparent background.</summary>
    [Parameter]
    public SwitchParameter Transparent { get; set; }

    /// <summary>Foreground color of QR modules.</summary>
    [Parameter]
    public SixLabors.ImageSharp.Color ForegroundColor { get; set; } = SixLabors.ImageSharp.Color.Black;

    /// <summary>Background color of the QR code.</summary>
    [Parameter]
    public SixLabors.ImageSharp.Color BackgroundColor { get; set; } = SixLabors.ImageSharp.Color.White;

    /// <summary>Pixel size for each QR module.</summary>
    [Parameter]
    public int PixelSize { get; set; } = 20;

    /// <inheritdoc />
    protected override void ProcessRecord() {
        if (PixelSize <= 0) {
            throw new ArgumentOutOfRangeException(nameof(PixelSize));
        }

        if (string.IsNullOrWhiteSpace(FilePath)) {
            FilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName().Split('.')[0] + ".png");
            WriteWarning($"New-ImageQRCode - No file path specified, saving to {FilePath}");
        }

        ImagePlayground.QrCode.Generate(Content, FilePath, Transparent.IsPresent, QrErrorCorrectionLevel.Q, ForegroundColor, BackgroundColor, PixelSize);

        if (Show.IsPresent) {
            ImagePlayground.Helpers.Open(Helpers.ResolvePath(FilePath), true);
        }
    }
}

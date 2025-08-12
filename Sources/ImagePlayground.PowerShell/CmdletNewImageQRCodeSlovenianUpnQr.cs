using System;
using ImagePlayground;
using QRCoder;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Generates a Slovenian UPN QR payment code.</summary>
/// <example>
///   <summary>Create UPN QR</summary>
///   <code>$upn = [QRCoder.PayloadGenerator+SlovenianUpnQr]::new('Payer','Addr','City','Rec','RAddr','RCity','SI123','Desc',1);New-ImageQRCodeSlovenianUpnQr -Payload $upn -FilePath upn.png</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageQRCodeSlovenianUpnQr")]
public sealed class NewImageQrCodeSlovenianUpnQrCmdlet : PSCmdlet {
    /// <summary>UPN payment payload.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public PayloadGenerator.SlovenianUpnQr Payload { get; set; } = null!;

    /// <summary>Location of the output image.</summary>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 1)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Opens the image after creation.</summary>
    [Parameter]
    public SwitchParameter Show { get; set; }

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
            WriteWarning($"New-ImageQRCodeSlovenianUpnQr - No file path specified, saving to {FilePath}");
        }

        var output = Helpers.ResolvePath(FilePath);
        ImagePlayground.QrCode.GenerateSlovenianUpnQr(Payload, output, false, ForegroundColor, BackgroundColor, PixelSize);

        if (Show.IsPresent) {
            ImagePlayground.Helpers.Open(output, true);
        }
    }
}

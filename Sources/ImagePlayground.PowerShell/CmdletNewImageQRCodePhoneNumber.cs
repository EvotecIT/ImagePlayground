using System;
using ImagePlayground;
using QRCoder;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Generates a QR code for dialling a phone number.</summary>
/// <example>
///   <summary>Create phone number QR</summary>
///   <code>New-ImageQRCodePhoneNumber -Number '+123456' -FilePath phone.png</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageQRCodePhoneNumber")]
public sealed class NewImageQrCodePhoneNumberCmdlet : PSCmdlet {
    /// <summary>Phone number to dial.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public string Number { get; set; } = string.Empty;

    /// <summary>Output path of the QR code image.</summary>
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
            WriteWarning($"New-ImageQRCodePhoneNumber - No file path specified, saving to {FilePath}");
        }

        var output = Helpers.ResolvePath(FilePath);
        Helpers.CreateParentDirectory(output);
        ImagePlayground.QrCode.GeneratePhoneNumber(Number, output, false, ForegroundColor, BackgroundColor, PixelSize);

        if (Show.IsPresent) {
            ImagePlayground.Helpers.Open(output, true);
        }
    }
}

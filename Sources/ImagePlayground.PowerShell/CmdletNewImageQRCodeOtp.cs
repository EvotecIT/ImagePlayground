using ImagePlayground;
using QRCoder;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Generates a QR code for one-time-password configuration.</summary>
/// <example>
///   <summary>Create OTP QR</summary>
///   <code>$otp = [QRCoder.PayloadGenerator+OneTimePassword]::new();$otp.Secret='ABC';$otp.Label='user';New-ImageQRCodeOtp -Payload $otp -FilePath otp.png</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageQRCodeOtp")]
public sealed class NewImageQrCodeOtpCmdlet : PSCmdlet {
    /// <summary>OTP payload object.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public PayloadGenerator.OneTimePassword Payload { get; set; } = null!;

    /// <summary>Path to save the QR code image.</summary>
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
        if (string.IsNullOrWhiteSpace(FilePath)) {
            FilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName().Split('.')[0] + ".png");
            WriteWarning($"New-ImageQRCodeOtp - No file path specified, saving to {FilePath}");
        }

        var output = Helpers.ResolvePath(FilePath);
        ImagePlayground.QrCode.GenerateOneTimePassword(Payload, output, false, ForegroundColor, BackgroundColor, PixelSize);

        if (Show.IsPresent) {
            ImagePlayground.Helpers.Open(output, true);
        }
    }
}

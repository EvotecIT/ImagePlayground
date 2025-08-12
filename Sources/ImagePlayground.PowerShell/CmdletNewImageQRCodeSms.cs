using System;
using ImagePlayground;
using QRCoder;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Generates a QR code containing an SMS message.</summary>
/// <example>
///   <summary>Create SMS QR</summary>
///   <code>New-ImageQRCodeSms -Number '+123456789' -Message 'Hello' -FilePath sms.png</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageQRCodeSms")]
public sealed class NewImageQrCodeSmsCmdlet : PSCmdlet {
    /// <summary>Recipient phone number.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public string Number { get; set; } = string.Empty;

    /// <summary>Text message.</summary>
    [Parameter(Position = 1)]
    public string? Message { get; set; }

    /// <summary>Output path for the QR code image.</summary>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 2)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Open the image after creation.</summary>
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
            WriteWarning($"New-ImageQRCodeSms - No file path specified, saving to {FilePath}");
        }

        var output = Helpers.ResolvePath(FilePath);
        Helpers.CreateParentDirectory(output);
        ImagePlayground.QrCode.GenerateSms(Number, Message, output, PayloadGenerator.SMS.SMSEncoding.SMS, false, ForegroundColor, BackgroundColor, PixelSize);

        if (Show.IsPresent) {
            ImagePlayground.Helpers.Open(output, true);
        }
    }
}

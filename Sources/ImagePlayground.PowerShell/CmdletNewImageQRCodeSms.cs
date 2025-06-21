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
    [Parameter(Mandatory = true, Position = 2)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Open the image after creation.</summary>
    [Parameter]
    public SwitchParameter Show { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        if (string.IsNullOrWhiteSpace(FilePath)) {
            FilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName().Split('.')[0] + ".png");
            WriteWarning($"New-ImageQRCodeSms - No file path specified, saving to {FilePath}");
        }

        var output = Helpers.ResolvePath(FilePath);
        ImagePlayground.QrCode.GenerateSms(Number, Message, output, PayloadGenerator.SMS.SMSEncoding.SMS, false);

        if (Show.IsPresent) {
            ImagePlayground.Helpers.Open(output, true);
        }
    }
}

using System;
using ImagePlayground;
using CodeGlyphX.Payloads;
using System.Management.Automation;
using System.Threading.Tasks;

namespace ImagePlayground.PowerShell;

/// <summary>Generates a QR code containing an SMS message.</summary>
/// <para>Use this cmdlet when a scan should open the SMS app with recipient and optional message body prefilled.</para>
/// <example>
///   <summary>Create a basic SMS QR code</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageQRCodeSms -Number '+123456789' -Message 'Hello' -FilePath sms.png</code>
///   <para>Creates a QR code that opens the SMS app with the recipient number and message body prefilled.</para>
/// </example>
/// <example>
///   <summary>Create an RSVP SMS QR code</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageQRCodeSms -Number '+48 500 600 700' -Message 'RSVP: I will attend' -FilePath rsvp-sms.png -ForegroundColor Teal -PixelSize 16 -Show</code>
///   <para>Generates a ready-to-send RSVP QR code for invitations or registration desks.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageQRCodeSms")]
public sealed class NewImageQrCodeSmsCmdlet : AsyncQrCodeCmdlet {
    /// <summary>Recipient phone number.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public string Number { get; set; } = string.Empty;

    /// <summary>Text message.</summary>
    [Parameter(Position = 1)]
    public string? Message { get; set; }

    /// <summary>Output path for the QR code image.</summary>
    /// <para>The image format is inferred from the file extension.</para>
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

    /// <summary>Use asynchronous processing.</summary>
    [Parameter]
    public SwitchParameter Async { get; set; }

    /// <inheritdoc />
    protected override async Task ProcessRecordAsync() {
        ValidatePixelSize(PixelSize);
        FilePath = EnsureQrOutputPath(FilePath);

        if (Async.IsPresent) {
            await ImagePlayground.QrCode.GenerateSmsAsync(Number, Message, FilePath, QrSmsEncoding.Sms, false, ForegroundColor, BackgroundColor, PixelSize, CancelToken).ConfigureAwait(false);
        } else {
            ImagePlayground.QrCode.GenerateSms(Number, Message, FilePath, QrSmsEncoding.Sms, false, ForegroundColor, BackgroundColor, PixelSize);
        }

        ShowGeneratedQrCode(FilePath, Show);
    }
}

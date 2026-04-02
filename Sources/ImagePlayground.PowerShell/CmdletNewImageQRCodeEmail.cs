using System;
using ImagePlayground;
using CodeGlyphX.Payloads;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Generates a QR code that opens an email draft.</summary>
/// <para>Use this cmdlet to create scannable mailto-style QR codes for support, sales, or campaign responses.</para>
/// <example>
///   <summary>Create a basic email QR code</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageQRCodeEmail -Email 'user@example.com' -Subject 'Hello' -Message 'Body' -FilePath qr.png</code>
///   <para>Creates a QR code that opens the default mail client with recipient, subject, and body prefilled.</para>
/// </example>
/// <example>
///   <summary>Create a support-contact QR code</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageQRCodeEmail -Email 'support@evotec.pl' -Subject 'Support Request' -Message 'Please describe the issue before sending.' -FilePath support-mail.png -ForegroundColor DarkSlateBlue -PixelSize 14 -Show</code>
///   <para>Generates a support-oriented email QR code and opens the image after creation.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageQRCodeEmail")]
public sealed class NewImageQrCodeEmailCmdlet : PSCmdlet {
    /// <summary>Recipient email address.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public string Email { get; set; } = string.Empty;

    /// <summary>Message subject.</summary>
    [Parameter(Position = 1)]
    public string? Subject { get; set; }

    /// <summary>Message body.</summary>
    [Parameter(Position = 2)]
    public string? Message { get; set; }

    /// <summary>Output path for the QR code image.</summary>
    /// <para>The image format is inferred from the file extension.</para>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 3)]
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
            WriteWarning($"New-ImageQRCodeEmail - No file path specified, saving to {FilePath}");
        }

        ImagePlayground.QrCode.GenerateEmail(FilePath, Email, Subject, Message, QrMailEncoding.Mailto, false, ForegroundColor, BackgroundColor, PixelSize);

        if (Show.IsPresent) {
            ImagePlayground.Helpers.Open(Helpers.ResolvePath(FilePath), true);
        }
    }
}

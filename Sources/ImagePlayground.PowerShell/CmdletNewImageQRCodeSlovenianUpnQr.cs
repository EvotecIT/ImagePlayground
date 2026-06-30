using System;
using ImagePlayground;
using CodeGlyphX.Payloads;
using System.Management.Automation;
using System.Threading.Tasks;

namespace ImagePlayground.PowerShell;

/// <summary>Generates a Slovenian UPN QR payment code.</summary>
/// <para>Use this cmdlet to render Slovenian UPN payment details into a scannable QR image.</para>
/// <example>
///   <summary>Create a Slovenian UPN QR payment code</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageQRCodeSlovenianUpnQr -PayerName 'John Doe' -PayerAddress 'Main Street 1' -PayerPlace 'Ljubljana' -RecipientName 'Evotec d.o.o.' -RecipientAddress 'Business Street 2' -RecipientPlace 'Maribor' -RecipientIban 'SI56192001234567890' -Description 'Invoice 2026-041' -Amount 199.99 -FilePath upn.png</code>
///   <para>Generates a UPN payment QR code from the payment fields supplied to the cmdlet.</para>
/// </example>
/// <example>
///   <summary>Create a branded UPN QR code and preview it</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageQRCodeSlovenianUpnQr -PayerName 'John Doe' -PayerAddress 'Main Street 1' -PayerPlace 'Ljubljana' -RecipientName 'Evotec d.o.o.' -RecipientAddress 'Business Street 2' -RecipientPlace 'Maribor' -RecipientIban 'SI56192001234567890' -Description 'Annual subscription' -Amount 49 -FilePath upn-brand.png -ForegroundColor DarkGreen -PixelSize 14 -Show</code>
///   <para>Creates a styled payment QR code and opens the resulting image after generation.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageQRCodeSlovenianUpnQr")]
public sealed class NewImageQrCodeSlovenianUpnQrCmdlet : AsyncQrCodeCmdlet {
    /// <summary>Payer name.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public string PayerName { get; set; } = string.Empty;

    /// <summary>Payer street address.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public string PayerAddress { get; set; } = string.Empty;

    /// <summary>Payer postal place.</summary>
    [Parameter(Mandatory = true, Position = 2)]
    public string PayerPlace { get; set; } = string.Empty;

    /// <summary>Recipient name.</summary>
    [Parameter(Mandatory = true, Position = 3)]
    public string RecipientName { get; set; } = string.Empty;

    /// <summary>Recipient street address.</summary>
    [Parameter(Mandatory = true, Position = 4)]
    public string RecipientAddress { get; set; } = string.Empty;

    /// <summary>Recipient postal place.</summary>
    [Parameter(Mandatory = true, Position = 5)]
    public string RecipientPlace { get; set; } = string.Empty;

    /// <summary>Recipient IBAN.</summary>
    [Parameter(Mandatory = true, Position = 6)]
    public string RecipientIban { get; set; } = string.Empty;

    /// <summary>Payment description.</summary>
    [Parameter(Mandatory = true, Position = 7)]
    public string Description { get; set; } = string.Empty;

    /// <summary>Payment amount.</summary>
    [Parameter(Mandatory = true, Position = 8)]
    public double Amount { get; set; }

    /// <summary>Location of the output image.</summary>
    /// <para>The image format is inferred from the file extension.</para>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 9)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Payment deadline.</summary>
    [Parameter]
    public DateTime? Deadline { get; set; }

    /// <summary>Recipient SI model.</summary>
    [Parameter]
    public string RecipientSiModel { get; set; } = "SI00";

    /// <summary>Recipient SI reference.</summary>
    [Parameter]
    public string RecipientSiReference { get; set; } = string.Empty;

    /// <summary>UPN payment code.</summary>
    [Parameter]
    public string Code { get; set; } = "OTHR";

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

    /// <summary>Use asynchronous processing.</summary>
    [Parameter]
    public SwitchParameter Async { get; set; }

    /// <inheritdoc />
    protected override async Task ProcessRecordAsync() {
        ValidatePixelSize(PixelSize);
        FilePath = EnsureQrOutputPath(FilePath);

        var payload = new SlovenianUpnQrPayload(PayerName, PayerAddress, PayerPlace, RecipientName, RecipientAddress, RecipientPlace, RecipientIban, Description, Amount, Deadline, RecipientSiModel, RecipientSiReference, Code);

        if (Async.IsPresent) {
            await ImagePlayground.QrCode.GenerateSlovenianUpnQrAsync(payload, FilePath, false, ForegroundColor, BackgroundColor, PixelSize, CancelToken).ConfigureAwait(false);
        } else {
            ImagePlayground.QrCode.GenerateSlovenianUpnQr(payload, FilePath, false, ForegroundColor, BackgroundColor, PixelSize);
        }

        ShowGeneratedQrCode(FilePath, Show);
    }
}

using System;
using ImagePlayground;
using CodeGlyphX.Payloads;
using System.Management.Automation;
using System.Threading.Tasks;

namespace ImagePlayground.PowerShell;

/// <summary>Generates a Swiss QR payment code.</summary>
/// <para>Use this cmdlet when a prepared <see cref="SwissQrCodePayload"/> should be rendered into a payment QR image.</para>
/// <example>
///   <summary>Create a Swiss QR payment code</summary>
///   <prefix>PS&gt; </prefix>
///   <code>$swiss = [CodeGlyphX.Payloads.SwissQrCodePayload]::new($iban, $currency, $creditor, $reference)
/// New-ImageQRCodeSwiss -Payload $swiss -FilePath swiss.png</code>
///   <para>Renders a Swiss payment QR code from a previously prepared payment payload object.</para>
/// </example>
/// <example>
///   <summary>Create a Swiss QR code with custom colors and preview</summary>
///   <prefix>PS&gt; </prefix>
///   <code>$swiss = [CodeGlyphX.Payloads.SwissQrCodePayload]::new($iban, $currency, $creditor, $reference)
/// New-ImageQRCodeSwiss -Payload $swiss -FilePath swiss-branded.png -ForegroundColor DarkBlue -BackgroundColor WhiteSmoke -PixelSize 14 -Show</code>
///   <para>Creates a branded QR image and opens it immediately after generation.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageQRCodeSwiss")]
public sealed class NewImageQrCodeSwissCmdlet : AsyncQrCodeCmdlet {
    /// <summary>Swiss QR payload data.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public SwissQrCodePayload Payload { get; set; } = null!;

    /// <summary>Path for the generated image.</summary>
    /// <para>The image format is inferred from the file extension.</para>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 1)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Opens the image once generated.</summary>
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
            await ImagePlayground.QrCode.GenerateSwissQrCodeAsync(Payload, FilePath, false, ForegroundColor, BackgroundColor, PixelSize, CancelToken).ConfigureAwait(false);
        } else {
            ImagePlayground.QrCode.GenerateSwissQrCode(Payload, FilePath, false, ForegroundColor, BackgroundColor, PixelSize);
        }

        ShowGeneratedQrCode(FilePath, Show);
    }
}

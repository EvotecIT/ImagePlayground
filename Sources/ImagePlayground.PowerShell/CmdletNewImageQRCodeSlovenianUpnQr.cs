using System;
using ImagePlayground;
using CodeGlyphX.Payloads;
using System.Management.Automation;
using System.Threading.Tasks;

namespace ImagePlayground.PowerShell;

/// <summary>Generates a Slovenian UPN QR payment code.</summary>
/// <para>Use this cmdlet when a prepared Slovenian UPN payment payload should be rendered into a scannable QR image.</para>
/// <example>
///   <summary>Create a Slovenian UPN QR payment code</summary>
///   <prefix>PS&gt; </prefix>
///   <code>$upn = [CodeGlyphX.Payloads.SlovenianUpnQrPayload]::new('John Doe','Main Street 1','Ljubljana','Evotec d.o.o.','Business Street 2','Maribor','SI56192001234567890','Invoice 2026-041',19999)
/// New-ImageQRCodeSlovenianUpnQr -Payload $upn -FilePath upn.png</code>
///   <para>Generates a UPN payment QR code from a complete Slovenian payment payload object.</para>
/// </example>
/// <example>
///   <summary>Create a branded UPN QR code and preview it</summary>
///   <prefix>PS&gt; </prefix>
///   <code>$upn = [CodeGlyphX.Payloads.SlovenianUpnQrPayload]::new('John Doe','Main Street 1','Ljubljana','Evotec d.o.o.','Business Street 2','Maribor','SI56192001234567890','Annual subscription',4900)
/// New-ImageQRCodeSlovenianUpnQr -Payload $upn -FilePath upn-brand.png -ForegroundColor DarkGreen -PixelSize 14 -Show</code>
///   <para>Creates a styled payment QR code and opens the resulting image after generation.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageQRCodeSlovenianUpnQr")]
public sealed class NewImageQrCodeSlovenianUpnQrCmdlet : AsyncQrCodeCmdlet {
    /// <summary>UPN payment payload.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public SlovenianUpnQrPayload Payload { get; set; } = null!;

    /// <summary>Location of the output image.</summary>
    /// <para>The image format is inferred from the file extension.</para>
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

    /// <summary>Use asynchronous processing.</summary>
    [Parameter]
    public SwitchParameter Async { get; set; }

    /// <inheritdoc />
    protected override async Task ProcessRecordAsync() {
        ValidatePixelSize(PixelSize);
        FilePath = EnsureQrOutputPath(FilePath);

        if (Async.IsPresent) {
            await ImagePlayground.QrCode.GenerateSlovenianUpnQrAsync(Payload, FilePath, false, ForegroundColor, BackgroundColor, PixelSize, CancelToken).ConfigureAwait(false);
        } else {
            ImagePlayground.QrCode.GenerateSlovenianUpnQr(Payload, FilePath, false, ForegroundColor, BackgroundColor, PixelSize);
        }

        ShowGeneratedQrCode(FilePath, Show);
    }
}

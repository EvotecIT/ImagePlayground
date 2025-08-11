using ImagePlayground;
using QRCoder;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Generates a Swiss QR payment code.</summary>
/// <example>
///   <summary>Create Swiss QR</summary>
///   <code>$swiss = [QRCoder.PayloadGenerator+SwissQrCode]::new($iban,$currency,$cred,$ref);New-ImageQRCodeSwiss -Payload $swiss -FilePath swiss.png</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageQRCodeSwiss")]
public sealed class NewImageQrCodeSwissCmdlet : PSCmdlet {
    /// <summary>Swiss QR payload data.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public PayloadGenerator.SwissQrCode Payload { get; set; } = null!;

    /// <summary>Path for the generated image.</summary>
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

    /// <inheritdoc />
    protected override void ProcessRecord() {
        if (string.IsNullOrWhiteSpace(FilePath)) {
            FilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName().Split('.')[0] + ".png");
            WriteWarning($"New-ImageQRCodeSwiss - No file path specified, saving to {FilePath}");
        }

        var output = Helpers.ResolvePath(FilePath);
        ImagePlayground.QrCode.GenerateSwissQrCode(Payload, output, false, ForegroundColor, BackgroundColor, PixelSize);

        if (Show.IsPresent) {
            ImagePlayground.Helpers.Open(output, true);
        }
    }
}

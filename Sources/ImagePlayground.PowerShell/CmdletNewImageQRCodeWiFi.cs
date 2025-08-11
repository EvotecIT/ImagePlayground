using ImagePlayground;
using QRCoder;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a WiFi QR code image.</summary>
/// <example>
///   <summary>Create WiFi QR</summary>
///   <code>New-ImageQRCodeWiFi -SSID Test -Password pass123 -FilePath wifi.png</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageQRCodeWiFi")]
[Alias("New-QRCodeWiFi")]
public sealed class NewImageQrCodeWiFiCmdlet : PSCmdlet {
    /// <summary>WiFi network name.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public string SSID { get; set; } = string.Empty;

    /// <summary>WiFi password.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public string Password { get; set; } = string.Empty;

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
        if (string.IsNullOrWhiteSpace(FilePath)) {
            FilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName().Split('.')[0] + ".png");
            WriteWarning($"New-ImageQRCodeWiFi - No file path specified, saving to {FilePath}");
        }

        var output = Helpers.ResolvePath(FilePath);
        ImagePlayground.QrCode.GenerateWiFi(SSID, Password, output, false, ForegroundColor, BackgroundColor, PixelSize);

        if (Show.IsPresent) {
            ImagePlayground.Helpers.Open(output, true);
        }
    }
}

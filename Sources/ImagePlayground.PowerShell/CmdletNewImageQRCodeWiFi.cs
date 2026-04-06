using System;
using ImagePlayground;
using System.Management.Automation;
using System.Threading.Tasks;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a WiFi QR code image.</summary>
/// <para>The generated QR code uses a WiFi payload that can be scanned by mobile devices to prefill network settings.</para>
/// <example>
///   <summary>Create WiFi QR</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageQRCodeWiFi -SSID Test -Password pass123 -FilePath wifi.png</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageQRCodeWiFi")]
[Alias("New-QRCodeWiFi")]
public sealed class NewImageQrCodeWiFiCmdlet : AsyncQrCodeCmdlet {
    /// <summary>WiFi network name.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public string SSID { get; set; } = string.Empty;

    /// <summary>WiFi password.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public string Password { get; set; } = string.Empty;

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
            await ImagePlayground.QrCode.GenerateWiFiAsync(SSID, Password, FilePath, false, ForegroundColor, BackgroundColor, PixelSize, CancelToken).ConfigureAwait(false);
        } else {
            ImagePlayground.QrCode.GenerateWiFi(SSID, Password, FilePath, false, ForegroundColor, BackgroundColor, PixelSize);
        }

        ShowGeneratedQrCode(FilePath, Show);
    }
}

using System;
using ImagePlayground;
using QRCoder;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Generates a QR code with geolocation data.</summary>
/// <example>
///   <summary>Create geolocation QR</summary>
///   <code>New-ImageQRCodeGeoLocation -Latitude '52.1' -Longitude '21.0' -FilePath geo.png</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageQRCodeGeoLocation")]
public sealed class NewImageQrCodeGeoLocationCmdlet : PSCmdlet {
    /// <summary>Latitude value.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public string Latitude { get; set; } = string.Empty;

    /// <summary>Longitude value.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public string Longitude { get; set; } = string.Empty;

    /// <summary>Output path for the QR code.</summary>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 2)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Open image after creation.</summary>
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
            WriteWarning($"New-ImageQRCodeGeoLocation - No file path specified, saving to {FilePath}");
        }

        var output = Helpers.ResolvePath(FilePath);
        Helpers.CreateParentDirectory(output);
        ImagePlayground.QrCode.GenerateGeoLocation(Latitude, Longitude, output, PayloadGenerator.Geolocation.GeolocationEncoding.GEO, false, ForegroundColor, BackgroundColor, PixelSize);

        if (Show.IsPresent) {
            ImagePlayground.Helpers.Open(output, true);
        }
    }
}

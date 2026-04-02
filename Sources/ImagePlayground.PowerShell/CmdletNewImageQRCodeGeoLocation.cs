using System;
using ImagePlayground;
using CodeGlyphX.Payloads;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Generates a QR code with geolocation data.</summary>
/// <para>Use this cmdlet to create location QR codes that open map applications at a specific coordinate.</para>
/// <example>
///   <summary>Create a basic geolocation QR code</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageQRCodeGeoLocation -Latitude '52.2297' -Longitude '21.0122' -FilePath geo.png</code>
///   <para>Generates a QR code that opens the target coordinates in compatible map applications.</para>
/// </example>
/// <example>
///   <summary>Create an event-location QR code</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageQRCodeGeoLocation -Latitude '51.1079' -Longitude '17.0385' -FilePath venue.png -ForegroundColor DarkGreen -PixelSize 16 -Show</code>
///   <para>Creates a location QR for signage, invitations, or venue directions and previews it immediately.</para>
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
    /// <para>The image format is inferred from the file extension.</para>
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

        ImagePlayground.QrCode.GenerateGeoLocation(Latitude, Longitude, FilePath, QrGeolocationEncoding.Geo, false, ForegroundColor, BackgroundColor, PixelSize);

        if (Show.IsPresent) {
            ImagePlayground.Helpers.Open(Helpers.ResolvePath(FilePath), true);
        }
    }
}

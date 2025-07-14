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

    /// <inheritdoc />
    protected override void ProcessRecord() {
        if (string.IsNullOrWhiteSpace(FilePath)) {
            FilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName().Split('.')[0] + ".png");
            WriteWarning($"New-ImageQRCodeGeoLocation - No file path specified, saving to {FilePath}");
        }

        var output = Helpers.ResolvePath(FilePath);
        ImagePlayground.QrCode.GenerateGeoLocation(Latitude, Longitude, output);

        if (Show.IsPresent) {
            ImagePlayground.Helpers.Open(output, true);
        }
    }
}

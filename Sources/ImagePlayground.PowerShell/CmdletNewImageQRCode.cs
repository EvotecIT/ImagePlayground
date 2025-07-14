using ImagePlayground;
using QRCoder;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Generates a QR code image.</summary>
/// <example>
///   <summary>Create QR code</summary>
///   <code>New-ImageQRCode -Content 'https://evotec.xyz' -FilePath qr.png</code>
/// </example>
/// <example>
///   <summary>Show QR code after generation</summary>
///   <code>New-ImageQRCode -Content 'text' -FilePath qr.png -Show</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageQRCode")]
[Alias("New-QRCode")]
public sealed class NewImageQrCodeCmdlet : PSCmdlet {
    /// <summary>Content encoded in the QR code.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public string Content { get; set; } = string.Empty;

    /// <summary>Output path for the QR code image.</summary>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 1)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Open the image after creation.</summary>
    [Parameter]
    public SwitchParameter Show { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        if (string.IsNullOrWhiteSpace(FilePath)) {
            FilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName().Split('.')[0] + ".png");
            WriteWarning($"New-ImageQRCode - No file path specified, saving to {FilePath}");
        }

        var output = Helpers.ResolvePath(FilePath);
        ImagePlayground.QrCode.Generate(Content, output, false);

        if (Show.IsPresent) {
            ImagePlayground.Helpers.Open(output, true);
        }
    }
}

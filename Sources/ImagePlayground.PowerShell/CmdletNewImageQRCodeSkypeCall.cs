using ImagePlayground;
using QRCoder;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Generates a QR code initiating a Skype call.</summary>
/// <example>
///   <summary>Create Skype call QR</summary>
///   <code>New-ImageQRCodeSkypeCall -UserName 'echo123' -FilePath skype.png</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageQRCodeSkypeCall")]
public sealed class NewImageQrCodeSkypeCallCmdlet : PSCmdlet {
    /// <summary>Skype username to call.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public string UserName { get; set; } = string.Empty;

    /// <summary>Output path for the QR image.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Opens the image after creation.</summary>
    [Parameter]
    public SwitchParameter Show { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        if (string.IsNullOrWhiteSpace(FilePath)) {
            FilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName().Split('.')[0] + ".png");
            WriteWarning($"New-ImageQRCodeSkypeCall - No file path specified, saving to {FilePath}");
        }

        var output = Helpers.ResolvePath(FilePath);
        ImagePlayground.QrCode.GenerateSkypeCall(UserName, output);

        if (Show.IsPresent) {
            ImagePlayground.Helpers.Open(output, true);
        }
    }
}

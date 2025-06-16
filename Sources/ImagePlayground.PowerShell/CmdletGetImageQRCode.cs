using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Reads QR code information from an image file.</summary>
/// <example>
///   <summary>Decode QR code</summary>
///   <code>Get-ImageQRCode -FilePath qr.png</code>
/// </example>
[Cmdlet(VerbsCommon.Get, "ImageQRCode")]
public sealed class GetImageQrCodeCmdlet : PSCmdlet {
    /// <summary>Path to the image file.</summary>
    /// <para>The file must exist.</para>
    [Parameter(Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <inheritdoc />
    protected override void ProcessRecord() {
        if (!File.Exists(FilePath)) {
            WriteWarning($"Get-ImageQRCode - File {FilePath} not found. Please check the path.");
            return;
        }

        var result = ImagePlayground.QrCode.Read(FilePath);
        WriteObject(result);
    }
}

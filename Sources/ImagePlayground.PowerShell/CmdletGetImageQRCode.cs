using ImagePlayground;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Reads QR code information from an image file.</summary>
/// <para>Returns the decoded content and symbology details.</para>
/// <example>
///   <summary>Decode QR code</summary>
///   <code>Get-ImageQRCode -FilePath qr.png</code>
/// </example>
/// <example>
///   <summary>Check the raw value</summary>
///   <code>(Get-ImageQRCode -FilePath qr.png).Message</code>
/// </example>
[Cmdlet(VerbsCommon.Get, "ImageQRCode")]
public sealed class GetImageQrCodeCmdlet : PSCmdlet {
    /// <summary>Path to the image file.</summary>
    /// <para>The file must exist.</para>
    [Parameter(Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var filePath = Helpers.ResolvePath(FilePath);
        if (!File.Exists(filePath)) {
            WriteWarning($"Get-ImageQRCode - File {FilePath} not found. Please check the path.");
            return;
        }

        var result = ImagePlayground.QrCode.Read(filePath);
        WriteObject(result);
    }
}

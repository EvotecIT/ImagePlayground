using ImagePlayground;
using System.IO;
using System.Management.Automation;
using System.Threading.Tasks;

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
public sealed class GetImageQrCodeCmdlet : AsyncImageCmdlet {
    /// <summary>Path to the image file.</summary>
    /// <para>The file must exist.</para>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Use asynchronous processing.</summary>
    [Parameter]
    public SwitchParameter Async { get; set; }

    /// <inheritdoc />
    protected override async Task ProcessRecordAsync() {
        var filePath = ResolveExistingFilePath(FilePath, "GetImageQRCodeFileNotFound", FilePath);
        var result = Async.IsPresent
            ? await ImagePlayground.QrCode.ReadAsync(filePath, CancelToken).ConfigureAwait(false)
            : ImagePlayground.QrCode.Read(filePath);
        WriteObject(result);
    }
}

using CodeGlyphX;
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
///   <code>(Get-ImageQRCode -FilePath qr.png).Text</code>
/// </example>
[Cmdlet(VerbsCommon.Get, "ImageQRCode")]
public sealed class GetImageQrCodeCmdlet : AsyncImageCmdlet {
    /// <summary>Path to the image file.</summary>
    /// <para>The file must exist.</para>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>CodeGlyphX recognition options used while decoding the QR image.</summary>
    /// <para>When omitted, ImagePlayground uses bounded fast-upright and robust-transform passes. Use <see cref="QrPixelDecodeOptions.Stylized"/> for QR art or other difficult images.</para>
    [Parameter]
    public QrPixelDecodeOptions? DecodeOptions { get; set; }

    /// <inheritdoc />
    protected override async Task ProcessRecordAsync() {
        var filePath = ResolveExistingFilePath(FilePath, "GetImageQRCodeFileNotFound", FilePath);
        var result = await ImagePlayground.QrCode.ReadAsync(filePath, CancelToken, DecodeOptions).ConfigureAwait(false);
        WriteObject(result);
    }
}

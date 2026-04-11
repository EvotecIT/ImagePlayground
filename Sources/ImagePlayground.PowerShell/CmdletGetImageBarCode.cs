using ImagePlayground;
using System.IO;
using System.Management.Automation;
using System.Threading.Tasks;

namespace ImagePlayground.PowerShell;

/// <summary>Reads barcode information from an image file.</summary>
/// <example>
///   <summary>Decode barcode</summary>
///   <code>Get-ImageBarCode -FilePath barcode.png</code>
/// </example>
[Cmdlet(VerbsCommon.Get, "ImageBarCode")]
public sealed class GetImageBarCodeCmdlet : AsyncImageCmdlet {
    /// <summary>Path to the image.</summary>
    /// <para>The file must exist.</para>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Use asynchronous processing.</summary>
    [Parameter]
    public SwitchParameter Async { get; set; }

    /// <inheritdoc />
    protected override async Task ProcessRecordAsync() {
        var filePath = ResolveExistingFilePath(FilePath, "GetImageBarCodeFileNotFound", FilePath);
        var result = Async.IsPresent
            ? await ImagePlayground.BarCode.ReadAsync(filePath, CancelToken).ConfigureAwait(false)
            : ImagePlayground.BarCode.Read(filePath);
        WriteObject(result);
    }
}

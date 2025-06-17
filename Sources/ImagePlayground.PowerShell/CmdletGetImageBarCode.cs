using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Reads barcode information from an image file.</summary>
/// <example>
///   <summary>Decode barcode</summary>
///   <code>Get-ImageBarCode -FilePath barcode.png</code>
/// </example>
[Cmdlet(VerbsCommon.Get, "ImageBarCode")]
public sealed class GetImageBarCodeCmdlet : PSCmdlet {
    /// <summary>Path to the image.</summary>
    /// <para>The file must exist.</para>
    [Parameter(Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <inheritdoc />
    protected override void ProcessRecord() {
        string fullPath = ImagePlayground.Helpers.ResolvePath(FilePath);
        if (!File.Exists(fullPath)) {
            WriteWarning($"Get-ImageBarCode - File {FilePath} not found. Please check the path.");
            return;
        }

        var result = ImagePlayground.BarCode.Read(fullPath);
        WriteObject(result);
    }
}

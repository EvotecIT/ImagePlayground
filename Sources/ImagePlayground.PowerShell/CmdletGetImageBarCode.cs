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
        if (!File.Exists(FilePath)) {
            WriteWarning($"Get-ImageBarCode - File {FilePath} not found. Please check the path.");
            return;
        }

        var result = ImagePlayground.BarCode.Read(FilePath);
        WriteObject(result);
    }
}

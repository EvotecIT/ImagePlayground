using ImagePlayground;
using CodeGlyphX;
using System.IO;
using System.Management.Automation;
using System.Threading.Tasks;

namespace ImagePlayground.PowerShell;

/// <summary>Reads barcode information from an image file.</summary>
/// <example>
///   <summary>Decode barcode</summary>
///   <code>Get-ImageBarCode -FilePath barcode.png</code>
/// </example>
/// <example>
///   <summary>Scan selected formats and inspect completion details</summary>
///   <prefix>PS&gt; </prefix>
///   <code>$options = [CodeGlyphX.ScanOptions]::new()
/// $options.Formats = [CodeGlyphX.SymbolFormat[]]@('Ean', 'DataMatrix')
/// $options.TimeoutMilliseconds = 5000
/// $scan = Get-ImageBarCode -FilePath barcode.png -ScanOptions $options -Detailed
/// $scan.CompletionReason
/// $scan.Symbols</code>
/// </example>
[Cmdlet(VerbsCommon.Get, "ImageBarCode")]
[OutputType(typeof(DetectedSymbol), typeof(ScanResult))]
public sealed class GetImageBarCodeCmdlet : AsyncImageCmdlet {
    /// <summary>Path to the image.</summary>
    /// <para>The file must exist.</para>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Recognition formats, deadline, image limits, and accuracy options.</summary>
    /// <para>When omitted, scans allow five seconds and exclude QR, Pharmacode, and Patch Code. Explicit formats can opt into Pharmacode or Patch Code. A new ScanOptions object uses CodeGlyphX defaults, including its 500 ms total deadline.</para>
    [Parameter]
    public ScanOptions? ScanOptions { get; set; }

    /// <summary>Return the complete ScanResult, including partial symbols and completion reason.</summary>
    /// <para>Without this switch, return the first DetectedSymbol; cancellation and deadlines produce errors. Detailed results retain structured cancellation and deadline status.</para>
    [Parameter]
    public SwitchParameter Detailed { get; set; }

    /// <inheritdoc />
    protected override async Task ProcessRecordAsync() {
        var filePath = ResolveExistingFilePath(FilePath, "GetImageBarCodeFileNotFound", FilePath);
        if (Detailed.IsPresent) {
            WriteObject(await BarCode.ScanAsync(filePath, ScanOptions, CancelToken).ConfigureAwait(false));
        } else {
            WriteObject(await BarCode.ReadAsync(filePath, CancelToken, ScanOptions).ConfigureAwait(false));
        }
    }
}

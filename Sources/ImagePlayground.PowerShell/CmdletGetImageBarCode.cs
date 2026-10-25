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
///   <summary>Scan only the formats you expect</summary>
///   <prefix>PS&gt; </prefix>
///   <code>Get-ChildItem *.png | Get-ImageBarCode -Type Ean, Code128 -TimeoutSeconds 2 | Select-Object Type, Text</code>
///   <para>Restricting the formats makes scans faster and avoids false positives from unrelated symbologies.</para>
/// </example>
/// <example>
///   <summary>Inspect completion details</summary>
///   <prefix>PS&gt; </prefix>
///   <code>$scan = Get-ImageBarCode -FilePath barcode.png -Type DataMatrix -Detailed
/// $scan.CompletionReason
/// $scan.Symbols</code>
/// </example>
[Cmdlet(VerbsCommon.Get, "ImageBarCode")]
[OutputType(typeof(DetectedSymbol), typeof(ScanResult))]
public sealed class GetImageBarCodeCmdlet : AsyncImageCmdlet {
    /// <summary>Path to the image.</summary>
    /// <para>The file must exist.</para>
    [Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Mandatory = true, Position = 0)]
    [Alias("FullName", "Path")]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Formats to look for. Defaults to every image-scannable format except QR, Pharmacode, and Patch Code.</summary>
    [Parameter]
    public SymbolFormat[]? Type { get; set; }

    /// <summary>Scan deadline in seconds. Defaults to five seconds.</summary>
    [Parameter]
    [ValidateRange(0.1D, 600D)]
    public double TimeoutSeconds { get; set; } = 5D;

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
        var options = ResolveScanOptions();
        if (Detailed.IsPresent) {
            WriteObject(await BarCode.ScanAsync(filePath, options, CancelToken).ConfigureAwait(false));
        } else {
            WriteObject(await BarCode.ReadAsync(filePath, CancelToken, options).ConfigureAwait(false));
        }
    }

    private ScanOptions? ResolveScanOptions() {
        var bound = MyInvocation.BoundParameters;
        if (!bound.ContainsKey(nameof(Type)) && !bound.ContainsKey(nameof(TimeoutSeconds))) {
            return ScanOptions;
        }
        var options = BarCode.CopyScanOptions(ScanOptions);
        if (Type != null && Type.Length > 0) {
            options.Formats = Type;
        }
        if (bound.ContainsKey(nameof(TimeoutSeconds))) {
            options.TimeoutMilliseconds = (int)System.Math.Round(TimeoutSeconds * 1000D);
        }
        return options;
    }
}

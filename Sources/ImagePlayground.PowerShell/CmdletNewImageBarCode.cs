using ImagePlayground;
using CodeGlyphX;
using System.Management.Automation;
using System.Threading.Tasks;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a barcode image.</summary>
/// <example>
///   <summary>Create barcode</summary>
///   <code>New-ImageBarCode -Type EAN -Value 9012341234571 -FilePath barcode.png</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageBarCode")]
public sealed class NewImageBarCodeCmdlet : AsyncImageCmdlet {
    /// <summary>Physical barcode format from the CodeGlyphX catalogue.</summary>
    /// <para>QR formats use New-ImageQRCode. MaxiCode requires specialized rendering, and GS1 Composite requires separate payloads; neither is supported by this single-value command.</para>
    [Parameter(Mandatory = true, Position = 0)]
    public SymbolFormat Type { get; set; }

    /// <summary>Value encoded in the barcode.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public string Value { get; set; } = string.Empty;

    /// <summary>Output path for the barcode image.</summary>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 2)]
    public string FilePath { get; set; } = string.Empty;

    /// <inheritdoc />
    protected override async Task ProcessRecordAsync() {
        var output = PowerShellPathResolver.ResolveFileSystemPath(this, FilePath);
        await BarCode.GenerateAsync(Type, Value, output, CancelToken).ConfigureAwait(false);
    }
}

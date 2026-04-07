using ImagePlayground;
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
    /// <summary>Barcode type.</summary>
    [Parameter(Mandatory = true, Position = 0)]
public BarcodeType Type { get; set; }

    /// <summary>Value encoded in the barcode.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public string Value { get; set; } = string.Empty;

    /// <summary>Output path for the barcode image.</summary>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 2)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Use asynchronous processing.</summary>
    [Parameter]
    public SwitchParameter Async { get; set; }

    /// <inheritdoc />
    protected override async Task ProcessRecordAsync() {
        var output = Helpers.ResolvePath(FilePath);
        if (Async.IsPresent) {
            await BarCode.GenerateAsync(Type, Value, output, CancelToken).ConfigureAwait(false);
        } else {
            BarCode.Generate(Type, Value, output);
        }
    }
}

using ImagePlayground;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a barcode image.</summary>
/// <example>
///   <summary>Create barcode</summary>
///   <code>New-ImageBarCode -Type EAN -Value 9012341234571 -FilePath barcode.png</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageBarCode")]
public sealed class NewImageBarCodeCmdlet : PSCmdlet {
    /// <summary>Barcode type.</summary>
    [Parameter(Mandatory = true, Position = 0)]
public BarcodeType Type { get; set; }

    /// <summary>Value encoded in the barcode.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public string Value { get; set; } = string.Empty;

    /// <summary>Output path for the barcode image.</summary>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 2)]
    public string FilePath { get; set; } = string.Empty;

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var output = Helpers.ResolvePath(FilePath);
        BarCode.Generate(Type, Value, output);
    }
}

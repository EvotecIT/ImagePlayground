using System;
using ImagePlayground;
using QRCoder;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Generates a Girocode QR code.</summary>
/// <example>
///   <summary>Create Girocode</summary>
///   <code>New-ImageQRCodeGirocode -Iban 'DE12500105170648489890' -Bic 'COBADEFFXXX' -Name 'Test' -Amount 12.34 -FilePath giro.png</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageQRCodeGirocode")]
public sealed class NewImageQrCodeGirocodeCmdlet : PSCmdlet {
    /// <summary>IBAN of the payee.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public string Iban { get; set; } = string.Empty;

    /// <summary>BIC of the payee.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public string Bic { get; set; } = string.Empty;

    /// <summary>Recipient name.</summary>
    [Parameter(Mandatory = true, Position = 2)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Transfer amount.</summary>
    [Parameter(Mandatory = true, Position = 3)]
    public decimal Amount { get; set; }

    /// <summary>Optional remittance information.</summary>
    [Parameter(Position = 4)]
    public string? RemittanceInformation { get; set; }

    /// <summary>Path to save the QR code.</summary>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 5)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Opens the image after creation.</summary>
    [Parameter]
    public SwitchParameter Show { get; set; }

    /// <summary>Foreground color of QR modules.</summary>
    [Parameter]
    public SixLabors.ImageSharp.Color ForegroundColor { get; set; } = SixLabors.ImageSharp.Color.Black;

    /// <summary>Background color of the QR code.</summary>
    [Parameter]
    public SixLabors.ImageSharp.Color BackgroundColor { get; set; } = SixLabors.ImageSharp.Color.White;

    /// <summary>Pixel size for each QR module.</summary>
    [Parameter]
    public int PixelSize { get; set; } = 20;

    /// <inheritdoc />
    protected override void ProcessRecord() {
        if (PixelSize <= 0) {
            throw new ArgumentOutOfRangeException(nameof(PixelSize));
        }

        if (string.IsNullOrWhiteSpace(FilePath)) {
            FilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName().Split('.')[0] + ".png");
            WriteWarning($"New-ImageQRCodeGirocode - No file path specified, saving to {FilePath}");
        }

        var output = Helpers.ResolvePath(FilePath);
        ImagePlayground.QrCode.GenerateGirocode(Iban, Bic, Name, Amount, output, RemittanceInformation, PayloadGenerator.Girocode.TypeOfRemittance.Unstructured, null, null, PayloadGenerator.Girocode.GirocodeVersion.Version1, PayloadGenerator.Girocode.GirocodeEncoding.ISO_8859_1, false, ForegroundColor, BackgroundColor, PixelSize);

        if (Show.IsPresent) {
            ImagePlayground.Helpers.Open(output, true);
        }
    }
}

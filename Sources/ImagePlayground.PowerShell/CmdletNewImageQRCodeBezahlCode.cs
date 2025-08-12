using System;
using ImagePlayground;
using QRCoder;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Generates a BezahlCode QR for German payments.</summary>
/// <example>
///   <summary>Create BezahlCode</summary>
///   <code>New-ImageQRCodeBezahlCode -Authority SinglePayment -Name 'Tester' -Account '123' -Bnc '10020030' -Iban 'DE123' -Bic 'BIC' -Reason 'Invoice' -FilePath bezahl.png</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageQRCodeBezahlCode")]
public sealed class NewImageQrCodeBezahlCodeCmdlet : PSCmdlet {
    /// <summary>Payment authority type.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public PayloadGenerator.BezahlCode.AuthorityType Authority { get; set; }

    /// <summary>Payer or payee name.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Account number.</summary>
    [Parameter(Mandatory = true, Position = 2)]
    public string Account { get; set; } = string.Empty;

    /// <summary>Bank number code.</summary>
    [Parameter(Mandatory = true, Position = 3)]
    public string Bnc { get; set; } = string.Empty;

    /// <summary>International bank account number.</summary>
    [Parameter(Mandatory = true, Position = 4)]
    public string Iban { get; set; } = string.Empty;

    /// <summary>BIC/SWIFT code.</summary>
    [Parameter(Mandatory = true, Position = 5)]
    public string Bic { get; set; } = string.Empty;

    /// <summary>Reason for payment.</summary>
    [Parameter(Mandatory = true, Position = 6)]
    public string Reason { get; set; } = string.Empty;

    /// <summary>Output image path.</summary>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 7)]
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
            WriteWarning($"New-ImageQRCodeBezahlCode - No file path specified, saving to {FilePath}");
        }

        ImagePlayground.QrCode.GenerateBezahlCode(Authority, Name, Account, Bnc, Iban, Bic, Reason, FilePath, false, ForegroundColor, BackgroundColor, PixelSize);

        if (Show.IsPresent) {
            ImagePlayground.Helpers.Open(Helpers.ResolvePath(FilePath), true);
        }
    }
}

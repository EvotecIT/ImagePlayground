using ImagePlayground;
using QRCoder;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Generates a QR code for Bitcoin-like payments.</summary>
/// <example>
///   <summary>Create Bitcoin QR</summary>
///   <code>New-ImageQRCodeBitcoin -Currency Bitcoin -Address '1BoatSLRHtKNngkdXEeobR76b53LETtpyT' -Amount 0.01 -FilePath btc.png</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageQRCodeBitcoin")]
public sealed class NewImageQrCodeBitcoinCmdlet : PSCmdlet {
    /// <summary>Type of cryptocurrency.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public PayloadGenerator.BitcoinLikeCryptoCurrencyAddress.BitcoinLikeCryptoCurrencyType Currency { get; set; }

    /// <summary>Destination address.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public string Address { get; set; } = string.Empty;

    /// <summary>Optional payment amount.</summary>
    [Parameter(Position = 2)]
    public double? Amount { get; set; }

    /// <summary>Optional transaction label.</summary>
    [Parameter(Position = 3)]
    public string? Label { get; set; }

    /// <summary>Optional message.</summary>
    [Parameter(Position = 4)]
    public string? Message { get; set; }

    /// <summary>Path to the output image.</summary>
    [Parameter(Mandatory = true, Position = 5)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Opens the image after creation.</summary>
    [Parameter]
    public SwitchParameter Show { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        if (string.IsNullOrWhiteSpace(FilePath)) {
            FilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName().Split('.')[0] + ".png");
            WriteWarning($"New-ImageQRCodeBitcoin - No file path specified, saving to {FilePath}");
        }

        var output = Helpers.ResolvePath(FilePath);
        ImagePlayground.QrCode.GenerateBitcoinAddress(Currency, Address, Amount, Label, Message, output);

        if (Show.IsPresent) {
            ImagePlayground.Helpers.Open(output, true);
        }
    }
}

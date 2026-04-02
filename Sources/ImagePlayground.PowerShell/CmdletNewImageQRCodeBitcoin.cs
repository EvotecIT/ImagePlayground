using System;
using ImagePlayground;
using CodeGlyphX.Payloads;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Generates a QR code for Bitcoin-like payments.</summary>
/// <para>Use this cmdlet to create payment request QR codes for Bitcoin and similar supported cryptocurrencies.</para>
/// <example>
///   <summary>Create a basic Bitcoin payment QR</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageQRCodeBitcoin -Currency Bitcoin -Address '1BoatSLRHtKNngkdXEeobR76b53LETtpyT' -Amount 0.01 -FilePath btc.png</code>
///   <para>Creates a payment QR code that includes the destination address and requested amount.</para>
/// </example>
/// <example>
///   <summary>Create a crypto donation QR code with label and note</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageQRCodeBitcoin -Currency Bitcoin -Address '1BoatSLRHtKNngkdXEeobR76b53LETtpyT' -Amount 0.005 -Label 'Evotec Donation' -Message 'Thank you for supporting the project' -FilePath donation.png -ForegroundColor DarkOrange -PixelSize 18 -Show</code>
///   <para>Generates a donation-style payment QR code and opens it after creation.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageQRCodeBitcoin")]
public sealed class NewImageQrCodeBitcoinCmdlet : PSCmdlet {
    /// <summary>Type of cryptocurrency.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public QrBitcoinLikeType Currency { get; set; }

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
    /// <para>The image format is inferred from the file extension.</para>
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
            WriteWarning($"New-ImageQRCodeBitcoin - No file path specified, saving to {FilePath}");
        }

        ImagePlayground.QrCode.GenerateBitcoinAddress(Currency, Address, Amount, Label, Message, FilePath, false, ForegroundColor, BackgroundColor, PixelSize);

        if (Show.IsPresent) {
            ImagePlayground.Helpers.Open(Helpers.ResolvePath(FilePath), true);
        }
    }
}

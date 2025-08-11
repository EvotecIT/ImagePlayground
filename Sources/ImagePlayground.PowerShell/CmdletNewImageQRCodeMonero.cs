using ImagePlayground;
using QRCoder;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Generates a QR code for a Monero transaction.</summary>
/// <example>
///   <summary>Create Monero QR</summary>
///   <code>New-ImageQRCodeMonero -Address '44AFFq5kSiGBoZ...'' -Amount 1.0 -FilePath xmr.png</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageQRCodeMonero")]
public sealed class NewImageQrCodeMoneroCmdlet : PSCmdlet {
    /// <summary>Destination wallet address.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public string Address { get; set; } = string.Empty;

    /// <summary>Optional payment amount.</summary>
    [Parameter(Position = 1)]
    public float? Amount { get; set; }

    /// <summary>Optional payment identifier.</summary>
    [Parameter(Position = 2)]
    public string? PaymentId { get; set; }

    /// <summary>Recipient name.</summary>
    [Parameter(Position = 3)]
    public string? RecipientName { get; set; }

    /// <summary>Payment description.</summary>
    [Parameter(Position = 4)]
    public string? Description { get; set; }

    /// <summary>Path to save the QR code image.</summary>
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
        if (string.IsNullOrWhiteSpace(FilePath)) {
            FilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName().Split('.')[0] + ".png");
            WriteWarning($"New-ImageQRCodeMonero - No file path specified, saving to {FilePath}");
        }

        var output = Helpers.ResolvePath(FilePath);
        ImagePlayground.QrCode.GenerateMoneroTransaction(Address, Amount, PaymentId, RecipientName, Description, output, false, ForegroundColor, BackgroundColor, PixelSize);

        if (Show.IsPresent) {
            ImagePlayground.Helpers.Open(output, true);
        }
    }
}

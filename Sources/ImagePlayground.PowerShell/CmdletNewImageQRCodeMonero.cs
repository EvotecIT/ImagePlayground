using System;
using ImagePlayground;
using System.Management.Automation;
using System.Threading.Tasks;

namespace ImagePlayground.PowerShell;

/// <summary>Generates a QR code for a Monero transaction.</summary>
/// <example>
///   <summary>Create Monero QR</summary>
///   <code>New-ImageQRCodeMonero -Address '44AFFq5kSiGBoZ...'' -Amount 1.0 -FilePath xmr.png</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageQRCodeMonero")]
public sealed class NewImageQrCodeMoneroCmdlet : AsyncQrCodeCmdlet {
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

    /// <summary>Use asynchronous processing.</summary>
    [Parameter]
    public SwitchParameter Async { get; set; }

    /// <inheritdoc />
    protected override async Task ProcessRecordAsync() {
        ValidatePixelSize(PixelSize);
        FilePath = EnsureQrOutputPath(FilePath);

        if (Async.IsPresent) {
            await ImagePlayground.QrCode.GenerateMoneroTransactionAsync(Address, Amount, PaymentId, RecipientName, Description, FilePath, false, ForegroundColor, BackgroundColor, PixelSize, CancelToken).ConfigureAwait(false);
        } else {
            ImagePlayground.QrCode.GenerateMoneroTransaction(Address, Amount, PaymentId, RecipientName, Description, FilePath, false, ForegroundColor, BackgroundColor, PixelSize);
        }

        ShowGeneratedQrCode(FilePath, Show);
    }
}

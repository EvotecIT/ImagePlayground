using System;
using ImagePlayground;
using System.Management.Automation;
using System.Threading.Tasks;

namespace ImagePlayground.PowerShell;

/// <summary>Generates a QR code initiating a Skype call.</summary>
/// <para>Use this cmdlet for legacy Skype calling scenarios where a QR scan should start a call with a specific account.</para>
/// <example>
///   <summary>Create a basic Skype-call QR code</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageQRCodeSkypeCall -UserName 'echo123' -FilePath skype.png</code>
///   <para>Creates a QR code that opens Skype and targets the selected username for a call.</para>
/// </example>
/// <example>
///   <summary>Create a helpdesk Skype QR code</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageQRCodeSkypeCall -UserName 'evotec.helpdesk' -FilePath skype-helpdesk.png -ForegroundColor MidnightBlue -PixelSize 16 -Show</code>
///   <para>Generates a branded Skype call QR code and opens the resulting image after creation.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageQRCodeSkypeCall")]
public sealed class NewImageQrCodeSkypeCallCmdlet : AsyncQrCodeCmdlet {
    /// <summary>Skype username to call.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public string UserName { get; set; } = string.Empty;

    /// <summary>Output path for the QR image.</summary>
    /// <para>The image format is inferred from the file extension.</para>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 1)]
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
            await ImagePlayground.QrCode.GenerateSkypeCallAsync(UserName, FilePath, false, ForegroundColor, BackgroundColor, PixelSize, CancelToken).ConfigureAwait(false);
        } else {
            ImagePlayground.QrCode.GenerateSkypeCall(UserName, FilePath, false, ForegroundColor, BackgroundColor, PixelSize);
        }

        ShowGeneratedQrCode(FilePath, Show);
    }
}

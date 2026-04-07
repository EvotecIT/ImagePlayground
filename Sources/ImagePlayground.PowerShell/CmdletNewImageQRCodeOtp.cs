using System;
using ImagePlayground;
using CodeGlyphX;
using System.Management.Automation;
using System.Threading.Tasks;

namespace ImagePlayground.PowerShell;

/// <summary>Generates a QR code for one-time-password configuration.</summary>
/// <para>Use this cmdlet to create QR codes for authenticator apps that support TOTP or HOTP provisioning.</para>
/// <example>
///   <summary>Create a TOTP QR code</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageQRCodeOtp -Type Totp -SecretBase32 'JBSWY3DPEHPK3PXP' -Label 'john.doe@evotec.pl' -Issuer 'Evotec' -FilePath otp.png</code>
///   <para>Creates a QR code that can be scanned into a typical authenticator app for time-based MFA.</para>
/// </example>
/// <example>
///   <summary>Create an HOTP QR code with a custom counter</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageQRCodeOtp -Type Hotp -SecretBase32 'JBSWY3DPEHPK3PXP' -Label 'lab-token' -Issuer 'Evotec Lab' -Counter 10 -Digits 8 -Algorithm Sha256 -FilePath hotp.png</code>
///   <para>Creates a counter-based OTP payload for systems that use event-driven one-time passwords.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageQRCodeOtp")]
public sealed class NewImageQrCodeOtpCmdlet : AsyncQrCodeCmdlet {
    /// <summary>OTP type.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public OtpAuthType Type { get; set; } = OtpAuthType.Totp;

    /// <summary>Base32-encoded secret.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public string SecretBase32 { get; set; } = string.Empty;

    /// <summary>Account label.</summary>
    [Parameter(Position = 2)]
    public string? Label { get; set; }

    /// <summary>Issuer name.</summary>
    [Parameter(Position = 3)]
    public string? Issuer { get; set; }

    /// <summary>Hash algorithm.</summary>
    [Parameter]
    public OtpAlgorithm Algorithm { get; set; } = OtpAlgorithm.Sha1;

    /// <summary>Number of digits.</summary>
    [Parameter]
    public int Digits { get; set; } = 6;

    /// <summary>Period for TOTP.</summary>
    [Parameter]
    public int Period { get; set; } = 30;

    /// <summary>Counter for HOTP.</summary>
    [Parameter]
    public int? Counter { get; set; }

    /// <summary>Path to save the QR code image.</summary>
    /// <para>The image format is inferred from the file extension.</para>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 4)]
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
            await ImagePlayground.QrCode.GenerateOneTimePasswordAsync(Type, SecretBase32, FilePath, Label, Issuer, Algorithm, Digits, Period, Counter, false, ForegroundColor, BackgroundColor, PixelSize, CancelToken).ConfigureAwait(false);
        } else {
            ImagePlayground.QrCode.GenerateOneTimePassword(Type, SecretBase32, FilePath, Label, Issuer, Algorithm, Digits, Period, Counter, false, ForegroundColor, BackgroundColor, PixelSize);
        }

        ShowGeneratedQrCode(FilePath, Show);
    }
}

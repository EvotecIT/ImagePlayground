using System;
using ImagePlayground;
using QRCoder;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Generates a QR code for a Shadowsocks configuration.</summary>
/// <example>
///   <summary>Create Shadowsocks QR</summary>
///   <code>New-ImageQRCodeShadowSocks -Host 'example.com' -Port 8388 -Password 'pwd' -Method Aes256Gcm -FilePath ss.png</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageQRCodeShadowSocks")]
public sealed class NewImageQrCodeShadowSocksCmdlet : PSCmdlet {
    /// <summary>Server host name.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public string Host { get; set; } = string.Empty;

    /// <summary>Server port.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public int Port { get; set; }

    /// <summary>Password for the server.</summary>
    [Parameter(Mandatory = true, Position = 2)]
    public string Password { get; set; } = string.Empty;

    /// <summary>Encryption method.</summary>
    [Parameter(Mandatory = true, Position = 3)]
    public PayloadGenerator.ShadowSocksConfig.Method Method { get; set; }

    /// <summary>Optional tag.</summary>
    [Parameter(Position = 4)]
    public string? Tag { get; set; }

    /// <summary>Path where the QR code image is stored.</summary>
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
            WriteWarning($"New-ImageQRCodeShadowSocks - No file path specified, saving to {FilePath}");
        }

        ImagePlayground.QrCode.GenerateShadowSocks(Host, Port, Password, Method, FilePath, Tag, false, ForegroundColor, BackgroundColor, PixelSize);

        if (Show.IsPresent) {
            ImagePlayground.Helpers.Open(Helpers.ResolvePath(FilePath), true);
        }
    }
}

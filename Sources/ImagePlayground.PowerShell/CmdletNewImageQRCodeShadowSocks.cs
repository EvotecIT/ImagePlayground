using System;
using ImagePlayground;
using CodeGlyphX.Payloads;
using System.Management.Automation;
using System.Threading.Tasks;

namespace ImagePlayground.PowerShell;

/// <summary>Generates a QR code for a Shadowsocks configuration.</summary>
/// <para>Use this cmdlet to share a client-ready Shadowsocks connection string as a scannable QR code.</para>
/// <example>
///   <summary>Create a basic Shadowsocks QR code</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageQRCodeShadowSocks -Host 'example.com' -Port 8388 -Password 'pwd' -Method Aes256Gcm -FilePath ss.png</code>
///   <para>Creates a QR code for importing a Shadowsocks connection into a compatible client.</para>
/// </example>
/// <example>
///   <summary>Create a tagged Shadowsocks client profile QR code</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageQRCodeShadowSocks -Host 'vpn.evotec.pl' -Port 8388 -Password 'StrongSecret!' -Method Chacha20IetfPoly1305 -Tag 'Warsaw Edge' -FilePath ss-warsaw.png -ForegroundColor Purple -PixelSize 14 -Show</code>
///   <para>Generates a named client profile QR code and opens it immediately after creation.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageQRCodeShadowSocks")]
public sealed class NewImageQrCodeShadowSocksCmdlet : AsyncQrCodeCmdlet {
    /// <summary>Server host name.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    [Alias("Host")]
    public string ServerHost { get; set; } = string.Empty;

    /// <summary>Server port.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public int Port { get; set; }

    /// <summary>Password for the server.</summary>
    [Parameter(Mandatory = true, Position = 2)]
    public string Password { get; set; } = string.Empty;

    /// <summary>Encryption method.</summary>
    [Parameter(Mandatory = true, Position = 3)]
    public QrShadowSocksMethod Method { get; set; }

    /// <summary>Optional tag.</summary>
    [Parameter(Position = 4)]
    public string? Tag { get; set; }

    /// <summary>Path where the QR code image is stored.</summary>
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

    /// <summary>Use asynchronous processing.</summary>
    [Parameter]
    public SwitchParameter Async { get; set; }

    /// <inheritdoc />
    protected override async Task ProcessRecordAsync() {
        ValidatePixelSize(PixelSize);
        FilePath = EnsureQrOutputPath(FilePath);

        if (Async.IsPresent) {
            await ImagePlayground.QrCode.GenerateShadowSocksAsync(ServerHost, Port, Password, Method, FilePath, Tag, false, ForegroundColor, BackgroundColor, PixelSize, CancelToken).ConfigureAwait(false);
        } else {
            ImagePlayground.QrCode.GenerateShadowSocks(ServerHost, Port, Password, Method, FilePath, Tag, false, ForegroundColor, BackgroundColor, PixelSize);
        }

        ShowGeneratedQrCode(FilePath, Show);
    }
}

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

    /// <inheritdoc />
    protected override void ProcessRecord() {
        if (string.IsNullOrWhiteSpace(FilePath)) {
            FilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName().Split('.')[0] + ".png");
            WriteWarning($"New-ImageQRCodeShadowSocks - No file path specified, saving to {FilePath}");
        }

        var output = Helpers.ResolvePath(FilePath);
        ImagePlayground.QrCode.GenerateShadowSocks(Host, Port, Password, Method, output, Tag);

        if (Show.IsPresent) {
            ImagePlayground.Helpers.Open(output, true);
        }
    }
}

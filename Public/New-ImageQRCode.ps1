function New-ImageQRCode {
    [cmdletBinding()]
    param(
        [Parameter(Mandatory)][string] $Content,
        [Parameter(Mandatory)][string] $FilePath,
        [ValidateSet('png', 'webp')][string] $FileFormat = 'png',
        [int] $Width = 256,
        [int] $Height = 256,
        [switch] $Transparent
    )

    # https://github.com/codebude/QRCoder/wiki/Advanced-usage---Payload-generators#320-whatsappmessage


    $QRCodeGenerator = [QRCoder.QRCodeGenerator]::new()
    $QRCodeData = $QRCodeGenerator.CreateQrCode($Content, [QRCoder.QRCodeGenerator+ECCLevel]::Q)
    $QRCode = [QRCoder.QRCode]::new($QRCodeData)
    $QrCodeImage = $QRCode.GetGraphic(20)
    if ($Transparent) {
        $QrCodeImage.MakeTransparent()
    }
    $QrCodeImage.Save($FilePath)

    <#

    System.Drawing.Bitmap GetGraphic(int pixelsPerModule)
    System.Drawing.Bitmap GetGraphic(int pixelsPerModule, string darkColorHtmlHex, string lightColorHtmlHex, bool drawQuietZones)
    System.Drawing.Bitmap GetGraphic(int pixelsPerModule, System.Drawing.Color darkColor, System.Drawing.Color lightColor, bool drawQuietZones)
    System.Drawing.Bitmap GetGraphic(int pixelsPerModule, System.Drawing.Color darkColor, System.Drawing.Color lightColor, System.Drawing.Bitmap icon, int iconSizePercent, int iconBorderWidth, bool drawQuietZones, System.Nullable[System.Drawing.Color] iconBackgroundC
    olor)

    QRCoder.QRCodeData CreateQrCode(QRCoder.PayloadGenerator+Payload payload)
    QRCoder.QRCodeData CreateQrCode(QRCoder.PayloadGenerator+Payload payload, QRCoder.QRCodeGenerator+ECCLevel eccLevel)
    QRCoder.QRCodeData CreateQrCode(string plainText, QRCoder.QRCodeGenerator+ECCLevel eccLevel, bool forceUtf8, bool utf8BOM, QRCoder.QRCodeGenerator+EciMode eciMode, int requestedVersion)
    QRCoder.QRCodeData CreateQrCode(byte[] binaryData, QRCoder.QRCodeGenerator+ECCLevel eccLevel)
    #>

}
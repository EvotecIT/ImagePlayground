function New-ASCIIQRCode {
    <#
    .SYNOPSIS
    Short description

    .DESCRIPTION
    Long description

    .PARAMETER Content
    Parameter description

    .EXAMPLE
    An example

    .NOTES
    https://github.com/codebude/QRCoder/wiki
    #>
    [cmdletBinding()]
    param(
        [string] $Content
    )
    $QRCodeGenerator = [QRCoder.QRCodeGenerator]::new()
    $QRCodeData = $QRCodeGenerator.CreateQrCode($Content, [QRCoder.QRCodeGenerator+ECCLevel]::Q)
    $QRCode = [QRCoder.AsciiQRCode]::new($QRCodeData)
    $QrCodeImage = $QRCode.GetGraphic(1)
    if ($Transparent) {
        $QrCodeImage.MakeTransparent()
    }
    $QrCodeImage
}
function New-QRCode {
    [Alias('New-ImageQRCode')]
    [cmdletBinding()]
    param(

    )

    [ImagePlayground.QrCode]::Generate($Content, $FilePath, $ImageFormat)
}
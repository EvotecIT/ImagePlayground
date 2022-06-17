function New-ImageQRCodeWiFi {
    [alias('New-QRCodeWiFi')]
    [cmdletBinding()]
    param(
        [string] $SSID,
        [string] $Password,
        [string] $FilePath,
        [System.Drawing.Imaging.ImageFormat] $ImageFormat,
        [switch] $Transparent
    )

    [ImagePlayground.QrCode]::GenerateWiFi($ssid, $password, $FilePath, $ImageFormat, $Transparent.IsPresent)

}
function New-ImageQRCodeWiFi {
    [alias('New-QRCodeWiFi')]
    [cmdletBinding()]
    param(
        [Parameter(Mandatory)][string] $SSID,
        [Parameter(Mandatory)][string] $Password,
        [Parameter(Mandatory)][string] $FilePath
    )

    [ImagePlayground.QrCode]::GenerateWiFi($ssid, $password, $FilePath, $false)

}
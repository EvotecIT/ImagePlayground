function New-ImageQRCodeWiFi {
    [alias('New-QRCodeWiFi')]
    [cmdletBinding()]
    param(
        [Parameter(Mandatory)][string] $SSID,
        [Parameter(Mandatory)][string] $Password,
        [Parameter(Mandatory)][string] $FilePath,
        [switch] $Transparent
    )

    [ImagePlayground.QrCode]::GenerateWiFi($ssid, $password, $FilePath, $Transparent.IsPresent)

}
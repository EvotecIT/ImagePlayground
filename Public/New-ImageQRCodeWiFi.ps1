function New-ImageQRCodeWiFi {
    <#
    .SYNOPSIS
    Creates QR code for WiFi connection

    .DESCRIPTION
    Creates QR code for WiFi connection

    .PARAMETER SSID
    SSID of the WiFi network

    .PARAMETER Password
    Password of the WiFi network

    .PARAMETER FilePath
    File path to where the image with QR code should be saved.

    .PARAMETER Show
    Opens the image in default image viewer after saving

    .EXAMPLE
    New-ImageQRCodeWiFi -SSID 'Evotec' -Password 'Evotec' -FilePath "$PSScriptRoot\Samples\QRCodeWiFi.png"

    .NOTES
    General notes
    #>
    [alias('New-QRCodeWiFi')]
    [cmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [ValidateNotNullOrEmpty()]
        [string] $SSID,
        [Parameter(Mandatory)]
        [string] $Password,
        [Parameter(Mandatory)]
        [ValidateNotNullOrEmpty()]
        [string] $FilePath,
        [switch] $Show
    )
    if ($FilePath) {
        $FilePath = $PSCmdlet.GetUnresolvedProviderPathFromPSPath($FilePath)
        if (Test-Path -LiteralPath $FilePath -PathType Leaf) {
            Write-Warning -Message "New-ImageQRCodeWiFi - File $FilePath found. Please check the path."
            return
        }
    }
    try {
        [ImagePlayground.QrCode]::GenerateWiFi($SSID, $Password, $FilePath, $false)
        if ($Show) {
            Invoke-Item -LiteralPath $FilePath
        }
    } catch {
        if ($PSBoundParameters.ErrorAction -eq 'Stop') {
            throw
        } else {
            Write-Warning -Message "New-ImageQRCodeWiFi - Error creating image $($_.Exception.Message)"
        }
    }
}

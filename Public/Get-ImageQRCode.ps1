function Get-ImageQRCode {
    <#
    .SYNOPSIS
    Gets QR code from image

    .DESCRIPTION
    Gets QR code from image

    .PARAMETER FilePath
    File path to image to be processed for QR code reading

    .EXAMPLE
    Get-ImageQRCode -FilePath "C:\Users\przemyslaw.klys\Downloads\IMG_4644.jpeg"

    .NOTES
    General notes
    #>
    [cmdletBinding()]
    param(
        [string] $FilePath
    )
    if ($FilePath -and (Test-Path -LiteralPath $FilePath)) {
        $QRCode = [ImagePlayground.QRCode]::Read($FilePath)
        $QRCode
    } else {
        Write-Warning -Message "Get-ImageQRCode - File $FilePath not found. Please check the path."
    }
}
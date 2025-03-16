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
        [Parameter(Mandatory)]
        [ValidateNotNullOrEmpty()]
        [string] $FilePath
    )
    $FilePath = $PSCmdlet.GetUnresolvedProviderPathFromPSPath($FilePath)

    if (-Not (Test-Path -LiteralPath $FilePath -PathType Leaf)) {
        Write-Warning -Message "Get-ImageQRCode - File $FilePath not found. Please check the path."
        return
    }
    [ImagePlayground.QRCode]::Read($FilePath)
}

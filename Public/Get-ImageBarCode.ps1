function Get-ImageBarCode {
    <#
    .SYNOPSIS
    Gets bar code from image

    .DESCRIPTION
    Gets bar code from image

    .PARAMETER FilePath
    File path to image to be processed for bar code reading

    .EXAMPLE
    Get-ImageBarCode -FilePath "C:\Users\przemyslaw.klys\Downloads\IMG_4644.jpeg"

    .NOTES
    General notes
    #>
    [cmdletBinding()]
    param(
        [string] $FilePath
    )
    if ($FilePath -and (Test-Path -LiteralPath $FilePath)) {
        $BarCode = [ImagePlayground.BarCode]::Read($FilePath)
        $BarCode
    } else {
        Write-Warning -Message "Get-ImageBarCode - File $FilePath not found. Please check the path."
    }
}
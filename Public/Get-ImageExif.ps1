function Get-ImageExif {
    <#
    .SYNOPSIS
    Gets EXIF data from image

    .DESCRIPTION
    Gets EXIF data from image.

    .PARAMETER FilePath
    File path to image to be processed for Exif Tag reading

    .EXAMPLE
    Get-ImageExif -FilePath "C:\Users\przemyslaw.klys\Downloads\IMG_4644.jpeg"

    .NOTES
    General notes
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][string] $FilePath,
        [switch] $Translate
    )
    if (-not (Test-Path $FilePath)) {
        Write-Warning -Message "Get-ImageExif - File not found: $FilePath"
        return
    }
    $Image = Get-Image -FilePath $FilePath
    try {
        if ($Translate) {
            $SingleExif = [ordered] @{}
            $Image.Metadata.ExifProfile.Values | ForEach-Object {
                $SingleExif[$_.Tag.ToString()] = $_.Value
            }
            [PSCustomObject] $SingleExif
        } else {
            $Image.Metadata.ExifProfile.Values
        }
    }
    finally {
        $Image.Dispose()
    }
}
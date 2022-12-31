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
        [Parameter(Mandatory)][string] $FilePath
    )
    $Image = Get-Image -FilePath $FilePath
    $Image.Metadata.ExifProfile.Values
}
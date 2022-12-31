function Set-ImageExif {
    <#
    .SYNOPSIS
    Sets EXIF tag to specific value

    .DESCRIPTION
    Sets EXIF tag to specific value

    .PARAMETER FilePath
    File path to image to be processed for Exif Tag manipulation. If FilePathOutput is not specified, the image will be overwritten.

    .PARAMETER FilePathOutput
    File path to output image. If not specified, the image will be overwritten.

    .PARAMETER ExifTag
    Exif Tag to be set

    .PARAMETER Value
    Value to be set

    .EXAMPLE
    $setImageExifSplat = @{
        FilePath       = "C:\Users\przemyslaw.klys\Downloads\IMG_4644.jpeg"
        ExifTag        = ([SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag]::DateTimeOriginal)
        Value          = ([DateTime]::Now).ToString("yyyy:MM:dd HH:mm:ss")
        FilePathOutput = "$PSScriptRoot\Output\IMG_4644.jpeg"
    }

    Set-ImageExif @setImageExifSplat

    .NOTES
    General notes
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][string] $FilePath,
        [string] $FilePathOutput,
        [Parameter(Mandatory)][SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag] $ExifTag,
        [Parameter(Mandatory)] $Value
    )
    $Image = Get-Image -FilePath "C:\Users\przemyslaw.klys\Downloads\IMG_4644.jpeg"
    # void SetValue[TValueType](SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag[TValueType] tag, TValueType value)
    $Image.Metadata.ExifProfile.SetValue($ExifTag, $Value)
    if ($FilePathOutput) {
        Save-Image -Image $Image -FilePath $FilePathOutput
    } else {
        Save-Image -Image $Image -FilePath $FilePath
    }
}
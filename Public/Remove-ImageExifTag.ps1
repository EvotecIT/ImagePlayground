function Remove-ImageExif {
    <#
    .SYNOPSIS
    Removes EXIF data from image

    .DESCRIPTION
    Removes EXIF data from image.

    .PARAMETER FilePath
    File path to image to be processed for Exif Tag removal

    .PARAMETER FilePathOutput
    File path to where the image with removed Exif Tag should be saved. If not specified, the original image will be overwritten.

    .PARAMETER ExifTag
    Exif Tag to be removed from image

    .PARAMETER All
    Removes all Exif Tags from image

    .EXAMPLE
    Remove-ImageExif -FilePath "C:\Users\przemyslaw.klys\Downloads\IMG_4644.jpeg" -ExifTag [SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag]::ExposureTime

    .EXAMPLE
    Remove-ImageExif -FilePath "C:\Users\przemyslaw.klys\Downloads\IMG_4644.jpeg" -All

    .NOTES
    General notes
    #>
    [CmdletBinding(DefaultParameterSetName = 'RemoveExifTag')]
    param(
        [Parameter(Mandatory, ParameterSetName = 'All')]
        [Parameter(Mandatory, ParameterSetName = 'RemoveExifTag')]
        [string] $FilePath,
        [Parameter(Mandatory, ParameterSetName = 'All')]
        [Parameter(Mandatory, ParameterSetName = 'RemoveExifTag')]
        [string] $FilePathOutput,
        [Parameter(Mandatory, ParameterSetName = 'RemoveExifTag')]
        [SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag[]] $ExifTag,
        [Parameter(Mandatory, ParameterSetName = 'All')][switch] $All
    )
    $Image = Get-Image -FilePath $FilePath
    try {
        if ($All) {
            Write-Verbose "Remove-ImageExif: Removing all Exif tags"
            $Image.Metadata.ExifProfile.Values.Clear()
        } else {
            foreach ($Tag in $ExifTag) {
                Write-Verbose "Remove-ImageExif: Removing $Tag"
                $Image.Metadata.ExifProfile.RemoveValue($Tag)
            }
        }
        if ($FilePathOutput) {
            Save-Image -Image $Image -FilePath $FilePathOutput
        } else {
            Save-Image -Image $Image -FilePath $FilePath
        }
    }
    finally {
        $Image.Dispose()
    }
}
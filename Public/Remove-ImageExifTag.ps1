function Remove-ImageExif {
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
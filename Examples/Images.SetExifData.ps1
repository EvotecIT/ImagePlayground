# Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

Get-ImageExif -FilePath "$PSScriptRoot\Samples\Snow.jpeg" -Translate | Format-List Datetime*, GPS*

$setImageExifSplat = @{
    FilePath       = "$PSScriptRoot\Samples\Snow.jpeg"
    ExifTag        = ([SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag]::DateTimeOriginal)
    Value          = ([DateTime]::Now).ToString("yyyy:MM:dd HH:mm:ss")
    FilePathOutput = "$PSScriptRoot\Output\Snow_Updated.jpeg"
}

Set-ImageExif @setImageExifSplat

Get-ImageExif -FilePath "$PSScriptRoot\Output\Snow_Updated.jpeg" -Translate | Format-List Datetime*, GPS*

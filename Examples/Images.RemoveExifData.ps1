# Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

Get-ImageExif -FilePath "$PSScriptRoot\Samples\Snow.jpeg" | Format-Table

$removeImageExifSplat = @{
    FilePath       = "$PSScriptRoot\Samples\Snow.jpeg"
    ExifTag        = [SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag]::GPSLatitude, [SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag]::GPSLongitude
    FilePathOutput = "$PSScriptRoot\Output\Snow_NOGPS.jpeg"
}
Remove-ImageExif @removeImageExifSplat

$removeImageExifSplat = @{
    FilePath       = "$PSScriptRoot\Samples\Snow.jpeg"
    All            = $true
    FilePathOutput = "$PSScriptRoot\Output\Snow_NOEXIF.jpeg"
}
Remove-ImageExif @removeImageExifSplat

Get-ImageExif -FilePath "$PSScriptRoot\Output\Snow_NOGPS.jpeg" | Format-Table
Get-ImageExif -FilePath "$PSScriptRoot\Output\Snow_NOEXIF.jpeg" | Format-Table

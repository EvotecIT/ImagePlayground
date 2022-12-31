Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

Get-ImageExif -FilePath "C:\Users\przemyslaw.klys\Downloads\IMG_4644.jpeg" | Format-Table

$removeImageExifSplat = @{
    FilePath       = "C:\Users\przemyslaw.klys\Downloads\IMG_4644.jpeg"
    ExifTag        = [SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag]::GPSLatitude, [SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag]::GPSLongitude
    FilePathOutput = "$PSScriptRoot\Output\IMG_46441.jpeg"
}
Remove-ImageExif @removeImageExifSplat

$removeImageExifSplat = @{
    FilePath       = "C:\Users\przemyslaw.klys\Downloads\IMG_4644.jpeg"
    All            = $true
    FilePathOutput = "$PSScriptRoot\Output\IMG_46442.jpeg"
}
Remove-ImageExif @removeImageExifSplat

Get-ImageExif -FilePath $PSScriptRoot\Output\IMG_46441.jpeg | Format-Table
Get-ImageExif -FilePath $PSScriptRoot\Output\IMG_46442.jpeg | Format-Table
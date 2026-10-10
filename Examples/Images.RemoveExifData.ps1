Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

Get-ImageExif -FilePath "$PSScriptRoot\Samples\Snow.jpeg" | Format-Table

$removeImageExifSplat = @{
    FilePath       = "$PSScriptRoot\Samples\Snow.jpeg"
    ExifTag        = [OfficeIMO.Drawing.OfficeExifTag]::GPSLatitude, [OfficeIMO.Drawing.OfficeExifTag]::GPSLongitude
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
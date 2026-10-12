Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

$Image = Get-Image -FilePath "$PSScriptRoot\Samples\Snow.jpeg"
try {
    $Image.GetExifValues() | Format-Table
    $Image.SetExifValue([OfficeIMO.Drawing.OfficeExifTag]::DateTimeOriginal, (Get-Date -Format 'yyyy:MM:dd HH:mm:ss'))

    # GPS coordinates are degrees, minutes, and seconds, expressed as unsigned fractions.
    [OfficeIMO.Drawing.OfficeRational[]] $Latitude = @(
        [OfficeIMO.Drawing.OfficeRational]::new(46, 1)
        [OfficeIMO.Drawing.OfficeRational]::new(32, 1)
        [OfficeIMO.Drawing.OfficeRational]::new(1328, 100)
    )
    [OfficeIMO.Drawing.OfficeRational[]] $Longitude = @(
        [OfficeIMO.Drawing.OfficeRational]::new(6, 1)
        [OfficeIMO.Drawing.OfficeRational]::new(35, 1)
        [OfficeIMO.Drawing.OfficeRational]::new(1000, 100)
    )
    $Image.SetExifValue([OfficeIMO.Drawing.OfficeExifTag]::GPSLatitudeRef, 'N')
    $Image.SetExifValue([OfficeIMO.Drawing.OfficeExifTag]::GPSLongitudeRef, 'E')
    $Image.SetExifValue([OfficeIMO.Drawing.OfficeExifTag]::GPSLatitude, $Latitude)
    $Image.SetExifValue([OfficeIMO.Drawing.OfficeExifTag]::GPSLongitude, $Longitude)
    Save-Image -Image $Image -FilePath "$PSScriptRoot\Samples\Snow-GPS.jpeg"
    $Image.GetExifValues() | Format-Table
} finally {
    $Image.Dispose()
}

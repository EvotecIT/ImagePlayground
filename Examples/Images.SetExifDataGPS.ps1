Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

# details on rational numbers
# https://github.com/SixLabors/ImageSharp/discussions/1295

$Image = Get-Image -FilePath "C:\Users\przemyslaw.klys\Downloads\IMG_4644.jpeg"
$Image.Metadata.ExifProfile.Values | Format-Table

# lets set some dates
$TimeNow = [DateTime]::Now
$CurrentDateTime = $TimeNow.ToString("yyyy:MM:dd HH:mm:ss")
$Image.Metadata.ExifProfile.SetValue([SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag]::DateTimeOriginal, $CurrentDateTime)

# lets get latitudes and longitudes
$Lat = $Image.Metadata.ExifProfile.Values | Where-Object { $_.Tag -like 'GPSLatitude' }
$Lat.Value | Format-Table

$Lon = $Image.Metadata.ExifProfile.Values | Where-Object { $_.Tag -like 'GPSLongitude' }
$Lon.Value | Format-Table

# lets remove current values
$null = $Image.Metadata.ExifProfile.RemoveValue([SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag]::GPSLatitude)
$null = $Image.Metadata.ExifProfile.RemoveValue([SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag]::GPSLongitude)

# lets see current values for all exif
$Image.Metadata.ExifProfile.Values | Format-Table

# now lets add new values in place for latitudes and longitudes
$Latitude = @(
    [SixLabors.ImageSharp.Rational]::new(46, 1)
    [SixLabors.ImageSharp.Rational]::new(32, 1)
    [SixLabors.ImageSharp.Rational]::new(1328, 100)
)
$Image.Metadata.ExifProfile.SetValue([SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag]::GPSLatitude, $Latitude)

$Longitude = @(
    [SixLabors.ImageSharp.Rational]::new(46, 1)
    [SixLabors.ImageSharp.Rational]::new(32, 1)
    [SixLabors.ImageSharp.Rational]::new(1328, 100)
)
$Image.Metadata.ExifProfile.SetValue([SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag]::GPSLongitude, $Longitude)

# lets see current values for GPSLatitude
$Lat = $Image.Metadata.ExifProfile.Values | Where-Object { $_.Tag -like 'GPSLatitude' }
$Lat.Value | Format-Table

$Lon = $Image.Metadata.ExifProfile.Values | Where-Object { $_.Tag -like 'GPSLongitude' }
$Lon.Value | Format-Table
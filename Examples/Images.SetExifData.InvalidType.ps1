Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

# This will throw because the value type does not match the tag's expected type
$setImageExifSplat = @{
    FilePath = "$PSScriptRoot\Samples\Snow.jpeg"
    ExifTag  = [SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag]::DateTimeOriginal
    Value    = 123 # should be a string or DateTime
}

Set-ImageExif @setImageExifSplat



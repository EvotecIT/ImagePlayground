Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

# Load a HEIF or AVIF image and convert it to PNG
$source = 'C:\Images\photo.heic'
$dest = Join-Path $PSScriptRoot 'Output\photo.png'

$image = Get-Image -FilePath $source
Save-Image -Image $image -FilePath $dest
$image.Dispose()

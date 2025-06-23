Import-Module .\ImagePlayground.psd1 -Force

# Resolve a remote file path. The temporary file is removed after processing.
$url = 'https://example.com/image.png'
$path = [ImagePlayground.Helpers]::ResolvePath($url)
Write-Host "Downloaded to $path"

# Do something with the file...

[ImagePlayground.Helpers]::CleanupTempFiles()

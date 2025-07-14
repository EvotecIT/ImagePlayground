Import-Module .\ImagePlayground.psd1 -Force

$url = 'https://example.com/image.png'
$task = [ImagePlayground.Helpers]::ResolvePathAsync($url)
$path = $task.GetAwaiter().GetResult()
Write-Host "Downloaded to $path"
[ImagePlayground.Helpers]::CleanupTempFiles()

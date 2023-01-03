Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

$Image = Get-Image -FilePath $PSScriptRoot\Samples\LogoEvotec.png
$Rectangle = [SixLabors.ImageSharp.Rectangle]::new(30, 30, 400, 100)
$Image.Crop($Rectangle)
Save-Image -Image $Image -Open -FilePath $PSScriptRoot\Output\LogoEvotecChanged.png
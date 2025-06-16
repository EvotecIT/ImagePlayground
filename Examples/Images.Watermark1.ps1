Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

# Add watermark
$Image = Get-Image -FilePath "$PSScriptRoot\Samples\PrzemyslawKlysAndKulkozaurr.jpg"
# void WatermarkImage(string filePath, ImagePlayground.Image+WatermarkPlacement placement, float opacity = 1, float padding = 18, int rotate = 0, SixLabors.ImageSharp.Processing.FlipMode flipMode = SixLabors.ImageSharp.Processing.FlipMode.None, int watermarkPercentage = 20)

# place in the middle
$Image.WatermarkImage("$PSScriptRoot\Samples\LogoEvotec.png", [ImagePlayground.Image+WatermarkPlacement]::Middle, 0.5, 0.5, 90, [SixLabors.ImageSharp.Processing.FlipMode]::Vertical, 100)

# place with x,y
$Image.WatermarkImage("$PSScriptRoot\Samples\LogoEvotec.png", 50, 100, 0.5, 0.5)

# Add watermark with rotation 90 degrees
Save-Image -Image $Image -Open -FilePath $PSScriptRoot\Output\QRCodeWithImage.jpg
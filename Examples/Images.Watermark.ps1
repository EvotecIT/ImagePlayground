Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

# Add watermark
$Image = Get-Image -FilePath $PSScriptRoot\Samples\PrzemyslawKlysAndKulkozaurr.jpg
# void WatermarkImage(string filePath, ImagePlayground.Image+WatermarkPlacement placement, float opacity = 1, float padding = 18, int rotate = 0, SixLabors.ImageSharp.Processing.FlipMode flipMode = SixLabors.ImageSharp.Processing.FlipMode.None, int watermarkPercentage = 20)
$Image.WatermarkImage("$PSScriptRoot\Samples\LogoEvotec.png", [ImagePlayground.Image+WatermarkPlacement]::Middle, 0.5, 0.5)
# Add watermark with rotation 90 degrees
$Image.WatermarkImage("$PSScriptRoot\Samples\LogoEvotec.png", [ImagePlayground.Image+WatermarkPlacement]::TopLeft, 1, 18, 90)
# Add watermark with text
# There are 2 methods to add watermark with text
#void Watermark(string text, float x, float y, SixLabors.ImageSharp.Color color, float fontSize = 16, string fontFamilyName = "Arial", float padding = 18)
#void Watermark(string text, ImagePlayground.Image+WatermarkPlacement placement, SixLabors.ImageSharp.Color color, float fontSize = 16, string fontFamilyName = "Arial", float padding = 18)
$Image.Watermark("Evotec", [ImagePlayground.Image+WatermarkPlacement]::TopRight, [SixLabors.ImageSharp.Color]::Blue, 50, "Calibri")

Save-Image -Image $Image -Open -FilePath $PSScriptRoot\Output\PrzemyslawKlysAndKulkozaurrWatermarkWithText.jpg
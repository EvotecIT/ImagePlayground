Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

$Image = Get-Image -FilePath $PSScriptRoot\Samples\LogoEvotec.png
$Image.BlackWhite()
$Image.BackgroundColor("Red")
Save-Image -Image $Image -Open -FilePath $PSScriptRoot\Output\LogoEvotecChanged.png

# Save Pixalate
$Image = Get-Image -FilePath $PSScriptRoot\Samples\PrzemyslawKlysAndKulkozaurr.jpg
$Image.Pixelate(30)
Save-Image -Image $Image -Open -FilePath $PSScriptRoot\Output\PrzemyslawKlysAndKulkozaurrPixelate.jpg

# Save as Polaroid
$Image = Get-Image -FilePath $PSScriptRoot\Samples\PrzemyslawKlysAndKulkozaurr.jpg
$Image.Polaroid()
Save-Image -Image $Image -Open -FilePath $PSScriptRoot\Output\PrzemyslawKlysAndKulkozaurrPolaroid.jpg

# Add watermark
$Image = Get-Image -FilePath $PSScriptRoot\Samples\PrzemyslawKlysAndKulkozaurr.jpg
$Image.WatermarkImage("$PSScriptRoot\Samples\LogoEvotec.png",[ImagePlayground.Image+WatermarkPlacement]::Middle, 0.5, 0.5)
# Add watermark with rotation 90 degrees
$Image.WatermarkImage("$PSScriptRoot\Samples\LogoEvotec.png",[ImagePlayground.Image+WatermarkPlacement]::TopLeft, 1, 18, 90)
# Resize 200% in the same image
$Image.Resize(200)
# Rotate 30 degrees in the same image
$Image.Rotate(30)
Save-Image -Image $Image -Open -FilePath $PSScriptRoot\Output\PrzemyslawKlysAndKulkozaurrWatermark.jpg
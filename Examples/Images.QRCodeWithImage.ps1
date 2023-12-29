Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

New-ImageQRCode -Content 'https://evotec.xyz' -FilePath "$PSScriptRoot\Samples\QRCode.png" -Verbose

# Add watermark
$Image = Get-Image -FilePath "$PSScriptRoot\Samples\QRCode.png"
# void WatermarkImage(string filePath, ImagePlayground.Image+WatermarkPlacement placement, float opacity = 1, float padding = 18, int rotate = 0, SixLabors.ImageSharp.Processing.FlipMode flipMode = SixLabors.ImageSharp.Processing.FlipMode.None, int watermarkPercentage = 20)
$Image.WatermarkImage("$PSScriptRoot\Samples\LogoEvotec.png", [ImagePlayground.Image+WatermarkPlacement]::TopLeft, 1, 0.5, 0, [SixLabors.ImageSharp.Processing.FlipMode]::None, 50)
# Add watermark with rotation 90 degrees
Save-Image -Image $Image -Open -FilePath $PSScriptRoot\Output\QRCodeWithImage.jpg
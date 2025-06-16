<p align="center">
  <a href="https://dev.azure.com/evotecpl/ImagePlayground/_build/results?buildId=latest"><img src="https://img.shields.io/azure-devops/build/evotecpl/39c74615-8f34-4af0-a835-68dc33f9214f/14?label=Azure%20Pipelines&style=flat-square"></a>
  <a href="https://www.powershellgallery.com/packages/ImagePlayground"><img src="https://img.shields.io/powershellgallery/v/ImagePlayground.svg?style=flat-square"></a>
  <a href="https://www.powershellgallery.com/packages/ImagePlayground"><img src="https://img.shields.io/powershellgallery/vpre/ImagePlayground.svg?label=powershell%20gallery%20preview&colorB=yellow&style=flat-square"></a>
  <a href="https://github.com/EvotecIT/ImagePlayground"><img src="https://img.shields.io/github/license/EvotecIT/ImagePlayground.svg?style=flat-square"></a>
</p>

<p align="center">
  <a href="https://www.powershellgallery.com/packages/ImagePlayground"><img src="https://img.shields.io/powershellgallery/p/ImagePlayground.svg?style=flat-square"></a>
  <a href="https://github.com/EvotecIT/ImagePlayground"><img src="https://img.shields.io/github/languages/top/evotecit/ImagePlayground.svg?style=flat-square"></a>
  <a href="https://github.com/EvotecIT/ImagePlayground"><img src="https://img.shields.io/github/languages/code-size/evotecit/ImagePlayground.svg?style=flat-square"></a>
  <a href="https://www.powershellgallery.com/packages/ImagePlayground"><img src="https://img.shields.io/powershellgallery/dt/ImagePlayground.svg?style=flat-square"></a>
</p>

<p align="center">
  <a href="https://twitter.com/PrzemyslawKlys"><img src="https://img.shields.io/twitter/follow/PrzemyslawKlys.svg?label=Twitter%20%40PrzemyslawKlys&style=flat-square&logo=twitter"></a>
  <a href="https://evotec.xyz/hub"><img src="https://img.shields.io/badge/Blog-evotec.xyz-2A6496.svg?style=flat-square"></a>
  <a href="https://www.linkedin.com/in/pklys"><img src="https://img.shields.io/badge/LinkedIn-pklys-0077B5.svg?logo=LinkedIn&style=flat-square"></a>
</p>

`ImagePlayground` is a C# library and a PowerShell module that allows you to play with images in different ways.
It allows to create QR codes, BAR codes, Charts and do image manipulation.
It provides ability to read QR codes and BAR codes.
It provides ability to manipulate imagates by adding text, resizing, cropping, rotating, blurring, sharpening, etc.
It provides way to add watermark to images - either by text or by image.

## Known Issues

This module will work fine for PowerShell 5.1 and PowerShell 7+. It works on Windows, Linux and macOS, with an exception to support of charts on Linux and macOS. There is a plan to add support for charts on Linux and macOS in the future.

**Currently the module has a problem when running in VSCode PowerShell extension when on PowerShell 5.1 (other versions work fine!)**
It works fine when running in PowerShell 5.1 console, or ISE (shrug!).

## Features

- ☑️ QR Codes
  - ☑️ QR Code Standard
  - ☑️ QR Code WiFi
  - ☑️ QR Code Contact
  - 📥 Others to be added
- ☑️ Barcode
  - ☑️ EAN
  - ☑️ Code.39
- ☑️ Barcode Reader - library can read barcodes
- ☑️ QR Code Reader - library can read QR codes
- ☑️ Image Combine
- ☑️ Image Resize
- ☑️ Image Crop
- ☑️ Image Rotate
- ☑️ Image Grayscale
- ☑️ Image Blur
- ☑️ Image Sharpen
- ☑️ Many more

## Installation

```powershell
Install-Module ImagePlayGround -Force -Verbose
```

## Usage

### Creating and reading QR Codes

Creating basic QR Code

```powershell
New-ImageQRCode -Content 'https://evotec.xyz' -FilePath "$PSScriptRoot\Samples\QRCode.png"
```

![QRCode.png](https://github.com/EvotecIT/ImagePlayground/blob/feb33319f00df1933f53a4df89d65aa498278e41/Examples/Samples/QRCode.png)

Creating QR codes and reading them back is as easy as:

```powershell
New-ImageQRContact -FilePath "$PSScriptRoot\Samples\QRCodeContact.png" -outputType VCard4 -Firstname "Przemek" -Lastname "Klys" -MobilePhone "+48 500 000 000"

$Message = Get-ImageQRCode -FilePath "$PSScriptRoot\Samples\QRCodeContact.png"
$Message | Format-List *

New-ImageQRCodeWiFi -SSID 'Evotec' -Password 'EvotecPassword' -FilePath "$PSScriptRoot\Samples\QRCodeWiFi.png"

$Message = Get-ImageQRCode -FilePath "$PSScriptRoot\Samples\QRCodeWiFi.png"
$Message | Format-List *

New-ImageQRCode -Content 'https://evotec.xyz' -FilePath "$PSScriptRoot\Samples\QRCode.png"

$Message = Get-ImageQRCode -FilePath "$PSScriptRoot\Samples\QRCode.png"
$Message | Format-List *
```

### Creating charts

#### Bar charts

```powershell
New-ImageChart {
    New-ImageChartBar -Value 5 -Label "C#"
    New-ImageChartBar -Value 12 -Label "C++"
    New-ImageChartBar -Value 10 -Label "PowerShell"
} -Show -FilePath $PSScriptRoot\Samples\ChartsBar1.png
```

![ChartsBar.png](https://github.com/EvotecIT/ImagePlayground/blob/feb33319f00df1933f53a4df89d65aa498278e41/Examples/Samples/ChartsBar.png)

#### Pie charts

```powershell
New-ImageChart {
    New-ImageChartPie -Name "C#" -Value 5
    New-ImageChartPie -Name "C++" -Value 12
    New-ImageChartPie -Name "PowerShell" -Value 10
} -Show -FilePath $PSScriptRoot\Output\ChartsPie1.png -Width 500 -Height 500
```

![ChartsPie.png](https://github.com/EvotecIT/ImagePlayground/blob/feb33319f00df1933f53a4df89d65aa498278e41/Examples/Samples/ChartsPie.png)

#### Line charts

```powershell
New-ImageChart {
    New-ImageChartLine -Value 5, 10, 12, 18, 10, 13 -Name "C#"
    New-ImageChartLine -Value 10,15,30,40,50,60 -Name "C++"
    New-ImageChartLine -Value 10,5,12,18,30,60 -Name "PowerShell"
} -Show -FilePath $PSScriptRoot\Output\ChartsLine1.png
```

![ChartsLine.png](https://github.com/EvotecIT/ImagePlayground/blob/feb33319f00df1933f53a4df89d65aa498278e41/Examples/Samples/ChartsLine.png)

#### Radar charts

```powershell
New-ImageChart {
    New-ImageChartRadial -Name "C#" -Value 5
    New-ImageChartRadial -Name "AutoIt v3" -Value 50
    New-ImageChartRadial -Name "PowerShell" -Value 10
    New-ImageChartRadial -Name "C++" -Value 18
    New-ImageChartRadial -Name "F#" -Value 100
} -Show -FilePath $PSScriptRoot\Samples\ChartsRadial.png -Width 500 -Height 500
```

![ChartsRadial.png](https://raw.githubusercontent.com/EvotecIT/ImagePlayground/master/Examples/Samples/ChartsRadial.png)

### Reading bar codes

```powershell
Get-ImageBarCode -FilePath $PSScriptRoot\Samples\BarcodeEAN13.png
Get-ImageBarCode -FilePath $PSScriptRoot\Samples\BarcodeEAN7.png
```

### Image processing

Image processing exposes several methods. It allows to resize, crop, rotate, grayscale, blur, sharpen and more.
You can use all available methods as shown below:

```powershell
AdaptiveThreshold Method     void AdaptiveThreshold()
AddImage          Method     void AddImage(string filePath, int x, int y, float opacity), void AddImage(SixLabors.ImageSharp.Image image, int x, int y, float opacity), void AddImage(SixLabors.ImageSharp.Image image, SixLabors.ImageSharp.Point location, float opacity)
AddText           Method     void AddText(float x, float y, string text, SixLabors.ImageSharp.Color color, float fontSize = 16, string fontFamilyName = "Arial")
AutoOrient        Method     void AutoOrient()
BackgroundColor   Method     void BackgroundColor(SixLabors.ImageSharp.Color color)
BlackWhite        Method     void BlackWhite()
BokehBlur         Method     void BokehBlur()
BoxBlur           Method     void BoxBlur()
Brightness        Method     void Brightness(float amount)
Contrast          Method     void Contrast(float amount)
Crop              Method     void Crop(SixLabors.ImageSharp.Rectangle rectangle)
Filter            Method     void Filter(SixLabors.ImageSharp.ColorMatrix colorMatrix)
Flip              Method     void Flip(SixLabors.ImageSharp.Processing.FlipMode flipMode)
GaussianBlur      Method     void GaussianBlur(System.Nullable[float] sigma)
GaussianSharpen   Method     void GaussianSharpen(System.Nullable[float] sigma)
GetTextSize       Method     SixLabors.Fonts.FontRectangle GetTextSize(string text, float fontSize, string fontFamilyName)
Grayscale         Method     void Grayscale(SixLabors.ImageSharp.Processing.GrayscaleMode grayscaleMode = SixLabors.ImageSharp.Processing.GrayscaleMode.Bt709)
Hue               Method     void Hue(float degrees)
OilPaint          Method     void OilPaint(), void OilPaint(int levels, int brushSize)
Pixelate          Method     void Pixelate(), void Pixelate(int size)
Polaroid          Method     void Polaroid()
Resize            Method     void Resize(System.Nullable[int] width, System.Nullable[int] height, bool keepAspectRatio = True), void Resize(int percentage)
Rotate            Method     void Rotate(SixLabors.ImageSharp.Processing.RotateMode rotateMode), void Rotate(float degrees)
RotateFlip        Method     void RotateFlip(SixLabors.ImageSharp.Processing.RotateMode rotateMode, SixLabors.ImageSharp.Processing.FlipMode flipMode)
Saturate          Method     void Saturate(float amount)
Watermark         Method     void Watermark(string text, float x, float y, SixLabors.ImageSharp.Color color, float fontSize = 16, string fontFamilyName = "Arial", float padding = 18), void Watermark(string text, ImagePlayground.Image+WatermarkPlacement placement, SixLabors.ImageSharp.Color color, float fontSize = 16, string fontFamilyName = "Arial", fl…
WatermarkImage    Method     void WatermarkImage(string filePath, ImagePlayground.Image+WatermarkPlacement placement, float opacity = 1, float padding = 18, int rotate = 0, SixLabors.ImageSharp.Processing.FlipMode flipMode = SixLabors.ImageSharp.Processing.FlipMode.None, int watermarkPercentage = 100)
```

#### Converting images

```powershell
ConvertTo-Image -FilePath $PSScriptRoot\Samples\LogoEvotec.png -OutputPath $PSScriptRoot\Output\LogoEvotec.jpg
```

#### Resizing images

```powershell
Resize-Image -FilePath $PSScriptRoot\Samples\LogoEvotec.png -OutputPath $PSScriptRoot\Output\LogoEvotecResize.png -Width 100 -Height 100

Resize-Image -FilePath $PSScriptRoot\Samples\LogoEvotec.png -OutputPath $PSScriptRoot\Output\LogoEvotecResizeMaintainAspectRatio.png -Width 300

Resize-Image -FilePath $PSScriptRoot\Samples\LogoEvotec.png -OutputPath $PSScriptRoot\Output\LogoEvotecResizePercent.png -Percentage 200
```

#### Manipulating images

```powershell
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
# Use cmdlet for a quick overlay
Add-ImageWatermark -FilePath $PSScriptRoot\Samples\PrzemyslawKlysAndKulkozaurr.jpg -OutputPath $PSScriptRoot\Output\Watermark.png -WatermarkPath $PSScriptRoot\Samples\LogoEvotec.png

# Resize 200% in the same image
$Image.Resize(200)
# Rotate 30 degrees in the same image
$Image.Rotate(30)
Save-Image -Image $Image -Open -FilePath $PSScriptRoot\Output\PrzemyslawKlysAndKulkozaurrWatermark.jpg
```

#### Adding watermarks

```powershell
# Add watermark
$Image = Get-Image -FilePath $PSScriptRoot\Samples\PrzemyslawKlysAndKulkozaurr.jpg
# void WatermarkImage(string filePath, ImagePlayground.Image+WatermarkPlacement placement, float opacity = 1, float padding = 18, int rotate = 0, SixLabors.ImageSharp.Processing.FlipMode flipMode = SixLabors.ImageSharp.Processing.FlipMode.None, int watermarkPercentage = 100)
$Image.WatermarkImage("$PSScriptRoot\Samples\LogoEvotec.png", [ImagePlayground.Image+WatermarkPlacement]::Middle, 0.5, 0.5)
# Add watermark with rotation 90 degrees
$Image.WatermarkImage("$PSScriptRoot\Samples\LogoEvotec.png", [ImagePlayground.Image+WatermarkPlacement]::TopLeft, 1, 18, 90)
# Add watermark with text
# There are 2 methods to add watermark with text
#void Watermark(string text, float x, float y, SixLabors.ImageSharp.Color color, float fontSize = 16, string fontFamilyName = "Arial", float padding = 18)
#void Watermark(string text, ImagePlayground.Image+WatermarkPlacement placement, SixLabors.ImageSharp.Color color, float fontSize = 16, string fontFamilyName = "Arial", float padding = 18)
$Image.Watermark("Evotec", [ImagePlayground.Image+WatermarkPlacement]::TopRight, [SixLabors.ImageSharp.Color]::Blue, 50, "Calibri")

Save-Image -Image $Image -Open -FilePath $PSScriptRoot\Output\PrzemyslawKlysAndKulkozaurrWatermarkWithText.jpg
```

![PrzemyslawKlysAndKulkozaurrWatermarkWithText.jpg](https://github.com/EvotecIT/ImagePlayground/blob/feb33319f00df1933f53a4df89d65aa498278e41/Examples/Samples/PrzemyslawKlysAndKulkozaurrWatermarkWithText.jpg)

#### Get EXIF data

```powershell
$Image = Get-Image -FilePath "C:\Users\przemyslaw.klys\Downloads\IMG_4644.jpeg"
$Image.Width
$Image.Height
$Image.Metadata
$Image.Metadata.ExifProfile | Format-List
$Image.Metadata.ExifProfile.Values | Format-Table
$Image.Metadata.IccProfile.Header | Format-Table
$Image.Metadata.IccProfile.Entries | Format-Table
```

![Exif.png](https://github.com/EvotecIT/ImagePlayground/blob/feb33319f00df1933f53a4df89d65aa498278e41/Examples/Samples/Exif.png)

#### Set EXIF data

```powershell
Get-ImageExif -FilePath "C:\Users\przemyslaw.klys\Downloads\IMG_4644.jpeg" -Translate | Format-List Datetime*, GPS*

$setImageExifSplat = @{
    FilePath       = "C:\Users\przemyslaw.klys\Downloads\IMG_4644.jpeg"
    ExifTag        = ([SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag]::DateTimeOriginal)
    Value          = ([DateTime]::Now).ToString("yyyy:MM:dd HH:mm:ss")
    FilePathOutput = "$PSScriptRoot\Output\IMG_4644.jpeg"
}

Set-ImageExif @setImageExifSplat

Get-ImageExif -FilePath $PSScriptRoot\Output\IMG_4644.jpeg -Translate | Format-List Datetime*, GPS*
```

#### Remove EXIF data

```powershell
Get-ImageExif -FilePath "C:\Users\przemyslaw.klys\Downloads\IMG_4644.jpeg" | Format-Table

$removeImageExifSplat = @{
    FilePath       = "C:\Users\przemyslaw.klys\Downloads\IMG_4644.jpeg"
    ExifTag        = [SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag]::GPSLatitude, [SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag]::GPSLongitude
    FilePathOutput = "$PSScriptRoot\Output\IMG_46441.jpeg"
}
Remove-ImageExif @removeImageExifSplat

$removeImageExifSplat = @{
    FilePath       = "C:\Users\przemyslaw.klys\Downloads\IMG_4644.jpeg"
    All            = $true
    FilePathOutput = "$PSScriptRoot\Output\IMG_46442.jpeg"
}
Remove-ImageExif @removeImageExifSplat

Get-ImageExif -FilePath $PSScriptRoot\Output\IMG_46441.jpeg | Format-Table
Get-ImageExif -FilePath $PSScriptRoot\Output\IMG_46442.jpeg | Format-Table
```

### Libraries

#### Current libraries and their licenses:
- [QRCoder-ImageSharp](https://github.com/JPlenert/QRCoder-ImageSharp) - licensed MIT
- [ScottPlot](https://github.com/ScottPlot/ScottPlot) - licensed MIT
- [Barcoder](https://github.com/huysentruitw/barcoder) - licensed MIT
- [BarcodeReader.ImageSharp](https://github.com/baltermia/barcodereader-imagesharp) - licensed MIT
- [SixLabors.ImageSharp](https://github.com/SixLabors/ImageSharp) - licensed Apache 2.0
- [Codeuctivity.ImageSharpCompare](https://github.com/Codeuctivity/ImageSharp.Compare) - licensed Apache 2.0

#### Future Considerations / Alternatives
If any of the libraries above prove insufficient, we can add more libraries to the list or replace with those

- [OxyPlot](https://github.com/oxyplot/oxyplot) - licensed MIT
- [Microcharts](https://github.com/microcharts-dotnet/Microcharts) - licensed MIT


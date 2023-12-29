### 0.0.6 - WIP
- Added new library: Codeuctivity.ImageSharpCompare 2.0.76
- Added Image Compare feature

### 0.0.5 - 2023.12.29

## What's new
- Fixes issue with resizing when providing only one dimension
- Added ability to create new empty image
- Added `New-ImageChartRadial` that allows you to create a radial chart
- Added new methods:
  - Dither
  - DrawLines
  - DrawPolygon
  - Fill
  - HistogramEqualization
  - Kodachrome
  - Lightness
  - Lomograph
  - Invert
  - Opacity
  - Sepia
  - Skew
  - Vignette

**Full Changelog**: https://github.com/EvotecIT/ImagePlayground/compare/v0.0.4...v0.0.5

```powershell
New-ImageChart {
    New-ImageChartRadial -Name "C#" -Value 5
    New-ImageChartRadial -Name "AutoIt v3" -Value 50
    New-ImageChartRadial -Name "PowerShell" -Value 10
    New-ImageChartRadial -Name "C++" -Value 18
    New-ImageChartRadial -Name "F#" -Value 100
} -Show -FilePath $PSScriptRoot\Samples\ChartsPie.png -Width 500 -Height 500
```

### 0.0.4 - 2022.12.31
- Added `Set-ImageExif` that sets exif data on an image.
- Added `Get-ImageExif` that gets exif data from an image.
- Added `Remove-ImageExif` that removes exif data from an image.
- Updated some docs

### 0.0.3 - 2022.12.30
- Added `Merge-Image` that merges multiple images into one
- Added `New-ImageBarCode` that creates a barcode image
- Removed Libraries from GIT, you will have to build them using PSPublishModule in the builds folder if you want to play with GIT version

### 0.0.2 - 2022.12.29
- Works correctly now on Windows PowerShell 5.1 (thank you @jborean93)
- Added line charts, bar charts, and pie charts
- Added image manipulation capabilities
- Added image watermark by text and image
- Added image exif reading
- Added image convert to other formats
- Added image resize functionality

### 0.0.1 - 2022.07.24
- Initial release to test things out
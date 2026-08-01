## Unreleased

### Added

- Added `New-ImageVisualStory` and `New-ImageVisualMotionCue` as thin PowerShell adapters over ChartForgeX visual grids and motion timelines. SVG and HTML keep script-free motion; PNG renders the same completed state.
- Added PowerShell-native `New-ImageConsoleStoryCommand`, `New-ImageConsoleStoryOutput`, `New-ImageConsoleStoryTable`, `New-ImageConsoleStoryBlankLine`, and `New-ImageConsoleStoryPause` definitions for composing terminal SVG, GIF, APNG, HTML, and PNG stories without direct fluent-builder calls.

### Breaking changes

- Removed the PowerShell `-Async` parameter. Affected cmdlets now always use their cancellable asynchronous core implementation.
- Removed the unused `ImagePlayground.BarcodeResult` type; CodeGlyphX owns barcode result contracts for .NET callers.
- Removed the unused Microsoft.CSharp dependency and limited System.Text.Json and System.Memory package references to .NET Standard 2.0 and .NET Framework 4.7.2.
- Removed the Windows-only `ImagePlayground.Gdi` project and its `System.Drawing` implementation.

### Changed

- Upgraded the PowerShell visual adapter to ChartForgeX 1.2.0 for generic named motion targets, deterministic static fallback, and reduced-motion and print-safe output.
- Kept every `New-ImageChart*` command as a thin PowerShell adapter over ChartForgeX while keeping the C# packages as separate capability owners.
- Upgraded the PowerShell chart adapter to ChartForgeX 1.0.2, mapped `New-ImageChartPolar` angle/radius pairs to true polar geometry, and preserved explicit line-marker `None`/`Circle` semantics.
- Unified topology direction values on ChartForgeX's product-neutral `VisualLinkDirection` contract.
- Loads QR images through ImageSharp and delegates bounded fast/robust pixel decoding to CodeGlyphX.
- Aligned asynchronous cmdlets with the PSPublishModule `AsyncPSCmdlet` lifecycle for cancellation and thread-safe pipeline writes.
- Made local ChartForgeX and CodeGlyphX project references opt-in so builds use published packages by default.
- Made module signing opt-in for local and CI builds.

### 0.0.8 - 2024.06.05
- Bump dependencies
- Fix signing
- Small fixes

### 0.0.7 - 2024.06.05
- Add another method `$Image.WatermarkImage("$PSScriptRoot\Samples\LogoEvotec.png",50,100, 0.5, 0.5)`

### 0.0.6 - 2023.12.30
- Added new library: Codeuctivity.ImageSharpCompare
- Added Image Compare feature
- Bump dependencies
- Improved some code

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

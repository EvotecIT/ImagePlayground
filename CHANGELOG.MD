## Unreleased

### Added

- Added independent `MacOS`, `WindowsTerminal`, `Minimal`, and `None` console window styles without coupling chrome to prompt dialect or color theme.
- Added `New-ImageVisualStory` and `New-ImageVisualMotionCue` as thin PowerShell adapters over ChartForgeX visual grids and motion timelines. SVG and HTML keep script-free motion; PNG renders the same completed state.
- Added `ConvertTo-ImageStorySource`, `New-ImageStoryPanel`, `New-ImageStoryScene`, `New-ImageStoryOutcome`, and `New-ImageStory` for generic source/terminal/media/result presentations whose completed scene must visibly satisfy every declared outcome.
- Added native PowerShell syntax spans through `System.Management.Automation` and the optional `ImagePlayground.Syntax.TreeSitter` package for AST-backed C# and Bash highlighting without adding its native runtime payload to the normal module.
- Added PowerShell-native `New-ImageConsoleStoryCommand`, `New-ImageConsoleStoryOutput`, `New-ImageConsoleStoryTable`, `New-ImageConsoleStoryBlankLine`, and `New-ImageConsoleStoryPause` definitions for composing terminal SVG, GIF, APNG, HTML, and PNG stories without direct fluent-builder calls.
- Made the chart, visual block, QR, barcode, and topology commands scriptable without `[void]`, .NET method calls, reflection, or ChartForgeX/CodeGlyphX type names:
  - `New-ImageChart` adds `-XLabel` (aliases `-Category`, `-XLabels`) and `-SecondaryYTitle`, accepts up to 10000 px, combines bar, line, step line, area, step area, stacked area, scatter, bubble, lollipop, and range band definitions in one chart, and writes JPEG, WebP, GIF, BMP, TIFF, and TGA as well as PNG, SVG, and HTML.
  - `New-ImageChartLine` configures markers per series with `-Marker` (Circle, Square, Diamond, Triangle, Plus, Cross, Star) and `-MarkerSize`; `New-ImageChartBar`, `New-ImageChartLine`, and `New-ImageChartArea` add `-SecondaryAxis`; `New-ImageChartOptions` adds `-BarLayout Grouped|Stacked`.
  - `New-ImageTableBlock` takes pipeline objects and selects columns with `-Property` names or Format-Table style calculated properties (`Name`, `Expression`, `FormatString`, `Alignment`, `Width`), plus `-Header` and `-Align`.
  - `New-ImageTableBlock`, `New-ImageListBlock`, and `New-ImageTimelineBlock` add `-Width` and `-Height`; `New-ImageTimelineBlock` takes items from the pipeline.
  - `ConvertTo-ImageVisualArtifact` accepts `VisualGrid` objects; `Export-ImageVisualArtifact`, `New-ImageVisualGrid`, and static `New-ImageVisualStory` output write JPEG, WebP, GIF, BMP, TIFF, and TGA.
  - `New-ImageVisualGrid -PanelWidth` works without `-PanelHeight` by resizing content to the panel width with natural row heights.
  - `New-ImageQRCode` adds `-ErrorCorrection`, `-Size`, `-QuietZone`, `-ModuleShape`, `-EyeShape`, `-EyeColor`, and `-LogoScale`.
  - `New-ImageBarCode` adds `-ModuleSize`, `-QuietZone`, `-Height`, `-ShowText`, `-Text`, `-ForegroundColor`, and `-BackgroundColor`; `Get-ImageBarCode` adds `-Type` and `-TimeoutSeconds` and binds `FullName` from `Get-ChildItem`.
  - `New-ImageTopologyNode -Layer` orders columns or rows in the Layered layout.

### Breaking changes

- Migrated barcode APIs to CodeGlyphX 3.0.0 `SymbolFormat` and `DetectedSymbol`; `Get-ImageBarCode -Detailed` and `BarCode.Scan` expose the owner's `ScanResult` for partial results and completion details.
- Removed the PowerShell `-Async` parameter. Affected cmdlets now always use their cancellable asynchronous core implementation.
- Removed the unused `ImagePlayground.BarcodeResult` type; CodeGlyphX owns barcode result contracts for .NET callers.
- Removed the unused Microsoft.CSharp dependency and limited System.Text.Json and System.Memory package references to .NET Standard 2.0 and .NET Framework 4.7.2.
- Removed the Windows-only `ImagePlayground.Gdi` project and its `System.Drawing` implementation.
- Mixed chart definitions and mixed line-marker policies no longer throw; compatible series render together and markers are per series. `ChartLine.MarkerSize` no longer sets the chart-wide theme marker radius.
- `New-ImageTopologyGroup` no longer defaults to 320 x 220 at (0, 0); unbound position and size are left to the layout. Supply `-X`, `-Y`, `-Width`, and `-Height` for manual group placement.
- `New-ImageTableBlock` parameters are now `-Property` and `-InputObject`; `-Column` and `-Row` remain as aliases. Tables are built once in `EndProcessing` from all pipeline input.

### Changed

- Routed barcode format dispatch and QR logo rendering through CodeGlyphX, enabling DotCode, Han Xin, stacked GS1 DataBar, and SVG logos while preserving typed payment encoding. MaxiCode and single-value GS1 Composite generation now fail explicitly before writing output.
- Upgraded the PowerShell visual adapter to ChartForgeX 1.3.0 for generic visual stories, outcome validation, syntax spans, deterministic static fallback, and reduced-motion and print-safe output.
- Kept every `New-ImageChart*` command as a thin PowerShell adapter over ChartForgeX while keeping the C# packages as separate capability owners.
- Upgraded the PowerShell chart adapter to ChartForgeX 1.0.2, mapped `New-ImageChartPolar` angle/radius pairs to true polar geometry, and preserved explicit line-marker `None`/`Circle` semantics.
- Unified topology direction values on ChartForgeX's product-neutral `VisualLinkDirection` contract.
- Loads QR images through ImageSharp and delegates bounded fast/robust pixel decoding to CodeGlyphX.
- Aligned asynchronous cmdlets with the PSPublishModule `AsyncPSCmdlet` lifecycle for cancellation and thread-safe pipeline writes.
- Made local ChartForgeX and CodeGlyphX project references opt-in so builds use published packages by default.
- Made module signing opt-in for local and CI builds.
- QR and barcode PNG output is compressed (CodeGlyphX writes stored PNGs by default): a URL QR code drops from about 69 KB to under 1 KB.
- Index-based chart series get whole-number category labels by default instead of fractional axis ticks.

### Fixed

- `New-ImageTableBlock -Row` accepts `PSCustomObject` rows instead of failing with "Row value count must match table column count".
- `-Layout DenseGrouped` places every group instead of stacking unpositioned groups at the origin.

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

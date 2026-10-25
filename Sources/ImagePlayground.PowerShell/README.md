# ImagePlayground.PowerShell

This project builds the binary ImagePlayground PowerShell module. It provides one command surface over the `ImagePlayground`, `OfficeIMO.Core`, `ChartForgeX`, `ChartForgeX.Visuals`, `ChartForgeX.Stories`, and `CodeGlyphX` .NET packages.

Use it for image conversion, resizing, composition, text, watermarks, metadata, thumbnails, icons, mosaics, grids, avatars, GIFs, charts, topology diagrams, QR codes, and barcodes. The cmdlets stay thin: OfficeIMO.Core owns raster processing and metadata; ChartForgeX owns charts and topology; ChartForgeX.Visuals owns composition; ChartForgeX.Stories owns animation and stories; CodeGlyphX owns code generation and decoding.

```powershell
Install-Module -Name ImagePlayground -Scope CurrentUser
Import-Module ImagePlayground

Resize-Image -FilePath '.\photo.jpg' -OutputPath '.\photo-small.jpg' -Width 800
Add-ImageWatermark -FilePath '.\photo.jpg' -OutputPath '.\marked.jpg' -WatermarkPath '.\logo.png'
```

Cmdlets expose one execution mode. Where the core has a cancellable asynchronous file API, the cmdlet uses it internally; callers do not select an implementation with `-Async`.

## ChartForgeX 3 migration

Native charts, grids, canvases, topology and stories share `ChartForgeX.Themes.VisualTheme`. A theme contains independent light and dark colors; choose its appearance with `VisualThemeMode` and create custom snapshots with `theme.Configure(...)`. Chart and factual-block `-Theme Light`/`Dark` shortcuts remain available.

`New-ImageConsoleStory -Palette`, `New-ImageConsoleStoryTab -Palette`, `New-ImageStory -Theme` and replay-panel `-ReplayTheme` accept this shared theme. Use `-ThemeMode Light` or `-ReplayThemeMode Light` to select a light presentation. An already-authored native object keeps its theme unless the command explicitly receives an override.

Terminal cursor and window controls belong to `TerminalStyleOptions`, supplied through `-Style`; replace `New-ImageConsoleStoryPalette -Cursor` with that style's `Cursor` property. Generic-story layout controls similarly belong to `VisualStoryStyleOptions`.

`New-ImageChartRadial` continues to render 0–100 progress rings. Native callers use `Chart.AddProgressRing` for that behavior; `AddRadialBar` now renders signed numeric radial bars. The shared [ChartForgeX migration guide](https://github.com/EvotecIT/ChartForgeX/blob/main/docs/v2/migration.md) covers renamed configuration callbacks, typed observations and stable series handles.

## CodeGlyphX 3 migration

The module uses CodeGlyphX 3.1.0. Barcode generation and recognition share its `SymbolFormat` catalogue. PowerShell enum names remain case-insensitive; C# callers replace `BarcodeType` with `SymbolFormat`, including capitalization changes such as `EAN` → `Ean`, `PDF417` → `Pdf417`, and `UPCA` → `UpcA`. These format names also change beyond capitalization:

- `GS1_128` → `Gs1Code128`
- `GS1DataBarOmni` → `Gs1DataBarOmnidirectional`
- `GS1DataBarStackedOmni` → `Gs1DataBarStackedOmnidirectional`
- `UspsImb` → `UspsIntelligentMail`

`Get-ImageBarCode` and `ImagePlayground.BarCode.Read` return `DetectedSymbol`. Replace the former `Kind` and format-specific result inspection with `Format` and `Text`. `HasRawBytes` reports whether exact decoded payload bytes are available through the read-only `RawBytes` property. Ordinary reads return the first barcode or no object after a completed scan finds none. Cancellation raises an error, and a deadline raises `TimeoutException` in the C# helper. `Get-ImageQRCode` retains its `QrDecoded` result and existing QR decode options.

Use `-Detailed` or `ImagePlayground.BarCode.Scan` when callers need multiple symbols, partial results, or completion details:

```powershell
$scan = Get-ImageBarCode -FilePath '.\codes.png' -Type Ean, DataMatrix -TimeoutSeconds 5 -Detailed
$scan.CompletionReason
$scan.Symbols | Select-Object Format, Text
```

`-Type` limits recognition to the formats you expect, which is faster and avoids false positives; `-TimeoutSeconds` sets the deadline. Without them the adapter scans the default formats with a five-second deadline. An explicit `-ScanOptions` object retains CodeGlyphX defaults, including its 500 ms deadline, unless changed by the caller or overridden by `-Type` and `-TimeoutSeconds`. The scan deadline starts after input path resolution and any URL download. The C# helpers skip path resolution when their argument or options token is already cancelled: `BarCode.Read` throws, while `BarCode.Scan` returns structured cancellation. Default barcode scans exclude QR formats, Pharmacode, and Patch Code; explicit formats can opt into the latter two. QR formats use `Get-ImageQRCode` or the CodeGlyphX scanner directly. Detailed results retain structured cancellation and deadline status, including any partial symbols. The adapter does not modify supplied options.

DotCode, Han Xin, and stacked GS1 DataBar generation use the owner's matrix renderer. MaxiCode needs a renderer for its hexagonal geometry and is rejected. GS1 Composite needs separate linear and composite payloads and is rejected by the single-value barcode command; use CodeGlyphX's composite encoder for that workflow.

Typed QR payment payloads retain the owner's recommended encoding. Slovenian UPN QR uses version 15, medium error correction, and ISO-8859-2. Explicit error correction selections on other QR helpers remain effective. `-LogoPath` embeds a validated logo in raster or SVG output through CodeGlyphX. The logo fits within one eighth of the image width, retaining its aspect ratio, unless `-LogoScale` sets it as a fraction of the symbol width. OfficeIMO.Core normalizes the logo before the owner embeds it without a background plate. Invalid logos leave an existing destination intact.

`New-ImageQRCode` exposes a small styling subset of CodeGlyphX: `-ErrorCorrection`, `-Size` (target pixels including the quiet zone), `-QuietZone`, `-ModuleShape`, `-EyeShape`, and `-EyeColor`. `New-ImageBarCode` exposes `-ModuleSize`, `-QuietZone`, `-Height`, `-ShowText`/`-Text`, and colours. QR and barcode PNG files are written with CodeGlyphX's maximum PNG compression.

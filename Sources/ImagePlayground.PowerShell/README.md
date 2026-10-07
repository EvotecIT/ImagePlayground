# ImagePlayground.PowerShell

This project builds the binary ImagePlayground PowerShell module. It provides one command surface over the `ImagePlayground`, `ChartForgeX`, and `CodeGlyphX` .NET packages.

Use it for image conversion, resizing, composition, text, watermarks, metadata, thumbnails, icons, mosaics, grids, avatars, GIFs, charts, topology diagrams, QR codes, and barcodes. The cmdlets stay thin: ChartForgeX owns chart rendering, CodeGlyphX owns code generation and decoding, and ImagePlayground owns image manipulation.

```powershell
Install-Module -Name ImagePlayground -Scope CurrentUser
Import-Module ImagePlayground

Resize-Image -FilePath '.\photo.jpg' -OutputPath '.\photo-small.jpg' -Width 800
Add-ImageWatermark -FilePath '.\photo.jpg' -OutputPath '.\marked.jpg' -WatermarkPath '.\logo.png'
```

Cmdlets expose one execution mode. Where the core has a cancellable asynchronous file API, the cmdlet uses it internally; callers do not select an implementation with `-Async`.

## CodeGlyphX 3 migration

The module uses CodeGlyphX 3.0.0. Barcode generation and recognition share its `SymbolFormat` catalogue. PowerShell enum names remain case-insensitive; C# callers replace `BarcodeType` with `SymbolFormat`, including capitalization changes such as `EAN` → `Ean`, `PDF417` → `Pdf417`, and `UPCA` → `UpcA`. These format names also change beyond capitalization:

- `GS1_128` → `Gs1Code128`
- `GS1DataBarOmni` → `Gs1DataBarOmnidirectional`
- `GS1DataBarStackedOmni` → `Gs1DataBarStackedOmnidirectional`
- `UspsImb` → `UspsIntelligentMail`

`Get-ImageBarCode` and `ImagePlayground.BarCode.Read` return `DetectedSymbol`. Replace the former `Kind` and format-specific result inspection with `Format` and `Text`. `HasRawBytes` reports whether exact decoded payload bytes are available through the read-only `RawBytes` property. Ordinary reads return the first barcode or no object after a completed scan finds none. Cancellation raises an error, and a deadline raises `TimeoutException` in the C# helper. `Get-ImageQRCode` retains its `QrDecoded` result and existing QR decode options.

Use `-Detailed` or `ImagePlayground.BarCode.Scan` when callers need multiple symbols, partial results, or completion details:

```powershell
$options = [CodeGlyphX.ScanOptions]::new()
$options.Formats = [CodeGlyphX.SymbolFormat[]]@('Ean', 'DataMatrix')
$options.TimeoutMilliseconds = 5000
$scan = Get-ImageBarCode -FilePath '.\codes.png' -ScanOptions $options -Detailed
$scan.CompletionReason
$scan.Symbols | Select-Object Format, Text
```

Omitting `-ScanOptions` gives the adapter a five-second scan deadline. An explicit `ScanOptions` object retains CodeGlyphX defaults, including its 500 ms deadline, unless changed by the caller. The scan deadline starts after input path resolution and any URL download. The C# helpers skip path resolution when their argument or options token is already cancelled: `BarCode.Read` throws, while `BarCode.Scan` returns structured cancellation. Default barcode scans exclude QR formats, Pharmacode, and Patch Code; explicit formats can opt into the latter two. QR formats use `Get-ImageQRCode` or the CodeGlyphX scanner directly. Detailed results retain structured cancellation and deadline status, including any partial symbols. The adapter does not modify supplied options.

DotCode, Han Xin, and stacked GS1 DataBar generation use the owner's matrix renderer. MaxiCode needs a renderer for its hexagonal geometry and is rejected. GS1 Composite needs separate linear and composite payloads and is rejected by the single-value barcode command; use CodeGlyphX's composite encoder for that workflow.

Typed QR payment payloads retain the owner's recommended encoding. Slovenian UPN QR uses version 15, medium error correction, and ISO-8859-2. Explicit error correction selections on other QR helpers remain effective. `-LogoPath` embeds a validated logo in raster or SVG output through CodeGlyphX. The logo fits within one eighth of the image width, retaining its aspect ratio. ImageSharp normalizes the logo before the owner embeds it without a background plate. Invalid logos leave an existing destination intact.

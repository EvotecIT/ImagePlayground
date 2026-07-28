# ImagePlayground

ImagePlayground provides image manipulation APIs for .NET and matching PowerShell commands. The primary engine uses [SixLabors.ImageSharp](https://github.com/SixLabors/ImageSharp), so the main package works without `System.Drawing`.

## Choose the package that owns the job

| Need | Package |
| --- | --- |
| Resize, crop, convert, compare, compose, watermark, draw text, edit metadata, create thumbnails, icons, mosaics, grids, avatars, or GIFs from .NET | [`ImagePlayground`](https://www.nuget.org/packages/ImagePlayground) |
| Image manipulation, charts, topology diagrams, QR codes, and barcodes from PowerShell | [`ImagePlayground`](https://www.powershellgallery.com/packages/ImagePlayground) PowerShell module |
| Create charts or topology diagrams | [`ChartForgeX`](https://www.nuget.org/packages/ChartForgeX) |
| Create or decode QR codes and barcodes | [`CodeGlyphX`](https://www.nuget.org/packages/CodeGlyphX) |

The `ImagePlayground` .NET package does not wrap ChartForgeX or CodeGlyphX. C# callers reference the package that owns the capability. The ImagePlayground PowerShell module intentionally aggregates all three packages behind one command surface.

## Install

For .NET:

```shell
dotnet add package ImagePlayground
```

For PowerShell 5.1 or PowerShell 7+:

```powershell
Install-Module -Name ImagePlayground -Scope CurrentUser
Import-Module ImagePlayground
```

## .NET examples

Resize an image while preserving its aspect ratio:

```csharp
using ImagePlayground;

ImageHelper.Resize(
    filePath: "photo.jpg",
    outFilePath: "photo-small.jpg",
    width: 800,
    height: 800,
    keepAspectRatio: true);
```

Use the object API for several edits before one save:

```csharp
using ImagePlayground;
using SixLabors.ImageSharp;

using var image = Image.Load("photo.jpg");
image.Resize(1200, 1200, keepAspectRatio: true);
image.Watermark(
    "Internal",
    WatermarkPlacement.BottomRight,
    Color.White,
    fontSize: 28);
image.Save("photo-watermarked.jpg");
```

The core package targets .NET Standard 2.0, .NET Framework 4.7.2, .NET 8, and .NET 10.

## PowerShell examples

```powershell
Resize-Image -FilePath '.\photo.jpg' -OutputPath '.\photo-small.jpg' -Width 800

Add-ImageWatermark `
    -FilePath '.\photo.jpg' `
    -OutputPath '.\photo-watermarked.jpg' `
    -WatermarkPath '.\logo.png' `
    -Placement BottomRight `
    -Opacity 0.7

Export-ImageMetadata -FilePath '.\photo.jpg' -OutputPath '.\metadata.json'
```

Create and decode QR codes and barcodes through the same PowerShell module:

```powershell
New-ImageQRCode -Content 'https://evotec.xyz' -FilePath '.\qr-code.png'
$qrCode = Get-ImageQRCode -FilePath '.\qr-code.png'
$qrCode.Text

New-ImageBarCode -Type EAN -Value '5901234123457' -FilePath '.\barcode.png'
$barCode = Get-ImageBarCode -FilePath '.\barcode.png'
$barCode.Text
```

Render a ChartForgeX-backed chart without leaving the ImagePlayground PowerShell surface:

```powershell
New-ImageChart {
    New-ImageChartBar -Name 'C#' -Value 5 -Color CornflowerBlue
    New-ImageChartBar -Name 'PowerShell' -Value 12 -Color MediumSeaGreen
} -FilePath '.\languages.png' -Width 720 -Height 420 -ShowGrid
```

The `New-ImageChartBar`, `New-ImageChartLine`, `New-ImageChartDonut`, and other chart-definition commands remain the normal PowerShell experience. Advanced scripts can also pass a native `ChartForgeX.Core.Chart` through `-Chart` or configure one through `-ChartScript`.

Build an animated visual story from native ChartForgeX blocks while keeping ImagePlayground as the thin PowerShell surface:

```powershell
New-ImageVisualStory -StoryScript {
    param($Story)

    $projects = [ChartForgeX.VisualBlocks.MetricCard]::Create()
    [void] $projects.WithMetric('Maintained projects', '24').WithCaption('reusable libraries')
    [void] $Story.WithTitle('Engineering portfolio').WithColumns(1)
    [void] $Story.Add('projects', $projects)
} -MotionDefinition {
    New-ImageVisualMotionCue -TargetId title -Effect Reveal -DurationSeconds 0.65
    New-ImageVisualMotionCue -TargetId projects -Effect Rise -DelaySeconds 0.25
} -FilePath '.\portfolio.svg'
```

SVG and HTML preserve the script-free story. PNG renders the exact completed state, while reduced-motion and print users see all content immediately.

PowerShell commands have one execution path. Commands that can use asynchronous file APIs do so internally and honor pipeline cancellation; there is no `-Async` switch because a PowerShell command invocation still completes before returning control to the caller.

See [the generated command reference](Docs/Readme.md) and the focused scripts under [`Examples`](Examples).

## Breaking changes

ImagePlayground 3.0 removes the PowerShell `-Async` parameter and the Windows-only `ImagePlayground.Gdi` project. Invoke asynchronous-capable commands normally.

The PowerShell module keeps its chart-definition, topology, QR-code, and barcode commands. Those commands are thin adapters: ChartForgeX owns chart and topology rendering, CodeGlyphX owns code generation and decoding, and ImagePlayground keeps the PowerShell user experience. C# callers should use the owning packages directly.

## Build and test

```powershell
dotnet build .\Sources\ImagePlayground.sln --configuration Release
dotnet test .\Sources\ImagePlayground.Tests\ImagePlayground.Tests.csproj --configuration Release
pwsh -File .\ImagePlayground.Tests.ps1
```

The PowerShell module layout and command documentation are generated by PSPublishModule/PowerForge. Update C# XML documentation and build configuration rather than editing generated command pages by hand.

## License

ImagePlayground is available under the [MIT License](LICENSE).

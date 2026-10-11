# Managed image API migration

ImagePlayground uses OfficeIMO.Core for raster processing, metadata, and comparison, ChartForgeX.Visuals for composition, and ChartForgeX.Stories for GIF/APNG encoding. The editable image wrapper remains `ImagePlayground.Image`; its public APIs use owned types. Updating callers requires source changes when they used the previous image, font, EXIF, or comparison types.

## ChartForgeX 2.0 assembly ownership

The ChartForgeX packages use version 2.0.0 together. `ChartForgeX` owns chart, topology, geometry, raster primitives, and the neutral visual artifact envelope; `ChartForgeX.Visuals` owns composition, canvases, visual blocks, and watermark decoration; `ChartForgeX.Stories` owns stories, terminal playback, motion, and GIF/APNG encoding. Their namespaces remain unchanged. For example, `ChartForgeX.Raster.RasterAnimationEncoder` is in `ChartForgeX.Stories.dll`, while `ChartForgeX.Composition.ImageComposition` is in `ChartForgeX.Visuals.dll`.

The ImagePlayground NuGet package brings these dependencies automatically. Callers that directly reference an owning engine should use its corresponding package. Update assembly-qualified names or explicit assembly loads that previously resolved story or composition types from `ChartForgeX.dll`. The PowerShell module loads all three assemblies and retains its selected type accelerators. The optional Tree-sitter adapter references `ChartForgeX.Stories` for its story-source contracts.

Static `New-ImageVisualGrid` output remains a `ChartForgeX.VisualBlocks.VisualGrid`. With `-Motion`, it returns a detached `ChartForgeX.Motion.VisualMotionPresentation`; the presentation captures the grid and timeline without changing either caller object. Pipe that result to `New-ImageVisualStory`, or pass it through the command's typed `-Presentation` parameter. `New-ImageVisualStory -PassThru` returns the presentation when motion is supplied and the grid for static output. SVG and HTML retain the timeline; PNG contains the completed static picture.

```powershell
$motion = [ChartForgeX.Motion.VisualMotionTimeline]::Create().Rise('requests')
$presentation = New-ImageVisualGrid -ContentDefinition {
    New-ImageVisualGridItem -TargetId requests -Block (New-ImageMetricCard -Label Requests -Value 12840)
} -Motion $motion
$presentation | New-ImageVisualStory -FilePath '.\requests.svg'
```

## Replace public value types

| Previous type | Current type |
| --- | --- |
| `SixLabors.ImageSharp.Color` | `OfficeIMO.Drawing.OfficeColor` |
| `SixLabors.ImageSharp.PointF` | `OfficeIMO.Drawing.OfficePoint` |
| `SixLabors.ImageSharp.Rectangle` | `ImagePlayground.Rectangle` |
| `SixLabors.Fonts.HorizontalAlignment` | `OfficeIMO.Drawing.OfficeTextAlignment` |
| `SixLabors.Fonts.VerticalAlignment` | `OfficeIMO.Drawing.OfficeTextVerticalAlignment` |
| `SixLabors.ImageSharp.Processing.FlipMode` | `ImagePlayground.FlipMode` |
| `SixLabors.ImageSharp.Processing.RotateMode` | `ImagePlayground.RotateMode` |
| `SixLabors.ImageSharp.Processing.GrayscaleMode` | `OfficeIMO.Drawing.OfficeRasterGrayscaleMode` |
| `SixLabors.ImageSharp.ColorMatrix` | `OfficeIMO.Drawing.OfficeRasterColorMatrix` |
| EXIF `ExifTag` / `IExifValue` | `OfficeIMO.Drawing.OfficeExifTag` / `OfficeExifValue` |
| EXIF unsigned / signed fractions | `OfficeIMO.Drawing.OfficeRational` / `OfficeSignedRational` |
| Image metadata and frame collections | `OfficeIMO.Drawing.OfficeImageMetadata` / `OfficeRasterFrames` |
| `ImagePlayground.HeifImageInfo` and HEIF item models | `OfficeIMO.Drawing.OfficeHeifImageInfo` and the matching `OfficeHeif*` item models |

`OfficeColor` exposes byte `R`, `G`, `B`, and `A` channels, named colors such as `OfficeColor.Red`, and `Parse` / `ParseHex`. Read a pixel with `image.Raster.GetPixel(x, y)` and set it with `image.Raster.SetPixel(x, y, color)`. `GetPixels()` returns an independent RGBA byte array in row order.

`Image.GetImage(filePath)` returns the ImagePlayground wrapper. Code that previously retained a raw engine image should use `Image.Load(filePath)` and its `Raster`, `Frames`, and `Metadata` properties. `Image.FromRaster(raster)` retains the supplied mutable raster; use `Clone()` to keep an independent image. Dispose image wrappers after use.

```csharp
using ImagePlayground;
using OfficeIMO.Drawing;

using var image = Image.Load("photo.png");
image.Raster.SetPixel(0, 0, OfficeColor.Red);
image.AddText(20, 20, "Caption", OfficeColor.White, 24);
image.Save("captioned.png");
```

`Load` accepts owned `OfficeRasterDecodeOptions` for encoded-byte, pixel, memory, frame-count, and cancellation limits. Stream input reads from its current position and remains open. `SourceFormat` describes the detected container; `DefaultOutputFormat` describes the supported format used by parameterless stream export. An unsupported export container defaults to PNG. A filename extension does not change source-format detection.

Loading retains stored EXIF orientation by default so `AutoOrient` can apply it during editing. Passing `ApplyExifOrientation = true` normalizes pixels during decoding and resets the editable orientation tag to normal.

Use an explicit format for stream conversion. `Encode` prepares complete bytes and reports metadata omissions before any output is written:

```csharp
using var image = Image.Load("photo.jpg");
image.Resize(new OfficeRasterResizeOptions {
    Width = 1200,
    Height = 800,
    Fit = OfficeImageFit.Cover
});
var result = image.Encode(ImageType.Png);
byte[] bytes = result.RequireMetadataPreservation();
using var stream = image.ToStream(ImageType.Png);
```

`RequireMetadataPreservation()` throws when a supplied profile family cannot be retained. It does not promise lossless pixel compression. `EncodedBytes` belongs to the result; `Metadata` is an independent requested projection rather than a reread of density values rounded by the destination codec. Metadata describes the primary image. `ToRgbaImage()` and `FromRgbaImage()` copy pixels for ChartForgeX composition without sharing mutable buffers.

File saves replace the destination atomically after encoding. Stream saves keep the stream open; seekable streams are replaced, truncated, and rewound, while nonseekable streams receive bytes at their current position. A write error or cancellation during writing can leave partial stream output.

## Update PowerShell scripts

`Get-Image` returns an editable `ImagePlayground.Image`, and `Save-Image` accepts that wrapper. Use the owned color and EXIF types in typed parameters and method calls.

```powershell
$image = Get-Image -FilePath '.\photo.jpg'
try {
    $image.AddText(20, 20, 'Caption', [OfficeIMO.Drawing.OfficeColor]::White, 24)
    $image.SetExifValue([OfficeIMO.Drawing.OfficeExifTag]::Software, 'ImagePlayground')
    Save-Image -Image $image -FilePath '.\captioned.jpg' -Quality 90
} finally {
    $image.Dispose()
}
```

Object input to `Resize-Image` updates and emits the same image for further processing. File input requires `-OutputPath` and saves without emitting an editable object. The shared engine validates the requested size and memory budget; the former 1000-pixel command limit is removed.

```powershell
$image = Get-Image -FilePath '.\photo.jpg'
try {
    $image | Resize-Image -Width 1200 | Save-Image -FilePath '.\small.png'
    $stream = $image | Save-Image -AsStream -Format Png
    try {
        # Read the encoded PNG from position zero.
        $stream.Length
    } finally {
        $stream.Dispose()
    }
} finally {
    $image.Dispose()
}
```

`Save-Image -EncodingOptions` accepts `OfficeRasterEncodingOptions`. `-Quality` and `-CompressionLevel` override the corresponding settings. `-Format` applies to stream output; file output uses the destination extension. Omit the stream format to use `DefaultOutputFormat`.

## Edit EXIF and profile bytes

EXIF entries expose `Tag`, `DataType`, and `Value`. Replace `entry.GetValue()` with `entry.Value`. Call `image.SetExifValue(tag, value)` or `image.Metadata.SetExifValue(tag, value)`; the shared metadata API validates each supported tag's value. ASCII date fields use strings such as `2026:10:08 12:30:00`. Unsigned integral fields use the supported CLR integer type, and rational fields use `OfficeRational` values.

`ExifProfile`, `XmpProfile`, `IccProfile`, and `IptcProfile` are byte arrays. Profile getters and setters copy the bytes; modifying a returned array does not edit the metadata object. For EXIF, use the tag methods or assign a complete classic TIFF profile. For XMP, assign a UTF-8 packet:

```csharp
image.Metadata.XmpProfile = System.Text.Encoding.UTF8.GetBytes(
    "<x:xmpmeta xmlns:x=\"adobe:ns:meta/\" />");
image.SetExifValue(OfficeExifTag.Software, "ImagePlayground");
```

HEIF metadata APIs return OfficeIMO.Core's `OfficeHeifImageInfo` and use its shared container reader and writer. Their container-specific write constraints remain. See [HEIF metadata support](HEIC-Support-Investigation.md).

Raster saving preserves the profile families supported by the selected container: JPEG and TIFF retain EXIF, XMP, ICC, and IPTC; PNG and WebP retain EXIF, XMP, and ICC; GIF retains XMP and ICC; BMP retains ICC. PBM, TGA, and ICO do not carry these profile families. Call `image.GetEncodingMetadataOmissions(ImageType.Png)` before saving to inspect which present families the destination omits. The report and save leave the source metadata object unchanged. For example, converting a JPEG with IPTC to PNG omits IPTC while retaining its supported EXIF, XMP, and ICC profiles.

Lossless metadata import and editing reject profiles that their original container cannot represent. TIFF maker notes with offsets relative to the original file require explicit removal or replacement before raster re-encoding. Physical image density is preserved across supported unit conversions; `PhysicalDpiX` and `PhysicalDpiY` provide inch-based values, while `ResolutionUnits` describes the container's native representation.

The wrapper exposes primary/global TIFF metadata and applies its native resolution to every encoded page. Assign `Metadata.Resolution` to set the horizontal value, vertical value, and native unit together. The scalar resolution properties and generic EXIF density edits update the same authority. In encoder options, nullable `Resolution` overrides metadata only when supplied; it replaces the shared `DpiX` and `DpiY` settings.

## Read comparison results

Comparison returns `OfficeRasterComparisonResult`. `ChangedPixels` replaces `PixelErrorCount`; the metrics have their own defined meanings:

- `ChangedPixels` counts pixels whose premultiplied RGB or alpha differs.
- `MeanAbsoluteDifference` is the normalized mean difference across premultiplied RGBA channels.
- `Similarity` equals one minus that difference and ranges from zero to one.
- `MaximumChannelDifference` is the largest channel difference in byte units.
- `DifferenceImage` is an opaque visualization of RGB and alpha differences.

Compared images must have equal dimensions. Hidden RGB values in fully transparent pixels do not count as visible differences. Thresholds written for the former comparison library must be recalibrated against the new metrics.

```powershell
$result = Compare-Image -FilePath '.\expected.png' -FilePathToCompare '.\actual.png'
$result.ChangedPixels
$result.Similarity
Compare-Image -FilePath '.\expected.png' -FilePathToCompare '.\actual.png' -OutputPath '.\difference.png'
```

## Encoding and processing behavior

Resizing preserves aspect ratio by default. A single width or height determines the other dimension; supplying both dimensions fits each frame inside that bounding box. Set `keepAspectRatio: false` in .NET or use `Resize-Image -DontRespectAspectRatio` in PowerShell to stretch to the supplied dimensions. Percentage resizing retains integer truncation of each resulting dimension, with a minimum of one pixel.

The .NET options overload accepts the shared `OfficeRasterResizeOptions`: `Contain` fits without padding, `Cover` fills both bounds and crops the center, and `Stretch` uses the requested dimensions. Sampling and color space are explicit. The shared frame operation checks retained pixels and peak working memory before replacing the frame sequence. `OfficeRasterResampler.PlanResize` exposes the resulting geometry and memory estimate before allocation.

Use `OfficeRasterTextOptions` with `GetTextSize` and the boxed `AddText` overload to share font selection, shaping, wrapping, alignment, outline, and shadow settings between measurement and rendering. Caller-provided font collections avoid relying on installed system fonts. Cancellation tokens reach loading, resizing, drawing, text, and encoding operations.

Text and filters reserve their largest per-frame temporary storage alongside all retained source and result frames before editing begins. A rejected memory plan leaves the original sequence intact. The shared text and filter estimators expose the same policy to applications that compose Core operations directly.

`Avatar` edits each frame. `SaveAsAvatar` and `SaveAsCircularAvatar` create independent output and preserve the source pixels and metadata, including when encoding or writing fails. Their stream overloads write PNG, retain transparent corners, and keep the caller's stream open. PNG output preserves multiple frames as APNG with their timing and play count.

`AutoOrient` applies an existing EXIF orientation and resets it to normal. Images without that tag retain their pixels and metadata. Tiled watermarks use each frame's own dimensions, clip the last tiles at its edges, and retain timing and play count. `WatermarkImageTiled` accepts an optional cancellation token; cancellation leaves the original frame sequence intact.

Barcode generation takes `CodeGlyphX.SymbolFormat` in place of `BarcodeType`. Use names such as `Ean`, `UpcA`, `UpcE`, and `Pdf417`. `Get-ImageBarCode` returns a `DetectedSymbol` with `Text` and `Format`; `-Detailed` returns the complete `ScanResult`. Pass `-ScanOptions` to select formats, image limits, and a recognition deadline. The default barcode scan allows five seconds and excludes QR, Pharmacode, and Patch Code; explicit formats can select Pharmacode or Patch Code.

JPEG quality is clamped to 1 through 100. PNG compression levels are clamped to 0 through 9 and map to the shared encoder's stored (0), fastest (1 through 3), and optimal (4 through 9) profiles. WebP output is lossless by default; an explicit quality setting selects the managed lossy encoder, with quality clamped to 1 through 100.

Images expose frame timing and play count through `OfficeRasterFrames` and `OfficeRasterFrame`. GIF/APNG output uses the shared ChartForgeX animation encoder. Formats that cannot retain several frames reject such output so callers must select the intended frame explicitly.

Explicit zero-duration frames retain zero duration in GIF and APNG. GIF rounds positive delays to 10-millisecond units with a 10-millisecond minimum; APNG uses rational delays. Negative delays are rejected. Maximum supported delays are 655.35 seconds for GIF and 65,535 seconds for APNG. TIFF pages with no authored duration therefore export with zero delay unless the caller supplies timing. GIF pixels with alpha below 128 become transparent. Use APNG when partial alpha must be preserved.

Saving `.ico` retains the image's resolution entries, each up to 256 pixels on each axis. `SaveAsIcon(path, sizes)` derives a requested set of square entries from the first frame. ICO thumbnails resize the entries through the managed raster engine. Thumbnail generation skips undecodable files, preserves decodable originals whose extension has no encoder, and propagates resizing or encoding failures for supported output formats.

Managed decoding enforces encoded-size, pixel, frame, and working-memory limits. Supported codecs accept their documented format subsets, and malformed or unsupported data raises an image-load error. Pixel processing does not apply an embedded ICC color profile; workflows that require a color-managed display or print match need an explicit color-management stage. Text uses the shared font resolver, so metrics and glyph rendering can differ from the former font engine and across installed font sets.

Memory limits apply to each operation and retained frame sequence. Animation export checks the retained Core pixels and copied ChartForgeX buffers against a 256 MiB budget before making the copies; this budget does not set a limit on total process memory.

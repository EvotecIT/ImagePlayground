# Managed image API migration

ImagePlayground uses OfficeIMO.Core for raster processing, metadata, and comparison, and ChartForgeX for GIF/APNG encoding. The editable image wrapper remains `ImagePlayground.Image`; its public APIs use owned types. Updating callers requires source changes when they used the previous image, font, EXIF, or comparison types.

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

The wrapper exposes primary/global TIFF metadata and applies its native resolution to every encoded page. `Metadata.Resolution` returns an immutable snapshot containing the horizontal value, vertical value, and native unit.

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

Barcode generation takes `CodeGlyphX.SymbolFormat` in place of `BarcodeType`. Use names such as `Ean`, `UpcA`, `UpcE`, and `Pdf417`. `Get-ImageBarCode` returns a `DetectedSymbol` with `Text` and `Format`; `-Detailed` returns the complete `ScanResult`. Pass `-ScanOptions` to select formats, image limits, and a recognition deadline. The default barcode scan allows five seconds and excludes QR, Pharmacode, and Patch Code; explicit formats can select Pharmacode or Patch Code.

JPEG quality is clamped to 1 through 100. PNG compression levels are clamped to 0 through 9 and map to the shared encoder's stored (0), fastest (1 through 3), and optimal (4 through 9) profiles. WebP output is lossless by default; an explicit quality setting selects the managed lossy encoder, with quality clamped to 1 through 100.

Images expose frame timing and play count through `OfficeRasterFrames` and `OfficeRasterFrame`. GIF/APNG output uses the shared ChartForgeX animation encoder. Formats that cannot retain several frames reject such output so callers must select the intended frame explicitly.

Frames without a positive duration, including TIFF pages exported as animation, use 100 milliseconds. GIF stores timing in 10-millisecond units with a 10-millisecond minimum, and pixels with alpha below 128 become transparent. Use APNG when partial alpha must be preserved.

Saving `.ico` retains the image's resolution entries, each up to 256 pixels on each axis. `SaveAsIcon(path, sizes)` derives a requested set of square entries from the first frame. ICO thumbnails resize the entries through the managed raster engine. Thumbnail generation skips undecodable files, preserves decodable originals whose extension has no encoder, and propagates resizing or encoding failures for supported output formats.

Managed decoding enforces encoded-size, pixel, frame, and working-memory limits. Supported codecs accept their documented format subsets, and malformed or unsupported data raises an image-load error. Pixel processing does not apply an embedded ICC color profile; workflows that require a color-managed display or print match need an explicit color-management stage. Text uses the shared font resolver, so metrics and glyph rendering can differ from the former font engine and across installed font sets.

Memory limits apply to each operation and retained frame sequence. Animation export checks the retained Core pixels and copied ChartForgeX buffers against a 256 MiB budget before making the copies; this budget does not set a limit on total process memory.

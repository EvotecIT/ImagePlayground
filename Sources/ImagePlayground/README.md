# ImagePlayground

`ImagePlayground` is the cross-platform .NET image-processing package in this repository. It uses OfficeIMO.Core for managed raster processing, ChartForgeX.Visuals for composition, and ChartForgeX.Stories for GIF/APNG encoding, and targets .NET Standard 2.0, .NET Framework 4.7.2, .NET 8, and .NET 10.

```shell
dotnet add package ImagePlayground
```

Use it for image conversion, resizing, cropping, comparison, composition, drawing, text, watermarks, metadata, HEIF metadata, thumbnails, icons, grids, mosaics, avatars, and GIFs.

```csharp
using ImagePlayground;
using OfficeIMO.Drawing;

ImageHelper.Resize("photo.jpg", "photo-small.jpg", width: 800, height: null);

using var image = Image.Load("photo.jpg");
image.Resize(new OfficeRasterResizeOptions { Width = 1200 });
image.AddText(20, 20, "Photo", OfficeColor.White, 24);
image.Save("photo-resized.jpg");

// Choose the output container explicitly when writing to a stream.
using var png = image.ToStream(ImageType.Png);

// Inspect or require preservation of supplied metadata before writing.
var encoded = image.Encode(ImageType.Png);
byte[] pngBytes = encoded.RequireMetadataPreservation();
```

Related capabilities have separate owners:

- Use [ChartForgeX](https://www.nuget.org/packages/ChartForgeX) for charts and topology diagrams.
- Use [CodeGlyphX](https://www.nuget.org/packages/CodeGlyphX) for QR codes and barcodes.

Image wrappers expose `Raster` as `OfficeRasterImage`, `Frames` as `OfficeRasterFrames`, and `Metadata` as `OfficeImageMetadata`. Public color, geometry, EXIF, comparison, and processing types come from the owned libraries. Read the [migration guide](../../Docs/Managed-Image-Migration.md) when updating callers that used the previous image types.

`SourceFormat` reports the detected input container; `DefaultOutputFormat` controls default stream output. Decoder, resize, text, and encoder options come from OfficeIMO.Core, so the same settings work in applications that use that engine directly. `Encode` returns owned bytes, an independent metadata projection, and omitted-profile evidence. Pixel buffers and metadata remain editable without recompressing an image after every operation.

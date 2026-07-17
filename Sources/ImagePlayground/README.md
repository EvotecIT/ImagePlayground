# ImagePlayground

`ImagePlayground` is the cross-platform .NET image-processing package in this repository. It uses ImageSharp and targets .NET Standard 2.0, .NET Framework 4.7.2, .NET 8, and .NET 10.

```shell
dotnet add package ImagePlayground
```

Use it for image conversion, resizing, cropping, comparison, composition, drawing, text, watermarks, metadata, HEIF metadata, thumbnails, icons, grids, mosaics, avatars, and GIFs.

```csharp
using ImagePlayground;
using SixLabors.ImageSharp;

ImageHelper.Resize("photo.jpg", "photo-small.jpg", width: 800, height: null);

using var image = Image.Load("photo.jpg");
image.Resize(1200, 1200, keepAspectRatio: true);
image.Save("photo-resized.jpg");
```

Related capabilities have separate owners:

- Use [ChartForgeX](https://www.nuget.org/packages/ChartForgeX) for charts and topology diagrams.
- Use [CodeGlyphX](https://www.nuget.org/packages/CodeGlyphX) for QR codes and barcodes.

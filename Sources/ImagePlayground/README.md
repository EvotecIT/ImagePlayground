# ImagePlayground

ImagePlayground is the main .NET package for image processing, QR code generation and reading, barcode generation and reading, ChartForgeX-backed chart rendering, EXIF metadata, image comparison, thumbnails, icons, grids, mosaics, and animated GIF helpers.

Use this package for new .NET applications. The split `ImagePlayground.Core`, `ImagePlayground.Chart`, `ImagePlayground.BarCode`, and `ImagePlayground.QRCode` packages remain available as compatibility surfaces, but the main package is the preferred entry point.

## Supported Areas

- Image load, save, conversion, resize, crop, rotate, blur, sharpen, watermark, text, thumbnails, icons, grids, mosaics, and GIF creation.
- EXIF metadata read, write, import, export, and removal helpers.
- Image comparison helpers.
- QR code generation and reading powered by CodeGlyphX.
- Barcode generation and reading powered by CodeGlyphX.
- Chart rendering powered by ChartForgeX, including PNG, SVG, and HTML output.

## Related Packages

- `ChartForgeX` for direct chart, visual block, and topology rendering.
- `CodeGlyphX` for direct QR and barcode engine access.
- `ImagePlayground.Gdi` for optional Windows-focused GDI+ composition helpers.

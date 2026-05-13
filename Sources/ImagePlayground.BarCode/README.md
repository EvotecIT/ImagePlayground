# ImagePlayground.BarCode

ImagePlayground.BarCode contains the ImagePlayground barcode wrapper API backed by CodeGlyphX.

This package remains available for compatibility with projects that intentionally reference the split assembly. New ImagePlayground users should usually reference `ImagePlayground`; applications that need direct barcode engine access should reference `CodeGlyphX`.

## Supported Areas

- Barcode generation for Code128, Code93, Code39, KIX, UPC-A, UPC-E, EAN, Data Matrix, and PDF417.
- Barcode reading from image files.
- PNG-first rendering with conversion to supported image formats.
- Async generation and decode helpers where supported.

## Related Package

- `CodeGlyphX` is the direct engine for QR codes, 1D barcodes, matrix barcodes, payload builders, and rendering primitives.

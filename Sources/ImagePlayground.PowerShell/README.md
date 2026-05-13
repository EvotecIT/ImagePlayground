# ImagePlayground.PowerShell

ImagePlayground.PowerShell is the PowerShell module assembly for ImagePlayground cmdlets.

This package is primarily used by the ImagePlayground module packaging pipeline. Most PowerShell users should install the module from PowerShell Gallery with `Install-Module ImagePlayground`.

## Supported Areas

- Image manipulation cmdlets.
- QR code and barcode cmdlets powered by CodeGlyphX-backed ImagePlayground APIs.
- Chart cmdlets powered by ChartForgeX-backed ImagePlayground APIs.
- EXIF metadata, image comparison, thumbnails, icons, grids, mosaics, and GIF cmdlets.

Chart cmdlets can render native `ChartForgeX.Core.Chart` instances directly, so callers can use the full ChartForgeX builder API and let ImagePlayground handle module packaging, output paths, preview, and PNG/SVG/HTML file creation.

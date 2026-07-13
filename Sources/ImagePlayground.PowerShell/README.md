# ImagePlayground.PowerShell

This project builds the binary PowerShell module for ImagePlayground. Its cmdlets are a thin surface over the `ImagePlayground` .NET package.

The module provides commands for image conversion, resizing, composition, text, watermarks, metadata, thumbnails, icons, mosaics, grids, avatars, and GIFs. It does not include chart, topology, QR-code, or barcode commands; use ChartForgeX or CodeGlyphX directly for those capabilities.

```powershell
Install-Module -Name ImagePlayground -Scope CurrentUser
Import-Module ImagePlayground

Resize-Image -FilePath '.\photo.jpg' -OutputPath '.\photo-small.jpg' -Width 800
Add-ImageWatermark -FilePath '.\photo.jpg' -OutputPath '.\marked.jpg' -WatermarkPath '.\logo.png'
```

Cmdlets expose one execution mode. Where the core has a cancellable asynchronous file API, the cmdlet uses it internally; callers do not select an implementation with `-Async`.

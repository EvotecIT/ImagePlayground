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

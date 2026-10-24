---
title: "Style a QR code with a logo and caption"
description: "Create coordinated PNG and SVG QR artwork, draw a local logo and verify the payload with ImagePlayground."
layout: docs
meta.project_base_slug: "imageplayground"
meta.project_name: "ImagePlayground"
meta.project_section: "examples"
meta.project_hub_path: "/projects/imageplayground/"
meta.project_link_examples: "/projects/imageplayground/examples/"
---

Create a menu code with a caption, export a styled SVG, and add a logo whose size respects the QR correction budget. This example draws its own logo, so it needs no downloaded artwork or sample directory.

Install ImagePlayground, then save the script below as `QRCode-Art.ps1` and run it. It writes into `qr-art` beneath the current directory; use `-OutputDirectory` to choose another folder.

## Example

```powershell
#Requires -Modules ImagePlayground
param(
    [string] $OutputDirectory = (Join-Path (Get-Location) 'qr-art')
)

New-Item -ItemType Directory -Path $OutputDirectory -Force | Out-Null
$url = 'https://example.com/menu'

# Choose a coordinated look and keep the caption outside the QR quiet zone.
New-ImageQRCode -Content $url -FilePath (Join-Path $OutputDirectory 'menu.png') `
    -Preset Sunset -Frame ScanMePill -Caption 'VIEW THE MENU' -PixelSize 16 -ErrorCorrection H

# The extension selects SVG. EyeShape and EyeBall can be set independently.
New-ImageQRCode -Content $url -FilePath (Join-Path $OutputDirectory 'vector.svg') `
    -Preset Ink -EyeShape Rounded -EyeBall Circle -Frame CornerViewfinder `
    -Caption 'SCAN TO EXPLORE' -PixelSize 16 -ErrorCorrection H

# Draw the example logo locally; no external image is required.
$logo = Join-Path $OutputDirectory 'logo.png'
New-ImageCanvas -Width 80 -Height 80 -Theme Light -Backdrop Plain `
    -BackgroundTop '#431b1b' -BackgroundBottom '#431b1b' -PngOutputScale 1 `
    -LayerDefinition {
        New-ImageCanvasText -X 16 -Y 8 -Width 60 -Text 'E' -FontSize 56 -Color White -Emphasized
    } -FilePath $logo

New-ImageQRCode -Content $url -FilePath (Join-Path $OutputDirectory 'brand.png') `
    -Preset Brand -Gradient '#431b1b', '#8c2059' -Logo $logo -LogoScale 0.3 `
    -PixelSize 16 -ErrorCorrection H

# Inspect the generated payloads before distributing the final assets.
Get-ChildItem -LiteralPath $OutputDirectory -Filter '*.png' |
    Where-Object { $_.Name -ne 'logo.png' } |
    ForEach-Object {
        [pscustomobject] @{
            File = $_.Name
            Text = (Get-ImageQRCode -FilePath $_.FullName).Text
        }
    }
```

## Choosing a look

`-Preset` supplies the body, finder shapes and colours together. An explicit `-ForegroundColor` replaces the preset gradient; an explicit two-colour `-Gradient` takes precedence over that solid colour. `-EyeShape` controls the finder ring, and `-EyeBall` controls its centre. The output extension selects PNG or SVG.

Captions and frames sit outside the complete quiet zone. `-Size` bounds the QR panel; a caption or frame can make the complete canvas larger. `-Logo` is an alias for `-LogoPath`. Logo placement preserves functional patterns and throws when the requested minimum cannot fit or the rendered pixels fail validation.

Native validation checks the generated artwork. Inspect and scan the final exported asset on the intended device before distributing it, especially after resizing or printing. Windows PowerShell 5.1 uses a more limited managed decoder and can reject artwork that PowerShell 7 accepts.

## Source

- [QRCode-Art.ps1](https://github.com/EvotecIT/ImagePlayground/blob/master/Examples/QRCode-Art.ps1)

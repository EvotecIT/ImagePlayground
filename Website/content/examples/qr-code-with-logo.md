---
title: "Create a QR code with a logo"
description: "Use ImagePlayground to generate a QR code and add a watermark image."
layout: docs
meta.project_base_slug: "imageplayground"
meta.project_name: "ImagePlayground"
meta.project_section: "examples"
meta.project_hub_path: "/projects/imageplayground/"
meta.project_link_examples: "/projects/imageplayground/examples/"
---

This pattern is useful when a generated QR code needs to carry brand context in a report, handout, or static page.

It comes from the source example at `Examples/Images.QRCodeWithImage.ps1`.

## When to use this pattern

- You need a QR code as a generated image file.
- You want to add a logo or watermark after generation.
- The output should be produced from a repeatable PowerShell script.

## Example

```powershell
Import-Module ImagePlayground

$qrPath = "$PSScriptRoot\Samples\QRCode.png"
$outputPath = "$PSScriptRoot\Output\QRCodeWithImage.jpg"

New-ImageQRCode -Content 'https://example.com/helpdesk' -FilePath $qrPath -Verbose

$image = Get-Image -FilePath $qrPath
$image.WatermarkImage(
    "$PSScriptRoot\Samples\LogoEvotec.png",
    [ImagePlayground.WatermarkPlacement]::TopLeft,
    1,
    0.5,
    0,
    [SixLabors.ImageSharp.Processing.FlipMode]::None,
    50
)

Save-Image -Image $image -Open -FilePath $outputPath
```

## What this demonstrates

- generating a QR code image
- loading the image for additional processing
- saving a watermarked output file

## Source

- [Images.QRCodeWithImage.ps1](https://github.com/EvotecIT/ImagePlayground/blob/master/Examples/Images.QRCodeWithImage.ps1)

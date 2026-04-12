---
title: "Create and read barcodes"
description: "Use ImagePlayground to create EAN barcode images and read them back."
layout: docs
meta.project_base_slug: "imageplayground"
meta.project_name: "ImagePlayground"
meta.project_section: "examples"
meta.project_hub_path: "/projects/imageplayground/"
meta.project_link_examples: "/projects/imageplayground/examples/"
---

This pattern is useful when barcode generation and validation should live in one script.

It comes from the source example at `Examples/Barcode.Create.ps1`.

## When to use this pattern

- You need barcode images generated from known values.
- You want a quick read-back check after generation.
- The output should be a file that another process can attach or publish.

## Example

```powershell
Import-Module ImagePlayground

$outputPath = Join-Path $PSScriptRoot 'Output'
New-Item -Path $outputPath -ItemType Directory -Force | Out-Null

$ean13Path = Join-Path $outputPath 'BarcodeEAN13.png'
$ean7Path = Join-Path $outputPath 'BarcodeEAN7.png'

New-ImageBarCode -FilePath $ean13Path -Type EAN -Value '5901234123457'
New-ImageBarCode -FilePath $ean7Path -Type EAN -Value '5901234'

Get-ImageBarCode -FilePath $ean13Path
Get-ImageBarCode -FilePath $ean7Path
```

## What this demonstrates

- creating barcode images from PowerShell
- using EAN barcode values
- reading generated images back for validation

## Source

- [Barcode.Create.ps1](https://github.com/EvotecIT/ImagePlayground/blob/master/Examples/Barcode.Create.ps1)

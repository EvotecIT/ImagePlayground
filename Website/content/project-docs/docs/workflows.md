---
title: "Choose an ImagePlayground workflow"
description: "Choose the ImagePlayground command family that matches an image automation job."
layout: docs
---

ImagePlayground groups several related jobs behind one PowerShell module. Start with the output you need rather than with a file format.

## Process an existing image

Use `Get-Image` and `Save-Image` for a sequence of edits, or a focused cmdlet when the operation stands alone. Common operations include resizing, cropping, rotation, adjustment, blur, sharpening, text, watermarks, thumbnails, mosaics, merging, and conversion.

## Create or read a code

Use `New-ImageQRCode` and `Get-ImageQRCode` for general QR content. Typed commands cover contact, Wi-Fi, calendar, email, OTP, payment, cryptocurrency, phone, SMS, and location payloads. Barcode commands cover creation and readback workflows.

## Inspect or sanitize metadata

Use the EXIF and metadata commands to inspect provenance, export metadata for review, remove sensitive fields, or apply controlled updates. Treat metadata removal as a deliberate publishing step: always inspect the exported result before distributing an image.

## Build report graphics

Use the chart, grid, topology, hierarchy, visual canvas, and visual story commands when the result is a report asset rather than a photo edit. These surfaces are thin PowerShell adapters over ChartForgeX and can produce static SVG/PNG parity as well as optional HTML or animation where supported.

## Find the exact command

Open the [PowerShell API reference](/projects/imageplayground/api/) to search every exported cmdlet, parameter set, example, and output contract.

---
title: "Choose an ImagePlayground workflow"
description: "Choose the ImagePlayground command family that matches an image automation job."
layout: docs
---

ImagePlayground groups several related jobs behind one PowerShell module. Start with the output you need rather than with a file format.

The current source manifest exports 131 cmdlets. The groups below cover the whole surface; use the [PowerShell API reference](/projects/imageplayground/api/) when you need the exact parameter sets and examples for one command.

## Process an existing image

Use `Get-Image` and `Save-Image` for a sequence of edits, or a focused cmdlet when the operation stands alone. Common operations include resizing, cropping, rotation, adjustment, blur, sharpening, text, watermarks, thumbnails, mosaics, merging, and conversion.

## Create or read a code

Use `New-ImageQRCode` and `Get-ImageQRCode` for general QR content. Typed commands cover contact, Wi-Fi, calendar, email, OTP, payment, cryptocurrency, phone, SMS, and location payloads. Barcode commands cover creation and readback workflows.

## Inspect or sanitize metadata

Use the EXIF and metadata commands to inspect provenance, export metadata for review, remove sensitive fields, or apply controlled updates. Treat metadata removal as a deliberate publishing step: always inspect the exported result before distributing an image.

## Build report graphics

The chart family contains 34 commands spanning common business charts, statistical plots, gauges, progress visuals, heatmaps, treemaps, waterfalls, and annotations. Use `New-ImageChart` as the renderer and the other `New-ImageChart*` commands as typed definitions.

## Compose canvases and dashboards

Use `New-ImageCanvas` with positioned text and information tiles for social cards, covers, wallpapers, and announcements. Use `New-ImageVisualGrid` with metric cards, lists, tables, and timelines when the result is a dashboard or status summary.

## Map systems and organizations

Use `New-ImageTopology` for grouped nodes, named ports, detail rows, edges, route diagnostics, motion, and ordered scenarios. Use `New-ImageOrganizationChart` for reporting or ownership hierarchies.

## Publish visual stories

Console stories model commands, output, tables, pauses, palettes, and persistent tabs. Generic visual stories combine resolved source, terminal, text, SVG, and image panels into scenes with declared outcomes. Visual grids can also become script-free animated stories through named motion cues.

Charts, topology, hierarchy, canvases, grids, and stories are thin PowerShell adapters over ChartForgeX. They can produce static SVG/PNG output as well as optional HTML or animation where supported.

## Find the exact command

Open the [PowerShell API reference](/projects/imageplayground/api/) to search every exported cmdlet, parameter set, example, and output contract.

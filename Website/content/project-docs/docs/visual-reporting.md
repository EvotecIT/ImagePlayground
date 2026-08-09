---
title: "Charts, topology, and visual stories"
description: "Use ImagePlayground as a PowerShell surface for ChartForgeX report visuals."
layout: docs
---

ImagePlayground exposes ChartForgeX through PowerShell-oriented builders and export commands. The reusable rendering model stays in ChartForgeX; ImagePlayground supplies cmdlets, PowerShell parameter sets, packaging, and examples.

## Charts and themes

Build common charts with `New-ImageChart*` commands, apply a reusable theme, and save the same model as PNG, SVG, or HTML. Use a grid when several charts and exact-value blocks need one report surface.

## Organizations and topology

Use `New-ImageTopology*` commands for service maps, infrastructure, ownership, or organization data. Hierarchy layout policies can keep a dense branch compact or vertical without changing unrelated branches. Scenarios and motion cues can explain changes or routes while the static output remains deterministic.

## Visual canvases

Visual canvases combine text, charts, images, shapes, and reusable blocks into fixed-size assets such as report covers, social previews, wallpapers, or email graphics.

## Visual stories

Visual stories turn a sequence of charts, console scenes, and annotations into a single authored narrative. Use SVG or HTML for crisp scalable delivery, PNG for a fixed still, and GIF/APNG when a portable animation is the right output.

See the [curated examples](/projects/imageplayground/examples/) for complete scripts and the [ChartForgeX project hub](/projects/chartforgex/) for the underlying .NET model.

---
title: "Charts, topology, and visual stories"
description: "Use ImagePlayground as a PowerShell surface for ChartForgeX report visuals."
layout: docs
---

ImagePlayground exposes ChartForgeX through PowerShell-oriented builders and export commands. The reusable rendering model stays in ChartForgeX; ImagePlayground supplies cmdlets, PowerShell parameter sets, packaging, and examples.

## Charts and themes

Build common charts with `New-ImageChart*` commands, apply a reusable theme, and save the same model as PNG, SVG, or HTML. Use a grid when several charts and exact-value blocks need one report surface.

The default and `Light` selections use the canonical ChartForgeX light design. `Dark` uses its matching dark colors, typography, and frames. Explicit colors and named presentation themes remain supported. `New-ImageChart -Title ... -Subtitle ...` uses the same heading contract as grids and topology; unbound settings preserve an existing native chart.

`New-ImageChart -Definition` accepts `ImagePlayground.ChartDefinition[]`, including ordinary arrays of results from the chart data commands. Native `ChartForgeX.Core.Chart` objects use `-Chart` or pipeline input. PowerShell selects the input path from the model type and rejects unrelated objects during parameter binding.

One definition export combines series of the same chart kind. Mixing kinds in that input reports an error before creating output. For a supported combination of native series, build a `ChartForgeX.Core.Chart` and export it through `-Chart` or the pipeline.

Apply the same theme to the grid and its child blocks when composing a report. HTML grids reflow to one column on narrow screens. Fixed SVG and PNG exports retain their design dimensions, so author a one-column compact grid instead of reducing a wide image. The [service-health example](https://github.com/EvotecIT/ImagePlayground/blob/main/Examples/VisualGrid.ServiceHealth.ps1) shows both forms using full-size blocks.

`New-ImageCanvasText` inherits its canvas foreground when `-Color` is omitted. Explicit white, transparent and custom colors remain the author's choice across SVG and PNG output.

## Organizations and topology

Use `New-ImageTopology*` commands for service maps, infrastructure, ownership, or organization data. Hierarchy layout policies can keep a dense branch compact or vertical without changing unrelated branches. Scenarios and motion cues can explain changes or routes while the static output remains deterministic.

## Visual canvases

Visual canvases combine text, charts, images, shapes, and reusable blocks into fixed-size assets such as report covers, social previews, wallpapers, or email graphics.

`New-ImageCanvas -Theme Light` and `-Theme Dark` share the chart color and font roles. Custom backgrounds and layer colors still take precedence, and exporting an existing canvas preserves its theme unless a theme override is supplied. The [social-preview example](https://github.com/EvotecIT/ImagePlayground/blob/main/Examples/Canvas.SocialPreview.ps1) authors both a wide preview and a separate compact layout.

## Visual stories

Visual stories turn a sequence of charts, console scenes, and annotations into a single authored narrative. Use SVG or HTML for crisp scalable delivery, PNG for a fixed still, and GIF/APNG when a portable animation is the right output.

New generic stories use `VisualStoryTheme.GraphiteDark()`; pass `VisualStoryTheme.GraphiteLight()` or a custom theme when needed. Terminal `Light` and `Dark` use the matching canonical palette, while explicitly selected shell profiles preserve their familiar palettes. Exporting native stories preserves their existing theme and timeline.

See the [curated examples](/projects/imageplayground/examples/) for complete scripts and the [ChartForgeX project hub](/projects/chartforgex/) for the underlying .NET model.

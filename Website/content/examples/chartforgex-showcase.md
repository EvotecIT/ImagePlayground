---
title: "Create ChartForgeX charts"
description: "Generate PNG, SVG, HTML and transparent ChartForgeX chart overlays from ImagePlayground."
layout: docs
meta.project_base_slug: "imageplayground"
meta.project_name: "ImagePlayground"
meta.project_section: "examples"
meta.project_hub_path: "/projects/imageplayground/"
meta.project_link_examples: "/projects/imageplayground/examples/"
---

ImagePlayground keeps chart construction in ChartForgeX and provides a thin PowerShell rendering command. The same native chart can be written as PNG, SVG, or a standalone HTML page.

```powershell
using module ImagePlayground

$cpu = [ChartForgeX.Core.ChartPoints]::FromValues(31,42,37,55,68,61,74,58,49,63)
$memory = [ChartForgeX.Core.ChartPoints]::FromValues(48,51,55,57,60,62,59,64,66,69)
$chart = [ChartForgeX.Core.Chart]::Create()
$chart.WithTitle('Workstation health').WithSize(920, 520).WithTheme([ChartForgeX.Themes.ChartTheme]::ReportDark()).WithXAxis('Sample').WithYAxis('Usage %').WithGrid().WithLegend() | Out-Null
$chart.AddSmoothLine('CPU', $cpu, [ChartForgeX.Primitives.ChartColor]::FromHex('#38BDF8')) | Out-Null
$chart.AddSmoothLine('Memory', $memory, [ChartForgeX.Primitives.ChartColor]::FromHex('#34D399')) | Out-Null

$chart | New-ImageChart -FilePath .\Examples\Samples\ChartsChartForgeXTrend.png
$chart | New-ImageChart -FilePath .\Examples\Samples\ChartsChartForgeXTrend.svg
$chart | New-ImageChart -FilePath .\Examples\Samples\ChartsChartForgeXTrend.html
```

The repository script `Examples\Charts.ChartForgeX.Showcase.ps1` contains the complete example.

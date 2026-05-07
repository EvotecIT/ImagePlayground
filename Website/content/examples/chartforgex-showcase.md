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

ImagePlayground uses ChartForgeX for chart rendering, so the same chart definition can be written as PNG, SVG or a standalone HTML page. Transparent rendering is useful when the chart should be placed over an existing image instead of framed as a report card.

```powershell
Import-Module .\ImagePlayground.psd1 -Force

$definitions = @(
    New-ImageChartLine -Name 'CPU' -Value 31,42,37,55,68,61,74,58,49,63 -Color DeepSkyBlue -Marker Circle -Smooth
    New-ImageChartLine -Name 'Memory' -Value 48,51,55,57,60,62,59,64,66,69 -Color MediumSeaGreen -Marker Square -Smooth
)

$options = New-ImageChartOptions -Transparent -NoCard -NoPlotBackground -ShowLegend -LegendPosition Bottom -ShowDataLabels

New-ImageChart -Definition $definitions -Theme Dark -ShowGrid -XTitle 'Sample' -YTitle 'Usage %' -Options $options -FilePath .\Examples\Samples\ChartsChartForgeXTransparent.png -Width 760 -Height 420
New-ImageChart -Definition $definitions -Theme Dark -ShowGrid -XTitle 'Sample' -YTitle 'Usage %' -Options $options -FilePath .\Examples\Samples\ChartsChartForgeXTrend.svg -Width 760 -Height 420
New-ImageChart -Definition $definitions -Theme Dark -ShowGrid -XTitle 'Sample' -YTitle 'Usage %' -Options $options -FilePath .\Examples\Samples\ChartsChartForgeXTrend.html -Width 760 -Height 420
```

The repository script `Examples\Charts.ChartForgeX.Showcase.ps1` also creates donut, progress, pictorial and word-cloud samples.

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

ImagePlayground keeps chart construction in ChartForgeX and provides a thin PowerShell rendering command. Reuse the same definitions for PNG, SVG, or a standalone HTML page. The light/dark selections share canonical color roles and typography, while explicit series colors remain available.

```powershell
Import-Module ImagePlayground

$definitions = @(
    New-ImageChartLine -Name CPU -Value 31,42,37,55,68,61,74,58,49,63 -Smooth
    New-ImageChartLine -Name Memory -Value 48,51,55,57,60,62,59,64,66,69 -Smooth
)
$options = New-ImageChartOptions -ShowLegend -LegendPosition Bottom -TickCount 4
foreach ($extension in 'png', 'svg', 'html') {
    New-ImageChart -Definition $definitions -Title 'Workstation health' -Subtitle 'Ten samples of resource usage' -Theme Dark -ShowGrid -XTitle Sample -YTitle 'Usage %' -Options $options -Width 760 -Height 420 -FilePath ".\workstation-health.$extension"
}
```

The repository script `Examples\Charts.ChartForgeX.Showcase.ps1` contains the complete example and a separately authored 368px chart. Fixed SVG and PNG output keep their design size; select the compact export when a wide chart would make its labels too small.

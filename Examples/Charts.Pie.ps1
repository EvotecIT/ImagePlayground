﻿Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

New-ImageChart {
    New-ImageChartPie -Name "C#" -Value 5
    New-ImageChartPie -Name "C++" -Value 12
    New-ImageChartPie -Name "PowerShell" -Value 10
} -Show -FilePath $PSScriptRoot\Output\ChartsPie1.png -Width 500 -Height 500
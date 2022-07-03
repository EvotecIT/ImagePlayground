﻿Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

New-ImageChart {
    New-ImageChartBar -Value 5, 10 -Label "C#"
    New-ImageChartBar -Value 12, 18 -Label "C++"
    New-ImageChartBar -Value 10, 13 -Label "PowerShell"
} -Show -FilePath $PSScriptRoot\Samples\ChartsBar.png


New-ImageChart {
    New-ImageChartBar -Value 5 -Label "C#"
    New-ImageChartBar -Value 12 -Label "C++"
    New-ImageChartBar -Value 10 -Label "PowerShell"
} -Show -FilePath $PSScriptRoot\Samples\ChartsBar1.png
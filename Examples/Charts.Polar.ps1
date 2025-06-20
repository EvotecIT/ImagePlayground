Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

New-ImageChart {
    New-ImageChartPolar -Name "Series1" -Angle 0,1.57 -Value 1,2
    New-ImageChartPolar -Name "Series2" -Angle 0.5,2.2 -Value 2,1.5
} -Show -FilePath $PSScriptRoot\Output\ChartsPolar.png -Width 500 -Height 500

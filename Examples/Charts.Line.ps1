Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

New-ImageChart {
    New-ImageChartLine -Value 5, 10, 12, 18, 10, 13 -Name "C#" -Marker FilledCircle
    New-ImageChartLine -Value 10,15,30,40,50,60 -Name "C++" -Marker FilledSquare
    New-ImageChartLine -Value 10,5,12,18,30,60 -Name "PowerShell" -Marker OpenCircle
} -Show -FilePath $PSScriptRoot\Output\ChartsLine.png -Width 500 -Height 500
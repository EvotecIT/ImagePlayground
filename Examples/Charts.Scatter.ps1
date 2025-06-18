Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

New-ImageChart {
    New-ImageChartScatter -Name "First" -X 1,2,3 -Y 4,5,6
    New-ImageChartScatter -Name "Second" -X 1,2,3 -Y 3,2,1
} -Show -FilePath $PSScriptRoot\Output\ChartsScatter.png -Width 500 -Height 500

Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

New-ImageChart {
    New-ImageChartArea -Value 5,10,12,18,10,13 -Name "C#"
    New-ImageChartArea -Value 10,15,30,40,50,60 -Name "C++"
} -Show -FilePath $PSScriptRoot\Output\ChartsArea.png -Width 500 -Height 500

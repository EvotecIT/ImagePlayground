Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

New-ImageChart {
    New-ImageChartBar -Value 5 -Label "C#"
    New-ImageChartBar -Value 12 -Label "C++"
    New-ImageChartBar -Value 10 -Label "PowerShell"
} -Show -FilePath $PSScriptRoot\Samples\ChartsBar.png -Width 500 -Height 500
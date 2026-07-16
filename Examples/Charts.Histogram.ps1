Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

New-ImageChart {
    New-ImageChartHistogram -Name 'Data' -Values 1,2,3,3,4,5 -BinSize 2
} -Show -FilePath $PSScriptRoot\Output\ChartsHistogram.png -Width 500 -Height 500

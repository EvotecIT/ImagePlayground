Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

$matrix = [double[,]]::new(2,2)
$matrix[0,0] = 1
$matrix[0,1] = 2
$matrix[1,0] = 3
$matrix[1,1] = 4

New-ImageChart {
    New-ImageChartHeatmap -Name 'Heat' -Matrix $matrix
} -Show -FilePath $PSScriptRoot\Output\ChartsHeatmap.png -Width 500 -Height 500

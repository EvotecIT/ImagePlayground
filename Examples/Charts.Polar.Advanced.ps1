Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

New-ImageChart -ChartsDefinition {
    New-ImageChartPolar -Name "Demo" -Angle 0,1.57,3.14 -Value 1,2,1 -Color ([SixLabors.ImageSharp.Color]::Red)
} -XTitle 'Angle (rad)' -YTitle 'Radius' -ShowGrid -FilePath $PSScriptRoot\Output\ChartsPolarAdvanced.png -Width 600 -Height 400

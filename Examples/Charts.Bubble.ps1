Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

New-ImageChart {
    New-ImageChartBubble -Name "First" -X 1,2,3 -Y 4,5,6 -Size 10,20,30 -Color Blue
    New-ImageChartBubble -Name "Second" -X 1,2,3 -Y 3,2,1 -Size 15,10,25 -Color Red
} -Show -FilePath $PSScriptRoot\Output\ChartsBubble.png -Width 500 -Height 500

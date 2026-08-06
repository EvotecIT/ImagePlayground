Import-Module "$PSScriptRoot\..\ImagePlayground.psd1" -Force

New-ImageChart -ChartsDefinition {
    New-ImageChartRadar -Name 'Current' -Category 1, 2, 3, 4, 5 -Value 82, 68, 91, 74, 88 -Color '#2563EB'
    New-ImageChartRadar -Name 'Target' -Category 1, 2, 3, 4, 5 -Value 90, 85, 88, 92, 90 -Color '#14B8A6'
} -Theme Aurora -FilePath "$PSScriptRoot\radar.svg" -Width 640 -Height 480

Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

New-ImageChart -ChartsDefinition {
    New-ImageChartLine -Name 'Sample' -Value 1,3,2,5 -Marker FilledCircle
} -AnnotationsDefinition {
    New-ImageChartAnnotation -X 3 -Y 5 -Text 'Peak' -Arrow
} -Show -FilePath $PSScriptRoot\Samples\ChartsAnnotated.png -Width 500 -Height 300


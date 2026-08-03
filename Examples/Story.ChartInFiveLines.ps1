Import-Module $PSScriptRoot\..\ImagePlayground.psd1 -Force

$outputRoot = Join-Path -Path $PSScriptRoot -ChildPath 'Output'
$chartPath = Join-Path -Path $outputRoot -ChildPath 'weekly-builds.png'
$storyPath = Join-Path -Path $outputRoot -ChildPath 'chart-in-five-lines.gif'
if (-not (Test-Path -Path $outputRoot)) {
    New-Item -Path $outputRoot -ItemType Directory | Out-Null
}

$sourceText = @'
New-ImageChart {
    New-ImageChartLine -Name Builds -Value 12, 18, 15, 24, 31
} -FilePath '.\weekly-builds.png' -Width 900 -Height 500
'@

# Producing a real artifact is explicit. New-ImageStory itself never executes this code.
New-ImageChart {
    New-ImageChartLine -Name Builds -Value 12, 18, 15, 24, 31
} -FilePath $chartPath -Width 900 -Height 500

$source = ConvertTo-ImageStorySource -Text $sourceText -Language PowerShell
$code = New-ImageStoryPanel -Id code -Title 'PowerShell' -Source $source
$chart = New-ImageStoryPanel -Id chart -Title 'Result' -MediaPath $chartPath -AccessibleText 'Weekly builds line chart'
$write = New-ImageStoryScene -Id write -Title 'Write the chart' -Panels $code
$complete = New-ImageStoryScene -Id complete -Title 'See the result' -Layout Split -Panels $code, $chart
$outcome = New-ImageStoryOutcome -Id chart -Label 'The weekly builds chart is visible.' -PanelId chart

New-ImageStory -Title 'A chart in five lines' -Description 'PowerShell source and its real rendered result.' `
    -Scenes $write, $complete -Outcomes $outcome -FilePath $storyPath `
    -BundlePath (Join-Path -Path $outputRoot -ChildPath 'chart-story-bundle')

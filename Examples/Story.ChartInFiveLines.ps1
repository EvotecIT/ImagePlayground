param(
    [string] $OutputDirectory = (Join-Path -Path $PSScriptRoot -ChildPath 'Output'),
    [ValidateSet('Light', 'Dark')]
    [string] $Theme = 'Light'
)

Import-Module "$PSScriptRoot\..\ImagePlayground.psd1" -Force

$storyTheme = if ($Theme -eq 'Dark') {
    [ChartForgeX.Stories.VisualStoryTheme]::GraphiteDark()
} else {
    [ChartForgeX.Stories.VisualStoryTheme]::GraphiteLight()
}
$suffix = $Theme.ToLowerInvariant()
$wideSourceText = @'
$options = New-ImageChartOptions -TickCount 4
New-ImageChart {
    New-ImageChartLine -Name Builds `
        -Value 12, 18, 15, 24, 31
} -Theme {0} -Width {1} -Height 320 `
    -Title 'Weekly builds' `
    -FilePath '.\weekly-builds.png' `
    -Options $options
'@
$compactSourceText = @'
$series = @{
    Name = 'Builds'
    Value = 12, 18, 15, 24, 31
}
$options = New-ImageChartOptions `
    -TickCount 4
$file = '.\weekly-builds.png'
$chart = @{
    Theme = '{0}'
    Width = {1}
    Height = 320
    Title = 'Weekly builds'
    FilePath = $file
    Options = $options
}
New-ImageChart {
    New-ImageChartLine @series
} @chart
'@
$options = New-ImageChartOptions -TickCount 4

foreach ($compact in $false, $true) {
    $sizeSuffix = if ($compact) { '-compact' } else { '' }
    $chartWidth = if ($compact) { 368 } else { 560 }
    $storyWidth = if ($compact) { 480 } else { 1200 }
    $storyHeight = if ($compact) { 1240 } else { 675 }
    $storyTitle = 'Weekly builds recipe'
    $codeWeight = if ($compact) { 1.3 } else { 1 }
    $completedLayout = if ($compact) { 'Stacked' } else { 'Split' }
    $chartPath = Join-Path -Path $OutputDirectory -ChildPath "weekly-builds-$suffix$sizeSuffix.png"

    # Producing the chart is explicit. Story authoring never executes displayed source.
    New-ImageChart {
        New-ImageChartLine -Name Builds -Value 12, 18, 15, 24, 31
    } -Title 'Weekly builds' -Theme $Theme -Options $options -FilePath $chartPath -Width $chartWidth -Height 320

    # Replace only declared placeholders: PowerShell source braces remain literal.
    $sourceText = if ($compact) { $compactSourceText } else { $wideSourceText }
    $resolvedSource = $sourceText.Replace('{0}', $Theme).Replace('{1}', [string] $chartWidth)
    $source = ConvertTo-ImageStorySource -Text $resolvedSource -Language PowerShell
    $code = New-ImageStoryPanel -Id code -Title PowerShell -Source $source -Weight $codeWeight
    $chart = New-ImageStoryPanel -Id chart -Title Result -MediaPath $chartPath -AccessibleText 'Weekly builds line chart'
    $write = New-ImageStoryScene -Id write -Title 'Write the chart' -Panels $code
    $complete = New-ImageStoryScene -Id complete -Title 'See the result' -Layout $completedLayout -Panels $code, $chart
    $outcome = New-ImageStoryOutcome -Id chart -Label 'Weekly builds visible' -PanelId chart

    foreach ($extension in 'svg', 'html', 'png') {
        New-ImageStory -Title $storyTitle -Description 'PowerShell source and rendered chart.' -Theme $storyTheme -Width $storyWidth -Height $storyHeight -Scenes $write, $complete -Outcomes $outcome -FilePath (Join-Path -Path $OutputDirectory -ChildPath "chart-in-five-lines-$suffix$sizeSuffix.$extension")
    }
    if (-not $compact) {
        New-ImageStory -Title $storyTitle -Description 'PowerShell source and rendered chart.' -Theme $storyTheme -Width $storyWidth -Height $storyHeight -Scenes $write, $complete -Outcomes $outcome -FilePath (Join-Path -Path $OutputDirectory -ChildPath "chart-in-five-lines-$suffix.gif") -BundlePath (Join-Path -Path $OutputDirectory -ChildPath "chart-story-$suffix-bundle")
    }
}

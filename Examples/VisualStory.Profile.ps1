param(
    [string] $OutputDirectory = (Join-Path -Path $PSScriptRoot -ChildPath 'Output'),
    [ValidateSet('Light', 'Dark')]
    [string] $Theme = 'Light'
)

Import-Module "$PSScriptRoot\..\ImagePlayground.psd1" -Force

$items = @(
    New-ImageVisualGridItem -TargetId projects -Block (New-ImageMetricCard -Label 'Maintained projects' -Value 24 -Caption 'Reusable libraries' -Width 336 -Height 176 -Theme $Theme)
    New-ImageVisualGridItem -TargetId community -Block (New-ImageMetricCard -Label 'Community stars' -Value 8200 -Format N0 -Caption 'Across active projects' -Width 336 -Height 176 -Theme $Theme)
)
$motionDefinition = {
    New-ImageVisualMotionCue -TargetId title -Effect Reveal -DurationSeconds 0.65
    New-ImageVisualMotionCue -TargetId subtitle -Effect Fade -DelaySeconds 0.12 -DurationSeconds 0.5
    New-ImageVisualMotionCue -TargetId projects -Effect Rise -DelaySeconds 0.28 -DurationSeconds 0.6
    New-ImageVisualMotionCue -TargetId community -Effect Rise -DelaySeconds 0.4 -DurationSeconds 0.6
}
$suffix = $Theme.ToLowerInvariant()
$wide = New-ImageVisualGrid -Title 'Engineering portfolio' -Subtitle 'Reusable libraries and community' -Columns 2 -Gap 16 -Padding 24 -Theme $Theme -Content $items
$compact = New-ImageVisualGrid -Title 'Engineering portfolio' -Subtitle 'Reusable libraries and community' -Columns 1 -Gap 16 -Padding 16 -Theme $Theme -Content $items

foreach ($extension in 'svg', 'html', 'png') {
    $wide | New-ImageVisualStory -MotionDefinition $motionDefinition -FilePath (Join-Path -Path $OutputDirectory -ChildPath "engineering-portfolio-$suffix.$extension")
    $compact | New-ImageVisualStory -MotionDefinition $motionDefinition -FilePath (Join-Path -Path $OutputDirectory -ChildPath "engineering-portfolio-$suffix-compact.$extension")
}

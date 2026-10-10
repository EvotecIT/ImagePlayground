param(
    [string] $OutputDirectory = (Join-Path -Path $PSScriptRoot -ChildPath 'Output'),
    [ValidateSet('Light', 'Dark')]
    [string] $Theme = 'Light'
)

Import-Module "$PSScriptRoot\..\ImagePlayground.psd1" -Force

$blocks = @(
    New-ImageMetricCard -Label Requests -Value 12840 -Format N0 -Trend '+12% this week' -Caption 'Completed API requests' -Status Positive -MiniValues 8200, 9400, 10100, 12840 -Width 336 -Height 176 -Theme $Theme
    New-ImageListBlock -Title Checks -Item API, Database, Queue -Value Healthy, Warning, Healthy -Status Positive, Warning, Positive -Theme $Theme
    New-ImageTableBlock -Title Services -Column Name, Status -StatusColumn Status -Row @(
        @{ Name = 'API'; Status = 'Healthy' }
        @{ Name = 'Database'; Status = 'Warning' }
        @{ Name = 'Queue'; Status = 'Healthy' }
    ) -Theme $Theme
    New-ImageTimelineBlock -Title Activity -Theme $Theme -ItemDefinition {
        New-ImageTimelineItem -Kind Event -Title 'Build completed' -Timestamp '14:20' -Status Positive
        New-ImageTimelineItem -Kind Event -Title 'Latency warning' -Timestamp '14:22' -Status Warning
        New-ImageTimelineItem -Kind ChecklistItem -Title 'Smoke tests passed' -Completed
    }
)
# Author each block at its intended viewing size. Fixed exports retain these design pixels.
[void] $blocks[1].WithSize(336, 176)
[void] $blocks[2].WithSize(336, 204)
[void] $blocks[3].WithSize(336, 220)

$wide = New-ImageVisualGrid -Title 'Service health' -Subtitle 'Last refresh 14:24 UTC' -Columns 2 -Gap 16 -Padding 24 -AdaptiveRowHeights -Theme $Theme -Content $blocks
$compact = New-ImageVisualGrid -Title 'Service health' -Subtitle 'Last refresh 14:24 UTC' -Columns 1 -Gap 16 -Padding 16 -AdaptiveRowHeights -Theme $Theme -Content $blocks
$suffix = $Theme.ToLowerInvariant()

# HTML reflows the same full-size blocks; a fixed SVG or PNG needs its own compact composition.
foreach ($extension in 'html', 'svg', 'png') {
    $wide | New-ImageVisualStory -FilePath (Join-Path -Path $OutputDirectory -ChildPath "service-health-$suffix.$extension")
}
foreach ($extension in 'svg', 'png') {
    $compact | New-ImageVisualStory -FilePath (Join-Path -Path $OutputDirectory -ChildPath "service-health-$suffix-compact.$extension")
}

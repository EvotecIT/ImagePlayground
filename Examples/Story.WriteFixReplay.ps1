param(
    [string] $OutputDirectory = (Join-Path $PSScriptRoot 'Output/WriteFixReplay')
)

# Import ImagePlayground before running this example. These trusted commands execute
# here, explicitly; story construction and rendering only consume their results.
$ErrorActionPreference = 'Stop'
$badText = '[Math]::Roundd(12.345, 2)'
$goodText = '[Math]::Round(12.345, 2)'
try {
    & { [Math]::Roundd(12.345, 2) }
} catch {
    $failure = $_.Exception.Message
}
$result = & { [Math]::Round(12.345, 2) }

$empty = ConvertTo-ImageStorySource -Text '' -Language PowerShell
$badSource = ConvertTo-ImageStorySource -Text $badText -Language PowerShell
$goodSource = ConvertTo-ImageStorySource -Text $goodText -Language PowerShell
$writing = [ChartForgeX.Stories.StorySourceTimeline]::Create($empty).Type($badText, [TimeSpan]::FromSeconds(2))
$fixing = [ChartForgeX.Stories.StorySourceTimeline]::Create($badSource).
    Select(8, 6, [TimeSpan]::FromSeconds(0.75)).
    Edit(8, 6, (ConvertTo-ImageStorySource -Text 'Round' -Language PowerShell), [TimeSpan]::FromSeconds(0.75))
$editor = [ChartForgeX.Stories.VisualStorySourceOptions]::new('round.ps1', 18)
$terminal = [ChartForgeX.Stories.VisualStoryTerminalOptions]::new(18)

$failedRun = New-ImageStoryReplay -DurationSeconds 3 -Events @(
    @{ TimestampSeconds = 0.5; Kind = 'Command'; Text = $badText }
    @{ TimestampSeconds = 1; Kind = 'Output'; Text = $failure; Tone = 'Error' }
)
$successfulRun = New-ImageStoryReplay -DurationSeconds 3 -Events @(
    @{ TimestampSeconds = 0.5; Kind = 'Command'; Text = $goodText }
    @{ TimestampSeconds = 1; Kind = 'Output'; Text = ([string] $result); Tone = 'Success' }
)

$scenes = @(
    New-ImageStoryScene -Id write -Title '01  Write the script' -DurationSeconds 3 -Panels (
        New-ImageStoryPanel -Id code -SourceTimeline $writing -SourceOptions $editor
    )
    New-ImageStoryScene -Id fail -Title '02  Read the actual error' -DurationSeconds 3 -Layout Split -Panels @(
        New-ImageStoryPanel -Id code -Source $badSource -SourceOptions $editor
        New-ImageStoryPanel -Id terminal -Replay $failedRun -TerminalOptions $terminal
    )
    New-ImageStoryScene -Id fix -Title '03  Correct the method' -DurationSeconds 2 -Panels (
        New-ImageStoryPanel -Id code -SourceTimeline $fixing -SourceOptions $editor
    )
    New-ImageStoryScene -Id result -Title '04  Replay the result' -DurationSeconds 3 -Layout Split -Panels @(
        New-ImageStoryPanel -Id code -Source $goodSource -SourceOptions $editor
        New-ImageStoryPanel -Id terminal -Replay $successfulRun -TerminalOptions $terminal
    )
)
$outcome = New-ImageStoryOutcome -Id rounded -Label ([string] $result) -PanelId terminal
New-ImageStory -Title 'Write. Fix. Replay.' -Description 'A captured PowerShell error, an animated source correction, and the real result.' `
    -Scenes $scenes -Outcomes $outcome -FilePath (Join-Path $OutputDirectory 'round.html') `
    -BundlePath (Join-Path $OutputDirectory 'bundle') -BundleFormats Svg, Html, Png, Gif, Apng, Transcript `
    -Width 960 -Height 540 -FramesPerSecond 6 -EndHoldSeconds 2 -PlayCount 1 -Player

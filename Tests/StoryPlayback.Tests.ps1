Describe 'Prepared story playback and recorded events' {
    BeforeAll {
        if ($env:IMAGEPLAYGROUND_TEST_MODULE_PATH) {
            Import-Module $env:IMAGEPLAYGROUND_TEST_MODULE_PATH -Force -ErrorAction Stop
        } else {
            $env:IMAGEPLAYGROUND_DEVELOPMENT = '1'
            Import-Module "$PSScriptRoot/../ImagePlayground.psd1" -Force -ErrorAction Stop
        }
    }

    It 'accepts piped JSON records without executing displayed commands' {
        $untouched = Join-Path $TestDrive 'never-executed.txt'
        $events = @(
            @{ TimestampSeconds = 0; Kind = 'Command'; Text = "Set-Content '$untouched' changed" }
            @{ TimestampSeconds = 8; Kind = 'Output'; Text = 'Ready'; Tone = 'Success' }
        ) | ConvertTo-Json | ConvertFrom-Json
        $replay = $events | New-ImageStoryReplay -DurationSeconds 12 -WorkingDirectory demo
        $replay.Events.Count | Should -Be 2
        $replay.Events[1].Timestamp.TotalSeconds | Should -Be 8
        $short = $replay.CompressPauses([TimeSpan]::FromSeconds(1))
        $short.Events[1].Timestamp.TotalSeconds | Should -Be 1
        $short.Events[1].OriginalTimestamp.TotalSeconds | Should -Be 8
        Test-Path -LiteralPath $untouched | Should -BeFalse
        $panel = $short | New-ImageStoryPanel -Id replay -TerminalOptions ([ChartForgeX.Stories.VisualStoryTerminalOptions]::new(18))
        $panel.Surface.AccessibleText | Should -Match 'Ready'
        $panel.Surface.Replay.Duration.TotalSeconds | Should -Be 2
    }

    It 'passes source edits and fixed viewport options to the owning renderer' {
        $source = ConvertTo-ImageStorySource -Text '' -Language PowerShell
        $timeline = [ChartForgeX.Stories.StorySourceTimeline]::Create($source).Type('Write-Output ready', [TimeSpan]::FromSeconds(1))
        $options = [ChartForgeX.Stories.VisualStorySourceOptions]::new('demo.ps1', 18, $true)
        $panel = $timeline | New-ImageStoryPanel -Id code -SourceOptions $options
        $panel.Surface.Timeline.Duration.TotalSeconds | Should -Be 1
        $panel.Surface.Options.FileName | Should -Be 'demo.ps1'
        $panel.Surface.Options.FontSize | Should -Be 18
    }

    It 'shares the prepared clock and finite play count across a portable bundle' {
        $replay = New-ImageStoryReplay -DurationSeconds 1 -Events @(
            @{ TimestampSeconds = 0; Kind = 'Command'; Text = 'Write-Output ready' }
            @{ TimestampSeconds = 0.5; Kind = 'Output'; Text = 'Ready'; Tone = 'Success' }
        )
        $panel = New-ImageStoryPanel -Id terminal -Replay $replay
        $scene = New-ImageStoryScene -Id complete -Title Complete -DurationSeconds 1 -Panels $panel
        $outcome = New-ImageStoryOutcome -Id ready -Label Ready -PanelId terminal
        $bundle = Join-Path $TestDrive 'bundle'
        $file = Join-Path $TestDrive 'story.html'
        New-ImageStory -Title Replay -Scenes $scene -Outcomes $outcome -Width 480 -Height 320 -FilePath $file `
            -EndHoldSeconds 0.5 -TransitionSeconds 0 -FramesPerSecond 4 -PlayCount 2 -Player `
            -BundlePath $bundle -BundleFormats Svg, Html, Gif, Apng, Transcript
        $manifest = Get-Content (Join-Path $bundle 'story.json') -Raw | ConvertFrom-Json
        $manifest.playback.contentSeconds | Should -Be 1
        $manifest.playback.durationSeconds | Should -Be 1.5
        $manifest.playback.playCount | Should -Be 2
        $manifest.chapters[0].startSeconds | Should -Be 0
        foreach ($path in $file, (Join-Path $bundle 'story.html'), (Join-Path $bundle 'story.svg')) {
            $text = [IO.File]::ReadAllText($path)
            $text | Should -Match 'data-cfx-motion-duration="1.5"'
            $text | Should -Match 'data-cfx-motion-plays="2"'
        }
        foreach ($name in 'story.gif', 'story.apng', 'story.png', 'story.txt') {
            (Get-Item (Join-Path $bundle $name)).Length | Should -BeGreaterThan 20
        }
    }

    It 'preserves an existing file when animated export validation fails' {
        $panel = New-ImageStoryPanel -Id result -Text Ready
        $scene = New-ImageStoryScene -Id complete -Title Complete -DurationSeconds 1 -Panels $panel
        $outcome = New-ImageStoryOutcome -Id ready -Label Ready -PanelId result
        foreach ($extension in 'gif', 'apng') {
            $file = Join-Path $TestDrive "keep.$extension"
            [IO.File]::WriteAllText($file, 'Previous deliverable')
            $settings = @{ FramesPerSecond = 60 }
            if ($extension -eq 'apng') { $settings.MaximumFrames = 2 }
            { New-ImageStory -Title Ready -Scenes $scene -Outcomes $outcome -FilePath $file @settings -ErrorAction Stop } | Should -Throw
            [IO.File]::ReadAllText($file) | Should -BeExactly 'Previous deliverable'
        }
    }
}

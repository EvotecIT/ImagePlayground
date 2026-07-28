Describe 'New-ImageVisualStory' {
    BeforeAll {
        $env:IMAGEPLAYGROUND_DEVELOPMENT = '1'
        Import-Module -Name "$PSScriptRoot/../ImagePlayground.psd1" -Force
        $TestDir = Join-Path -Path $PSScriptRoot -ChildPath 'Artifacts'
        if (-not (Test-Path -Path $TestDir)) {
            New-Item -Path $TestDir -ItemType Directory | Out-Null
        }
    }

    It 'renders a script-free animated SVG from a story script and motion definition' {
        $file = Join-Path -Path $TestDir -ChildPath 'visual-story.svg'
        if (Test-Path -Path $file) {
            Remove-Item -Path $file
        }

        $story = New-ImageVisualStory -StoryScript {
            param($Story)
            [void] $Story.WithTitle('Engineering signal').WithSubtitle('Reusable visual storytelling').WithColumns(1)
            $metric = [ChartForgeX.VisualBlocks.MetricCard]::Create().WithMetric('Projects', '126')
            [void] $Story.Add('projects', $metric)
        } -MotionDefinition {
            New-ImageVisualMotionCue -TargetId title -Effect Reveal -DurationSeconds 0.9
            New-ImageVisualMotionCue -TargetId subtitle -Effect Fade -DelaySeconds 0.15
            New-ImageVisualMotionCue -TargetId projects -Effect Rise -DelaySeconds 0.35 -DistancePixels 10
        } -FilePath $file -PassThru

        $story | Should -BeOfType 'ChartForgeX.VisualBlocks.VisualGrid'
        $story.Motion.Cues.Count | Should -Be 3
        Test-Path -Path $file | Should -BeTrue
        $svg = [System.IO.File]::ReadAllText($file)
        $svg | Should -Match 'data-cfx-motion="timeline"'
        $svg | Should -Match 'data-cfx-motion-target="projects"'
        $svg | Should -Match 'prefers-reduced-motion:reduce'
        $svg | Should -Not -Match '<script'
    }

    It 'accepts a native visual grid and renders the completed PNG state' {
        $file = Join-Path -Path $TestDir -ChildPath 'visual-story.png'
        if (Test-Path -Path $file) {
            Remove-Item -Path $file
        }

        $grid = [ChartForgeX.VisualBlocks.VisualGrid]::Create().WithTitle('Static fallback')
        [void] $grid.Add('metric', [ChartForgeX.VisualBlocks.MetricCard]::Create().WithMetric('Ready', 'Yes'))
        $motion = [ChartForgeX.Motion.VisualMotionTimeline]::Create().Rise('metric')
        $story = $grid | New-ImageVisualStory -Motion $motion -FilePath $file -PassThru

        $story | Should -BeOfType 'ChartForgeX.VisualBlocks.VisualGrid'
        Test-Path -Path $file | Should -BeTrue
        $bytes = [System.IO.File]::ReadAllBytes($file)
        $bytes[0] | Should -Be 137
        $bytes[1] | Should -Be 80
        $bytes[2] | Should -Be 78
        $bytes[3] | Should -Be 71
    }

    It 'rejects unsupported output extensions' {
        $file = Join-Path -Path $TestDir -ChildPath 'visual-story.gif'
        {
            New-ImageVisualStory -StoryScript {
                param($Story)
                [void] $Story.Add([ChartForgeX.VisualBlocks.MetricCard]::Create().WithMetric('Ready', 'Yes'))
            } -FilePath $file
        } | Should -Throw
    }
}

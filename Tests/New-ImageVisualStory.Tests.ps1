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

    It 'rejects unsupported output extensions before invoking the story script' {
        $file = Join-Path -Path $TestDir -ChildPath 'visual-story.gif'
        $global:ImagePlaygroundVisualStoryPathInvoked = $false

        try {
            {
                New-ImageVisualStory -StoryScript {
                    param($Story)
                    $global:ImagePlaygroundVisualStoryPathInvoked = $true
                    [void] $Story.Add([ChartForgeX.VisualBlocks.MetricCard]::Create().WithMetric('Ready', 'Yes'))
                } -FilePath $file
            } | Should -Throw

            $global:ImagePlaygroundVisualStoryPathInvoked | Should -BeFalse
        } finally {
            Remove-Variable -Name ImagePlaygroundVisualStoryPathInvoked -Scope Global -ErrorAction SilentlyContinue
        }
    }

    It 'rejects non-file-system output paths before invoking the story script' {
        $global:ImagePlaygroundVisualStoryProviderInvoked = $false

        try {
            {
                New-ImageVisualStory -StoryScript {
                    param($Story)
                    $global:ImagePlaygroundVisualStoryProviderInvoked = $true
                    [void] $Story.Add([ChartForgeX.VisualBlocks.MetricCard]::Create().WithMetric('Ready', 'Yes'))
                } -FilePath 'Env:\ImagePlaygroundVisualStory.svg'
            } | Should -Throw

            $global:ImagePlaygroundVisualStoryProviderInvoked | Should -BeFalse
        } finally {
            Remove-Variable -Name ImagePlaygroundVisualStoryProviderInvoked -Scope Global -ErrorAction SilentlyContinue
        }
    }

    It 'rejects multiple timelines emitted by one motion definition' {
        $file = Join-Path -Path $TestDir -ChildPath 'multiple-motion-timelines.svg'
        {
            New-ImageVisualStory -StoryScript {
                param($Story)
                [void] $Story.Add('metric', [ChartForgeX.VisualBlocks.MetricCard]::Create().WithMetric('Ready', 'Yes'))
            } -MotionDefinition {
                [ChartForgeX.Motion.VisualMotionTimeline]::Create().Fade('metric')
                [ChartForgeX.Motion.VisualMotionTimeline]::Create().Rise('metric')
            } -FilePath $file
        } | Should -Throw '*at most one VisualMotionTimeline*'
    }

    It 'rejects unsupported output mixed into a motion definition' {
        $file = Join-Path -Path $TestDir -ChildPath 'unsupported-motion-output.svg'
        if (Test-Path -Path $file) {
            Remove-Item -Path $file
        }

        {
            New-ImageVisualStory -StoryScript {
                param($Story)
                [void] $Story.Add('metric', [ChartForgeX.VisualBlocks.MetricCard]::Create().WithMetric('Ready', 'Yes'))
            } -MotionDefinition {
                New-ImageVisualMotionCue -TargetId metric -Effect Fade
                'misspelled helper output'
            } -FilePath $file
        } | Should -Throw '*unsupported output*'

        Test-Path -Path $file | Should -BeFalse
    }

    It 'rejects invalid motion output before invoking the story script' {
        $file = Join-Path -Path $TestDir -ChildPath 'invalid-motion-before-story.svg'
        $global:ImagePlaygroundVisualStoryMotionOrderInvoked = $false

        try {
            {
                New-ImageVisualStory -StoryScript {
                    param($Story)
                    $global:ImagePlaygroundVisualStoryMotionOrderInvoked = $true
                    [void] $Story.Add('metric', [ChartForgeX.VisualBlocks.MetricCard]::Create().WithMetric('Ready', 'Yes'))
                } -MotionDefinition {
                    'unsupported motion output'
                } -FilePath $file
            } | Should -Throw '*unsupported output*'

            $global:ImagePlaygroundVisualStoryMotionOrderInvoked | Should -BeFalse
            Test-Path -Path $file | Should -BeFalse
        } finally {
            Remove-Variable -Name ImagePlaygroundVisualStoryMotionOrderInvoked -Scope Global -ErrorAction SilentlyContinue
        }
    }

    It 'rejects conflicting motion sources before invoking the story script' {
        $file = Join-Path -Path $TestDir -ChildPath 'conflicting-motion-sources.svg'
        $global:ImagePlaygroundVisualStoryConflictInvoked = $false

        try {
            {
                New-ImageVisualStory -StoryScript {
                    param($Story)
                    $global:ImagePlaygroundVisualStoryConflictInvoked = $true
                    [void] $Story.Add('metric', [ChartForgeX.VisualBlocks.MetricCard]::Create().WithMetric('Ready', 'Yes'))
                } -Motion ([ChartForgeX.Motion.VisualMotionTimeline]::Create()) -MotionDefinition {
                    New-ImageVisualMotionCue -TargetId metric -Effect Fade
                } -FilePath $file
            } | Should -Throw '*either Motion or MotionDefinition*'

            $global:ImagePlaygroundVisualStoryConflictInvoked | Should -BeFalse
            Test-Path -Path $file | Should -BeFalse
        } finally {
            Remove-Variable -Name ImagePlaygroundVisualStoryConflictInvoked -Scope Global -ErrorAction SilentlyContinue
        }
    }
}

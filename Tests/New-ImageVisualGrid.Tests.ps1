Describe 'New-ImageVisualGrid' {
    BeforeAll {
        $env:IMAGEPLAYGROUND_DEVELOPMENT = '1'
        Import-Module -Name "$PSScriptRoot/../ImagePlayground.psd1" -Force
        $TestDir = Join-Path -Path $PSScriptRoot -ChildPath 'Artifacts'
        if (-not (Test-Path -Path $TestDir)) {
            New-Item -Path $TestDir -ItemType Directory | Out-Null
        }
    }

    It 'renders a dashboard from PowerShell-native visual blocks' {
        $file = Join-Path -Path $TestDir -ChildPath 'visual-grid.svg'
        Remove-Item -Path $file -ErrorAction SilentlyContinue

        $grid = New-ImageVisualGrid -Title 'Service health' -Columns 2 -Theme DashboardLight -ContentDefinition {
            New-ImageVisualGridItem -TargetId requests -Block (New-ImageMetricCard -Label Requests -Value 12840 -Trend '+12%' -Status Positive -MiniValues 8, 9, 10, 12)
            New-ImageListBlock -Title Checks -Item API, Database -Status Positive, Warning
            New-ImageTableBlock -Title Services -Column Name, Status -Row @{ Name = 'API'; Status = 'Healthy' }, @{ Name = 'Database'; Status = 'Warning' } -Dense
            New-ImageTimelineBlock -Title Activity -ItemDefinition {
                New-ImageTimelineItem -Kind Event -Title 'Build completed' -Timestamp '14:20' -Status Positive
                New-ImageTimelineItem -Kind ChecklistItem -Title 'Smoke tests' -Completed
            }
        } -FilePath $file -PassThru

        $grid | Should -BeOfType 'ChartForgeX.VisualBlocks.VisualGrid'
        $grid.Items.Count | Should -Be 4
        Test-Path -Path $file | Should -BeTrue
        $svg = Get-Content -Path $file -Raw
        $svg | Should -Match 'Service health'
        $svg | Should -Match 'data-cfx-target="requests"'
        $svg | Should -Match 'Build completed'
    }

    It 'returns an unrendered grid when FilePath is omitted' {
        $grid = New-ImageVisualGrid -Content (New-ImageMetricCard -Label Ready -Value Yes)

        $grid | Should -BeOfType 'ChartForgeX.VisualBlocks.VisualGrid'
        $grid.Items.Count | Should -Be 1
    }

    It 'returns a reusable motion presentation accepted by visual story export' {
        $motion = [ChartForgeX.Motion.VisualMotionTimeline]::Create().Rise('requests')
        $presentation = New-ImageVisualGrid -Content (New-ImageVisualGridItem -TargetId requests -Block (New-ImageMetricCard -Label Requests -Value 12840)) -Motion $motion
        $presentation | Should -BeOfType 'ChartForgeX.Motion.VisualMotionPresentation'
        $presentation.GetType().Assembly.GetName().Name | Should -Be 'ChartForgeX.Stories'
        [ChartForgeX.VisualBlocks.VisualGrid].Assembly.GetName().Name | Should -Be 'ChartForgeX.Visuals'
        $file = Join-Path -Path $TestDir -ChildPath 'motion-grid-story.svg'
        $exported = $presentation | New-ImageVisualStory -FilePath $file -PassThru
        [object]::ReferenceEquals($presentation, $exported) | Should -BeTrue
        [System.IO.File]::ReadAllText($file) | Should -Match 'data-cfx-motion-target="requests"'
    }

    It 'preserves the caller static grid while returning detached motion' {
        $grid = [ChartForgeX.VisualBlocks.VisualGrid]::Create()
        [void] $grid.Add('requests', [ChartForgeX.VisualBlocks.MetricCard]::Create().WithMetric('Requests', '12840'))
        $before = [ChartForgeX.VisualsExtensions]::ToSvg($grid)
        $motion = [ChartForgeX.Motion.VisualMotionTimeline]::Create().Rise('requests')
        $file = Join-Path -Path $TestDir -ChildPath 'detached-motion.html'
        $presentation = $grid | New-ImageVisualStory -Motion $motion -FilePath $file -PassThru
        $presentation | Should -BeOfType 'ChartForgeX.Motion.VisualMotionPresentation'
        [ChartForgeX.VisualsExtensions]::ToSvg($grid) | Should -BeExactly $before
        [void] $motion.Add('later-target', [ChartForgeX.Motion.VisualMotionEffect]::Fade)
        $presentation.ToSvg() | Should -Match 'data-cfx-motion-target="requests"'
        $presentation.ToSvg() | Should -Not -Match 'data-cfx-motion-target="later-target"'
    }

    It 'rejects unsupported output before invoking grid authoring' {
        $script:gridDefinitionInvoked = $false
        {
            New-ImageVisualGrid -ContentDefinition {
                $script:gridDefinitionInvoked = $true
                New-ImageMetricCard -Label CPU -Value '42%'
            } -FilePath (Join-Path -Path $TestDir -ChildPath 'grid.invalid')
        } | Should -Throw
        $script:gridDefinitionInvoked | Should -BeFalse
    }
}

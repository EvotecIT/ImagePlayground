Describe 'New-ImageVisualGrid' {
    BeforeAll {
        $PreviousDevelopment = [Environment]::GetEnvironmentVariable('IMAGEPLAYGROUND_DEVELOPMENT', 'Process')
        $modulePath = if ($env:IMAGEPLAYGROUND_TEST_MODULE_PATH) {
            $env:IMAGEPLAYGROUND_TEST_MODULE_PATH
        } else {
            $env:IMAGEPLAYGROUND_DEVELOPMENT = '1'
            Join-Path $PSScriptRoot '../ImagePlayground.psd1'
        }
        Import-Module -Name $modulePath -Force -ErrorAction Stop
        $TestDir = Join-Path -Path $TestDrive -ChildPath 'Artifacts'
        if (-not (Test-Path -Path $TestDir)) {
            New-Item -Path $TestDir -ItemType Directory | Out-Null
        }
    }

    AfterAll {
        [Environment]::SetEnvironmentVariable('IMAGEPLAYGROUND_DEVELOPMENT', $PreviousDevelopment, 'Process')
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

    It 'uses one canonical theme for the grid and authored factual blocks' -TestCases @(
        @{ Theme = 'Light' }
        @{ Theme = 'Dark' }
    ) {
        param($Theme)
        $blocks = @(
            New-ImageMetricCard -Label Requests -Value 12840 -Theme $Theme
            New-ImageListBlock -Title Checks -Item API, Database -Status Positive, Warning -Theme $Theme
            New-ImageTableBlock -Title Services -Column Name, Status -Row @{ Name = 'API'; Status = 'Healthy' } -Theme $Theme
            New-ImageTimelineBlock -Title Activity -Theme $Theme -ItemDefinition {
                New-ImageTimelineItem -Kind Event -Title 'Build completed' -Status Positive
            }
        )
        $grid = New-ImageVisualGrid -Content $blocks -Theme $Theme -Columns 1
        $mode = [ChartForgeX.Themes.VisualThemeMode] $Theme
        foreach ($block in $blocks) {
            $block.Options.ThemeMode | Should -Be $mode
            $grid.ThemeMode | Should -Be $mode
            $block.Options.Theme.Resolve($mode).Foreground.ToCss() | Should -BeExactly $grid.Theme.Resolve($mode).Foreground.ToCss()
            $block.Options.Theme.Typography.Family | Should -BeExactly $grid.Theme.Typography.Family
            $block.Options.Theme.Resolve($mode).Positive.ToCss() | Should -BeExactly $grid.Theme.Resolve($mode).Positive.ToCss()
        }
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
        $beforePath = Join-Path -Path $TestDir -ChildPath 'static-grid-before.svg'
        $afterPath = Join-Path -Path $TestDir -ChildPath 'static-grid-after.svg'
        $grid | New-ImageVisualStory -FilePath $beforePath
        $before = [System.IO.File]::ReadAllText($beforePath)
        $motion = [ChartForgeX.Motion.VisualMotionTimeline]::Create().Rise('requests')
        $file = Join-Path -Path $TestDir -ChildPath 'detached-motion.html'
        $presentation = $grid | New-ImageVisualStory -Motion $motion -FilePath $file -PassThru
        $presentation | Should -BeOfType 'ChartForgeX.Motion.VisualMotionPresentation'
        $grid | New-ImageVisualStory -FilePath $afterPath
        [System.IO.File]::ReadAllText($afterPath) | Should -BeExactly $before
        [void] $motion.Add('later-target', [ChartForgeX.Motion.VisualMotionEffect]::Fade)
        $presentation.ToSvg() | Should -Match 'data-cfx-motion-target="requests"'
        $presentation.ToSvg() | Should -Not -Match 'data-cfx-motion-target="later-target"'
    }

    It 'saves completed static pixels for motion output as <Extension>' -TestCases @(
        @{ Extension = 'ppm'; Signature = 'UDY=' }
        @{ Extension = 'png'; Signature = 'iVA=' }
        @{ Extension = 'tiff'; Signature = 'SUk=' }
    ) {
        param($Extension, $Signature)
        $motion = [ChartForgeX.Motion.VisualMotionTimeline]::Create().Rise('requests')
        $content = New-ImageVisualGridItem -TargetId requests -Block (New-ImageMetricCard -Label Requests -Value 12840)
        $staticPath = Join-Path -Path $TestDir -ChildPath ('static-grid.' + $Extension)
        $motionPath = Join-Path -Path $TestDir -ChildPath ('motion-grid.' + $Extension)

        New-ImageVisualGrid -Content $content -FilePath $staticPath
        $presentation = New-ImageVisualGrid -Content $content -Motion $motion -FilePath $motionPath -PassThru

        $presentation | Should -BeOfType 'ChartForgeX.Motion.VisualMotionPresentation'
        $bytes = [System.IO.File]::ReadAllBytes($motionPath)
        $bytes.Length | Should -BeGreaterThan 2
        [Convert]::ToBase64String([byte[]] $bytes[0..1]) | Should -Be $Signature
        [Convert]::ToBase64String($bytes) | Should -BeExactly ([Convert]::ToBase64String([System.IO.File]::ReadAllBytes($staticPath)))
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

    It 'resizes content to -PanelWidth when -PanelHeight is omitted' {
        $file = Join-Path -Path $TestDir -ChildPath 'grid-panel-width.png'
        $cards = 1..3 | ForEach-Object { New-ImageMetricCard -Label "Card $_" -Value $_ -Width 320 -Height 176 }

        $cards | New-ImageVisualGrid -Columns 3 -PanelWidth 240 -Gap 12 -Padding 12 -FilePath $file

        $cards[0].Options.Size.Width | Should -Be 240
        $cards[0].Options.Size.Height | Should -Be 132
        $image = Get-Image -FilePath $file
        try {
            $image.Width | Should -Be (3 * 240 + 2 * 12 + 2 * 12)
        } finally {
            $image.Dispose()
        }
    }

    It 'still requires -PanelWidth with -PanelHeight' {
        { New-ImageVisualGrid -PanelHeight 200 -Content (New-ImageMetricCard -Label A -Value 1) -ErrorAction Stop } | Should -Throw '*PanelHeight requires PanelWidth*'
    }

    It 'writes <Extension> output and converts to a visual artifact' -TestCases @(
        @{ Extension = 'webp' }
        @{ Extension = 'jpg' }
    ) {
        param($Extension)
        $file = Join-Path -Path $TestDir -ChildPath "grid-encoded.$Extension"
        $grid = New-ImageVisualGrid -Title 'Service health' -Columns 1 -Content (New-ImageListBlock -Title Checks -Item Backups -Status Positive) -FilePath $file -PassThru

        Test-Path -Path $file | Should -BeTrue
        $artifact = $grid | ConvertTo-ImageVisualArtifact -Id service-health
        $artifact.Id | Should -Be 'service-health'
        $artifact.Title | Should -Be 'Service health'
        $artifact.NaturalSize.Width | Should -BeGreaterThan 0
        $artifact.OfficeVisualSvg.Length | Should -BeGreaterThan 0
        $png = Join-Path -Path $TestDir -ChildPath "grid-artifact-$Extension.png"
        $artifact | Export-ImageVisualArtifact -FilePath $png
        Test-Path -Path $png | Should -BeTrue
    }
}

Describe 'Image story output paths' {
    BeforeAll {
        $env:IMAGEPLAYGROUND_DEVELOPMENT = '1'
        Import-Module -Name "$PSScriptRoot/../ImagePlayground.psd1" -Force
    }

    It 'resolves relative console story paths through the current FileSystem provider location' {
        Push-Location -Path $TestDrive
        try {
            New-ImageConsoleStory -Content {
                New-ImageConsoleStoryOutput -Text 'ready'
            } -FilePath '.\provider-console.svg'
        } finally {
            Pop-Location
        }

        Test-Path -LiteralPath (Join-Path -Path $TestDrive -ChildPath 'provider-console.svg') | Should -BeTrue
        if ($PSVersionTable.Platform -eq 'Unix') {
            [System.IO.File]::Exists([System.IO.Path]::Combine($TestDrive, '.\provider-console.svg')) | Should -BeFalse
        }
    }

    It 'resolves relative generic story and bundle paths through the current FileSystem provider location' {
        $panel = New-ImageStoryPanel -Id result -Text 'ready' -Emphasized
        $scene = New-ImageStoryScene -Id complete -Title Complete -Panels $panel
        $outcome = New-ImageStoryOutcome -Id ready -Label 'Ready is visible.' -PanelId result

        Push-Location -Path $TestDrive
        try {
            New-ImageStory -Title 'Provider path' -Scenes $scene -Outcomes $outcome `
                -FilePath '.\provider-story.svg' -BundlePath '.\provider-bundle' -BundleFormats Transcript
        } finally {
            Pop-Location
        }

        Test-Path -LiteralPath (Join-Path -Path $TestDrive -ChildPath 'provider-story.svg') | Should -BeTrue
        Test-Path -LiteralPath (Join-Path -Path $TestDrive -ChildPath 'provider-bundle/story.json') | Should -BeTrue
    }

    It 'resolves relative visual story paths through the current FileSystem provider location' {
        $grid = [ChartForgeX.VisualBlocks.VisualGrid]::Create().WithTitle('Provider path')
        [void] $grid.Add('metric', [ChartForgeX.VisualBlocks.MetricCard]::Create().WithMetric('Ready', 'Yes'))

        Push-Location -Path $TestDrive
        try {
            $grid | New-ImageVisualStory -FilePath '.\provider-visual.svg'
        } finally {
            Pop-Location
        }

        Test-Path -LiteralPath (Join-Path -Path $TestDrive -ChildPath 'provider-visual.svg') | Should -BeTrue
    }

    It 'rejects non-FileSystem providers for story output' {
        {
            New-ImageConsoleStory -Content {
                New-ImageConsoleStoryOutput -Text 'ready'
            } -FilePath 'Variable:\provider-console.svg'
        } | Should -Throw '*FileSystem provider*'
    }
}

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
        Test-Path -LiteralPath (Join-Path -Path $TestDrive -ChildPath 'provider-bundle/provider-story.png') | Should -BeTrue
    }

    It 'resolves relative media inputs through the current FileSystem provider location' {
        Copy-Item -LiteralPath "$PSScriptRoot/../Examples/Samples/ChartsBar.png" `
            -Destination (Join-Path -Path $TestDrive -ChildPath 'provider-media.png')

        Push-Location -Path $TestDrive
        try {
            $panel = New-ImageStoryPanel -Id result -MediaPath '.\provider-media.png' -AccessibleText 'Generated chart'
        } finally {
            Pop-Location
        }

        $panel.Surface.Kind.ToString() | Should -Be 'Media'
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
        $marker = Join-Path -Path $TestDrive -ChildPath 'console-story-content-ran.txt'
        $content = {
            [System.IO.File]::WriteAllText($marker, 'executed')
            New-ImageConsoleStoryOutput -Text 'ready'
        }.GetNewClosure()

        {
            New-ImageConsoleStory -Content $content -FilePath 'Variable:\provider-console.svg'
        } | Should -Throw '*FileSystem provider*'
        Test-Path -LiteralPath $marker | Should -BeFalse
    }

    It 'validates console story options before invoking Content' {
        $marker = Join-Path -Path $TestDrive -ChildPath 'invalid-console-content-ran.txt'
        $content = {
            [System.IO.File]::WriteAllText($marker, 'executed')
            New-ImageConsoleStoryOutput -Text 'ready'
        }.GetNewClosure()

        { New-ImageConsoleStory -Content $content -FilePath (Join-Path -Path $TestDrive -ChildPath 'story.txt') } |
            Should -Throw '*supports only*'
        Test-Path -LiteralPath $marker | Should -BeFalse

        { New-ImageConsoleStory -Content $content -Show } | Should -Throw '*requires -FilePath*'
        Test-Path -LiteralPath $marker | Should -BeFalse
    }

    It 'validates the generic story bundle before overwriting the primary output' {
        $output = Join-Path -Path $TestDrive -ChildPath 'existing-story.svg'
        [System.IO.File]::WriteAllText($output, 'existing output')
        $panel = New-ImageStoryPanel -Id result -Text 'ready' -Emphasized
        $scene = New-ImageStoryScene -Id complete -Title Complete -Panels $panel
        $outcome = New-ImageStoryOutcome -Id ready -Label 'Ready is visible.' -PanelId result

        {
            New-ImageStory -Title 'Provider path' -Scenes $scene -Outcomes $outcome `
                -FilePath $output -BundlePath 'Variable:\provider-bundle' -BundleFormats Transcript
        } | Should -Throw '*FileSystem provider*'
        Get-Content -LiteralPath $output -Raw | Should -BeExactly 'existing output'
    }
}

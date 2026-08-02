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

    It 'rejects an existing file as the generic story bundle before overwriting the primary output' {
        $output = Join-Path -Path $TestDrive -ChildPath 'existing-file-bundle-story.svg'
        $bundleFile = Join-Path -Path $TestDrive -ChildPath 'bundle-target'
        [System.IO.File]::WriteAllText($output, 'existing output')
        [System.IO.File]::WriteAllText($bundleFile, 'existing bundle file')
        $panel = New-ImageStoryPanel -Id result -Text 'ready' -Emphasized
        $scene = New-ImageStoryScene -Id complete -Title Complete -Panels $panel
        $outcome = New-ImageStoryOutcome -Id ready -Label 'Ready is visible.' -PanelId result

        {
            New-ImageStory -Title 'Provider path' -Scenes $scene -Outcomes $outcome `
                -FilePath $output -BundlePath $bundleFile -BundleFormats Transcript
        } | Should -Throw '*must resolve to a directory*'
        Get-Content -LiteralPath $output -Raw | Should -BeExactly 'existing output'
        Get-Content -LiteralPath $bundleFile -Raw | Should -BeExactly 'existing bundle file'
    }

    It 'rejects directory-valued bundle artifacts before overwriting the primary output' {
        $output = Join-Path -Path $TestDrive -ChildPath 'existing-artifact-story.svg'
        $bundle = Join-Path -Path $TestDrive -ChildPath 'artifact-bundle'
        $artifactDirectory = Join-Path -Path $bundle -ChildPath 'existing-artifact-story.png'
        [System.IO.File]::WriteAllText($output, 'existing output')
        New-Item -Path $artifactDirectory -ItemType Directory -Force | Out-Null
        $panel = New-ImageStoryPanel -Id result -Text 'ready' -Emphasized
        $scene = New-ImageStoryScene -Id complete -Title Complete -Panels $panel
        $outcome = New-ImageStoryOutcome -Id ready -Label 'Ready is visible.' -PanelId result

        {
            New-ImageStory -Title 'Invalid artifact destination' -Scenes $scene -Outcomes $outcome `
                -FilePath $output -BundlePath $bundle -BundleFormats Transcript
        } | Should -Throw '*artifact destination must resolve to a file*'
        Get-Content -LiteralPath $output -Raw | Should -BeExactly 'existing output'
        (Get-Item -LiteralPath $artifactDirectory).PSIsContainer | Should -BeTrue
    }

    It 'rejects colliding generic story output and bundle paths before writing either artifact' {
        $output = Join-Path -Path $TestDrive -ChildPath 'colliding-story.svg'
        $panel = New-ImageStoryPanel -Id result -Text 'ready' -Emphasized
        $scene = New-ImageStoryScene -Id complete -Title Complete -Panels $panel
        $outcome = New-ImageStoryOutcome -Id ready -Label 'Ready is visible.' -PanelId result

        {
            New-ImageStory -Title 'Provider path' -Scenes $scene -Outcomes $outcome `
                -FilePath $output -BundlePath $output -BundleFormats Transcript
        } | Should -Throw '*must not resolve to the same path*'
        Test-Path -LiteralPath $output | Should -BeFalse
    }

    It 'rejects a generic story bundle nested below the output file before overwriting it' {
        $output = Join-Path -Path $TestDrive -ChildPath 'parent-story.svg'
        $bundle = Join-Path -Path $output -ChildPath 'bundle'
        [System.IO.File]::WriteAllText($output, 'existing output')
        $panel = New-ImageStoryPanel -Id result -Text 'ready' -Emphasized
        $scene = New-ImageStoryScene -Id complete -Title Complete -Panels $panel
        $outcome = New-ImageStoryOutcome -Id ready -Label 'Ready is visible.' -PanelId result

        {
            New-ImageStory -Title 'Nested bundle path' -Scenes $scene -Outcomes $outcome `
                -FilePath $output -BundlePath $bundle -BundleFormats Transcript
        } | Should -Throw '*nested beneath*'
        Get-Content -LiteralPath $output -Raw | Should -BeExactly 'existing output'
    }

    It 'rejects bundle collisions reached through a symbolic directory alias before overwriting output' {
        $realRoot = Join-Path -Path $TestDrive -ChildPath 'real-story-root'
        $aliasRoot = Join-Path -Path $TestDrive -ChildPath 'story-alias'
        New-Item -ItemType Directory -Path $realRoot -Force | Out-Null
        try {
            if ($IsWindows -or $env:OS -eq 'Windows_NT') {
                New-Item -ItemType Junction -Path $aliasRoot -Target $realRoot -ErrorAction Stop | Out-Null
            } else {
                New-Item -ItemType SymbolicLink -Path $aliasRoot -Target $realRoot -ErrorAction Stop | Out-Null
            }
        } catch {
            Set-ItResult -Skipped -Because "Symbolic link creation is unavailable: $($_.Exception.Message)"
            return
        }

        $output = Join-Path -Path $aliasRoot -ChildPath 'story.svg'
        $bundle = Join-Path -Path $realRoot -ChildPath 'story.svg\bundle'
        [System.IO.File]::WriteAllText($output, 'existing output')
        $panel = New-ImageStoryPanel -Id result -Text 'ready' -Emphasized
        $scene = New-ImageStoryScene -Id complete -Title Complete -Panels $panel
        $outcome = New-ImageStoryOutcome -Id ready -Label 'Ready is visible.' -PanelId result
        $parameters = @{
            Title         = 'Aliased bundle path'
            Scenes        = $scene
            Outcomes      = $outcome
            FilePath      = $output
            BundlePath    = $bundle
            BundleFormats = 'Transcript'
        }

        { New-ImageStory @parameters } | Should -Throw '*nested beneath*'
        Get-Content -LiteralPath $output -Raw | Should -BeExactly 'existing output'
    }

    It 'uses the target volume case sensitivity when comparing output and bundle paths' {
        $probe = Join-Path -Path $TestDrive -ChildPath 'case-probe'
        New-Item -ItemType Directory -Path $probe | Out-Null
        $lower = Join-Path -Path $probe -ChildPath 'marker'
        Set-Content -LiteralPath $lower -Value 'probe'
        $caseInsensitive = Test-Path -LiteralPath (Join-Path -Path $probe -ChildPath 'MARKER')
        Remove-Item -LiteralPath $lower

        $output = Join-Path -Path $TestDrive -ChildPath 'Story.svg'
        $bundle = Join-Path -Path $TestDrive -ChildPath 'story.svg'
        $panel = New-ImageStoryPanel -Id result -Text 'ready' -Emphasized
        $scene = New-ImageStoryScene -Id complete -Title Complete -Panels $panel
        $outcome = New-ImageStoryOutcome -Id ready -Label 'Ready is visible.' -PanelId result
        $parameters = @{
            Title         = 'Case-sensitive paths'
            Scenes        = $scene
            Outcomes      = $outcome
            FilePath      = $output
            BundlePath    = $bundle
            BundleFormats = 'Transcript'
        }

        if ($caseInsensitive) {
            { New-ImageStory @parameters } | Should -Throw '*must not resolve to the same path*'
            Test-Path -LiteralPath $output | Should -BeFalse
        } else {
            New-ImageStory @parameters
            Test-Path -LiteralPath $output | Should -BeTrue
            Test-Path -LiteralPath (Join-Path -Path $bundle -ChildPath 'story.json') | Should -BeTrue
        }
    }

    It 'rejects a directory visual story output before invoking authoring blocks' {
        $output = Join-Path -Path $TestDrive -ChildPath 'directory-visual.svg'
        $storyMarker = Join-Path -Path $TestDrive -ChildPath 'directory-visual-story-ran.txt'
        $motionMarker = Join-Path -Path $TestDrive -ChildPath 'directory-visual-motion-ran.txt'
        $null = New-Item -Path $output -ItemType Directory
        $storyScript = {
            [System.IO.File]::WriteAllText($storyMarker, 'executed')
        }.GetNewClosure()
        $motionDefinition = {
            [System.IO.File]::WriteAllText($motionMarker, 'executed')
        }.GetNewClosure()

        {
            New-ImageVisualStory -StoryScript $storyScript -MotionDefinition $motionDefinition -FilePath $output
        } | Should -Throw '*must resolve to a file*'
        Test-Path -LiteralPath $storyMarker | Should -BeFalse
        Test-Path -LiteralPath $motionMarker | Should -BeFalse
        (Get-Item -LiteralPath $output).PSIsContainer | Should -BeTrue
    }

    It 'rejects a directory console output before invoking Content' {
        $output = Join-Path -Path $TestDrive -ChildPath 'directory-output.svg'
        $marker = Join-Path -Path $TestDrive -ChildPath 'directory-output-content-ran.txt'
        $null = New-Item -Path $output -ItemType Directory
        $content = {
            [System.IO.File]::WriteAllText($marker, 'executed')
            New-ImageConsoleStoryOutput -Text 'ready'
        }.GetNewClosure()

        { New-ImageConsoleStory -Content $content -FilePath $output } |
            Should -Throw '*must resolve to a file*'
        Test-Path -LiteralPath $marker | Should -BeFalse
        (Get-Item -LiteralPath $output).PSIsContainer | Should -BeTrue
    }
}

Describe 'Image console story step cmdlets' {
    BeforeAll {
        $env:IMAGEPLAYGROUND_DEVELOPMENT = '1'
        Import-Module -Name "$PSScriptRoot/../ImagePlayground.psd1" -Force
        $TestDir = Join-Path -Path $PSScriptRoot -ChildPath 'Artifacts'
        if (-not (Test-Path -Path $TestDir)) {
            New-Item -Path $TestDir -ItemType Directory | Out-Null
        }
    }

    It 'composes a PowerShell-native story without exposing fluent builders' {
        $file = Join-Path -Path $TestDir -ChildPath 'console-story-content.svg'
        $projects = @(
            [pscustomobject]@{ Name = 'ChartForgeX'; Stack = '.NET'; Stars = 1200 }
            [pscustomobject]@{ Name = 'ImagePlayground'; Stack = 'PowerShell'; Stars = 500 }
        )

        $story = New-ImageConsoleStory `
            -Title 'pwsh - C:\OpenSource' `
            -WorkingDirectory 'C:\OpenSource' `
            -Theme PowerShell `
            -WindowStyle WindowsTerminal `
            -Content {
                New-ImageConsoleStoryCommand -Text 'Get-ActivePortfolio'
                $projects | New-ImageConsoleStoryTable `
                    -Property Name, Stack, Stars `
                    -Header PROJECT, STACK, STARS `
                    -Align @{ Stars = 'Right' }
                New-ImageConsoleStoryBlankLine
                New-ImageConsoleStoryOutput -Text 'PASS  all checks' -Tone Success
            } `
            -FilePath $file `
            -PassThru

        $story | Should -BeOfType 'ChartForgeX.Terminal.TerminalStory'
        $story.Title | Should -Be 'pwsh - C:\OpenSource'
        $story.WindowStyle.ToString() | Should -Be 'WindowsTerminal'
        $story.Steps.Count | Should -Be 4
        $story.Steps[0].Kind.ToString() | Should -Be 'Command'
        $story.Steps[1].Kind.ToString() | Should -Be 'Table'
        $story.Steps[1].Table.Columns | Should -Be @('PROJECT', 'STACK', 'STARS')
        $story.Steps[1].Table.Rows[0] | Should -Be @('ChartForgeX', '.NET', '1200')
        $story.Steps[1].Table.Alignments[2].ToString() | Should -Be 'Right'
        $story.Steps[3].Tone.ToString() | Should -Be 'Success'

        $svg = [System.IO.File]::ReadAllText($file)
        $svg | Should -Match 'Get-ActivePortfolio'
        $svg | Should -Match 'ChartForgeX'
        $svg | Should -Match 'PASS  all checks'
        $svg | Should -Match 'data-cfx-window-style="WindowsTerminal"'
        $svg | Should -Match 'data-cfx-role="terminal-tab"'
        $svg | Should -Not -Match 'data-cfx-role="terminal-macos-controls"'
    }

    It 'keeps dialect, color palette, and window chrome independent' {
        foreach ($style in 'MacOS', 'WindowsTerminal', 'Minimal', 'None') {
            $file = Join-Path -Path $TestDir -ChildPath "console-story-$($style.ToLowerInvariant()).svg"
            $story = New-ImageConsoleStory `
                -Dialect PowerShell `
                -Theme Dark `
                -WindowStyle $style `
                -Content {
                    New-ImageConsoleStoryCommand -Text 'Get-Date'
                    New-ImageConsoleStoryOutput -Text 'Ready' -Tone Success
                } `
                -FilePath $file `
                -PassThru

            $story.Dialect.ToString() | Should -Be 'PowerShell'
            $story.WindowStyle.ToString() | Should -Be $style
            [System.IO.File]::ReadAllText($file) | Should -Match "data-cfx-window-style=`"$style`""
        }
    }

    It 'authors persistent profile tabs and switches back without losing buffers' {
        $file = Join-Path -Path $TestDir -ChildPath 'console-story-tabs.svg'
        $ubuntuPalette = New-ImageConsoleStoryPalette -Preset Ubuntu -Background '#24071B' -Accent '#FF6A2B'

        $story = New-ImageConsoleStory `
            -Title 'PowerShell' `
            -WorkingDirectory 'C:\Work' `
            -Theme Campbell `
            -WindowStyle WindowsTerminal `
            -Content {
                New-ImageConsoleStoryCommand -Text 'Get-ChildItem'
                New-ImageConsoleStoryTab -Id windows-powershell -Profile WindowsPowerShell -WorkingDirectory 'C:\Legacy'
                New-ImageConsoleStoryCommand -Text '$PSVersionTable.PSVersion'
                New-ImageConsoleStoryTab -Id ubuntu -Profile Ubuntu -WorkingDirectory '~/src' -Palette $ubuntuPalette
                New-ImageConsoleStoryCommand -Text 'dotnet test'
                Select-ImageConsoleStoryTab -Id main
                New-ImageConsoleStoryOutput -Text 'Back in PowerShell' -Tone Success
            } `
            -FilePath $file `
            -PassThru

        $story.Tabs.Count | Should -Be 3
        $story.ActiveTabId | Should -Be 'main'
        $story.Tabs[1].Title | Should -Be 'Windows PowerShell'
        $story.Tabs[2].Dialect.ToString() | Should -Be 'Bash'
        $story.Tabs[2].Theme.Background.ToCss() | Should -Be '#24071B'
        $story.Steps.Where({ $_.Kind.ToString() -eq 'OpenTab' }).Count | Should -Be 2
        $story.Steps.Where({ $_.Kind.ToString() -eq 'SelectTab' }).Count | Should -Be 1

        $svg = [System.IO.File]::ReadAllText($file)
        $svg | Should -Match 'data-cfx-tab="windows-powershell"'
        $svg | Should -Match 'data-cfx-tab="ubuntu"'
        $svg | Should -Match '#24071B'
        $svg | Should -Match '\[Ubuntu\] ~/src \$ dotnet test'
        $svg | Should -Match 'cfx-terminal-tab-final'
    }

    It 'accepts a reusable array of typed steps' {
        $file = Join-Path -Path $TestDir -ChildPath 'console-story-steps.svg'
        $steps = @(
            New-ImageConsoleStoryCommand -Text 'Get-Date'
            New-ImageConsoleStoryPause -Seconds 0.2
            New-ImageConsoleStoryOutput -Text 'Ready' -Tone Accent
        )

        $story = New-ImageConsoleStory -Step $steps -FilePath $file -PassThru

        $steps | Should -BeOfType 'ImagePlayground.PowerShell.ImageConsoleStoryStep'
        $story.Steps.Count | Should -Be 3
        $story.Steps[1].DurationSeconds | Should -Be 0.2
    }

    It 'accepts typed steps directly from the pipeline' {
        $file = Join-Path -Path $TestDir -ChildPath 'console-story-step-pipeline.svg'
        $story = @(
            New-ImageConsoleStoryCommand -Text 'Get-Date'
            New-ImageConsoleStoryOutput -Text 'Ready' -Tone Success
        ) | New-ImageConsoleStory -FilePath $file -PassThru

        $story.Steps.Count | Should -Be 2
        $story.Steps[0].Kind.ToString() | Should -Be 'Command'
        $story.Steps[1].Tone.ToString() | Should -Be 'Success'
        Test-Path -LiteralPath $file | Should -BeTrue
    }

    It 'renders SVG and GIF from the same composed story' {
        $svgFile = Join-Path -Path $TestDir -ChildPath 'console-story-steps-render.svg'
        $gifFile = Join-Path -Path $TestDir -ChildPath 'console-story-steps-render.gif'
        $story = New-ImageConsoleStory -Content {
            New-ImageConsoleStoryCommand -Text '.\Invoke-EnvironmentAudit.ps1' -DurationSeconds 0.05
            New-ImageConsoleStoryOutput -Text 'PASS  DNS' -Tone Success
        } -FilePath $svgFile -PassThru

        $story | New-ImageConsoleStory `
            -FilePath $gifFile `
            -FramesPerSecond 4 `
            -EndHoldSeconds 0.1 `
            -NoLoop

        [System.IO.File]::ReadAllText($svgFile) | Should -Match 'PASS  DNS'
        $bytes = [System.IO.File]::ReadAllBytes($gifFile)
        [System.Text.Encoding]::ASCII.GetString($bytes, 0, 6) | Should -Be 'GIF89a'
    }

    It 'rejects unrelated output from a Content block' {
        {
            New-ImageConsoleStory -Content {
                New-ImageConsoleStoryCommand -Text 'Get-Date'
                'unexpected output'
            } -FilePath (Join-Path -Path $TestDir -ChildPath 'invalid-content.svg')
        } | Should -Throw '*accepts only steps*'
    }

    It 'reports missing table properties clearly' {
        {
            [pscustomobject]@{ Name = 'ChartForgeX' } |
                New-ImageConsoleStoryTable -Property Name, Status
        } | Should -Throw "*does not contain property 'Status'*"
    }
}

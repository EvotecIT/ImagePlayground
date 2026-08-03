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

        $storyOptions = @{
            Title            = 'pwsh - C:\OpenSource'
            WorkingDirectory = 'C:\OpenSource'
            Theme            = 'PowerShell'
            WindowStyle      = 'WindowsTerminal'
            Content          = {
                New-ImageConsoleStoryCommand -Text 'Get-ActivePortfolio'
                $projects | New-ImageConsoleStoryTable -Property Name, Stack, Stars -Header PROJECT, STACK, STARS -Align @{ Stars = 'Right' }
                New-ImageConsoleStoryBlankLine
                New-ImageConsoleStoryOutput -Text 'PASS  all checks' -Style Success
            }
        }

        $story = New-ImageConsoleStory @storyOptions
        $story | Export-ImageConsoleStory -Path $file

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
            $storyOptions = @{
                Dialect     = 'PowerShell'
                Theme       = 'Dark'
                WindowStyle = $style
                Content     = {
                    New-ImageConsoleStoryCommand -Text 'Get-Date'
                    New-ImageConsoleStoryOutput -Text 'Ready' -Style Success
                }
            }
            $story = New-ImageConsoleStory @storyOptions
            $story | Export-ImageConsoleStory -Path $file

            $story.Dialect.ToString() | Should -Be 'PowerShell'
            $story.WindowStyle.ToString() | Should -Be $style
            [System.IO.File]::ReadAllText($file) | Should -Match "data-cfx-window-style=`"$style`""
        }
    }

    It 'opens new profile tabs atomically and switches back without losing buffers' {
        $file = Join-Path -Path $TestDir -ChildPath 'console-story-tabs.svg'
        $ubuntuPalette = New-ImageConsoleStoryPalette -Preset Ubuntu -Background '#24071B' -Accent '#FF6A2B'

        $story = New-ImageConsoleStory -WindowStyle WindowsTerminal -Speed Slow -Content {
                New-ImageConsoleStoryTab -Id PowerShell -Title 'PowerShell' -Profile PowerShell -Active
                New-ImageConsoleStoryCommand -Text 'Get-ChildItem'
                New-ImageConsoleStoryTab -Id WindowsPowerShell -Title 'Windows PowerShell' -Profile WindowsPowerShell -WorkingDirectory 'C:\Legacy'
                New-ImageConsoleStoryCommand -Text '$PSVersionTable.PSVersion'
                New-ImageConsoleStoryTab -Id Ubuntu -Title 'Ubuntu' -Profile Ubuntu -WorkingDirectory '~/src' -Palette $ubuntuPalette
                New-ImageConsoleStoryCommand -Text 'dotnet test'
                Select-ImageConsoleStoryTab -Id PowerShell
                New-ImageConsoleStoryOutput -Text 'Back in PowerShell' -Style Success
            }
        $story | Export-ImageConsoleStory -Path $file

        $story.Tabs.Count | Should -Be 3
        $story.ActiveTabId | Should -Be 'PowerShell'
        $story.TabHoldSeconds | Should -Be 2
        $story.Tabs[1].Title | Should -Be 'Windows PowerShell'
        $story.Tabs[2].Dialect.ToString() | Should -Be 'Bash'
        $story.Tabs[2].Theme.Background.ToCss() | Should -Be '#24071B'
        $story.Steps.Where({ $_.Kind.ToString() -eq 'OpenTab' }).Count | Should -Be 2
        $story.Steps.Where({ $_.Kind.ToString() -eq 'DeclareTab' }).Count | Should -Be 0
        $story.Steps.Where({ $_.Kind.ToString() -eq 'SelectTab' }).Count | Should -Be 1

        $svg = [System.IO.File]::ReadAllText($file)
        $svg | Should -Match 'data-cfx-tab="WindowsPowerShell"'
        $svg | Should -Match 'data-cfx-tab="Ubuntu"'
        $svg | Should -Match '#24071B'
        $svg | Should -Match '\[Ubuntu\] ~/src \$ dotnet test'
        $svg | Should -Match 'cfx-terminal-tab-final'
    }

    It 'prepares background tabs and makes every jump intentional' {
        $story = New-ImageConsoleStory -WindowStyle WindowsTerminal -Speed Slow -Content {
            New-ImageConsoleStoryTab -Id PowerShell -Profile PowerShell -Active
            New-ImageConsoleStoryTab -Id Logs -Title 'Build logs' -Profile PowerShell -Background
            New-ImageConsoleStoryCommand -Text 'dotnet build'
            New-ImageConsoleStoryOutput -Text 'Build succeeded.' -Style Success
            Select-ImageConsoleStoryTab -Id Logs
            New-ImageConsoleStoryOutput -Text 'Waiting for integration tests...' -Style Muted
            New-ImageConsoleStoryPause -Seconds 1.5
            Select-ImageConsoleStoryTab -Id PowerShell
            New-ImageConsoleStoryCommand -Text 'Get-ChildItem .\artifacts'
        }

        $story.ActiveTabId | Should -Be 'PowerShell'
        $story.Steps.Where({ $_.Kind.ToString() -eq 'DeclareTab' }).Count | Should -Be 1
        $story.Steps.Where({ $_.Kind.ToString() -eq 'SelectTab' }).Count | Should -Be 2
        $story.Steps.Where({ $_.Kind.ToString() -eq 'Pause' }).Count | Should -Be 1
        $story.Tabs.Where({ $_.Id -eq 'PowerShell' })[0].Id | Should -Be 'PowerShell'
        $story.Tabs.Where({ $_.Id -eq 'Logs' })[0].Title | Should -Be 'Build logs'
    }

    It 'accepts a reusable array of typed steps' {
        $file = Join-Path -Path $TestDir -ChildPath 'console-story-steps.svg'
        $steps = @(
            New-ImageConsoleStoryCommand -Text 'Get-Date'
            New-ImageConsoleStoryPause -Seconds 0.2
            New-ImageConsoleStoryOutput -Text 'Ready' -Style Accent
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
            New-ImageConsoleStoryOutput -Text 'Ready' -Style Success
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
            New-ImageConsoleStoryOutput -Text 'PASS  DNS' -Style Success
        }

        $story | Export-ImageConsoleStory -Path $svgFile
        $story | Export-ImageConsoleStory -Path $gifFile -FramesPerSecond 4 -EndHoldSeconds 0.1 -NoLoop

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

    It 'keeps Tone as a compatibility alias for Style' {
        $step = New-ImageConsoleStoryOutput -Text 'Ready' -Tone Success

        $step.Tone.ToString() | Should -Be 'Success'
        (Get-Command New-ImageConsoleStoryOutput).Parameters['Style'].Aliases | Should -Contain 'Tone'
    }

    It 'supports overall pacing with independent typing and tab dwell overrides' {
        $slow = New-ImageConsoleStory -Speed Slow -Content {
            New-ImageConsoleStoryCommand -Text 'Get-Date'
        }
        $custom = New-ImageConsoleStory -Speed Slow -TypingSpeed 36 -TabHoldSeconds 2.5 -Content {
            New-ImageConsoleStoryCommand -Text 'Get-Date'
        }

        $slow.CharactersPerSecond | Should -Be 28
        $slow.TabHoldSeconds | Should -Be 2
        $custom.CharactersPerSecond | Should -Be 36
        $custom.TabHoldSeconds | Should -Be 2.5
        (Get-Command New-ImageConsoleStory).Parameters['TypingSpeed'].Aliases | Should -Contain 'CharactersPerSecond'
    }

    It 'requires exactly one initial Active tab at the beginning of Content' {
        {
            New-ImageConsoleStory -Content {
                New-ImageConsoleStoryCommand -Text 'Get-Date'
                New-ImageConsoleStoryTab -Id PowerShell -Profile PowerShell -Active
            }
        } | Should -Throw '*first content step*'

        {
            New-ImageConsoleStory -Content {
                New-ImageConsoleStoryTab -Id PowerShell -Profile PowerShell -Active
                New-ImageConsoleStoryTab -Id Legacy -Profile WindowsPowerShell -Active
            }
        } | Should -Throw '*one -Active tab*'

        {
            New-ImageConsoleStoryTab -Id Invalid -Profile PowerShell -Active -Background
        } | Should -Throw
    }

    It 'ships parseable console story examples without continuation backticks' {
        $continuation = [regex]::Escape([string][char]96) + '\s*(?:\r?\n|$)'
        foreach ($example in Get-ChildItem -Path (Join-Path -Path $PSScriptRoot -ChildPath '..\Examples') -Filter 'ConsoleStory*.ps1') {
            $tokens = $null
            $errors = $null
            [void] [System.Management.Automation.Language.Parser]::ParseFile($example.FullName, [ref] $tokens, [ref] $errors)

            $errors | Should -BeNullOrEmpty -Because "$($example.Name) should parse on the current PowerShell parser"
            [System.IO.File]::ReadAllText($example.FullName) | Should -Not -Match $continuation
        }
    }
}

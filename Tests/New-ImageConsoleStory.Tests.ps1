Describe 'New-ImageConsoleStory' {
    BeforeAll {
        $env:IMAGEPLAYGROUND_DEVELOPMENT = '1'
        Import-Module -Name "$PSScriptRoot/../ImagePlayground.psd1" -Force
        $TestDir = Join-Path -Path $PSScriptRoot -ChildPath 'Artifacts'
        if (-not (Test-Path -Path $TestDir)) {
            New-Item -Path $TestDir -ItemType Directory | Out-Null
        }
    }

    It 'renders an authored PowerShell terminal presentation' {
        $file = Join-Path -Path $TestDir -ChildPath 'console-story.svg'
        if (Test-Path -Path $file) {
            Remove-Item -Path $file
        }

        $story = New-ImageConsoleStory -StoryScript {
            param($Console)
            [void] $Console.WithTitle('pwsh - C:\OpenSource').WithWorkingDirectory('C:\OpenSource')
            [void] $Console.Command('Get-ProjectStatus')
            [void] $Console.Output('Ready', [ChartForgeX.Terminal.TerminalTextTone]::Success)
        } -FilePath $file -PassThru

        $story | Should -BeOfType 'ChartForgeX.Terminal.TerminalStory'
        Test-Path -Path $file | Should -BeTrue
        $svg = [System.IO.File]::ReadAllText($file)
        $svg | Should -Match 'data-cfx-terminal="PowerShell"'
        $svg | Should -Match 'data-cfx-role="terminal-command"'
        $svg | Should -Match 'prefers-reduced-motion:reduce'
        $svg | Should -Not -Match '<script'
    }

    It 'turns captured transcript lines into a console story without executing the command' {
        $file = Join-Path -Path $TestDir -ChildPath 'captured-transcript.svg'
        if (Test-Path -Path $file) {
            Remove-Item -Path $file
        }

        $story = @('Checking domain...', 'PASS  DNS', 'PASS  Kerberos') |
            New-ImageConsoleStory -CommandText '.\Invoke-EnvironmentAudit.ps1' -WorkingDirectory 'C:\Audit' -FilePath $file -PassThru

        $story.Steps.Count | Should -Be 4
        $story.Steps[0].Text | Should -Be '.\Invoke-EnvironmentAudit.ps1'
        $story.Steps[1].Text | Should -Be 'Checking domain...'
        [System.IO.File]::ReadAllText($file) | Should -Match 'PASS  Kerberos'
    }

    It 'supports a custom prompt for captured transcript presentations' {
        $file = Join-Path -Path $TestDir -ChildPath 'custom-transcript.svg'
        $story = 'ready' |
            New-ImageConsoleStory -CommandText 'ship' -Dialect Custom -CustomPrompt 'demo> ' -FilePath $file -PassThru

        $story.CustomPrompt | Should -Be 'demo> '
        [System.IO.File]::ReadAllText($file) | Should -Match 'demo&gt; '
    }

    It 'accepts a native story and renders the completed PNG state' {
        $file = Join-Path -Path $TestDir -ChildPath 'console-story.png'
        if (Test-Path -Path $file) {
            Remove-Item -Path $file
        }

        $story = [ChartForgeX.Terminal.TerminalStory]::Create().Command('Get-Date').Output('Ready')
        $result = $story | Export-ImageConsoleStory -Path $file -PassThru

        $result | Should -BeOfType 'ChartForgeX.Terminal.TerminalStory'
        $bytes = [System.IO.File]::ReadAllBytes($file)
        $bytes[0] | Should -Be 137
        $bytes[1] | Should -Be 80
        $bytes[2] | Should -Be 78
        $bytes[3] | Should -Be 71
    }

    It 'rejects multiple pipeline stories before writing a fixed output path' {
        $file = Join-Path -Path $TestDir -ChildPath 'console-story-ambiguous.svg'
        if (Test-Path -Path $file) {
            Remove-Item -Path $file
        }
        $first = [ChartForgeX.Terminal.TerminalStory]::Create().Command('Get-Date').Output('First')
        $second = [ChartForgeX.Terminal.TerminalStory]::Create().Command('Get-Process').Output('Second')

        {
            @($first, $second) | Export-ImageConsoleStory -Path $file
        } | Should -Throw '*exactly one terminal story*'
        Test-Path -Path $file | Should -BeFalse
    }

    It 'renders portable GIF and APNG files from the same story timeline' {
        $file = Join-Path -Path $TestDir -ChildPath 'console-story.gif'
        $apngFile = Join-Path -Path $TestDir -ChildPath 'console-story.apng'
        if (Test-Path -Path $file) {
            Remove-Item -Path $file
        }
        if (Test-Path -Path $apngFile) {
            Remove-Item -Path $apngFile
        }

        $story = New-ImageConsoleStory -StoryScript {
            param($Console)
            [void] $Console.WithWidth(480).WithTiming(0, 200, 0).WithFinalPrompt($false)
            [void] $Console.Command('dotnet run', 0.05)
            [void] $Console.Output('Chart saved', [ChartForgeX.Terminal.TerminalTextTone]::Success)
        } -FilePath $file -FramesPerSecond 4 -EndHoldSeconds 0.1 -NoLoop -PassThru
        $story | Export-ImageConsoleStory -Path $apngFile -FramesPerSecond 4 -EndHoldSeconds 0.1

        $bytes = [System.IO.File]::ReadAllBytes($file)
        [System.Text.Encoding]::ASCII.GetString($bytes, 0, 6) | Should -Be 'GIF89a'
        [System.Text.Encoding]::ASCII.GetString($bytes) | Should -Not -Match 'NETSCAPE2.0'
        $apngBytes = [System.IO.File]::ReadAllBytes($apngFile)
        $apngBytes[0] | Should -Be 137
        [System.Text.Encoding]::ASCII.GetString($apngBytes) | Should -Match 'acTL'
    }

    It 'rejects unsupported output extensions' {
        {
            $story = New-ImageConsoleStory -StoryScript {
                param($Console)
                [void] $Console.Command('Get-Date')
            }
            $story | Export-ImageConsoleStory -Path (Join-Path -Path $TestDir -ChildPath 'console-story.webp')
        } | Should -Throw
    }
}

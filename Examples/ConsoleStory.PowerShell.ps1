$output = Join-Path -Path $PSScriptRoot -ChildPath 'Output\powershell-console-story.svg'

New-ImageConsoleStory -StoryScript {
    param($Console)

    $projects = [ChartForgeX.Terminal.TerminalTable]::Create()
    [void] $projects.WithColumns([string[]]@('PROJECT', 'STACK', 'STATUS'))
    [void] $projects.AddRow([object[]]@('ChartForgeX', '.NET', 'ready'))
    [void] $projects.AddRow([object[]]@('ImagePlayground', 'PowerShell', 'ready'))

    [void] $Console.WithTitle('pwsh - C:\OpenSource')
    [void] $Console.WithWorkingDirectory('C:\OpenSource')
    [void] $Console.Command('Get-ActivePortfolio | Format-Table')
    [void] $Console.Table($projects)
    [void] $Console.Blank()
    [void] $Console.Command('.\Invoke-ReleaseValidation.ps1')
    [void] $Console.Output('PASS  all checks', [ChartForgeX.Terminal.TerminalTextTone]::Success)
} -FilePath $output

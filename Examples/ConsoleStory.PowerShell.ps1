$svgOutput = Join-Path -Path $PSScriptRoot -ChildPath 'Output\powershell-console-story.svg'
$gifOutput = Join-Path -Path $PSScriptRoot -ChildPath 'Output\powershell-console-story.gif'

$projects = @(
    [pscustomobject]@{ Project = 'ChartForgeX'; Stack = '.NET'; Status = 'ready' }
    [pscustomobject]@{ Project = 'ImagePlayground'; Stack = 'PowerShell'; Status = 'ready' }
)

$storyOptions = @{
    Title            = 'pwsh - C:\OpenSource'
    WorkingDirectory = 'C:\OpenSource'
    Theme            = 'PowerShell'
    WindowStyle      = 'MacOS'
    Speed            = 'Normal'
    Content          = {
        New-ImageConsoleStoryCommand -Text 'Get-ActivePortfolio | Format-Table'
        $projects | New-ImageConsoleStoryTable -Property Project, Stack, Status -Header PROJECT, STACK, STATUS

        New-ImageConsoleStoryBlankLine
        New-ImageConsoleStoryCommand -Text '.\Invoke-ReleaseValidation.ps1'
        New-ImageConsoleStoryOutput -Text 'PASS  all checks' -Style Success
    }
}

$story = New-ImageConsoleStory @storyOptions
$story | Export-ImageConsoleStory -Path $svgOutput
$story | Export-ImageConsoleStory -Path $gifOutput -FramesPerSecond 8 -EndHoldSeconds 1.5

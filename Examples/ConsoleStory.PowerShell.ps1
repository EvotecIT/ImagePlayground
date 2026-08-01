$svgOutput = Join-Path -Path $PSScriptRoot -ChildPath 'Output\powershell-console-story.svg'
$gifOutput = Join-Path -Path $PSScriptRoot -ChildPath 'Output\powershell-console-story.gif'

$projects = @(
    [pscustomobject]@{ Project = 'ChartForgeX'; Stack = '.NET'; Status = 'ready' }
    [pscustomobject]@{ Project = 'ImagePlayground'; Stack = 'PowerShell'; Status = 'ready' }
)

$story = New-ImageConsoleStory `
    -Title 'pwsh - C:\OpenSource' `
    -WorkingDirectory 'C:\OpenSource' `
    -Theme PowerShell `
    -WindowStyle MacOS `
    -Content {
        New-ImageConsoleStoryCommand -Text 'Get-ActivePortfolio | Format-Table'
        $projects | New-ImageConsoleStoryTable `
            -Property Project, Stack, Status `
            -Header PROJECT, STACK, STATUS

        New-ImageConsoleStoryBlankLine
        New-ImageConsoleStoryCommand -Text '.\Invoke-ReleaseValidation.ps1'
        New-ImageConsoleStoryOutput -Text 'PASS  all checks' -Tone Success
    } `
    -FilePath $svgOutput `
    -PassThru

$story | New-ImageConsoleStory `
    -FilePath $gifOutput `
    -FramesPerSecond 8 `
    -EndHoldSeconds 1.5

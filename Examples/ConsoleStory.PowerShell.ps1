param(
    [string] $OutputDirectory = (Join-Path -Path $PSScriptRoot -ChildPath 'Output'),
    [ValidateSet('Light', 'Dark')]
    [string] $Theme = 'Dark'
)

Import-Module "$PSScriptRoot\..\ImagePlayground.psd1" -Force

$projects = @(
    [pscustomobject]@{ Project = 'ChartForgeX'; Stack = '.NET'; Status = 'ready' }
    [pscustomobject]@{ Project = 'ImagePlayground'; Stack = 'PowerShell'; Status = 'ready' }
)

$storyOptions = @{
    Title            = 'pwsh - OpenSource'
    WorkingDirectory = 'OpenSource'
    Theme            = $Theme
    WindowStyle      = 'WindowsTerminal'
    Width            = 640
    FontSize         = 22
    LineHeight       = 28
    Speed            = 'Normal'
    Content          = {
        New-ImageConsoleStoryCommand -Text 'Get-ActivePortfolio'
        $projects | New-ImageConsoleStoryTable -Property Project, Stack, Status -Header PROJECT, STACK, STATUS

        New-ImageConsoleStoryBlankLine
        New-ImageConsoleStoryCommand -Text './build.ps1'
        New-ImageConsoleStoryOutput -Text 'PASS  all checks' -Style Success
    }
}

$suffix = $Theme.ToLowerInvariant()
$story = New-ImageConsoleStory @storyOptions
foreach ($extension in 'svg', 'html', 'png', 'gif') {
    $story | Export-ImageConsoleStory -Path (Join-Path -Path $OutputDirectory -ChildPath "powershell-console-story-$suffix.$extension") -FramesPerSecond 8 -EndHoldSeconds 1.5
}

# Design a second terminal at the supported 480px minimum instead of shrinking a wide transcript.
$storyOptions.Width = 480
$storyOptions.FontSize = 20
$storyOptions.LineHeight = 26
$storyOptions.Content = {
    New-ImageConsoleStoryCommand -Text 'Get-ActivePortfolio'
    $projects | New-ImageConsoleStoryTable -Property Project, Stack -Header PROJECT, STACK
    foreach ($project in $projects) {
        New-ImageConsoleStoryOutput -Text ('{0}: {1}' -f $project.Project, $project.Status)
    }

    New-ImageConsoleStoryBlankLine
    New-ImageConsoleStoryCommand -Text './build.ps1'
    New-ImageConsoleStoryOutput -Text 'PASS  all checks' -Style Success
}
$compact = New-ImageConsoleStory @storyOptions
foreach ($extension in 'svg', 'html', 'png') {
    $compact | Export-ImageConsoleStory -Path (Join-Path -Path $OutputDirectory -ChildPath "powershell-console-story-$suffix-compact.$extension")
}

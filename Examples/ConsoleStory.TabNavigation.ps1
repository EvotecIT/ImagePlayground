$outputDirectory = Join-Path -Path $PSScriptRoot -ChildPath 'Artifacts'
New-Item -Path $outputDirectory -ItemType Directory -Force | Out-Null

$story = New-ImageConsoleStory -WindowStyle WindowsTerminal -Width 1100 -Speed Slow -TabHoldSeconds 2.5 -Content {
    New-ImageConsoleStoryTab -Id PowerShell -Title 'PowerShell' -Profile PowerShell -Active
    New-ImageConsoleStoryTab -Id Logs -Title 'Build logs' -Profile PowerShell -Background
    New-ImageConsoleStoryCommand -Text 'dotnet build'
    New-ImageConsoleStoryOutput -Text 'Build succeeded.' -Style Success

    New-ImageConsoleStoryTab -Id Ubuntu -Title 'Ubuntu' -Profile Ubuntu
    New-ImageConsoleStoryCommand -Text './package.sh'
    New-ImageConsoleStoryOutput -Text 'Linux package ready.' -Style Success

    Select-ImageConsoleStoryTab -Id Logs
    New-ImageConsoleStoryOutput -Text 'Waiting for integration tests...' -Style Muted
    New-ImageConsoleStoryPause -Seconds 1.5
    New-ImageConsoleStoryOutput -Text 'Integration tests passed.' -Style Success

    Select-ImageConsoleStoryTab -Id PowerShell
    New-ImageConsoleStoryPause -Seconds 1
    New-ImageConsoleStoryCommand -Text 'Get-ChildItem .\artifacts'
    New-ImageConsoleStoryOutput -Text 'The original PowerShell session continued.' -Style Accent
}

$story | Export-ImageConsoleStory -Path (Join-Path -Path $outputDirectory -ChildPath 'console-story-navigation.svg')
$story | Export-ImageConsoleStory -Path (Join-Path -Path $outputDirectory -ChildPath 'console-story-navigation.gif') -FramesPerSecond 6 -EndHoldSeconds 2

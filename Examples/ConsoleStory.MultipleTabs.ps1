$outputDirectory = Join-Path -Path $PSScriptRoot -ChildPath 'Artifacts'
New-Item -Path $outputDirectory -ItemType Directory -Force | Out-Null

$story = New-ImageConsoleStory -WindowStyle WindowsTerminal -Width 1100 -Speed Slow -Content {
    New-ImageConsoleStoryTab -Id PowerShell -Title 'PowerShell' -Profile PowerShell -Active
    New-ImageConsoleStoryCommand -Text 'dotnet build'
    New-ImageConsoleStoryOutput -Text 'Build succeeded.' -Style Success

    New-ImageConsoleStoryTab -Id WindowsPowerShell -Title 'Windows PowerShell' -Profile WindowsPowerShell
    New-ImageConsoleStoryCommand -Text '.\Invoke-LegacyTests.ps1'
    New-ImageConsoleStoryOutput -Text 'PS 5.1 compatibility passed.' -Style Success

    New-ImageConsoleStoryTab -Id Ubuntu -Title 'Ubuntu' -Profile Ubuntu
    New-ImageConsoleStoryCommand -Text './build.sh'
    New-ImageConsoleStoryOutput -Text 'Linux package ready.' -Style Success
}

$story | Export-ImageConsoleStory -Path (Join-Path -Path $outputDirectory -ChildPath 'console-story-tabs.svg')
$story | Export-ImageConsoleStory -Path (Join-Path -Path $outputDirectory -ChildPath 'console-story-tabs.gif') -FramesPerSecond 8 -EndHoldSeconds 1.5

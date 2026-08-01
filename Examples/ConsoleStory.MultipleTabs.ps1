$outputDirectory = Join-Path -Path $PSScriptRoot -ChildPath 'Artifacts'
New-Item -Path $outputDirectory -ItemType Directory -Force | Out-Null

$ubuntuPalette = New-ImageConsoleStoryPalette `
    -Preset Ubuntu `
    -Background '#24071B' `
    -Accent '#FF6A2B'

$story = New-ImageConsoleStory `
    -Title 'PowerShell' `
    -WorkingDirectory 'C:\OpenSource' `
    -Theme Campbell `
    -WindowStyle WindowsTerminal `
    -Width 1100 `
    -Content {
        New-ImageConsoleStoryCommand -Text 'Get-Module ImagePlayground -ListAvailable'
        New-ImageConsoleStoryOutput -Text 'ImagePlayground  3.2.0  C:\Modules\ImagePlayground' -Tone Success

        New-ImageConsoleStoryTab `
            -Id windows-powershell `
            -Profile WindowsPowerShell `
            -WorkingDirectory 'C:\Windows\System32'
        New-ImageConsoleStoryCommand -Text '$PSVersionTable.PSVersion'
        New-ImageConsoleStoryOutput -Text 'Major  Minor  Build  Revision' -Tone Accent
        New-ImageConsoleStoryOutput -Text '5      1      26100  4652'

        New-ImageConsoleStoryTab `
            -Id ubuntu `
            -Profile Ubuntu `
            -WorkingDirectory '~/src/ImagePlayground' `
            -Palette $ubuntuPalette
        New-ImageConsoleStoryCommand -Text 'dotnet test --no-restore'
        New-ImageConsoleStoryOutput -Text 'Passed!  Failed: 0, Passed: 169' -Tone Success

        Select-ImageConsoleStoryTab -Id main
        New-ImageConsoleStoryOutput -Text 'All environments are ready.' -Tone Success
    } `
    -FilePath (Join-Path -Path $outputDirectory -ChildPath 'console-story-tabs.svg') `
    -PassThru

$story | New-ImageConsoleStory `
    -FilePath (Join-Path -Path $outputDirectory -ChildPath 'console-story-tabs.png')

$story | New-ImageConsoleStory `
    -FilePath (Join-Path -Path $outputDirectory -ChildPath 'console-story-tabs.gif') `
    -FramesPerSecond 8 `
    -EndHoldSeconds 1.5

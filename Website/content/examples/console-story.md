---
title: Console Story
description: Present authored commands or captured script output as a polished SVG, GIF, APNG, HTML, or PNG.
weight: 34
---

Use `New-ImageConsoleStory` for profile introductions, script demonstrations, release validation, and documentation that should read like an authentic terminal session.

```powershell
$projects = @(
    [pscustomobject]@{ Project = 'ChartForgeX'; Stack = '.NET'; Status = 'ready' }
    [pscustomobject]@{ Project = 'ImagePlayground'; Stack = 'PowerShell'; Status = 'ready' }
)

New-ImageConsoleStory `
    -Title 'pwsh - C:\OpenSource' `
    -WorkingDirectory 'C:\OpenSource' `
    -Theme PowerShell `
    -WindowStyle MacOS `
    -Content {
        New-ImageConsoleStoryCommand -Text 'Get-ActivePortfolio | Format-Table'
        $projects | New-ImageConsoleStoryTable `
            -Property Project, Stack, Status `
            -Header PROJECT, STACK, STATUS
    } `
    -FilePath '.\portfolio.svg'
```

For a real script run, keep execution caller-controlled and pipe only the captured transcript to the renderer:

```powershell
$transcript = & .\Invoke-EnvironmentAudit.ps1 2>&1 |
    Out-String -Stream -Width 110

$story = $transcript | New-ImageConsoleStory `
    -CommandText '.\Invoke-EnvironmentAudit.ps1' `
    -Dialect PowerShell `
    -Theme PowerShell `
    -WindowStyle Minimal `
    -FilePath '.\audit-demo.svg' `
    -PassThru

$story | New-ImageConsoleStory `
    -FilePath '.\audit-demo.gif' `
    -FramesPerSecond 8 `
    -EndHoldSeconds 1.5
```

The command never invokes the text supplied through `-CommandText`. `-Dialect` selects prompt behavior, `-Theme` selects colors, and `-WindowStyle` selects `MacOS`, `WindowsTerminal`, `Minimal`, or `None` chrome. SVG and HTML animate without JavaScript. GIF and APNG reuse the same deterministic timeline for portable chat and documentation embeds; PNG, print, and reduced-motion output show the complete transcript.

## Persistent tabs and custom palettes

Tabs are real persistent sessions rather than decorative labels. Each tab retains its own transcript buffer, prompt dialect, working directory, palette, and icon when the story switches away and returns. The initial tab is named `main`.

```powershell
$ubuntu = New-ImageConsoleStoryPalette `
    -Preset Ubuntu `
    -Background '#24071B' `
    -Accent '#FF6A2B'

New-ImageConsoleStory `
    -Title PowerShell `
    -WorkingDirectory 'C:\OpenSource' `
    -Theme Campbell `
    -WindowStyle WindowsTerminal `
    -Content {
        New-ImageConsoleStoryCommand -Text 'Get-Module ImagePlayground'

        New-ImageConsoleStoryTab -Id legacy -Profile WindowsPowerShell
        New-ImageConsoleStoryCommand -Text '$PSVersionTable.PSVersion'

        New-ImageConsoleStoryTab `
            -Id ubuntu `
            -Profile Ubuntu `
            -WorkingDirectory '~/src' `
            -Palette $ubuntu
        New-ImageConsoleStoryCommand -Text 'dotnet test'
        New-ImageConsoleStoryOutput -Text 'Passed: 169' -Tone Success

        Select-ImageConsoleStoryTab -Id main
        New-ImageConsoleStoryOutput -Text 'All environments are ready.' -Tone Success
    } `
    -FilePath '.\multi-shell-demo.gif'
```

`-Profile` supplies sensible prompt, directory, icon, and palette defaults for `PowerShell`, `WindowsPowerShell`, `Ubuntu`, `Bash`, and `CommandPrompt`. `New-ImageConsoleStoryPalette` starts from a built-in palette and lets a demo override only the colors it needs. Window chrome remains independent, so the same multi-session story can use Windows Terminal, macOS, minimal, or chrome-free presentation.

The story is generic enough for a short product walkthrough: type a few C# or PowerShell lines, reveal status or output, pause, and continue. For example, a five-line chart demo can be presented as a C# interactive session and saved directly for Discord:

```powershell
New-ImageConsoleStory `
    -Title 'dotnet run - ChartForgeX' `
    -Dialect CSharp `
    -Theme Dark `
    -WindowStyle WindowsTerminal `
    -Width 1000 `
    -Content {
        New-ImageConsoleStoryCommand -Text 'using ChartForgeX; using ChartForgeX.Core; using System.Linq;' -DurationSeconds 0.65
        New-ImageConsoleStoryCommand -Text 'var chart = Chart.Create().WithTitle("Weekly builds");' -DurationSeconds 0.65
        New-ImageConsoleStoryCommand -Text 'chart.WithXLabels("Mon", "Tue", "Wed", "Thu", "Fri");' -DurationSeconds 0.65
        New-ImageConsoleStoryCommand -Text 'chart.AddLine("Builds", new[] { 12d, 18d, 15d, 24d, 31d }.Select((y, x) => new ChartPoint(x + 1, y)));' -DurationSeconds 1.1
        New-ImageConsoleStoryCommand -Text 'chart.SavePng("weekly-builds.png");' -DurationSeconds 0.65
        New-ImageConsoleStoryOutput -Text 'Saved weekly-builds.png (1000 x 560)' -Tone Success
    } `
    -FilePath '.\chart-in-five-lines.gif'
```

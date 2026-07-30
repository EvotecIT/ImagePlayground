---
title: Console Story
description: Present authored commands or captured script output as a polished SVG, GIF, APNG, HTML, or PNG.
weight: 34
---

Use `New-ImageConsoleStory` for profile introductions, script demonstrations, release validation, and documentation that should read like an authentic terminal session.

```powershell
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
} -FilePath '.\portfolio.svg'
```

For a real script run, keep execution caller-controlled and pipe only the captured transcript to the renderer:

```powershell
$transcript = & .\Invoke-EnvironmentAudit.ps1 2>&1 |
    Out-String -Stream -Width 110

$transcript | New-ImageConsoleStory `
    -CommandText '.\Invoke-EnvironmentAudit.ps1' `
    -Dialect PowerShell `
    -FilePath '.\audit-demo.svg'
```

The command never invokes the text supplied through `-CommandText`. PowerShell, Bash, command prompt, Python, C#, and custom dialects only control how prompts are presented. SVG and HTML animate without JavaScript. GIF and APNG reuse the same deterministic timeline for portable chat and documentation embeds; PNG, print, and reduced-motion output show the complete transcript.

The story is generic enough for a short product walkthrough: type a few C# or PowerShell lines, reveal status or output, pause, and continue. For example, a five-line chart demo can be presented as a C# interactive session and saved directly for Discord:

```powershell
New-ImageConsoleStory -StoryScript {
    param($Console)

    [void] $Console.WithTitle('dotnet run - ChartForgeX').WithDialect(
        [ChartForgeX.Terminal.TerminalDialect]::CSharp
    ).WithWidth(1000)
    [void] $Console.Command('using ChartForgeX; using ChartForgeX.Core; using System.Linq;', 0.65)
    [void] $Console.Command('var chart = Chart.Create().WithTitle("Weekly builds");', 0.65)
    [void] $Console.Command('chart.WithXLabels("Mon", "Tue", "Wed", "Thu", "Fri");', 0.65)
    [void] $Console.Command('chart.AddLine("Builds", new[] { 12d, 18d, 15d, 24d, 31d }.Select((y, x) => new ChartPoint(x + 1, y)));', 1.1)
    [void] $Console.Command('chart.SavePng("weekly-builds.png");', 0.65)
    [void] $Console.Output('Saved weekly-builds.png (1000 x 560)', [ChartForgeX.Terminal.TerminalTextTone]::Success)
} -FilePath '.\chart-in-five-lines.gif'
```

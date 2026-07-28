---
title: PowerShell Console Story
description: Present authored commands or captured PowerShell script output as a polished, script-free animated SVG.
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

The command never invokes the text supplied through `-CommandText`. PowerShell, Bash, command prompt, Python, C#, and custom dialects only control how prompts are presented. SVG and HTML animate without JavaScript; PNG, print, and reduced-motion output show the complete transcript.

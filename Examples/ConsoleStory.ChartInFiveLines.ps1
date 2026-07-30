$output = Join-Path -Path $PSScriptRoot -ChildPath 'Output\chart-in-five-lines.gif'

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
} -FilePath $output

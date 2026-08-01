$output = Join-Path -Path $PSScriptRoot -ChildPath 'Output\chart-in-five-lines.gif'

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
    -FilePath $output

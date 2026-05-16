using System;
using System.Collections.Generic;
using System.IO;
using ChartForgeX;
using ChartForgeX.Core;
using ChartForgeX.Primitives;

namespace ImagePlayground.Examples;
internal partial class Example {
    /// <summary>
    /// Generates sample charts demonstrating multiple chart types.
    /// </summary>
    /// <param name="folderPath">Location where output images will be saved.</param>
    public static void ChartsExamples(string folderPath) {
        Console.WriteLine("[*] Creating Bar chart");
        string bar = Path.Combine(folderPath, "ChartsBar.png");
        Chart.Create()
            .WithSize(500, 500)
            .WithXLabels("C#", "C++", "PowerShell")
            .AddBar("Usage", Points(5, 12, 10))
            .SavePng(bar);

        Console.WriteLine("[*] Creating Pie chart");
        string pie = Path.Combine(folderPath, "ChartsPie.png");
        Chart.Create()
            .WithSize(500, 500)
            .WithXLabels("C#", "C++", "PowerShell")
            .AddPie("Usage", Points(5, 12, 10))
            .SavePng(pie);

        Console.WriteLine("[*] Creating Line chart");
        string line = Path.Combine(folderPath, "ChartsLine.png");
        Chart.Create()
            .WithSize(500, 500)
            .AddLine("C#", Points(5, 10, 12, 18, 10, 13))
            .AddLine("C++", Points(10, 15, 30, 40, 50, 60))
            .AddLine("PowerShell", Points(10, 5, 12, 18, 30, 60))
            .SavePng(line);

        Console.WriteLine("[*] Creating Scatter chart");
        string scatter = Path.Combine(folderPath, "ChartsScatter.png");
        Chart.Create()
            .WithSize(500, 500)
            .AddScatter("First", new[] { new ChartPoint(1, 4), new ChartPoint(2, 5), new ChartPoint(3, 6) })
            .AddScatter("Second", new[] { new ChartPoint(1, 3), new ChartPoint(2, 2), new ChartPoint(3, 1) })
            .SavePng(scatter);

        Console.WriteLine("[*] Creating Radial chart");
        string radial = Path.Combine(folderPath, "ChartsRadial.png");
        Chart.Create()
            .WithSize(500, 500)
            .WithXLabels("C#", "AutoIt v3", "PowerShell", "C++", "F#")
            .AddRadialBar("Usage", Points(5, 50, 10, 18, 100))
            .SavePng(radial);

        Console.WriteLine("[*] Creating Line chart with annotations");
        string annotated = Path.Combine(folderPath, "ChartsAnnotated.png");
        Chart.Create()
            .WithSize(500, 500)
            .AddLine("C#", Points(5, 10, 12, 18, 10, 13))
            .AddVerticalLine(1, "first")
            .SavePng(annotated);

        Console.WriteLine("[*] Creating Heatmap chart");
        string heatmap = Path.Combine(folderPath, "ChartsHeatmap.png");
        Chart.Create()
            .WithSize(500, 500)
            .AddHeatmapRow("Heat 1", Points(1, 2))
            .AddHeatmapRow("Heat 2", Points(3, 4))
            .SavePng(heatmap);

        Console.WriteLine("[*] Creating Histogram chart");
        string hist = Path.Combine(folderPath, "ChartsHistogram.png");
        Chart.Create()
            .WithSize(500, 500)
            .AddHistogram("Data", new double[] { 1, 2, 3, 3, 4, 5 }, 2)
            .SavePng(hist);
    }

    private static IEnumerable<ChartPoint> Points(params double[] values) {
        for (var index = 0; index < values.Length; index++) {
            yield return new ChartPoint(index + 1, values[index]);
        }
    }
}

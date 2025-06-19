using System;
using System.Collections.Generic;
using System.IO;

namespace ImagePlayground.Examples {
    internal partial class Example {
        public static void ChartsExamples(string folderPath) {
            Console.WriteLine("[*] Creating Bar chart");
            string bar = Path.Combine(folderPath, "ChartsBar.png");
            var bars = new List<Charts.ChartDefinition> {
                new Charts.ChartBar("C#", new List<double> { 5 }),
                new Charts.ChartBar("C++", new List<double> { 12 }),
                new Charts.ChartBar("PowerShell", new List<double> { 10 })
            };
            Charts.Generate(bars, bar, 500, 500);

            Console.WriteLine("[*] Creating Pie chart");
            string pie = Path.Combine(folderPath, "ChartsPie.png");
            var pies = new List<Charts.ChartDefinition> {
                new Charts.ChartPie("C#", 5),
                new Charts.ChartPie("C++", 12),
                new Charts.ChartPie("PowerShell", 10)
            };
            Charts.Generate(pies, pie, 500, 500);

            Console.WriteLine("[*] Creating Line chart");
            string line = Path.Combine(folderPath, "ChartsLine.png");
            var lines = new List<Charts.ChartDefinition> {
                new Charts.ChartLine("C#", new List<double> { 5, 10, 12, 18, 10, 13 }),
                new Charts.ChartLine("C++", new List<double> { 10, 15, 30, 40, 50, 60 }),
                new Charts.ChartLine("PowerShell", new List<double> { 10, 5, 12, 18, 30, 60 })
            };
            Charts.Generate(lines, line, 500, 500);

            Console.WriteLine("[*] Creating Scatter chart");
            string scatter = Path.Combine(folderPath, "ChartsScatter.png");
            var scatters = new List<Charts.ChartDefinition> {
                new Charts.ChartScatter("First", new List<double> { 1, 2, 3 }, new List<double> { 4, 5, 6 }),
                new Charts.ChartScatter("Second", new List<double> { 1, 2, 3 }, new List<double> { 3, 2, 1 })
            };
            Charts.Generate(scatters, scatter, 500, 500);

            Console.WriteLine("[*] Creating Radial chart");
            string radial = Path.Combine(folderPath, "ChartsRadial.png");
            var radials = new List<Charts.ChartDefinition> {
                new Charts.ChartRadial("C#", 5),
                new Charts.ChartRadial("AutoIt v3", 50),
                new Charts.ChartRadial("PowerShell", 10),
                new Charts.ChartRadial("C++", 18),
                new Charts.ChartRadial("F#", 100)
            };
            Charts.Generate(radials, radial, 500, 500);

            Console.WriteLine("[*] Creating Line chart with annotations");
            string annotated = Path.Combine(folderPath, "ChartsAnnotated.png");
            var anns = new List<Charts.ChartAnnotation> {
                new Charts.ChartAnnotation(1, 12, "first", true)
            };
            Charts.Generate(lines, annotated, 500, 500, null, null, null, false, Charts.ChartTheme.Default, anns);
        }
    }
}

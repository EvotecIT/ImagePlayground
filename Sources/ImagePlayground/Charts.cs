using System;
using System.Collections.Generic;
using System.Linq;
using ScottPlot;

namespace ImagePlayground;

/// <summary>Chart generation helpers.</summary>
public static class Charts {
    /// <summary>Type of chart definition.</summary>
    public enum ChartDefinitionType {
        /// <summary>Bar chart.</summary>
        Bar,
        /// <summary>Line chart.</summary>
        Line,
        /// <summary>Pie chart.</summary>
        Pie,
        /// <summary>Radial gauge chart.</summary>
        Radial
    }

    /// <summary>Base class for chart definitions.</summary>
    public abstract class ChartDefinition {
        /// <summary>Chart type.</summary>
        public ChartDefinitionType Type { get; }
        /// <summary>Chart name.</summary>
        public string Name { get; }

        protected ChartDefinition(ChartDefinitionType type, string name) {
            Type = type;
            Name = name;
        }
    }

    /// <summary>Bar chart definition.</summary>
    public sealed class ChartBar : ChartDefinition {
        /// <summary>Bar values.</summary>
        public IList<double> Value { get; }

        public ChartBar(string name, IList<double> value) : base(ChartDefinitionType.Bar, name) {
            Value = value;
        }
    }

    /// <summary>Line chart definition.</summary>
    public sealed class ChartLine : ChartDefinition {
        /// <summary>Line values.</summary>
        public IList<double> Value { get; }

        public ChartLine(string name, IList<double> value) : base(ChartDefinitionType.Line, name) {
            Value = value;
        }
    }

    /// <summary>Pie chart definition.</summary>
    public sealed class ChartPie : ChartDefinition {
        /// <summary>Slice value.</summary>
        public double Value { get; }

        public ChartPie(string name, double value) : base(ChartDefinitionType.Pie, name) {
            Value = value;
        }
    }

    /// <summary>Radial gauge chart definition.</summary>
    public sealed class ChartRadial : ChartDefinition {
        /// <summary>Gauge value.</summary>
        public double Value { get; }

        public ChartRadial(string name, double value) : base(ChartDefinitionType.Radial, name) {
            Value = value;
        }
    }

    /// <summary>Options for bar charts.</summary>
    public sealed class ChartBarOptions {
        /// <summary>Whether to show values above bars.</summary>
        public bool ShowValuesAboveBars { get; }

        public ChartBarOptions(bool showValuesAboveBars) {
            ShowValuesAboveBars = showValuesAboveBars;
        }
    }

    /// <summary>Generate a chart based on provided definitions.</summary>
    public static void Generate(IEnumerable<ChartDefinition> definitions, string filePath, int width = 600, int height = 400, ChartBarOptions? barOptions = null) {
        if (definitions is null) throw new ArgumentNullException(nameof(definitions));
        var list = definitions.ToList();
        if (list.Count == 0) throw new ArgumentException("No chart definitions provided", nameof(definitions));

        var plot = new Plot();
        var type = list[0].Type;

        switch (type) {
            case ChartDefinitionType.Bar:
                var positions = new List<int>();
                var labels = new List<string>();
                var dict = new Dictionary<int, List<double>>();
                int pos = 0;
                foreach (var bar in list.Cast<ChartBar>()) {
                    for (int i = 0; i < bar.Value.Count; i++) {
                        if (!dict.TryGetValue(i, out var vals)) {
                            vals = new List<double>();
                            dict[i] = vals;
                        }
                        vals.Add(bar.Value[i]);
                    }
                    labels.Add(bar.Name);
                    positions.Add(pos++);
                }
                foreach (var kv in dict.OrderBy(k => k.Key)) {
                    var plottable = plot.Add.Bars(kv.Value.ToArray());
                    plottable.ValueLabelStyle.IsVisible = barOptions?.ShowValuesAboveBars == true;
                    if (barOptions?.ShowValuesAboveBars == true) {
                        for (int i = 0; i < plottable.Bars.Count; i++) {
                            plottable.Bars[i].ValueLabel = plottable.Bars[i].Value.ToString();
                        }
                    }
                }
                plot.Axes.Bottom.SetTicks(positions.Select(x => (double)x).ToArray(), labels.ToArray());
                break;
            case ChartDefinitionType.Line:
                foreach (var line in list.Cast<ChartLine>()) {
                    var sig = plot.Add.Signal(line.Value.ToArray());
                    sig.LegendText = line.Name;
                }
                plot.ShowLegend();
                break;
            case ChartDefinitionType.Pie:
                var pieValues = list.Cast<ChartPie>().Select(p => p.Value).ToArray();
                var pieLabels = list.Cast<ChartPie>().Select(p => p.Name).ToArray();
                var pie = plot.Add.Pie(pieValues);
                for (int i = 0; i < pie.Slices.Count && i < pieLabels.Length; i++) {
                    pie.Slices[i].Label = pieLabels[i];
                }
                break;
            case ChartDefinitionType.Radial:
                var rValues = list.Cast<ChartRadial>().Select(r => r.Value).ToArray();
                var rLabels = list.Cast<ChartRadial>().Select(r => r.Name).ToArray();
                var radial = plot.Add.RadialGaugePlot(rValues);
                radial.Labels = rLabels;
                plot.ShowLegend();
                break;
        }

        filePath = System.IO.Path.GetFullPath(filePath);
        plot.SavePng(filePath, width, height);
    }
}

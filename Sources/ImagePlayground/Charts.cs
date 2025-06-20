using System;
using System.Collections.Generic;
using System.Linq;
using ScottPlot;
using SixLabors.ImageSharp.PixelFormats;
using ImageColor = SixLabors.ImageSharp.Color;

namespace ImagePlayground;

/// <summary>Chart generation helpers.</summary>
public static class Charts {
    /// <summary>Type of chart definition.</summary>
    public enum ChartDefinitionType {
        /// <summary>Bar chart.</summary>
        Bar,
        /// <summary>Line chart.</summary>
        Line,
        /// <summary>Scatter chart.</summary>
        Scatter,
        /// <summary>Polar plot.</summary>
        Polar,
        /// <summary>Pie chart.</summary>
        Pie,
        /// <summary>Radial gauge chart.</summary>
        Radial,
        /// <summary>Heatmap chart.</summary>
        Heatmap,
        /// <summary>Histogram chart.</summary>
        Histogram
    }

    /// <summary>Available chart themes.</summary>
    public enum ChartTheme {
        /// <summary>Use the default ScottPlot style.</summary>
        Default,
        /// <summary>Apply the dark theme.</summary>
        Dark,
        /// <summary>Apply the light theme.</summary>
        Light
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

        /// <summary>Bar color.</summary>
        public ImageColor? Color { get; }

        public ChartBar(string name, IList<double> value, ImageColor? color = null) : base(ChartDefinitionType.Bar, name) {
            Value = value;
            Color = color;
        }
    }

    /// <summary>Line chart definition.</summary>
    public sealed class ChartLine : ChartDefinition {
        /// <summary>Line values.</summary>
        public IList<double> Value { get; }

        /// <summary>Line color.</summary>
        public ImageColor? Color { get; }

        public ChartLine(string name, IList<double> value, ImageColor? color = null) : base(ChartDefinitionType.Line, name) {
            Value = value;
            Color = color;
        }
    }

    /// <summary>Scatter chart definition.</summary>
    public sealed class ChartScatter : ChartDefinition {
        /// <summary>X values.</summary>
        public IList<double> X { get; }

        /// <summary>Y values.</summary>
        public IList<double> Y { get; }

        /// <summary>Point color.</summary>
        public ImageColor? Color { get; }

        public ChartScatter(string name, IList<double> x, IList<double> y, ImageColor? color = null) : base(ChartDefinitionType.Scatter, name) {
            X = x;
            Y = y;
            Color = color;
        }
    }

    /// <summary>Polar plot definition.</summary>
    public sealed class ChartPolar : ChartDefinition {
        /// <summary>Angle values.</summary>
        public IList<double> Angle { get; }

        /// <summary>Radius values.</summary>
        public IList<double> Value { get; }

        /// <summary>Line color.</summary>
        public ImageColor? Color { get; }

        public ChartPolar(string name, IList<double> angle, IList<double> value, ImageColor? color = null) : base(ChartDefinitionType.Polar, name) {
            Angle = angle;
            Value = value;
            Color = color;
        }
    }

    /// <summary>Pie chart definition.</summary>
    public sealed class ChartPie : ChartDefinition {
        /// <summary>Slice value.</summary>
        public double Value { get; }

        /// <summary>Slice color.</summary>
        public ImageColor? Color { get; }

        public ChartPie(string name, double value, ImageColor? color = null) : base(ChartDefinitionType.Pie, name) {
            Value = value;
            Color = color;
        }
    }

    /// <summary>Radial gauge chart definition.</summary>
    public sealed class ChartRadial : ChartDefinition {
        /// <summary>Gauge value.</summary>
        public double Value { get; }

        /// <summary>Gauge color.</summary>
        public ImageColor? Color { get; }

        public ChartRadial(string name, double value, ImageColor? color = null) : base(ChartDefinitionType.Radial, name) {
            Value = value;
            Color = color;
        }
    }

    /// <summary>Heatmap chart definition.</summary>
    public sealed class ChartHeatmap : ChartDefinition {
        /// <summary>Values of the heatmap.</summary>
        public double[,] Data { get; }

        public ChartHeatmap(string name, double[,] data) : base(ChartDefinitionType.Heatmap, name) {
            Data = data;
        }
    }

    /// <summary>Histogram chart definition.</summary>
    public sealed class ChartHistogram : ChartDefinition {
        /// <summary>Values for the histogram.</summary>
        public double[] Values { get; }

        /// <summary>Size of each bin.</summary>
        public int? BinSize { get; }

        public ChartHistogram(string name, double[] values, int? binSize = null) : base(ChartDefinitionType.Histogram, name) {
            Values = values;
            BinSize = binSize;
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


    /// <summary>Chart annotation definition.</summary>
    public sealed class ChartAnnotation {
        /// <summary>X coordinate of the annotation.</summary>
        public double X { get; }
        /// <summary>Y coordinate of the annotation.</summary>
        public double Y { get; }
        /// <summary>Annotation text.</summary>
        public string Text { get; }
        /// <summary>Display arrow.</summary>
        public bool Arrow { get; }

        public ChartAnnotation(double x, double y, string text, bool arrow = false) {
            X = x;
            Y = y;
            Text = text;
            Arrow = arrow;
        }
    }

    /// <summary>Generate a chart based on provided definitions.</summary>
    public static void Generate(
        IEnumerable<ChartDefinition> definitions,
        string filePath,
        int width = 600,
        int height = 400,
        ChartBarOptions? barOptions = null,
        string? xTitle = null,
        string? yTitle = null,
        bool showGrid = false,
        ChartTheme theme = ChartTheme.Default,
        IEnumerable<ChartAnnotation>? annotations = null) {
        if (definitions is null) throw new ArgumentNullException(nameof(definitions));
        var list = definitions.ToList();
        if (list.Count == 0) throw new ArgumentException("No chart definitions provided", nameof(definitions));

        var plot = new Plot();
        switch (theme) {
            case ChartTheme.Dark:
                new ScottPlot.PlotStyles.Dark().Apply(plot);
                break;
            case ChartTheme.Light:
                new ScottPlot.PlotStyles.Light().Apply(plot);
                break;
        }
        var type = list[0].Type;
        if (list.Any(d => d.Type != type))
            throw new ArgumentException("Mixed chart definition types provided. All chart definitions must have the same ChartDefinitionType.", nameof(definitions));

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
                    for (int i = 0; i < plottable.Bars.Count && i < list.Count; i++) {
                        var c = ((ChartBar)list[i]).Color;
                        if (c.HasValue) {
                            var px = c.Value.ToPixel<Rgba32>();
                            plottable.Bars[i].FillColor = new ScottPlot.Color(px.R, px.G, px.B, px.A);
                        }
                    }
                }
                plot.Axes.Bottom.SetTicks(positions.Select(x => (double)x).ToArray(), labels.ToArray());
                break;
            case ChartDefinitionType.Line:
                foreach (var line in list.Cast<ChartLine>()) {
                    var sig = plot.Add.Signal(line.Value.ToArray());
                    sig.LegendText = line.Name;
                    if (line.Color.HasValue) {
                        var px = line.Color.Value.ToPixel<Rgba32>();
                        sig.LineColor = new ScottPlot.Color(px.R, px.G, px.B, px.A);
                    }
                }
                plot.ShowLegend();
                break;
            case ChartDefinitionType.Scatter:
                foreach (var scatter in list.Cast<ChartScatter>()) {
                    var sc = plot.Add.Scatter(scatter.X.ToArray(), scatter.Y.ToArray());
                    sc.LegendText = scatter.Name;
                    if (scatter.Color.HasValue) {
                        var px = scatter.Color.Value.ToPixel<Rgba32>();
                        sc.Color = new ScottPlot.Color(px.R, px.G, px.B, px.A);
                    }
                }
                plot.ShowLegend();
                break;
            case ChartDefinitionType.Polar:
                foreach (var polar in list.Cast<ChartPolar>()) {
                    var pp = plot.Add.Polar(polar.Angle.ToArray(), polar.Value.ToArray());
                    pp.LegendText = polar.Name;
                    if (polar.Color.HasValue) {
                        var px = polar.Color.Value.ToPixel<Rgba32>();
                        pp.Color = new ScottPlot.Color(px.R, px.G, px.B, px.A);
                    }
                }
                plot.ShowLegend();
                break;
            case ChartDefinitionType.Pie:
                var pieValues = list.Cast<ChartPie>().Select(p => p.Value).ToArray();
                var pieLabels = list.Cast<ChartPie>().Select(p => p.Name).ToArray();
                var pie = plot.Add.Pie(pieValues);
                for (int i = 0; i < pie.Slices.Count && i < pieLabels.Length; i++) {
                    pie.Slices[i].Label = pieLabels[i];
                    var c = ((ChartPie)list[i]).Color;
                    if (c.HasValue) {
                        var px = c.Value.ToPixel<Rgba32>();
                        pie.Slices[i].FillColor = new ScottPlot.Color(px.R, px.G, px.B, px.A);
                    }
                }
                break;
            case ChartDefinitionType.Radial:
                var rValues = list.Cast<ChartRadial>().Select(r => r.Value).ToArray();
                var rLabels = list.Cast<ChartRadial>().Select(r => r.Name).ToArray();
                var radial = plot.Add.RadialGaugePlot(rValues);
                radial.Labels = rLabels;
                var rColors = list.Cast<ChartRadial>().Select(r => r.Color).ToArray();
                if (rColors.Any(c => c.HasValue)) {
                    radial.Colors = rColors.Select(c => {
                        if (c.HasValue) {
                            var px = c.Value.ToPixel<Rgba32>();
                            return new ScottPlot.Color(px.R, px.G, px.B, px.A);
                        }
                        return new ScottPlot.Color();
                    }).ToArray();
                }
                plot.ShowLegend();
                break;
            case ChartDefinitionType.Heatmap:
                foreach (var map in list.Cast<ChartHeatmap>()) {
                    plot.Add.Heatmap(map.Data);
                }
                break;
            case ChartDefinitionType.Histogram:
                foreach (var hist in list.Cast<ChartHistogram>()) {
                    var histogram = hist.BinSize.HasValue
                        ? ScottPlot.Statistics.Histogram.WithBinSize(hist.BinSize.Value, hist.Values)
                        : ScottPlot.Statistics.Histogram.WithBinCount(10, hist.Values);
                    var bp = plot.Add.Bars(histogram.Bins, histogram.Counts);
                    bp.LegendText = hist.Name;
                    foreach (var bar in bp.Bars) {
                        bar.Size = histogram.FirstBinSize * 0.8;
                    }
                }
                plot.ShowLegend();
                break;
        }

        if (!string.IsNullOrEmpty(xTitle)) {
            plot.Axes.Bottom.Label.Text = xTitle;
        }

        if (!string.IsNullOrEmpty(yTitle)) {
            plot.Axes.Left.Label.Text = yTitle;
        }

        if (showGrid) {
            plot.ShowGrid();
        } else {
            plot.HideGrid();
        }

        if (annotations is not null) {
            foreach (var ann in annotations) {
                var callout = plot.Add.Callout(ann.Text, new Coordinates(ann.X, ann.Y), new Coordinates(ann.X, ann.Y));
                if (!ann.Arrow) {
                    callout.ArrowLineWidth = 0;
                }
            }
        }

        filePath = Helpers.ResolvePath(filePath);
        plot.SavePng(filePath, width, height);
    }
}

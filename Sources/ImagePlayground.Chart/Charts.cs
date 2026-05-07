using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using ChartForgeX;
using ChartForgeX.Primitives;
using SixLabors.ImageSharp.PixelFormats;
using CfxChart = ChartForgeX.Core.Chart;
using CfxHeatmapScale = ChartForgeX.Core.ChartHeatmapScale;
using CfxLegendPosition = ChartForgeX.Core.ChartLegendPosition;
using CfxPictorialItem = ChartForgeX.Core.ChartPictorialItem;
using CfxPictorialShape = ChartForgeX.Core.ChartPictorialShape;
using CfxPieLabelContent = ChartForgeX.Core.ChartPieSliceLabelContent;
using CfxProgressItem = ChartForgeX.Core.ChartProgressItem;
using CfxTheme = ChartForgeX.Themes.ChartTheme;
using CfxBubble = ChartForgeX.Core.ChartBubble;
using CfxWordCloudItem = ChartForgeX.Core.ChartWordCloudItem;
using ImageColor = SixLabors.ImageSharp.Color;

namespace ImagePlayground;

/// <summary>Chart generation helpers.</summary>
public static class Charts {

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
        IEnumerable<ChartAnnotation>? annotations = null,
        ImageColor? background = null,
        ChartRenderOptions? options = null) {
        if (definitions is null) throw new ArgumentNullException(nameof(definitions));
        var list = definitions.ToList();
        if (list.Count == 0) throw new ArgumentException("No chart definitions provided", nameof(definitions));

        var type = list[0].Type;
        if (list.Any(d => d.Type != type)) {
            throw new ArgumentException("Mixed chart definition types provided. All chart definitions must have the same ChartDefinitionType.", nameof(definitions));
        }

        var chart = CfxChart.Create()
            .WithSize(width, height)
            .WithTheme(CreateTheme(theme, background))
            .WithGrid(showGrid);

        if (background.HasValue) {
            chart.WithTransparentBackground(false);
        }

        if (!string.IsNullOrEmpty(xTitle)) {
            chart.WithXAxis(xTitle!);
        }

        if (!string.IsNullOrEmpty(yTitle)) {
            chart.WithYAxis(yTitle!);
        }

        ApplyRenderOptions(chart, options);

        switch (type) {
            case ChartDefinitionType.Bar:
                foreach (var bar in list.Cast<ChartBar>()) {
                    chart.AddBar(bar.Name, BuildIndexedPoints(bar.Value), ToChartColor(bar.Color));
                }
                if (barOptions?.ShowValuesAboveBars == true) {
                    chart.WithDataLabels();
                }
                break;
            case ChartDefinitionType.Line:
                foreach (var line in list.Cast<ChartLine>()) {
                    var points = BuildIndexedPoints(line.Value);
                    if (line.Smooth) {
                        chart.AddSmoothLine(line.Name, points, ToChartColor(line.Color));
                    } else {
                        chart.AddLine(line.Name, points, ToChartColor(line.Color));
                    }

                    if (line.MarkerShape != ChartMarkerShape.None && line.MarkerSize.HasValue) {
                        chart.Options.Theme.MarkerRadius = Math.Max(0, line.MarkerSize.Value / 2d);
                    }
                }
                break;
            case ChartDefinitionType.Area:
                foreach (var area in list.Cast<ChartArea>()) {
                    chart.AddSmoothArea(area.Name, BuildIndexedPoints(area.Value), ToChartColor(area.Color));
                }
                break;
            case ChartDefinitionType.Scatter:
                foreach (var scatter in list.Cast<ChartScatter>()) {
                    chart.AddScatter(scatter.Name, BuildXYPoints(scatter.X, scatter.Y), ToChartColor(scatter.Color));
                }
                break;
            case ChartDefinitionType.Bubble:
                foreach (var bubble in list.Cast<ChartBubble>()) {
                    chart.AddBubble(bubble.Name, BuildBubbles(bubble.X, bubble.Y, bubble.Size), ToChartColor(bubble.Color));
                }
                break;
            case ChartDefinitionType.Polar:
                foreach (var polar in list.Cast<ChartPolar>()) {
                    chart.AddRadar(polar.Name, BuildXYPoints(polar.Angle, polar.Value), ToChartColor(polar.Color));
                }
                break;
            case ChartDefinitionType.PolarArea:
                foreach (var polar in list.Cast<ChartPolar>()) {
                    chart.AddPolarArea(polar.Name, BuildXYPoints(polar.Angle, polar.Value));
                }
                break;
            case ChartDefinitionType.Pie:
                chart.WithXLabels(list.Select(d => d.Name).ToArray());
                chart.AddPie("Values", list.Cast<ChartPie>().Select((pie, index) => new ChartPoint(index + 1, pie.Value)));
                ApplyPointColors(chart, list.Cast<ChartPie>().Select(pie => pie.Color).ToArray());
                break;
            case ChartDefinitionType.Donut:
                chart.WithXLabels(list.Select(d => d.Name).ToArray());
                chart.AddDonut("Values", list.Cast<ChartDonut>().Select((donut, index) => new ChartPoint(index + 1, donut.Value)));
                ApplyPointColors(chart, list.Cast<ChartDonut>().Select(donut => donut.Color).ToArray());
                break;
            case ChartDefinitionType.Radial:
                chart.AddRadialBar("Values", list.Cast<ChartRadial>().Select((radial, index) => new ChartPoint(index + 1, Clamp(radial.Value, 0, 100))));
                ApplyPointColors(chart, list.Cast<ChartRadial>().Select(radial => radial.Color).ToArray());
                break;
            case ChartDefinitionType.Gauge:
                var gauge = RequireSingle<ChartGauge>(list, type);
                chart.AddGauge(gauge.Name, gauge.Value, gauge.Minimum, gauge.Maximum, ToChartColor(gauge.Color));
                break;
            case ChartDefinitionType.Circle:
                var circle = RequireSingle<ChartCircle>(list, type);
                chart.AddCircle(circle.Name, circle.Value, circle.Minimum, circle.Maximum, ToChartColor(circle.Color));
                break;
            case ChartDefinitionType.ProgressBar:
                chart.AddProgressBars("Values", list.Cast<ChartProgress>().Select(progress => new CfxProgressItem(progress.Name, progress.Value, ToChartColor(progress.Color))), options?.ProgressMaximum ?? 100);
                break;
            case ChartDefinitionType.Pictorial:
                chart.AddPictorial("Values", list.Cast<ChartPictorial>().Select(item => new CfxPictorialItem(item.Name, item.Value, ToChartColor(item.Color))), ResolvePictorialShape(options?.PictorialSymbol));
                break;
            case ChartDefinitionType.WordCloud:
                chart.AddWordCloud("Values", list.Cast<ChartWordCloud>().Select(item => new CfxWordCloudItem(item.Name, item.Weight, ToChartColor(item.Color))));
                break;
            case ChartDefinitionType.Heatmap:
                foreach (var map in list.Cast<ChartHeatmap>()) {
                    AddHeatmap(chart, map);
                }
                break;
            case ChartDefinitionType.Histogram:
                foreach (var hist in list.Cast<ChartHistogram>()) {
                    chart.AddHistogram(hist.Name, hist.Values, ResolveBinCount(hist));
                }
                break;
        }

        if (annotations is not null) {
            foreach (var ann in annotations) {
                chart.AddVerticalLine(ann.X, ann.Text);
            }
        }

        filePath = Helpers.ResolvePath(filePath);
        Helpers.CreateParentDirectory(filePath);
        SaveChart(chart, filePath);
    }

    private static CfxTheme CreateTheme(ChartTheme theme, ImageColor? background) {
        var chartTheme = theme == ChartTheme.Dark ? CfxTheme.ReportDark() : CfxTheme.ReportLight();
        if (background.HasValue) {
            var color = ToChartColor(background.Value);
            chartTheme.Background = color;
            chartTheme.CardBackground = color;
            chartTheme.PlotBackground = WithAlpha(color, color.A);
        }

        return chartTheme;
    }

    private static void ApplyRenderOptions(CfxChart chart, ChartRenderOptions? options) {
        if (options == null) {
            return;
        }

        if (options.Palette != null && options.Palette.Count > 0) {
            chart.WithPalette(options.Palette.Select(ToChartColor).ToArray());
        }
        if (options.ShowLegend.HasValue) chart.WithLegend(options.ShowLegend.Value);
        if (options.ShowPointLegend.HasValue) chart.WithPointLegend(options.ShowPointLegend.Value);
        if (options.LegendPosition.HasValue) chart.WithLegendPosition(ResolveLegendPosition(options.LegendPosition.Value));
        if (options.ShowHeader.HasValue) chart.WithHeader(options.ShowHeader.Value);
        if (options.ShowCard.HasValue) chart.WithCard(options.ShowCard.Value);
        if (options.ShowPlotBackground.HasValue) chart.WithPlotBackground(options.ShowPlotBackground.Value);
        if (options.TransparentBackground.HasValue) chart.WithTransparentBackground(options.TransparentBackground.Value);
        if (options.ShowAxes.HasValue) chart.WithAxes(options.ShowAxes.Value);
        if (options.ShowXAxis.HasValue) chart.WithXAxisVisible(options.ShowXAxis.Value);
        if (options.ShowYAxis.HasValue) chart.WithYAxisVisible(options.ShowYAxis.Value);
        if (options.ShowAxisLines.HasValue) chart.WithAxisLines(options.ShowAxisLines.Value);
        if (options.ShowGrid.HasValue) chart.WithGrid(options.ShowGrid.Value);
        if (options.ShowDataLabels.HasValue) chart.WithDataLabels(options.ShowDataLabels.Value);
        if (options.TickCount.HasValue) chart.WithTickCount(options.TickCount.Value);
        if (options.XAxisMinimum.HasValue && options.XAxisMaximum.HasValue) chart.WithXAxisBounds(options.XAxisMinimum.Value, options.XAxisMaximum.Value);
        if (options.YAxisMinimum.HasValue && options.YAxisMaximum.HasValue) chart.WithYAxisBounds(options.YAxisMinimum.Value, options.YAxisMaximum.Value);
        if (options.HeatmapScale.HasValue) chart.WithHeatmapScale(ResolveHeatmapScale(options.HeatmapScale.Value));
        if (options.ShowHeatmapScale.HasValue) chart.WithHeatmapScaleLegend(options.ShowHeatmapScale.Value);
        if (options.ShowHeatmapColumnLabels.HasValue) chart.WithHeatmapColumnLabels(options.ShowHeatmapColumnLabels.Value);
        if (options.ShowDonutCenterLabel.HasValue) chart.WithDonutCenterLabel(options.ShowDonutCenterLabel.Value);
        if (options.DonutInnerRadiusRatio.HasValue) chart.WithDonutInnerRadiusRatio(options.DonutInnerRadiusRatio.Value);
        if (!string.IsNullOrWhiteSpace(options.DonutCenterValue) || !string.IsNullOrWhiteSpace(options.DonutCenterLabel)) chart.WithDonutCenterText(options.DonutCenterValue, options.DonutCenterLabel);
        if (options.PieLabelContent.HasValue) chart.WithPieSliceLabelContent(ResolvePieLabelContent(options.PieLabelContent.Value));
        if (options.ShowRadialBarCenterLabel.HasValue) chart.WithRadialBarCenterLabel(options.ShowRadialBarCenterLabel.Value);
        if (options.ShowCircleStatusLabel.HasValue) chart.WithCircleStatusLabel(options.ShowCircleStatusLabel.Value);
        if (options.ProgressMaximum.HasValue) chart.WithProgressMaximum(options.ProgressMaximum.Value);
        if (options.ShowProgressValues.HasValue) chart.WithProgressValues(options.ShowProgressValues.Value);
        if (options.ShowProgressHandles.HasValue) chart.WithProgressHandles(options.ShowProgressHandles.Value);
        if (options.ProgressBarThicknessRatio.HasValue) chart.WithProgressBarThickness(options.ProgressBarThicknessRatio.Value);
        if (options.PictorialSymbol.HasValue) chart.WithPictorialShape(ResolvePictorialShape(options.PictorialSymbol.Value));
        if (options.PictorialColumns.HasValue) chart.WithPictorialColumns(options.PictorialColumns.Value);
        if (options.PictorialMaximum.HasValue) chart.WithPictorialMaximum(options.PictorialMaximum.Value);
        if (options.ShowPictorialValues.HasValue) chart.WithPictorialValues(options.ShowPictorialValues.Value);
        if (options.WordCloudMaximumTerms.HasValue) chart.WithWordCloudMaximumTerms(options.WordCloudMaximumTerms.Value);
    }

    private static ChartPoint[] BuildIndexedPoints(IList<double> values) {
        var points = new ChartPoint[values.Count];
        for (var i = 0; i < values.Count; i++) {
            points[i] = new ChartPoint(i + 1, values[i]);
        }

        return points;
    }

    private static ChartPoint[] BuildXYPoints(IList<double> x, IList<double> y) {
        var count = Math.Min(x.Count, y.Count);
        var points = new ChartPoint[count];
        for (var i = 0; i < count; i++) {
            points[i] = new ChartPoint(x[i], y[i]);
        }

        return points;
    }

    private static CfxBubble[] BuildBubbles(IList<double> x, IList<double> y, IList<double> size) {
        var count = Math.Min(Math.Min(x.Count, y.Count), size.Count);
        var bubbles = new CfxBubble[count];
        for (var i = 0; i < count; i++) {
            bubbles[i] = new CfxBubble(x[i], y[i], Math.Max(0.001, size[i]));
        }

        return bubbles;
    }

    private static void AddHeatmap(CfxChart chart, ChartHeatmap map) {
        var rows = map.Data.GetLength(0);
        var columns = map.Data.GetLength(1);
        for (var row = 0; row < rows; row++) {
            var points = new ChartPoint[columns];
            for (var column = 0; column < columns; column++) {
                points[column] = new ChartPoint(column + 1, map.Data[row, column]);
            }

            chart.AddHeatmapRow(map.Name + " " + (row + 1).ToString(CultureInfo.InvariantCulture), points);
        }
    }

    private static void ApplyPointColors(CfxChart chart, ImageColor?[] colors) {
        if (chart.Series.Count == 0) {
            return;
        }

        for (var i = 0; i < colors.Length; i++) {
            var color = ToChartColor(colors[i]);
            if (color.HasValue) {
                chart.Series[0].WithPointColor(i, color.Value);
            }
        }
    }

    private static T RequireSingle<T>(IList<ChartDefinition> definitions, ChartDefinitionType type) where T : ChartDefinition {
        if (definitions.Count != 1) {
            throw new ArgumentException(type + " charts require exactly one definition.", nameof(definitions));
        }

        return (T)definitions[0];
    }

    private static CfxLegendPosition ResolveLegendPosition(ChartLegendPosition position) {
        switch (position) {
            case ChartLegendPosition.Top:
                return CfxLegendPosition.Top;
            case ChartLegendPosition.Left:
                return CfxLegendPosition.Left;
            case ChartLegendPosition.Right:
                return CfxLegendPosition.Right;
            default:
                return CfxLegendPosition.Bottom;
        }
    }

    private static CfxHeatmapScale ResolveHeatmapScale(ChartHeatmapColorScale scale) {
        switch (scale) {
            case ChartHeatmapColorScale.Semantic:
                return CfxHeatmapScale.Semantic;
            default:
                return CfxHeatmapScale.Sequential;
        }
    }

    private static CfxPictorialShape ResolvePictorialShape(ChartPictorialSymbol? shape) {
        switch (shape) {
            case ChartPictorialSymbol.Square:
                return CfxPictorialShape.Square;
            case ChartPictorialSymbol.Diamond:
                return CfxPictorialShape.Diamond;
            case ChartPictorialSymbol.Triangle:
                return CfxPictorialShape.Triangle;
            case ChartPictorialSymbol.Star:
                return CfxPictorialShape.Star;
            case ChartPictorialSymbol.Person:
                return CfxPictorialShape.Person;
            default:
                return CfxPictorialShape.Circle;
        }
    }

    private static CfxPieLabelContent ResolvePieLabelContent(ChartPieLabelContent content) {
        switch (content) {
            case ChartPieLabelContent.Value:
                return CfxPieLabelContent.Value;
            case ChartPieLabelContent.Percent:
                return CfxPieLabelContent.Percent;
            case ChartPieLabelContent.LabelAndValue:
                return CfxPieLabelContent.LabelAndValue;
            case ChartPieLabelContent.LabelAndPercent:
                return CfxPieLabelContent.LabelAndPercent;
            default:
                return CfxPieLabelContent.Label;
        }
    }

    private static int ResolveBinCount(ChartHistogram histogram) {
        if (!histogram.BinSize.HasValue || histogram.Values.Length == 0) {
            return 10;
        }

        var min = histogram.Values.Min();
        var max = histogram.Values.Max();
        if (Math.Abs(max - min) < 0.000001) {
            return 1;
        }

        return Math.Max(1, (int)Math.Ceiling((max - min) / histogram.BinSize.Value));
    }

    private static void SaveChart(CfxChart chart, string filePath) {
        var extension = Path.GetExtension(filePath).ToLowerInvariant();
        if (extension == ".svg") {
            File.WriteAllText(filePath, chart.ToSvg(), Encoding.UTF8);
        } else if (extension == ".html" || extension == ".htm") {
            File.WriteAllText(filePath, chart.ToHtmlPage(), Encoding.UTF8);
        } else {
            chart.SavePng(filePath);
        }
    }

    private static ChartColor? ToChartColor(ImageColor? color) {
        if (!color.HasValue) {
            return null;
        }

        return ToChartColor(color.Value);
    }

    private static ChartColor ToChartColor(ImageColor color) {
        var pixel = color.ToPixel<Rgba32>();
        return ChartColor.FromRgba(pixel.R, pixel.G, pixel.B, pixel.A);
    }

    private static ChartColor WithAlpha(ChartColor color, byte alpha) => ChartColor.FromRgba(color.R, color.G, color.B, alpha);

    private static double Clamp(double value, double min, double max) {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }
}

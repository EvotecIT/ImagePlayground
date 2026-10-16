using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using ChartForgeX;
using ChartForgeX.Core;
using ChartForgeX.Primitives;
using CfxChart = ChartForgeX.Core.Chart;
using CfxPictorialItem = ChartForgeX.Core.ChartPictorialItem;
using CfxProgressItem = ChartForgeX.Core.ChartProgressItem;
using CfxTheme = ChartForgeX.Themes.ChartTheme;
using CfxBubble = ChartForgeX.Core.ChartBubble;
using CfxBoxPlot = ChartForgeX.Core.ChartBoxPlot;
using CfxInterval = ChartForgeX.Core.ChartInterval;
using CfxRangeBand = ChartForgeX.Core.ChartRangeBand;
using CfxHierarchyItem = ChartForgeX.Core.ChartHierarchyItem;
using CfxWordCloudItem = ChartForgeX.Core.ChartWordCloudItem;

namespace ImagePlayground;

/// <summary>Chart generation helpers.</summary>
internal static class Charts {
    /// <summary>Create a ChartForgeX chart with ImagePlayground render settings applied.</summary>
    public static CfxChart Create(
        int width = 600,
        int height = 400,
        string? xTitle = null,
        string? yTitle = null,
        bool showGrid = false,
        ChartTheme theme = ChartTheme.Default,
        ChartColor? background = null,
        ChartRenderOptions? options = null) {
        var chart = CfxChart.Create()
            .WithSize(width, height)
            .WithTheme(CreateTheme(theme))
            .WithGrid(showGrid);

        ApplySettings(chart, xTitle, yTitle, background, options);
        return chart;
    }

    /// <summary>Build a ChartForgeX chart from ImagePlayground PowerShell definitions.</summary>
    public static CfxChart Build(
        IEnumerable<ChartDefinition> definitions,
        int width = 600,
        int height = 400,
        string? xTitle = null,
        string? yTitle = null,
        bool showGrid = false,
        ChartTheme theme = ChartTheme.Default,
        IEnumerable<ChartAnnotationDefinition>? annotations = null,
        ChartColor? background = null,
        ChartRenderOptions? options = null) {
        if (definitions is null) throw new ArgumentNullException(nameof(definitions));
        var list = definitions.ToList();
        if (list.Count == 0) throw new ArgumentException("No chart definitions provided", nameof(definitions));
        if (list.Any(definition => definition is null)) throw new ArgumentException("Chart definitions cannot contain null entries.", nameof(definitions));

        var types = list.Select(d => d.GetType()).Distinct().ToList();
        var chart = Create(width, height, xTitle, yTitle, showGrid, theme, background, options);
        var type = types[0];

        if (types.All(CombinableTypes.Contains)) {
            foreach (var definition in list) {
                AddCombinableSeries(chart, definition);
            }
            ApplyDefaultCategoryLabels(chart, list);
        } else if (types.Count > 1) {
            throw new ArgumentException(DescribeIncompatibleMix(types), nameof(definitions));
        } else if (type == typeof(ChartHorizontalBar)) {
            foreach (var bar in list.Cast<ChartHorizontalBar>()) chart.AddHorizontalBar(bar.Name, BuildIndexedPoints(bar.Value), bar.Color);
        } else if (type == typeof(ChartRangeBar)) {
            foreach (var range in list.Cast<ChartRangeBar>()) chart.AddRangeBar(range.Name, BuildIntervals(range.X, range.Start, range.End), range.Color);
        } else if (type == typeof(ChartBoxPlotSeries)) {

            foreach (var box in list.Cast<ChartBoxPlotSeries>()) chart.AddBoxPlot(box.Name, BuildBoxPlots(box), box.Color);
        } else if (type == typeof(ChartPolar)) {
            foreach (var polar in list.Cast<ChartPolar>()) chart.AddPolar(polar.Name, BuildXYPoints(polar.Angle, polar.Radius), polar.Color);
        } else if (type == typeof(ChartRadar)) {
            AddRadarDefinitions(chart, list.Cast<ChartRadar>());
        } else if (type == typeof(ChartPie)) {
            chart.WithXLabels(list.Select(d => d.Name).ToArray());
            chart.AddPie("Values", list.Cast<ChartPie>().Select((pie, index) => new ChartPoint(index + 1, pie.Value)));
            ApplyPointColors(chart, list.Cast<ChartPie>().Select(pie => pie.Color).ToArray());
        } else if (type == typeof(ChartDonut)) {
            chart.WithXLabels(list.Select(d => d.Name).ToArray());
            chart.AddDonut("Values", list.Cast<ChartDonut>().Select((donut, index) => new ChartPoint(index + 1, donut.Value)));
            ApplyPointColors(chart, list.Cast<ChartDonut>().Select(donut => donut.Color).ToArray());
        } else if (type == typeof(ChartRadial)) {
            chart.WithXLabels(list.Select(d => d.Name).ToArray());
            chart.AddRadialBar("Values", list.Cast<ChartRadial>().Select((radial, index) => new ChartPoint(index + 1, RequirePercent(radial.Value, radial.Name))));
            ApplyPointColors(chart, list.Cast<ChartRadial>().Select(radial => radial.Color).ToArray());
        } else if (type == typeof(ChartSlope)) {
            AddSlopeDefinitions(chart, list.Cast<ChartSlope>());
        } else if (type == typeof(ChartGauge)) {
            var gauge = RequireSingle<ChartGauge>(list);
            chart.AddGauge(gauge.Name, gauge.Value, gauge.Minimum, gauge.Maximum, gauge.Color);
        } else if (type == typeof(ChartCircle)) {
            var circle = RequireSingle<ChartCircle>(list);
            chart.AddCircle(circle.Name, circle.Value, circle.Minimum, circle.Maximum, circle.Color);
        } else if (type == typeof(ChartBullet)) {
            foreach (var bullet in list.Cast<ChartBullet>()) chart.AddBullet(bullet.Name, bullet.Value, bullet.Target, bullet.Minimum, bullet.Maximum, bullet.RangeEnds, bullet.Color);
        } else if (type == typeof(ChartWaterfall)) {
            var waterfall = RequireSingle<ChartWaterfall>(list);
            ApplyLabels(chart, waterfall.Labels);
            chart.AddWaterfall(waterfall.Name, BuildIndexedPoints(waterfall.Value), waterfall.Color);
        } else if (type == typeof(ChartFunnel)) {
            chart.WithXLabels(list.Select(d => d.Name).ToArray());
            chart.AddFunnel("Values", list.Cast<ChartFunnel>().Select((funnel, index) => new ChartPoint(index + 1, funnel.Value)));
            ApplyPointColors(chart, list.Cast<ChartFunnel>().Select(funnel => funnel.Color).ToArray());
        } else if (type == typeof(ChartTreemap)) {
            chart.AddTreemap("Values", list.Cast<ChartTreemap>().Select((item, index) => new CfxHierarchyItem("item-" + index.ToString(CultureInfo.InvariantCulture), item.Name, value: item.Value)));
            ApplyPointColors(chart, list.Cast<ChartTreemap>().Select(item => item.Color).ToArray());
        } else if (type == typeof(ChartProgress)) {
            chart.AddProgressBars("Values", list.Cast<ChartProgress>().Select(progress => new CfxProgressItem(progress.Name, progress.Value, progress.Color)), options?.ProgressMaximum ?? 100);
        } else if (type == typeof(ChartPictorial)) {
            chart.AddPictorial("Values", list.Cast<ChartPictorial>().Select(item => new CfxPictorialItem(item.Name, item.Value, item.Color)), ResolvePictorialShape(options?.PictorialSymbol ?? ChartPictorialSymbol.Circle));
        } else if (type == typeof(ChartWordCloud)) {
            chart.AddWordCloud("Values", list.Cast<ChartWordCloud>().Select(item => new CfxWordCloudItem(item.Name, item.Weight, item.Color)));
        } else if (type == typeof(ChartHeatmap)) {
            foreach (var map in list.Cast<ChartHeatmap>()) AddHeatmap(chart, map);
        } else if (type == typeof(ChartHistogram)) {
            AddHistogramDefinitions(chart, list.Cast<ChartHistogram>());
        } else {
            throw new ArgumentException("Unsupported chart definition type '" + type.FullName + "'.", nameof(definitions));
        }

        if (annotations is not null) {
            foreach (var ann in annotations) {
                if (ann is null) {
                    throw new ArgumentException("Chart annotations cannot contain null entries.", nameof(annotations));
                }

                if (ann.Arrow) {
                    if (HasExclusiveSeries(chart)) {
                        throw new ArgumentException("Point-callout annotations cannot be used with exclusive chart kinds because they add a scatter series.", nameof(annotations));
                    }

                    chart.AddPointCallout(ann.Text, ann.X, ann.Y);
                } else {
                    chart.AddVerticalLine(ann.X, ann.Text);
                }
            }
        }

        return chart;
    }

    /// <summary>Generate and save a chart based on provided definitions.</summary>
    public static void Generate(
        IEnumerable<ChartDefinition> definitions,
        string filePath,
        int width = 600,
        int height = 400,
        string? xTitle = null,
        string? yTitle = null,
        bool showGrid = false,
        ChartTheme theme = ChartTheme.Default,
        IEnumerable<ChartAnnotationDefinition>? annotations = null,
        ChartColor? background = null,
        ChartRenderOptions? options = null) {
        var chart = Build(definitions, width, height, xTitle, yTitle, showGrid, theme, annotations, background, options);
        Save(chart, filePath);
    }

    /// <summary>Apply ImagePlayground render settings to an existing ChartForgeX chart.</summary>
    public static void ApplySettings(CfxChart chart, string? xTitle = null, string? yTitle = null, ChartColor? background = null, ChartRenderOptions? options = null, bool? showGrid = null, ChartTheme? theme = null) {
        if (chart is null) throw new ArgumentNullException(nameof(chart));

        if (theme.HasValue) chart.WithTheme(CreateTheme(theme.Value));
        if (showGrid.HasValue) chart.WithGrid(showGrid.Value);

        if (background.HasValue) {
            ApplyBackground(chart.Options.Theme, background.Value);
            chart.WithTransparentBackground(false);
        }

        if (!string.IsNullOrEmpty(xTitle)) {
            chart.WithXAxis(xTitle!);
        }

        if (!string.IsNullOrEmpty(yTitle)) {
            chart.WithYAxis(yTitle!);
        }

        ApplyRenderOptions(chart, options);
    }

    /// <summary>Save a ChartForgeX chart to SVG, HTML, or PNG based on the file extension.</summary>
    public static void Save(CfxChart chart, string filePath) {
        if (chart is null) throw new ArgumentNullException(nameof(chart));
        filePath = Path.GetFullPath(filePath);
        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory)) {
            Directory.CreateDirectory(directory);
        }

        SaveChart(chart, filePath);
    }

    private static CfxTheme CreateTheme(ChartTheme theme) => ChartThemeResolver.Resolve(theme);

    private static void ApplyRenderOptions(CfxChart chart, ChartRenderOptions? options) {
        if (options == null) {
            return;
        }

        if (options.Palette != null && options.Palette.Count > 0) {
            chart.WithPalette(options.Palette.ToArray());
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
        if (options.BarLayout.HasValue) chart.WithBarMode(options.BarLayout.Value == ChartBarLayout.Stacked ? ChartBarMode.Stacked : ChartBarMode.Grouped);
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
        if (options.ShowRadialBarCenterLabel.HasValue) chart.WithProgressRingCenterLabel(options.ShowRadialBarCenterLabel.Value);
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

    private static ChartForgeX.Core.ChartLegendPosition ResolveLegendPosition(ImagePlayground.ChartLegendPosition position) {
        switch (position) {
            case ImagePlayground.ChartLegendPosition.Top:
                return ChartForgeX.Core.ChartLegendPosition.Top;
            case ImagePlayground.ChartLegendPosition.Left:
                return ChartForgeX.Core.ChartLegendPosition.Left;
            case ImagePlayground.ChartLegendPosition.Right:
                return ChartForgeX.Core.ChartLegendPosition.Right;
            default:
                return ChartForgeX.Core.ChartLegendPosition.Bottom;
        }
    }

    private static ChartHeatmapScale ResolveHeatmapScale(ChartHeatmapColorScale scale) {
        return scale == ChartHeatmapColorScale.Semantic ? ChartHeatmapScale.Semantic : ChartHeatmapScale.Sequential;
    }

    private static ChartPictorialShape ResolvePictorialShape(ChartPictorialSymbol symbol) {
        switch (symbol) {
            case ChartPictorialSymbol.Square:
                return ChartPictorialShape.Square;
            case ChartPictorialSymbol.Diamond:
                return ChartPictorialShape.Diamond;
            case ChartPictorialSymbol.Triangle:
                return ChartPictorialShape.Triangle;
            case ChartPictorialSymbol.Star:
                return ChartPictorialShape.Star;
            case ChartPictorialSymbol.Person:
                return ChartPictorialShape.Person;
            default:
                return ChartPictorialShape.Circle;
        }
    }

    private static ChartPieSliceLabelContent ResolvePieLabelContent(ChartPieLabelContent content) {
        switch (content) {
            case ChartPieLabelContent.Value:
                return ChartPieSliceLabelContent.Value;
            case ChartPieLabelContent.Percent:
                return ChartPieSliceLabelContent.Percent;
            case ChartPieLabelContent.LabelAndValue:
                return ChartPieSliceLabelContent.LabelAndValue;
            case ChartPieLabelContent.LabelAndPercent:
                return ChartPieSliceLabelContent.LabelAndPercent;
            default:
                return ChartPieSliceLabelContent.Label;
        }
    }

    private static ChartPoint[] BuildIndexedPoints(IList<double> values) {
        return ChartPoints.FromValues(values);
    }

    private static ChartPoint[] BuildXYPoints(IList<double> x, IList<double> y) {
        return ChartPoints.FromXY(x, y);
    }

    private static CfxBubble[] BuildBubbles(IList<double> x, IList<double> y, IList<double> size) {
        return ChartBubbles.FromXYSize(x, y, size);
    }

    private static void AddSlopeDefinitions(CfxChart chart, IEnumerable<ChartSlope> slopes) {
        var list = slopes.ToList();
        var labels = ResolveSlopeAxisLabels(list);
        foreach (var slope in list) {
            chart.AddSlope(slope.Name, slope.Start, slope.End, labels.Start, labels.End, slope.Color);
        }
    }

    private static (string Start, string End) ResolveSlopeAxisLabels(IEnumerable<ChartSlope> slopes) {
        string? start = null;
        string? end = null;
        foreach (var slope in slopes) {
            if (!string.IsNullOrWhiteSpace(slope.StartLabel)) {
                if (start is null) {
                    start = slope.StartLabel;
                } else if (!string.Equals(start, slope.StartLabel, StringComparison.Ordinal)) {
                    throw new ArgumentException("Slope chart definitions must use one shared start label.", nameof(slopes));
                }
            }

            if (!string.IsNullOrWhiteSpace(slope.EndLabel)) {
                if (end is null) {
                    end = slope.EndLabel;
                } else if (!string.Equals(end, slope.EndLabel, StringComparison.Ordinal)) {
                    throw new ArgumentException("Slope chart definitions must use one shared end label.", nameof(slopes));
                }
            }
        }

        return (start ?? "Start", end ?? "End");
    }

    private static CfxInterval[] BuildIntervals(IList<double> x, IList<double> start, IList<double> end) {
        if (x is null) throw new ArgumentNullException(nameof(x));
        if (start is null) throw new ArgumentNullException(nameof(start));
        if (end is null) throw new ArgumentNullException(nameof(end));
        if (x.Count != start.Count || x.Count != end.Count) {
            throw new ArgumentException("Range bar X, start, and end values must contain the same number of items.", nameof(x));
        }

        var intervals = new CfxInterval[x.Count];
        for (var i = 0; i < intervals.Length; i++) {
            intervals[i] = new CfxInterval(x[i], start[i], end[i]);
        }

        return intervals;
    }

    private static CfxRangeBand[] BuildRanges(IList<double> x, IList<double> lower, IList<double> upper) {
        if (x is null) throw new ArgumentNullException(nameof(x));
        if (lower is null) throw new ArgumentNullException(nameof(lower));
        if (upper is null) throw new ArgumentNullException(nameof(upper));
        if (x.Count != lower.Count || x.Count != upper.Count) {
            throw new ArgumentException("Range X, lower, and upper values must contain the same number of items.", nameof(x));
        }

        var ranges = new CfxRangeBand[x.Count];
        for (var i = 0; i < ranges.Length; i++) {
            ranges[i] = new CfxRangeBand(x[i], lower[i], upper[i]);
        }

        return ranges;
    }

    private static CfxBoxPlot[] BuildBoxPlots(ChartBoxPlotSeries box) {
        if (box is null) throw new ArgumentNullException(nameof(box));
        if (box.X is null) throw new ArgumentNullException(nameof(box.X));
        if (box.Minimum is null) throw new ArgumentNullException(nameof(box.Minimum));
        if (box.Q1 is null) throw new ArgumentNullException(nameof(box.Q1));
        if (box.Median is null) throw new ArgumentNullException(nameof(box.Median));
        if (box.Q3 is null) throw new ArgumentNullException(nameof(box.Q3));
        if (box.Maximum is null) throw new ArgumentNullException(nameof(box.Maximum));
        if (box.X.Count != box.Minimum.Count ||
            box.X.Count != box.Q1.Count ||
            box.X.Count != box.Median.Count ||
            box.X.Count != box.Q3.Count ||
            box.X.Count != box.Maximum.Count) {
            throw new ArgumentException("Box plot X, minimum, quartile, median, and maximum values must contain the same number of items.", nameof(box));
        }

        var boxes = new CfxBoxPlot[box.X.Count];
        for (var i = 0; i < boxes.Length; i++) {
            boxes[i] = new CfxBoxPlot(box.X[i], box.Minimum[i], box.Q1[i], box.Median[i], box.Q3[i], box.Maximum[i]);
        }

        return boxes;
    }

    private static void AddHeatmap(CfxChart chart, ChartHeatmap map) {
        if (map.Data is null) throw new ArgumentException("Heatmap data cannot be null.", nameof(map));
        var rows = map.Data.GetLength(0);
        var columns = map.Data.GetLength(1);
        if (rows == 0 || columns == 0) throw new ArgumentException("Heatmap data cannot be empty.", nameof(map));

        for (var row = 0; row < rows; row++) {
            var points = new ChartPoint[columns];
            for (var column = 0; column < columns; column++) {
                points[column] = new ChartPoint(column + 1, map.Data[row, column]);
            }

            chart.AddHeatmapRow(map.Name + " " + (row + 1).ToString(CultureInfo.InvariantCulture), points);
        }
    }

    private static void AddRadarDefinitions(CfxChart chart, IEnumerable<ChartRadar> definitions) {
        var series = definitions
            .Select(radar => new {
                Radar = radar,
                Points = BuildXYPoints(radar.Category, radar.Value)
            })
            .ToArray();

        var categoryCount = series
            .SelectMany(item => item.Points.Select(point => point.X))
            .Distinct()
            .Count();
        if (categoryCount < 3) {
            throw new ArgumentException("Radar charts require at least three categories.", nameof(definitions));
        }

        foreach (var item in series) {
            chart.AddRadar(item.Radar.Name, item.Points, item.Radar.Color);
        }
    }

    private static void AddHistogramDefinitions(CfxChart chart, IEnumerable<ChartHistogram> definitions) {
        var histograms = definitions
            .Select(histogram => new HistogramDefinition(histogram, ValidateHistogramValues(histogram)))
            .ToArray();

        if (histograms.Length == 0) {
            return;
        }

        var binSize = ResolveSharedBinSize(histograms.Select(item => item.Definition));
        var min = histograms.SelectMany(item => item.Values).Min();
        var max = histograms.SelectMany(item => item.Values).Max();
        var layout = binSize.HasValue
            ? ChartHistogramBinLayout.FromWidth(min, max, binSize.Value)
            : ChartHistogramBinLayout.FromCount(min, max, 10);
        foreach (var histogram in histograms) chart.AddHistogram(histogram.Definition.Name, histogram.Values, layout);
    }

    private static void ApplyPointColors(CfxChart chart, ChartColor?[] colors) {
        if (chart.Series.Count == 0) {
            return;
        }

        for (var i = 0; i < colors.Length; i++) {
            var color = colors[i];
            if (color.HasValue) {
                chart.Series[0].WithPointColor(i, color.Value);
            }
        }
    }

    private static void ApplyLabels(CfxChart chart, IList<string>? labels) {
        if (labels is null || labels.Count == 0) {
            return;
        }

        chart.WithXLabels(labels.ToArray());
    }

    internal static bool HasExclusiveSeries(CfxChart chart) {
        foreach (var series in chart.Series) {
            if (ChartSeriesKindCapabilities.IsExclusive(series.Kind)) return true;
        }

        return false;
    }

    private static T RequireSingle<T>(IList<ChartDefinition> definitions) where T : ChartDefinition {
        if (definitions.Count != 1) {
            throw new ArgumentException(typeof(T).Name + " charts require exactly one definition.", nameof(definitions));
        }

        return (T)definitions[0];
    }

    private static double[] ValidateHistogramValues(ChartHistogram histogram) {
        if (histogram.Values is null) {
            throw new ArgumentNullException(nameof(histogram), "Histogram values cannot be null.");
        }

        if (histogram.Values.Length == 0) {
            throw new ArgumentException("Histogram values must contain at least one value.", nameof(histogram));
        }

        for (var i = 0; i < histogram.Values.Length; i++) {
            if (double.IsNaN(histogram.Values[i]) || double.IsInfinity(histogram.Values[i])) {
                throw new ArgumentOutOfRangeException(nameof(histogram), histogram.Values[i], "Histogram values must be finite.");
            }
        }

        if (histogram.BinSize.HasValue && histogram.BinSize.Value <= 0) {
            throw new ArgumentOutOfRangeException(nameof(histogram), histogram.BinSize.Value, "Histogram bin size must be greater than zero.");
        }

        return histogram.Values;
    }

    private static int? ResolveSharedBinSize(IEnumerable<ChartHistogram> histograms) {
        int? binSize = null;
        foreach (var histogram in histograms) {
            if (!histogram.BinSize.HasValue) {
                continue;
            }

            if (binSize.HasValue && binSize.Value != histogram.BinSize.Value) {
                throw new ArgumentException("Multiple histogram definitions must use the same bin size so they share one binning scheme.", nameof(histograms));
            }

            binSize = histogram.BinSize.Value;
        }

        return binSize;
    }

    private static void SaveChart(CfxChart chart, string filePath) {
        chart.Save(filePath);
    }

    private static void ApplyBackground(CfxTheme chartTheme, ChartColor color) {
        chartTheme.Background = color;
        chartTheme.CardBackground = color;
        chartTheme.PlotBackground = WithAlpha(color, color.A);
    }

    /// <summary>Definition kinds that add one cartesian series each and can share a chart with each other.</summary>
    private static readonly HashSet<Type> CombinableTypes = new() {
        typeof(ChartBar),
        typeof(ChartLine),
        typeof(ChartStepLine),
        typeof(ChartArea),
        typeof(ChartStepArea),
        typeof(ChartStackedArea),
        typeof(ChartScatter),
        typeof(ChartBubbleSeries),
        typeof(ChartLollipop),
        typeof(ChartRangeBandSeries)
    };

    private static void AddCombinableSeries(CfxChart chart, ChartDefinition definition) {
        switch (definition) {
            case ChartBar bar:
                chart.AddBar(bar.Name, BuildIndexedPoints(bar.Value), bar.Color);
                break;
            case ChartLine line:
                var points = BuildIndexedPoints(line.Value);
                if (line.Smooth) chart.AddSmoothLine(line.Name, points, line.Color);
                else chart.AddLine(line.Name, points, line.Color);
                ApplyLineMarkers(chart.Series[chart.Series.Count - 1], line);
                break;
            case ChartStepLine stepLine:
                chart.AddStepLine(stepLine.Name, BuildIndexedPoints(stepLine.Value), stepLine.Color);
                break;
            case ChartArea area:
                chart.AddArea(area.Name, BuildIndexedPoints(area.Value), area.Color);
                break;
            case ChartStepArea stepArea:
                chart.AddStepArea(stepArea.Name, BuildIndexedPoints(stepArea.Value), stepArea.Color);
                break;
            case ChartStackedArea stacked:
                if (stacked.Smooth) chart.AddSmoothStackedArea(stacked.Name, BuildIndexedPoints(stacked.Value), stacked.Color);
                else chart.AddStackedArea(stacked.Name, BuildIndexedPoints(stacked.Value), stacked.Color);
                break;
            case ChartScatter scatter:
                chart.AddScatter(scatter.Name, BuildXYPoints(scatter.X, scatter.Y), scatter.Color);
                break;
            case ChartBubbleSeries bubble:
                chart.AddBubble(bubble.Name, BuildBubbles(bubble.X, bubble.Y, bubble.Size), bubble.Color);
                break;
            case ChartLollipop lollipop:
                chart.AddLollipop(lollipop.Name, BuildIndexedPoints(lollipop.Value), lollipop.Color);
                break;
            case ChartRangeBandSeries range:
                var ranges = BuildRanges(range.X, range.Lower, range.Upper);
                if (range.Area) chart.AddRangeArea(range.Name, ranges, range.Color, range.Smooth);
                else chart.AddRangeBand(range.Name, ranges, range.Color);
                break;
            default:
                throw new ArgumentException("Unsupported chart definition type '" + definition.GetType().FullName + "'.", nameof(definition));
        }

        if (definition.SecondaryAxis) {
            chart.Series[chart.Series.Count - 1].YAxis = ChartAxisSide.Secondary;
        }
    }

    /// <summary>Index-based series are placed at x = 1..n; without labels the axis would show fractional ticks such as 0.5 and 1.5.</summary>
    private static void ApplyDefaultCategoryLabels(CfxChart chart, IList<ChartDefinition> definitions) {
        if (chart.Options.XAxisLabels.Count > 0) {
            return;
        }

        var count = 0;
        foreach (var definition in definitions) {
            IList<double>? values = definition switch {
                ChartBar bar => bar.Value,
                ChartLine line => line.Value,
                ChartStepLine stepLine => stepLine.Value,
                ChartArea area => area.Value,
                ChartStepArea stepArea => stepArea.Value,
                ChartStackedArea stacked => stacked.Value,
                ChartLollipop lollipop => lollipop.Value,
                _ => null
            };
            if (values is null) {
                return;
            }
            count = Math.Max(count, values.Count);
        }

        if (count > 0) {
            chart.WithXLabels(Enumerable.Range(1, count).Select(index => index.ToString(CultureInfo.InvariantCulture)).ToArray());
        }
    }

    private static void ApplyLineMarkers(ChartSeries series, ChartLine line) {
        if (line.MarkerSize.HasValue && line.MarkerSize.Value < 0) {
            throw new ArgumentOutOfRangeException(nameof(line), line.MarkerSize.Value, "Marker size must be zero or greater.");
        }

        var size = line.MarkerSize ?? 0f;
        if (size <= 0 || line.MarkerShape == ChartMarkerShape.None) {
            series.ConfigureMarkers(markers => markers.Enabled = false);
            return;
        }

        series.ConfigureMarkers(markers => {
            markers.Enabled = true;
            markers.Shape = ResolveMarkerShape(line.MarkerShape);
            markers.Radius = size / 2d;
        });
    }

    private static ChartForgeX.Core.ChartMarkerShape ResolveMarkerShape(ChartMarkerShape shape) {
        switch (shape) {
            case ChartMarkerShape.Square:
                return ChartForgeX.Core.ChartMarkerShape.Square;
            case ChartMarkerShape.Diamond:
                return ChartForgeX.Core.ChartMarkerShape.Diamond;
            case ChartMarkerShape.Triangle:
                return ChartForgeX.Core.ChartMarkerShape.Triangle;
            case ChartMarkerShape.Plus:
                return ChartForgeX.Core.ChartMarkerShape.Plus;
            case ChartMarkerShape.Cross:
                return ChartForgeX.Core.ChartMarkerShape.Cross;
            case ChartMarkerShape.Star:
                return ChartForgeX.Core.ChartMarkerShape.Star;
            default:
                return ChartForgeX.Core.ChartMarkerShape.Circle;
        }
    }

    private static string DescribeIncompatibleMix(IEnumerable<Type> types) {
        var names = string.Join(", ", types.Select(DefinitionLabel));
        return "These chart definitions cannot share one chart: " + names + ". Bar, line, step line, area, step area, stacked area, scatter, bubble, lollipop, and range band definitions can be combined; other chart kinds need their own New-ImageChart call.";
    }

    private static string DefinitionLabel(Type type) {
        var name = type.Name;
        if (name.StartsWith("Chart", StringComparison.Ordinal)) name = name.Substring(5);
        if (name.EndsWith("Series", StringComparison.Ordinal)) name = name.Substring(0, name.Length - 6);
        return name;
    }

    private static double RequirePercent(double value, string name) {
        if (value < 0 || value > 100) {
            throw new ArgumentOutOfRangeException(nameof(value), value, "Radial values must be between zero and one hundred. Invalid series: " + name);
        }

        return value;
    }

    private static ChartColor WithAlpha(ChartColor color, byte alpha) => ChartColor.FromRgba(color.R, color.G, color.B, alpha);

    private readonly struct HistogramDefinition {
        public HistogramDefinition(ChartHistogram definition, double[] values) {
            Definition = definition;
            Values = values;
        }

        public ChartHistogram Definition { get; }

        public double[] Values { get; }
    }
}

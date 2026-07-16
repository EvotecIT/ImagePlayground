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
using CfxTreemapItem = ChartForgeX.Core.ChartTreemapItem;
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

        var type = list[0].GetType();
        if (list.Any(d => d.GetType() != type)) {
            throw new ArgumentException("Mixed chart definition types provided. All chart definitions must have the same concrete definition type.", nameof(definitions));
        }

        var chart = Create(width, height, xTitle, yTitle, showGrid, theme, background, options);

        if (type == typeof(ChartBar)) {
            foreach (var bar in list.Cast<ChartBar>()) chart.AddBar(bar.Name, BuildIndexedPoints(bar.Value), bar.Color);
        } else if (type == typeof(ChartHorizontalBar)) {
            foreach (var bar in list.Cast<ChartHorizontalBar>()) chart.AddHorizontalBar(bar.Name, BuildIndexedPoints(bar.Value), bar.Color);
        } else if (type == typeof(ChartLine)) {
            ApplySharedLineMarkerSize(chart, list.Cast<ChartLine>());
            foreach (var line in list.Cast<ChartLine>()) {
                var points = BuildIndexedPoints(line.Value);
                if (line.Smooth) chart.AddSmoothLine(line.Name, points, line.Color);
                else chart.AddLine(line.Name, points, line.Color);
            }
        } else if (type == typeof(ChartStepLine)) {
            foreach (var line in list.Cast<ChartStepLine>()) chart.AddStepLine(line.Name, BuildIndexedPoints(line.Value), line.Color);
        } else if (type == typeof(ChartArea)) {
            foreach (var area in list.Cast<ChartArea>()) chart.AddArea(area.Name, BuildIndexedPoints(area.Value), area.Color);
        } else if (type == typeof(ChartStepArea)) {
            foreach (var area in list.Cast<ChartStepArea>()) chart.AddStepArea(area.Name, BuildIndexedPoints(area.Value), area.Color);
        } else if (type == typeof(ChartStackedArea)) {
            foreach (var area in list.Cast<ChartStackedArea>()) {
                if (area.Smooth) chart.AddSmoothStackedArea(area.Name, BuildIndexedPoints(area.Value), area.Color);
                else chart.AddStackedArea(area.Name, BuildIndexedPoints(area.Value), area.Color);
            }
        } else if (type == typeof(ChartScatter)) {
            foreach (var scatter in list.Cast<ChartScatter>()) chart.AddScatter(scatter.Name, BuildXYPoints(scatter.X, scatter.Y), scatter.Color);
        } else if (type == typeof(ChartBubbleSeries)) {
            foreach (var bubble in list.Cast<ChartBubbleSeries>()) chart.AddBubble(bubble.Name, BuildBubbles(bubble.X, bubble.Y, bubble.Size), bubble.Color);
        } else if (type == typeof(ChartLollipop)) {
            foreach (var lollipop in list.Cast<ChartLollipop>()) chart.AddLollipop(lollipop.Name, BuildIndexedPoints(lollipop.Value), lollipop.Color);
        } else if (type == typeof(ChartRangeBar)) {
            foreach (var range in list.Cast<ChartRangeBar>()) chart.AddRangeBar(range.Name, BuildIntervals(range.X, range.Start, range.End), range.Color);
        } else if (type == typeof(ChartRangeBandSeries)) {
            foreach (var range in list.Cast<ChartRangeBandSeries>()) {
                var ranges = BuildRanges(range.X, range.Lower, range.Upper);
                if (range.Area) chart.AddRangeArea(range.Name, ranges, range.Color, range.Smooth);
                else chart.AddRangeBand(range.Name, ranges, range.Color);
            }
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
            chart.AddTreemap("Values", list.Cast<ChartTreemap>().Select(item => new CfxTreemapItem(item.Name, item.Value)));
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

    private static CfxTheme CreateTheme(ChartTheme theme) => theme == ChartTheme.Dark ? CfxTheme.ReportDark() : CfxTheme.ReportLight();

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
            switch (series.Kind) {
                case ChartSeriesKind.Heatmap:
                case ChartSeriesKind.HexbinHeatmap:
                case ChartSeriesKind.CalendarHeatmap:
                case ChartSeriesKind.DottedMap:
                case ChartSeriesKind.TileMap:
                case ChartSeriesKind.RegionMap:
                case ChartSeriesKind.Gauge:
                case ChartSeriesKind.Circle:
                case ChartSeriesKind.RadialBar:
                case ChartSeriesKind.LayeredRadial:
                case ChartSeriesKind.Bullet:
                case ChartSeriesKind.Waterfall:
                case ChartSeriesKind.Polar:
                case ChartSeriesKind.Radar:
                case ChartSeriesKind.Funnel:
                case ChartSeriesKind.Treemap:
                case ChartSeriesKind.Timeline:
                case ChartSeriesKind.Gantt:
                case ChartSeriesKind.Sankey:
                case ChartSeriesKind.Tree:
                case ChartSeriesKind.Sunburst:
                case ChartSeriesKind.Pictorial:
                case ChartSeriesKind.ProgressBar:
                case ChartSeriesKind.WordCloud:
                case ChartSeriesKind.Pie:
                case ChartSeriesKind.Donut:
                case ChartSeriesKind.PolarArea:
                    return true;
            }
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

    private static void ApplySharedLineMarkerSize(CfxChart chart, IEnumerable<ChartLine> lines) {
        double? markerRadius = null;
        foreach (var line in lines) {
            if (line.MarkerSize.HasValue && line.MarkerSize.Value < 0) {
                throw new ArgumentOutOfRangeException(nameof(line), line.MarkerSize.Value, "Marker size must be zero or greater.");
            }

            var candidate = Math.Max(0, (line.MarkerSize ?? 0) / 2d);
            if (markerRadius.HasValue && Math.Abs(markerRadius.Value - candidate) > 0.000001) {
                throw new ArgumentException("ImagePlayground line definitions require a shared marker size for all line series because ChartForgeX marker radius is chart-wide.", nameof(lines));
            }

            markerRadius = candidate;
        }

        if (markerRadius.HasValue) {
            chart.Options.Theme.MarkerRadius = markerRadius.Value;
        }
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

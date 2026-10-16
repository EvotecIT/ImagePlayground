using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ChartForgeX.Core;
using CfxChart = ChartForgeX.Core.Chart;

namespace ImagePlayground;

internal static partial class Charts {
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

    private static void AddCombinableSeries(CfxChart chart, ChartDefinition definition, ChartSeriesId id) {
        ChartSeries series;
        switch (definition) {
            case ChartBar bar:
                series = ChartSeries.CreateBar(id, bar.Name, BuildIndexedPoints(bar.Value), bar.Color);
                break;
            case ChartLine line:
                series = ChartSeries.CreateLine(id, line.Name, BuildIndexedPoints(line.Value), line.Color);
                series.Smooth = line.Smooth;
                ApplyLineMarkers(series, line);
                break;
            case ChartStepLine stepLine:
                series = new ChartSeries(id, stepLine.Name, ChartSeriesKind.StepLine, BuildIndexedPoints(stepLine.Value)) { Color = stepLine.Color };
                break;
            case ChartArea area:
                series = ChartSeries.CreateArea(id, area.Name, BuildIndexedPoints(area.Value), area.Color);
                break;
            case ChartStepArea stepArea:
                series = new ChartSeries(id, stepArea.Name, ChartSeriesKind.StepArea, BuildIndexedPoints(stepArea.Value)) { Color = stepArea.Color };
                break;
            case ChartStackedArea stacked:
                series = new ChartSeries(id, stacked.Name, ChartSeriesKind.StackedArea, BuildIndexedPoints(stacked.Value)) { Color = stacked.Color, Smooth = stacked.Smooth };
                break;
            case ChartScatter scatter:
                series = ChartSeries.CreateScatter(id, scatter.Name, BuildXYPoints(scatter.X, scatter.Y), scatter.Color);
                break;
            case ChartBubbleSeries bubble:
                series = ChartSeries.CreateBubble(id, bubble.Name, BuildBubbles(bubble.X, bubble.Y, bubble.Size), bubble.Color);
                break;
            case ChartLollipop lollipop:
                series = new ChartSeries(id, lollipop.Name, ChartSeriesKind.Lollipop, BuildIndexedPoints(lollipop.Value)) { Color = lollipop.Color };
                break;
            case ChartRangeBandSeries range:
                var ranges = BuildRanges(range.X, range.Lower, range.Upper);
                series = range.Area ? ChartSeries.CreateRangeArea(id, range.Name, ranges, range.Color, range.Smooth)
                    : ChartSeries.CreateRangeBand(id, range.Name, ranges, range.Color);
                break;
            default:
                throw new ArgumentException("Unsupported chart definition type '" + definition.GetType().FullName + "'.", nameof(definition));
        }

        if (definition.SecondaryAxis) {
            series.UseSecondaryYAxis();
        }
        chart.AddSeries(series);
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

}

using System;
using System.Collections.Generic;
using ChartForgeX.Primitives;
using CfxChart = ChartForgeX.Core.Chart;
using CfxHeatmapScale = ChartForgeX.Core.ChartHeatmapScale;
using CfxLegendPosition = ChartForgeX.Core.ChartLegendPosition;
using CfxPictorialShape = ChartForgeX.Core.ChartPictorialShape;
using CfxPieLabelContent = ChartForgeX.Core.ChartPieSliceLabelContent;
using CfxTheme = ChartForgeX.Themes.ChartTheme;
using SimpleAnnotation = ChartForgeX.Simple.ChartAnnotationDefinition;
using SimpleChart = ChartForgeX.Simple.Charts;
using SimpleDefinition = ChartForgeX.Simple.ChartDefinition;
using SimpleOptions = ChartForgeX.Simple.ChartRenderOptions;

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
        return SimpleChart.Create(width, height, xTitle, yTitle, showGrid, ResolveTheme(theme), background, ConvertOptions(options));
    }

    /// <summary>Save a ChartForgeX chart to SVG, HTML, or PNG based on the file extension.</summary>
    public static void Save(CfxChart chart, string filePath) {
        SimpleChart.Save(chart, filePath);
    }

    /// <summary>Generate a chart based on provided definitions.</summary>
    public static void Generate(
        IEnumerable<SimpleDefinition> definitions,
        string filePath,
        int width = 600,
        int height = 400,
        ChartBarOptions? barOptions = null,
        string? xTitle = null,
        string? yTitle = null,
        bool showGrid = false,
        ChartTheme theme = ChartTheme.Default,
        IEnumerable<SimpleAnnotation>? annotations = null,
        ChartColor? background = null,
        ChartRenderOptions? options = null) {
        var simpleOptions = ConvertOptions(options);
        if (barOptions?.ShowValuesAboveBars == true && !simpleOptions.ShowDataLabels.HasValue) {
            simpleOptions.ShowDataLabels = true;
        }

        SimpleChart.Generate(definitions, filePath, width, height, xTitle, yTitle, showGrid, ResolveTheme(theme), annotations, background, simpleOptions);
    }

    /// <summary>Apply ImagePlayground render settings to an existing ChartForgeX chart.</summary>
    public static void ApplySettings(CfxChart chart, string? xTitle = null, string? yTitle = null, ChartColor? background = null, ChartRenderOptions? options = null, bool? showGrid = null, ChartTheme? theme = null) {
        if (theme.HasValue) {
            chart.WithTheme(ResolveTheme(theme.Value));
        }

        if (showGrid.HasValue) {
            chart.WithGrid(showGrid.Value);
        }

        SimpleChart.ApplySettings(chart, xTitle, yTitle, background, ConvertOptions(options));
    }

    private static CfxTheme ResolveTheme(ChartTheme theme) => theme == ChartTheme.Dark ? CfxTheme.ReportDark() : CfxTheme.ReportLight();

    private static SimpleOptions ConvertOptions(ChartRenderOptions? options) {
        var converted = new SimpleOptions();
        if (options == null) {
            return converted;
        }

        converted.Palette = options.Palette;
        converted.ShowLegend = options.ShowLegend;
        converted.ShowPointLegend = options.ShowPointLegend;
        if (options.LegendPosition.HasValue) converted.LegendPosition = ResolveLegendPosition(options.LegendPosition.Value);
        converted.ShowHeader = options.ShowHeader;
        converted.ShowCard = options.ShowCard;
        converted.ShowPlotBackground = options.ShowPlotBackground;
        converted.TransparentBackground = options.TransparentBackground;
        converted.ShowAxes = options.ShowAxes;
        converted.ShowXAxis = options.ShowXAxis;
        converted.ShowYAxis = options.ShowYAxis;
        converted.ShowAxisLines = options.ShowAxisLines;
        converted.ShowGrid = options.ShowGrid;
        converted.ShowDataLabels = options.ShowDataLabels;
        converted.TickCount = options.TickCount;
        converted.XAxisMinimum = options.XAxisMinimum;
        converted.XAxisMaximum = options.XAxisMaximum;
        converted.YAxisMinimum = options.YAxisMinimum;
        converted.YAxisMaximum = options.YAxisMaximum;
        if (options.HeatmapScale.HasValue) converted.HeatmapScale = ResolveHeatmapScale(options.HeatmapScale.Value);
        converted.ShowHeatmapScale = options.ShowHeatmapScale;
        converted.ShowHeatmapColumnLabels = options.ShowHeatmapColumnLabels;
        converted.ShowDonutCenterLabel = options.ShowDonutCenterLabel;
        converted.DonutInnerRadiusRatio = options.DonutInnerRadiusRatio;
        converted.DonutCenterValue = options.DonutCenterValue;
        converted.DonutCenterLabel = options.DonutCenterLabel;
        if (options.PieLabelContent.HasValue) converted.PieLabelContent = ResolvePieLabelContent(options.PieLabelContent.Value);
        converted.ShowRadialBarCenterLabel = options.ShowRadialBarCenterLabel;
        converted.ShowCircleStatusLabel = options.ShowCircleStatusLabel;
        converted.ProgressMaximum = options.ProgressMaximum;
        converted.ShowProgressValues = options.ShowProgressValues;
        converted.ShowProgressHandles = options.ShowProgressHandles;
        converted.ProgressBarThicknessRatio = options.ProgressBarThicknessRatio;
        if (options.PictorialSymbol.HasValue) converted.PictorialShape = ResolvePictorialShape(options.PictorialSymbol.Value);
        converted.PictorialColumns = options.PictorialColumns;
        converted.PictorialMaximum = options.PictorialMaximum;
        converted.ShowPictorialValues = options.ShowPictorialValues;
        converted.WordCloudMaximumTerms = options.WordCloudMaximumTerms;
        return converted;
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

    private static CfxPictorialShape ResolvePictorialShape(ChartPictorialSymbol shape) {
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
}

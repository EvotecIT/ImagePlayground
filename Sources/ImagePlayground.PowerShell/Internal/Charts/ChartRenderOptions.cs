using System.Collections.Generic;
using ImageColor = SixLabors.ImageSharp.Color;

namespace ImagePlayground;

/// <summary>Renderer-neutral chart options exposed by ImagePlayground.</summary>
public sealed class ChartRenderOptions {
    /// <summary>Optional color palette used by series and point-based charts.</summary>
    public IList<ImageColor>? Palette { get; set; }

    /// <summary>Whether to render the legend.</summary>
    public bool? ShowLegend { get; set; }

    /// <summary>Whether legends for point-based charts list individual points.</summary>
    public bool? ShowPointLegend { get; set; }

    /// <summary>Legend placement.</summary>
    public ChartLegendPosition? LegendPosition { get; set; }

    /// <summary>Whether to render the chart header.</summary>
    public bool? ShowHeader { get; set; }

    /// <summary>Whether to render the outer chart card.</summary>
    public bool? ShowCard { get; set; }

    /// <summary>Whether to render the plot background.</summary>
    public bool? ShowPlotBackground { get; set; }

    /// <summary>Whether the chart background should be transparent.</summary>
    public bool? TransparentBackground { get; set; }

    /// <summary>Whether to render axes and tick labels.</summary>
    public bool? ShowAxes { get; set; }

    /// <summary>Whether to render the x-axis.</summary>
    public bool? ShowXAxis { get; set; }

    /// <summary>Whether to render the y-axis.</summary>
    public bool? ShowYAxis { get; set; }

    /// <summary>Whether to render axis rules.</summary>
    public bool? ShowAxisLines { get; set; }

    /// <summary>Whether to render grid lines.</summary>
    public bool? ShowGrid { get; set; }

    /// <summary>Whether to render data labels for supported charts.</summary>
    public bool? ShowDataLabels { get; set; }

    /// <summary>Preferred number of axis ticks.</summary>
    public int? TickCount { get; set; }

    /// <summary>Explicit x-axis minimum.</summary>
    public double? XAxisMinimum { get; set; }

    /// <summary>Explicit x-axis maximum.</summary>
    public double? XAxisMaximum { get; set; }

    /// <summary>Explicit y-axis minimum.</summary>
    public double? YAxisMinimum { get; set; }

    /// <summary>Explicit y-axis maximum.</summary>
    public double? YAxisMaximum { get; set; }

    /// <summary>Heatmap color scale.</summary>
    public ChartHeatmapColorScale? HeatmapScale { get; set; }

    /// <summary>Whether to render the heatmap scale legend.</summary>
    public bool? ShowHeatmapScale { get; set; }

    /// <summary>Whether to render heatmap column labels.</summary>
    public bool? ShowHeatmapColumnLabels { get; set; }

    /// <summary>Whether to render the donut center label.</summary>
    public bool? ShowDonutCenterLabel { get; set; }

    /// <summary>Donut hole radius ratio.</summary>
    public double? DonutInnerRadiusRatio { get; set; }

    /// <summary>Optional donut center value text.</summary>
    public string? DonutCenterValue { get; set; }

    /// <summary>Optional donut center label text.</summary>
    public string? DonutCenterLabel { get; set; }

    /// <summary>Pie and donut slice label content.</summary>
    public ChartPieLabelContent? PieLabelContent { get; set; }

    /// <summary>Whether to render radial-bar center labels.</summary>
    public bool? ShowRadialBarCenterLabel { get; set; }

    /// <summary>Whether to render circle status labels.</summary>
    public bool? ShowCircleStatusLabel { get; set; }

    /// <summary>Progress maximum used by progress-bar charts.</summary>
    public double? ProgressMaximum { get; set; }

    /// <summary>Whether to render progress-bar values.</summary>
    public bool? ShowProgressValues { get; set; }

    /// <summary>Whether to render progress-bar handles.</summary>
    public bool? ShowProgressHandles { get; set; }

    /// <summary>Progress-bar thickness ratio.</summary>
    public double? ProgressBarThicknessRatio { get; set; }

    /// <summary>Pictorial chart symbol.</summary>
    public ChartPictorialSymbol? PictorialSymbol { get; set; }

    /// <summary>Number of pictorial symbols per row.</summary>
    public int? PictorialColumns { get; set; }

    /// <summary>Optional pictorial maximum.</summary>
    public double? PictorialMaximum { get; set; }

    /// <summary>Whether to render pictorial values.</summary>
    public bool? ShowPictorialValues { get; set; }

    /// <summary>Maximum number of word cloud terms.</summary>
    public int? WordCloudMaximumTerms { get; set; }
}

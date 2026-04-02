using System.Drawing;

namespace ImagePlayground.Gdi.Charts;

/// <summary>Options for heatmap charts.</summary>
public sealed class ChartHeatmapOptions {
    /// <summary>Canvas configuration.</summary>
    public ChartCanvas Canvas { get; set; } = new();

    /// <summary>Chart style.</summary>
    public ChartStyle Style { get; set; } = new();

    /// <summary>Heatmap values.</summary>
    public double[,] Values { get; set; } = new double[0, 0];

    /// <summary>Optional minimum value override.</summary>
    public double? Min { get; set; }

    /// <summary>Optional maximum value override.</summary>
    public double? Max { get; set; }

    /// <summary>Color gradient.</summary>
    public ColorGradient Gradient { get; set; } = new(new[] {
        new ColorStop(0f, Color.DarkBlue),
        new ColorStop(0.5f, Color.LimeGreen),
        new ColorStop(1f, Color.Red)
    });

    /// <summary>Legend options.</summary>
    public ChartLegend Legend { get; set; } = new() { Show = false };

    /// <summary>Title options.</summary>
    public ChartTitle Title { get; set; } = new();
}

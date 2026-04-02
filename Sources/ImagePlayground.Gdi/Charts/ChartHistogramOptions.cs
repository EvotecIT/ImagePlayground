using System.Collections.Generic;
using System.Drawing;

namespace ImagePlayground.Gdi.Charts;

/// <summary>Options for histogram charts.</summary>
public sealed class ChartHistogramOptions {
    /// <summary>Canvas configuration.</summary>
    public ChartCanvas Canvas { get; set; } = new();

    /// <summary>Chart style.</summary>
    public ChartStyle Style { get; set; } = new();

    /// <summary>Values to bin.</summary>
    public IReadOnlyList<double> Values { get; set; } = Array.Empty<double>();

    /// <summary>Number of bins.</summary>
    public int BinCount { get; set; } = 10;

    /// <summary>Histogram bar color.</summary>
    public Color Color { get; set; } = Color.SteelBlue;

    /// <summary>Axis configuration.</summary>
    public ChartAxis YAxis { get; set; } = new();

    /// <summary>Legend options.</summary>
    public ChartLegend Legend { get; set; } = new() { Show = false };

    /// <summary>Title options.</summary>
    public ChartTitle Title { get; set; } = new();
}

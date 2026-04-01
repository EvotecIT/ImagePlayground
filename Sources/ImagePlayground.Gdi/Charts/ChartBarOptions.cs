using System.Collections.Generic;
using System.Drawing;

namespace ImagePlayground.Gdi.Charts;

/// <summary>Defines a bar chart series.</summary>
public sealed class ChartBarSeries {
    /// <summary>Series name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Series values.</summary>
    public IReadOnlyList<double> Values { get; set; } = Array.Empty<double>();

    /// <summary>Series color.</summary>
    public Color Color { get; set; } = Color.SteelBlue;
}

/// <summary>Options for rendering bar charts.</summary>
public sealed class ChartBarOptions {
    /// <summary>Canvas configuration.</summary>
    public ChartCanvas Canvas { get; set; } = new();

    /// <summary>Chart style.</summary>
    public ChartStyle Style { get; set; } = new();

    /// <summary>Axis configuration.</summary>
    public ChartAxis YAxis { get; set; } = new();

    /// <summary>Categories for each bar group.</summary>
    public IReadOnlyList<string> Categories { get; set; } = Array.Empty<string>();

    /// <summary>Series to plot.</summary>
    public IReadOnlyList<ChartBarSeries> Series { get; set; } = Array.Empty<ChartBarSeries>();

    /// <summary>Whether bars are stacked.</summary>
    public bool Stacked { get; set; }

    /// <summary>Spacing between bar groups (0-1).</summary>
    public float GroupGap { get; set; } = 0.2f;

    /// <summary>Spacing between bars within a group (0-1).</summary>
    public float BarGap { get; set; } = 0.1f;

    /// <summary>Legend options.</summary>
    public ChartLegend Legend { get; set; } = new();

    /// <summary>Title options.</summary>
    public ChartTitle Title { get; set; } = new();
}

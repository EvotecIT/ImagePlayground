using System.Collections.Generic;
using System.Drawing;

namespace ImagePlayground.Gdi.Charts;

/// <summary>Defines a pie slice.</summary>
public sealed class ChartPieSlice {
    /// <summary>Slice label.</summary>
    public string Label { get; set; } = string.Empty;

    /// <summary>Slice value.</summary>
    public double Value { get; set; }

    /// <summary>Slice color.</summary>
    public Color Color { get; set; } = Color.SteelBlue;
}

/// <summary>Options for pie and donut charts.</summary>
public sealed class ChartPieOptions {
    /// <summary>Canvas configuration.</summary>
    public ChartCanvas Canvas { get; set; } = new();

    /// <summary>Chart style.</summary>
    public ChartStyle Style { get; set; } = new();

    /// <summary>Pie slices.</summary>
    public IReadOnlyList<ChartPieSlice> Slices { get; set; } = Array.Empty<ChartPieSlice>();

    /// <summary>Legend options.</summary>
    public ChartLegend Legend { get; set; } = new();

    /// <summary>Title options.</summary>
    public ChartTitle Title { get; set; } = new();

    /// <summary>Inner radius ratio for donut charts (0 = pie).</summary>
    public float InnerRadiusRatio { get; set; }
}

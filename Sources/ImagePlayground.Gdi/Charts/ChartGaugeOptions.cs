using System.Collections.Generic;
using System.Drawing;

namespace ImagePlayground.Gdi.Charts;

/// <summary>Defines a gauge range.</summary>
public sealed class ChartGaugeRange {
    /// <summary>Start value.</summary>
    public double Start { get; set; }

    /// <summary>End value.</summary>
    public double End { get; set; }

    /// <summary>Range color.</summary>
    public Color Color { get; set; } = Color.LimeGreen;
}

/// <summary>Options for gauge charts.</summary>
public sealed class ChartGaugeOptions {
    /// <summary>Canvas configuration.</summary>
    public ChartCanvas Canvas { get; set; } = new();

    /// <summary>Chart style.</summary>
    public ChartStyle Style { get; set; } = new();

    /// <summary>Minimum value.</summary>
    public double Min { get; set; }

    /// <summary>Maximum value.</summary>
    public double Max { get; set; } = 100;

    /// <summary>Current value.</summary>
    public double Value { get; set; }

    /// <summary>Gauge ranges.</summary>
    public IReadOnlyList<ChartGaugeRange> Ranges { get; set; } = Array.Empty<ChartGaugeRange>();

    /// <summary>Title options.</summary>
    public ChartTitle Title { get; set; } = new();
}

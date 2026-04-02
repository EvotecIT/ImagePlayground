using System.Collections.Generic;
using System.Drawing;

namespace ImagePlayground.Gdi.Charts;

/// <summary>Represents a scatter point.</summary>
public readonly struct ChartPoint {
    /// <summary>X value.</summary>
    public double X { get; }

    /// <summary>Y value.</summary>
    public double Y { get; }

    /// <summary>Creates a new point.</summary>
    public ChartPoint(double x, double y) {
        X = x;
        Y = y;
    }
}

/// <summary>Represents a bubble point.</summary>
public readonly struct ChartBubblePoint {
    /// <summary>X value.</summary>
    public double X { get; }

    /// <summary>Y value.</summary>
    public double Y { get; }

    /// <summary>Bubble size.</summary>
    public double Size { get; }

    /// <summary>Creates a new bubble point.</summary>
    public ChartBubblePoint(double x, double y, double size) {
        X = x;
        Y = y;
        Size = size;
    }
}

/// <summary>Defines a scatter series.</summary>
public sealed class ChartScatterSeries {
    /// <summary>Series name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Series points.</summary>
    public IReadOnlyList<ChartPoint> Points { get; set; } = Array.Empty<ChartPoint>();

    /// <summary>Series color.</summary>
    public Color Color { get; set; } = Color.SteelBlue;

    /// <summary>Marker size in pixels.</summary>
    public float MarkerSize { get; set; } = 6f;
}

/// <summary>Defines a bubble series.</summary>
public sealed class ChartBubbleSeries {
    /// <summary>Series name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Series points.</summary>
    public IReadOnlyList<ChartBubblePoint> Points { get; set; } = Array.Empty<ChartBubblePoint>();

    /// <summary>Series color.</summary>
    public Color Color { get; set; } = Color.CornflowerBlue;

    /// <summary>Minimum bubble radius in pixels.</summary>
    public float MinRadius { get; set; } = 6f;

    /// <summary>Maximum bubble radius in pixels.</summary>
    public float MaxRadius { get; set; } = 24f;
}

/// <summary>Options for rendering scatter charts.</summary>
public sealed class ChartScatterOptions {
    /// <summary>Canvas configuration.</summary>
    public ChartCanvas Canvas { get; set; } = new();

    /// <summary>Chart style.</summary>
    public ChartStyle Style { get; set; } = new();

    /// <summary>X axis configuration.</summary>
    public ChartAxis XAxis { get; set; } = new();

    /// <summary>Y axis configuration.</summary>
    public ChartAxis YAxis { get; set; } = new();

    /// <summary>Scatter series.</summary>
    public IReadOnlyList<ChartScatterSeries> Series { get; set; } = Array.Empty<ChartScatterSeries>();

    /// <summary>Legend options.</summary>
    public ChartLegend Legend { get; set; } = new();

    /// <summary>Title options.</summary>
    public ChartTitle Title { get; set; } = new();
}

/// <summary>Options for rendering bubble charts.</summary>
public sealed class ChartBubbleOptions {
    /// <summary>Canvas configuration.</summary>
    public ChartCanvas Canvas { get; set; } = new();

    /// <summary>Chart style.</summary>
    public ChartStyle Style { get; set; } = new();

    /// <summary>X axis configuration.</summary>
    public ChartAxis XAxis { get; set; } = new();

    /// <summary>Y axis configuration.</summary>
    public ChartAxis YAxis { get; set; } = new();

    /// <summary>Bubble series.</summary>
    public IReadOnlyList<ChartBubbleSeries> Series { get; set; } = Array.Empty<ChartBubbleSeries>();

    /// <summary>Legend options.</summary>
    public ChartLegend Legend { get; set; } = new();

    /// <summary>Title options.</summary>
    public ChartTitle Title { get; set; } = new();
}

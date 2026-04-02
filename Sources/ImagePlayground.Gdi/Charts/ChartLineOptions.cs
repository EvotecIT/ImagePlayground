using System.Collections.Generic;
using System.Drawing;

namespace ImagePlayground.Gdi.Charts;

/// <summary>Supported marker shapes.</summary>
public enum ChartMarkerShape {
    /// <summary>No markers.</summary>
    None,
    /// <summary>Circle marker.</summary>
    Circle,
    /// <summary>Square marker.</summary>
    Square,
    /// <summary>Diamond marker.</summary>
    Diamond
}

/// <summary>Defines a line or area series.</summary>
public sealed class ChartLineSeries {
    /// <summary>Series name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Series values.</summary>
    public IReadOnlyList<double> Values { get; set; } = Array.Empty<double>();

    /// <summary>Series color.</summary>
    public Color Color { get; set; } = Color.SteelBlue;

    /// <summary>Line thickness.</summary>
    public float Thickness { get; set; } = 2f;

    /// <summary>Marker size in pixels.</summary>
    public float MarkerSize { get; set; } = 4f;

    /// <summary>Marker shape.</summary>
    public ChartMarkerShape MarkerShape { get; set; } = ChartMarkerShape.None;

    /// <summary>Optional fill color for area charts.</summary>
    public Color? FillColor { get; set; }
}

/// <summary>Options for rendering line/area charts.</summary>
public sealed class ChartLineOptions {
    /// <summary>Canvas configuration.</summary>
    public ChartCanvas Canvas { get; set; } = new();

    /// <summary>Chart style.</summary>
    public ChartStyle Style { get; set; } = new();

    /// <summary>Axis configuration.</summary>
    public ChartAxis YAxis { get; set; } = new();

    /// <summary>Optional X axis labels.</summary>
    public IReadOnlyList<string> Categories { get; set; } = Array.Empty<string>();

    /// <summary>Series to plot.</summary>
    public IReadOnlyList<ChartLineSeries> Series { get; set; } = Array.Empty<ChartLineSeries>();

    /// <summary>Legend options.</summary>
    public ChartLegend Legend { get; set; } = new();

    /// <summary>Title options.</summary>
    public ChartTitle Title { get; set; } = new();

    /// <summary>When true, fills area under each line.</summary>
    public bool FillArea { get; set; }
}

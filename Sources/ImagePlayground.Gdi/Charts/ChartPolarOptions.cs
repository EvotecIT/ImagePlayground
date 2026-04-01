using System.Collections.Generic;
using System.Drawing;

namespace ImagePlayground.Gdi.Charts;

/// <summary>Defines a polar point.</summary>
public readonly struct ChartPolarPoint {
    /// <summary>Angle in degrees.</summary>
    public double Angle { get; }

    /// <summary>Radius value.</summary>
    public double Radius { get; }

    /// <summary>Creates a polar point.</summary>
    public ChartPolarPoint(double angle, double radius) {
        Angle = angle;
        Radius = radius;
    }
}

/// <summary>Defines a polar series.</summary>
public sealed class ChartPolarSeries {
    /// <summary>Series name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Series points.</summary>
    public IReadOnlyList<ChartPolarPoint> Points { get; set; } = Array.Empty<ChartPolarPoint>();

    /// <summary>Series color.</summary>
    public Color Color { get; set; } = Color.SteelBlue;

    /// <summary>Line thickness.</summary>
    public float Thickness { get; set; } = 2f;
}

/// <summary>Options for polar charts.</summary>
public sealed class ChartPolarOptions {
    /// <summary>Canvas configuration.</summary>
    public ChartCanvas Canvas { get; set; } = new();

    /// <summary>Chart style.</summary>
    public ChartStyle Style { get; set; } = new();

    /// <summary>Polar series.</summary>
    public IReadOnlyList<ChartPolarSeries> Series { get; set; } = Array.Empty<ChartPolarSeries>();

    /// <summary>Legend options.</summary>
    public ChartLegend Legend { get; set; } = new();

    /// <summary>Title options.</summary>
    public ChartTitle Title { get; set; } = new();
}

/// <summary>Defines a radial series.</summary>
public sealed class ChartRadialSeries {
    /// <summary>Series name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Series values.</summary>
    public IReadOnlyList<double> Values { get; set; } = Array.Empty<double>();

    /// <summary>Series color.</summary>
    public Color Color { get; set; } = Color.SteelBlue;
}

/// <summary>Options for radial bar charts.</summary>
public sealed class ChartRadialOptions {
    /// <summary>Canvas configuration.</summary>
    public ChartCanvas Canvas { get; set; } = new();

    /// <summary>Chart style.</summary>
    public ChartStyle Style { get; set; } = new();

    /// <summary>Categories.</summary>
    public IReadOnlyList<string> Categories { get; set; } = Array.Empty<string>();

    /// <summary>Radial series.</summary>
    public IReadOnlyList<ChartRadialSeries> Series { get; set; } = Array.Empty<ChartRadialSeries>();

    /// <summary>Legend options.</summary>
    public ChartLegend Legend { get; set; } = new();

    /// <summary>Title options.</summary>
    public ChartTitle Title { get; set; } = new();
}

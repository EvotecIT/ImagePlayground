using System.Collections.Generic;
using ChartForgeX.Primitives;

namespace ImagePlayground;

/// <summary>Polar chart definition expressed as angle and radius pairs.</summary>
public sealed class ChartPolar : ChartDefinition {
    /// <summary>Angle values in radians.</summary>
    public IList<double> Angle { get; }

    /// <summary>Radius values.</summary>
    public IList<double> Radius { get; }

    /// <summary>Line color.</summary>
    public ChartColor? Color { get; }

    /// <summary>Create a polar chart definition.</summary>
    /// <param name="name">Series name.</param>
    /// <param name="angle">Angle values in radians.</param>
    /// <param name="radius">Radius values.</param>
    /// <param name="color">Optional line color.</param>
    public ChartPolar(string name, IList<double> angle, IList<double> radius, ChartColor? color = null) : base(name) {
        Angle = angle;
        Radius = radius;
        Color = color;
    }
}

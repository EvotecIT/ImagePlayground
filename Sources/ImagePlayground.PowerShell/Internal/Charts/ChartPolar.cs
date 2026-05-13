using System.Collections.Generic;
using ChartForgeX.Primitives;

namespace ImagePlayground;

/// <summary>Polar plot definition.</summary>
internal sealed class ChartPolar : ChartDefinition {
    /// <summary>Angle values.</summary>
    public IList<double> Angle { get; }

    /// <summary>Radius values.</summary>
    public IList<double> Value { get; }

    /// <summary>Line color.</summary>
    public ChartColor? Color { get; }

    /// <summary>Create a polar chart definition.</summary>
    /// <param name="name">Series name.</param>
    /// <param name="angle">Angle values.</param>
    /// <param name="value">Radius values.</param>
    /// <param name="color">Optional line color.</param>
    public ChartPolar(string name, IList<double> angle, IList<double> value, ChartColor? color = null) : base(ChartDefinitionType.Polar, name) {
        Angle = angle;
        Value = value;
        Color = color;
    }
}

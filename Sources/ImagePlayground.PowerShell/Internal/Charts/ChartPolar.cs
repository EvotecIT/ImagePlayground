using System.Collections.Generic;
using ImageColor = SixLabors.ImageSharp.Color;

namespace ImagePlayground;

/// <summary>Polar plot definition.</summary>
public sealed class ChartPolar : ChartDefinition {
    /// <summary>Angle values.</summary>
    public IList<double> Angle { get; }

    /// <summary>Radius values.</summary>
    public IList<double> Value { get; }

    /// <summary>Line color.</summary>
    public ImageColor? Color { get; }

    /// <summary>Create a polar chart definition.</summary>
    /// <param name="name">Series name.</param>
    /// <param name="angle">Angle values.</param>
    /// <param name="value">Radius values.</param>
    /// <param name="color">Optional line color.</param>
    public ChartPolar(string name, IList<double> angle, IList<double> value, ImageColor? color = null) : base(ChartDefinitionType.Polar, name) {
        Angle = angle;
        Value = value;
        Color = color;
    }
}


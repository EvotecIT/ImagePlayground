using System.Collections.Generic;
using ImageColor = SixLabors.ImageSharp.Color;

namespace ImagePlayground;

/// <summary>Area chart definition.</summary>
public sealed class ChartArea : ChartDefinition {
    /// <summary>Area values.</summary>
    public IList<double> Value { get; }

    /// <summary>Fill color.</summary>
    public ImageColor? Color { get; }

    /// <summary>Create an area chart definition.</summary>
    /// <param name="name">Series name.</param>
    /// <param name="value">Y values.</param>
    /// <param name="color">Optional fill color.</param>
    public ChartArea(string name, IList<double> value, ImageColor? color = null) : base(ChartDefinitionType.Area, name) {
        Value = value;
        Color = color;
    }
}

using System.Collections.Generic;
using ImageColor = SixLabors.ImageSharp.Color;

namespace ImagePlayground;

/// <summary>Bar chart definition.</summary>
public sealed class ChartBar : ChartDefinition {
    /// <summary>Bar values.</summary>
    public IList<double> Value { get; }

    /// <summary>Bar color.</summary>
    public ImageColor? Color { get; }

    /// <summary>Create a bar chart definition.</summary>
    /// <param name="name">Series name.</param>
    /// <param name="value">Bar values.</param>
    /// <param name="color">Optional bar color.</param>
    public ChartBar(string name, IList<double> value, ImageColor? color = null) : base(ChartDefinitionType.Bar, name) {
        Value = value;
        Color = color;
    }
}


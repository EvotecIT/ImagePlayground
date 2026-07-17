using System.Collections.Generic;
using ChartForgeX.Primitives;

namespace ImagePlayground;

/// <summary>Horizontal bar chart definition.</summary>
public sealed class ChartHorizontalBar : ChartDefinition {
    /// <summary>Bar values.</summary>
    public IList<double> Value { get; }

    /// <summary>Bar color.</summary>
    public ChartColor? Color { get; }

    /// <summary>Create a horizontal bar chart definition.</summary>
    /// <param name="name">Series name.</param>
    /// <param name="value">Bar values.</param>
    /// <param name="color">Optional bar color.</param>
    public ChartHorizontalBar(string name, IList<double> value, ChartColor? color = null) : base(name) {
        Value = value;
        Color = color;
    }
}

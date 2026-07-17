using System.Collections.Generic;
using ChartForgeX.Primitives;

namespace ImagePlayground;

/// <summary>Stacked area chart definition.</summary>
public sealed class ChartStackedArea : ChartDefinition {
    /// <summary>Area values.</summary>
    public IList<double> Value { get; }

    /// <summary>Area color.</summary>
    public ChartColor? Color { get; }

    /// <summary>Render the area using a smooth curve.</summary>
    public bool Smooth { get; }

    /// <summary>Create a stacked area chart definition.</summary>
    /// <param name="name">Series name.</param>
    /// <param name="value">Area values.</param>
    /// <param name="color">Optional area color.</param>
    /// <param name="smooth">Render as a smooth curve.</param>
    public ChartStackedArea(string name, IList<double> value, ChartColor? color = null, bool smooth = false) : base(name) {
        Value = value;
        Color = color;
        Smooth = smooth;
    }
}

using System.Collections.Generic;
using ChartForgeX.Primitives;

namespace ImagePlayground;

/// <summary>Radar chart definition.</summary>
public sealed class ChartRadar : ChartDefinition {
    /// <summary>Category values.</summary>
    public IList<double> Category { get; }

    /// <summary>Radar values.</summary>
    public IList<double> Value { get; }

    /// <summary>Line color.</summary>
    public ChartColor? Color { get; }

    /// <summary>Create a radar chart definition.</summary>
    /// <param name="name">Series name.</param>
    /// <param name="category">Category values.</param>
    /// <param name="value">Radar values.</param>
    /// <param name="color">Optional line color.</param>
    public ChartRadar(string name, IList<double> category, IList<double> value, ChartColor? color = null) : base(name) {
        Category = category;
        Value = value;
        Color = color;
    }
}

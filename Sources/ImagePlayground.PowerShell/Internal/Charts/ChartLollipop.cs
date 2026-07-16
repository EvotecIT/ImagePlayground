using System.Collections.Generic;
using ChartForgeX.Primitives;

namespace ImagePlayground;

/// <summary>Lollipop chart definition.</summary>
public sealed class ChartLollipop : ChartDefinition {
    /// <summary>Lollipop values.</summary>
    public IList<double> Value { get; }

    /// <summary>Lollipop color.</summary>
    public ChartColor? Color { get; }

    /// <summary>Create a lollipop chart definition.</summary>
    /// <param name="name">Series name.</param>
    /// <param name="value">Lollipop values.</param>
    /// <param name="color">Optional lollipop color.</param>
    public ChartLollipop(string name, IList<double> value, ChartColor? color = null) : base(name) {
        Value = value;
        Color = color;
    }
}

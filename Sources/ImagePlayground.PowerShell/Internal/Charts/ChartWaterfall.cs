using System.Collections.Generic;
using ChartForgeX.Primitives;

namespace ImagePlayground;

/// <summary>Waterfall chart definition.</summary>
public sealed class ChartWaterfall : ChartDefinition {
    /// <summary>Delta values.</summary>
    public IList<double> Value { get; }

    /// <summary>Optional step labels.</summary>
    public IList<string>? Labels { get; }

    /// <summary>Waterfall color.</summary>
    public ChartColor? Color { get; }

    /// <summary>Create a waterfall chart definition.</summary>
    /// <param name="name">Series name.</param>
    /// <param name="value">Delta values.</param>
    /// <param name="labels">Optional step labels.</param>
    /// <param name="color">Optional waterfall color.</param>
    public ChartWaterfall(string name, IList<double> value, IList<string>? labels = null, ChartColor? color = null) : base(name) {
        Value = value;
        Labels = labels;
        Color = color;
    }
}

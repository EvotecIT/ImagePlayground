using System.Collections.Generic;
using ChartForgeX.Primitives;

namespace ImagePlayground;

/// <summary>Step area chart definition.</summary>
public sealed class ChartStepArea : ChartDefinition {
    /// <summary>Area values.</summary>
    public IList<double> Value { get; }

    /// <summary>Area color.</summary>
    public ChartColor? Color { get; }

    /// <summary>Create a step area chart definition.</summary>
    /// <param name="name">Series name.</param>
    /// <param name="value">Area values.</param>
    /// <param name="color">Optional area color.</param>
    public ChartStepArea(string name, IList<double> value, ChartColor? color = null) : base(name) {
        Value = value;
        Color = color;
    }
}

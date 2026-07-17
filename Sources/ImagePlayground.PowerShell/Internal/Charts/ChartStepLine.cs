using System.Collections.Generic;
using ChartForgeX.Primitives;

namespace ImagePlayground;

/// <summary>Step line chart definition.</summary>
public sealed class ChartStepLine : ChartDefinition {
    /// <summary>Line values.</summary>
    public IList<double> Value { get; }

    /// <summary>Line color.</summary>
    public ChartColor? Color { get; }

    /// <summary>Create a step line chart definition.</summary>
    /// <param name="name">Series name.</param>
    /// <param name="value">Line values.</param>
    /// <param name="color">Optional line color.</param>
    public ChartStepLine(string name, IList<double> value, ChartColor? color = null) : base(name) {
        Value = value;
        Color = color;
    }
}

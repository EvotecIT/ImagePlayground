using System.Collections.Generic;
using ChartForgeX.Primitives;

namespace ImagePlayground;

/// <summary>Range bar chart definition.</summary>
public sealed class ChartRangeBar : ChartDefinition {
    /// <summary>X or category values.</summary>
    public IList<double> X { get; }

    /// <summary>Interval start values.</summary>
    public IList<double> Start { get; }

    /// <summary>Interval end values.</summary>
    public IList<double> End { get; }

    /// <summary>Range bar color.</summary>
    public ChartColor? Color { get; }

    /// <summary>Create a range bar chart definition.</summary>
    /// <param name="name">Series name.</param>
    /// <param name="x">X or category values.</param>
    /// <param name="start">Interval start values.</param>
    /// <param name="end">Interval end values.</param>
    /// <param name="color">Optional range bar color.</param>
    public ChartRangeBar(string name, IList<double> x, IList<double> start, IList<double> end, ChartColor? color = null) : base(name) {
        X = x;
        Start = start;
        End = end;
        Color = color;
    }
}

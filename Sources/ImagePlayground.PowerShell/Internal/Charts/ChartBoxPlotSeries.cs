using System.Collections.Generic;
using ChartForgeX.Primitives;

namespace ImagePlayground;

/// <summary>Box plot chart definition.</summary>
public sealed class ChartBoxPlotSeries : ChartDefinition {
    /// <summary>X or category values.</summary>
    public IList<double> X { get; }

    /// <summary>Minimum whisker values.</summary>
    public IList<double> Minimum { get; }

    /// <summary>First quartile values.</summary>
    public IList<double> Q1 { get; }

    /// <summary>Median values.</summary>
    public IList<double> Median { get; }

    /// <summary>Third quartile values.</summary>
    public IList<double> Q3 { get; }

    /// <summary>Maximum whisker values.</summary>
    public IList<double> Maximum { get; }

    /// <summary>Box plot color.</summary>
    public ChartColor? Color { get; }

    /// <summary>Create a box plot chart definition.</summary>
    /// <param name="name">Series name.</param>
    /// <param name="x">X or category values.</param>
    /// <param name="minimum">Minimum whisker values.</param>
    /// <param name="q1">First quartile values.</param>
    /// <param name="median">Median values.</param>
    /// <param name="q3">Third quartile values.</param>
    /// <param name="maximum">Maximum whisker values.</param>
    /// <param name="color">Optional box plot color.</param>
    public ChartBoxPlotSeries(string name, IList<double> x, IList<double> minimum, IList<double> q1, IList<double> median, IList<double> q3, IList<double> maximum, ChartColor? color = null) : base(name) {
        X = x;
        Minimum = minimum;
        Q1 = q1;
        Median = median;
        Q3 = q3;
        Maximum = maximum;
        Color = color;
    }
}

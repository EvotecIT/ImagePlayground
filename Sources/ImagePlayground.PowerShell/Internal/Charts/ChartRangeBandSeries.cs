using System.Collections.Generic;
using ChartForgeX.Primitives;

namespace ImagePlayground;

/// <summary>Range band chart definition.</summary>
public sealed class ChartRangeBandSeries : ChartDefinition {
    /// <summary>X or category values.</summary>
    public IList<double> X { get; }

    /// <summary>Lower values.</summary>
    public IList<double> Lower { get; }

    /// <summary>Upper values.</summary>
    public IList<double> Upper { get; }

    /// <summary>Range band color.</summary>
    public ChartColor? Color { get; }

    /// <summary>Render the range as an area instead of a band.</summary>
    public bool Area { get; }

    /// <summary>Render range-area boundaries using smooth curves.</summary>
    public bool Smooth { get; }

    /// <summary>Create a range band chart definition.</summary>
    /// <param name="name">Series name.</param>
    /// <param name="x">X or category values.</param>
    /// <param name="lower">Lower values.</param>
    /// <param name="upper">Upper values.</param>
    /// <param name="color">Optional range band color.</param>
    /// <param name="area">Render as a range area.</param>
    /// <param name="smooth">Render range-area boundaries using smooth curves.</param>
    public ChartRangeBandSeries(string name, IList<double> x, IList<double> lower, IList<double> upper, ChartColor? color = null, bool area = false, bool smooth = true) : base(name) {
        X = x;
        Lower = lower;
        Upper = upper;
        Color = color;
        Area = area;
        Smooth = smooth;
    }
}

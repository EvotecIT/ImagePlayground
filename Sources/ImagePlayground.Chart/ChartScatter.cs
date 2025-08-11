using System.Collections.Generic;
using ImageColor = SixLabors.ImageSharp.Color;

namespace ImagePlayground;

/// <summary>Scatter chart definition.</summary>
public sealed class ChartScatter : ChartDefinition {
    /// <summary>X values.</summary>
    public IList<double> X { get; }

    /// <summary>Y values.</summary>
    public IList<double> Y { get; }

    /// <summary>Point color.</summary>
    public ImageColor? Color { get; }

    /// <summary>Create a scatter chart definition.</summary>
    /// <param name="name">Series name.</param>
    /// <param name="x">X data points.</param>
    /// <param name="y">Y data points.</param>
    /// <param name="color">Optional point color.</param>
    public ChartScatter(string name, IList<double> x, IList<double> y, ImageColor? color = null) : base(ChartDefinitionType.Scatter, name) {
        X = x;
        Y = y;
        Color = color;
    }
}


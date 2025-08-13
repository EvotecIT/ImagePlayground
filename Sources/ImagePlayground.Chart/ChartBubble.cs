using System.Collections.Generic;
using ImageColor = SixLabors.ImageSharp.Color;

namespace ImagePlayground;

/// <summary>Bubble chart definition.</summary>
public sealed class ChartBubble : ChartDefinition {
    /// <summary>X values.</summary>
    public IList<double> X { get; }

    /// <summary>Y values.</summary>
    public IList<double> Y { get; }

    /// <summary>Size values.</summary>
    public IList<double> Size { get; }

    /// <summary>Bubble color.</summary>
    public ImageColor? Color { get; }

    /// <summary>Create a bubble chart definition.</summary>
    /// <param name="name">Series name.</param>
    /// <param name="x">X data points.</param>
    /// <param name="y">Y data points.</param>
    /// <param name="size">Bubble sizes.</param>
    /// <param name="color">Optional bubble color.</param>
    public ChartBubble(string name, IList<double> x, IList<double> y, IList<double> size, ImageColor? color = null) : base(ChartDefinitionType.Bubble, name) {
        X = x;
        Y = y;
        Size = size;
        Color = color;
    }
}

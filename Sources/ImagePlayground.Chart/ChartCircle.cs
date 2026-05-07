using ImageColor = SixLabors.ImageSharp.Color;

namespace ImagePlayground;

/// <summary>Circle status chart definition.</summary>
public sealed class ChartCircle : ChartDefinition {
    /// <summary>Circle value.</summary>
    public double Value { get; }

    /// <summary>Circle minimum.</summary>
    public double Minimum { get; }

    /// <summary>Circle maximum.</summary>
    public double Maximum { get; }

    /// <summary>Circle color.</summary>
    public ImageColor? Color { get; }

    /// <summary>Create a circle status chart definition.</summary>
    /// <param name="name">Circle label.</param>
    /// <param name="value">Circle value.</param>
    /// <param name="minimum">Circle minimum.</param>
    /// <param name="maximum">Circle maximum.</param>
    /// <param name="color">Optional circle color.</param>
    public ChartCircle(string name, double value, double minimum = 0, double maximum = 100, ImageColor? color = null) : base(ChartDefinitionType.Circle, name) {
        Value = value;
        Minimum = minimum;
        Maximum = maximum;
        Color = color;
    }
}

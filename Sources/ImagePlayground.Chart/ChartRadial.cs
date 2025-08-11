using ImageColor = SixLabors.ImageSharp.Color;

namespace ImagePlayground;

/// <summary>Radial gauge chart definition.</summary>
public sealed class ChartRadial : ChartDefinition {
    /// <summary>Gauge value.</summary>
    public double Value { get; }

    /// <summary>Gauge color.</summary>
    public ImageColor? Color { get; }

    /// <summary>Create a radial gauge definition.</summary>
    /// <param name="name">Gauge label.</param>
    /// <param name="value">Gauge value.</param>
    /// <param name="color">Optional gauge color.</param>
    public ChartRadial(string name, double value, ImageColor? color = null) : base(ChartDefinitionType.Radial, name) {
        Value = value;
        Color = color;
    }
}


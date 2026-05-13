using ImageColor = SixLabors.ImageSharp.Color;

namespace ImagePlayground;

/// <summary>Pie chart definition.</summary>
public sealed class ChartPie : ChartDefinition {
    /// <summary>Slice value.</summary>
    public double Value { get; }

    /// <summary>Slice color.</summary>
    public ImageColor? Color { get; }

    /// <summary>Create a pie slice definition.</summary>
    /// <param name="name">Slice label.</param>
    /// <param name="value">Slice value.</param>
    /// <param name="color">Optional slice color.</param>
    public ChartPie(string name, double value, ImageColor? color = null) : base(ChartDefinitionType.Pie, name) {
        Value = value;
        Color = color;
    }
}


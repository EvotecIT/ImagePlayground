using ChartForgeX.Primitives;

namespace ImagePlayground;

/// <summary>Treemap chart item definition.</summary>
public sealed class ChartTreemap : ChartDefinition {
    /// <summary>Item value.</summary>
    public double Value { get; }

    /// <summary>Item color.</summary>
    public ChartColor? Color { get; }

    /// <summary>Create a treemap item definition.</summary>
    /// <param name="name">Item label.</param>
    /// <param name="value">Item value.</param>
    /// <param name="color">Optional item color.</param>
    public ChartTreemap(string name, double value, ChartColor? color = null) : base(name) {
        Value = value;
        Color = color;
    }
}

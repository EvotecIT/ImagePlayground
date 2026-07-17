using ChartForgeX.Primitives;

namespace ImagePlayground;

/// <summary>Funnel chart item definition.</summary>
public sealed class ChartFunnel : ChartDefinition {
    /// <summary>Stage value.</summary>
    public double Value { get; }

    /// <summary>Stage color.</summary>
    public ChartColor? Color { get; }

    /// <summary>Create a funnel chart item definition.</summary>
    /// <param name="name">Stage label.</param>
    /// <param name="value">Stage value.</param>
    /// <param name="color">Optional stage color.</param>
    public ChartFunnel(string name, double value, ChartColor? color = null) : base(name) {
        Value = value;
        Color = color;
    }
}

using ChartForgeX.Primitives;

namespace ImagePlayground;

/// <summary>Donut chart slice definition.</summary>
internal sealed class ChartDonut : ChartDefinition {
    /// <summary>Slice value.</summary>
    public double Value { get; }

    /// <summary>Slice color.</summary>
    public ChartColor? Color { get; }

    /// <summary>Create a donut slice definition.</summary>
    /// <param name="name">Slice label.</param>
    /// <param name="value">Slice value.</param>
    /// <param name="color">Optional slice color.</param>
    public ChartDonut(string name, double value, ChartColor? color = null) : base(ChartDefinitionType.Donut, name) {
        Value = value;
        Color = color;
    }
}

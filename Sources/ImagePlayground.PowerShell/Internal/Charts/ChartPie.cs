using ChartForgeX.Primitives;

namespace ImagePlayground;

/// <summary>Pie chart definition.</summary>
internal sealed class ChartPie : ChartDefinition {
    /// <summary>Slice value.</summary>
    public double Value { get; }

    /// <summary>Slice color.</summary>
    public ChartColor? Color { get; }

    /// <summary>Create a pie slice definition.</summary>
    /// <param name="name">Slice label.</param>
    /// <param name="value">Slice value.</param>
    /// <param name="color">Optional slice color.</param>
    public ChartPie(string name, double value, ChartColor? color = null) : base(ChartDefinitionType.Pie, name) {
        Value = value;
        Color = color;
    }
}

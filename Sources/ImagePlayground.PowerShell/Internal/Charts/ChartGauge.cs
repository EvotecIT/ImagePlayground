using ChartForgeX.Primitives;

namespace ImagePlayground;

/// <summary>Gauge chart definition.</summary>
public sealed class ChartGauge : ChartDefinition {
    /// <summary>Gauge value.</summary>
    public double Value { get; }

    /// <summary>Gauge minimum.</summary>
    public double Minimum { get; }

    /// <summary>Gauge maximum.</summary>
    public double Maximum { get; }

    /// <summary>Gauge color.</summary>
    public ChartColor? Color { get; }

    /// <summary>Create a gauge chart definition.</summary>
    /// <param name="name">Gauge label.</param>
    /// <param name="value">Gauge value.</param>
    /// <param name="minimum">Gauge minimum.</param>
    /// <param name="maximum">Gauge maximum.</param>
    /// <param name="color">Optional gauge color.</param>
    public ChartGauge(string name, double value, double minimum = 0, double maximum = 100, ChartColor? color = null) : base(ChartDefinitionType.Gauge, name) {
        Value = value;
        Minimum = minimum;
        Maximum = maximum;
        Color = color;
    }
}

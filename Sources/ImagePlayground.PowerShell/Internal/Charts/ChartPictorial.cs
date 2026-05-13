using ChartForgeX.Primitives;

namespace ImagePlayground;

/// <summary>Pictorial chart row definition.</summary>
public sealed class ChartPictorial : ChartDefinition {
    /// <summary>Pictorial value.</summary>
    public double Value { get; }

    /// <summary>Pictorial color.</summary>
    public ChartColor? Color { get; }

    /// <summary>Create a pictorial chart row definition.</summary>
    /// <param name="name">Pictorial label.</param>
    /// <param name="value">Pictorial value.</param>
    /// <param name="color">Optional pictorial color.</param>
    public ChartPictorial(string name, double value, ChartColor? color = null) : base(ChartDefinitionType.Pictorial, name) {
        Value = value;
        Color = color;
    }
}

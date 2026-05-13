using ChartForgeX.Primitives;

namespace ImagePlayground;

/// <summary>Progress-bar chart row definition.</summary>
internal sealed class ChartProgress : ChartDefinition {
    /// <summary>Progress value.</summary>
    public double Value { get; }

    /// <summary>Progress color.</summary>
    public ChartColor? Color { get; }

    /// <summary>Create a progress-bar row definition.</summary>
    /// <param name="name">Progress label.</param>
    /// <param name="value">Progress value.</param>
    /// <param name="color">Optional progress color.</param>
    public ChartProgress(string name, double value, ChartColor? color = null) : base(ChartDefinitionType.ProgressBar, name) {
        Value = value;
        Color = color;
    }
}

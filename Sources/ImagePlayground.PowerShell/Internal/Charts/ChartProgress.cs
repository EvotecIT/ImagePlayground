using ImageColor = SixLabors.ImageSharp.Color;

namespace ImagePlayground;

/// <summary>Progress-bar chart row definition.</summary>
public sealed class ChartProgress : ChartDefinition {
    /// <summary>Progress value.</summary>
    public double Value { get; }

    /// <summary>Progress color.</summary>
    public ImageColor? Color { get; }

    /// <summary>Create a progress-bar row definition.</summary>
    /// <param name="name">Progress label.</param>
    /// <param name="value">Progress value.</param>
    /// <param name="color">Optional progress color.</param>
    public ChartProgress(string name, double value, ImageColor? color = null) : base(ChartDefinitionType.ProgressBar, name) {
        Value = value;
        Color = color;
    }
}

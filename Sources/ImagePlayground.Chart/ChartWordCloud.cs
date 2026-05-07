using ImageColor = SixLabors.ImageSharp.Color;

namespace ImagePlayground;

/// <summary>Word cloud term definition.</summary>
public sealed class ChartWordCloud : ChartDefinition {
    /// <summary>Term weight.</summary>
    public double Weight { get; }

    /// <summary>Term color.</summary>
    public ImageColor? Color { get; }

    /// <summary>Create a word cloud term definition.</summary>
    /// <param name="text">Term text.</param>
    /// <param name="weight">Term weight.</param>
    /// <param name="color">Optional term color.</param>
    public ChartWordCloud(string text, double weight, ImageColor? color = null) : base(ChartDefinitionType.WordCloud, text) {
        Weight = weight;
        Color = color;
    }
}

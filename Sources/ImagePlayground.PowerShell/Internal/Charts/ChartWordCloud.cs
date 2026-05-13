using ChartForgeX.Primitives;

namespace ImagePlayground;

/// <summary>Word cloud term definition.</summary>
internal sealed class ChartWordCloud : ChartDefinition {
    /// <summary>Term weight.</summary>
    public double Weight { get; }

    /// <summary>Term color.</summary>
    public ChartColor? Color { get; }

    /// <summary>Create a word cloud term definition.</summary>
    /// <param name="text">Term text.</param>
    /// <param name="weight">Term weight.</param>
    /// <param name="color">Optional term color.</param>
    public ChartWordCloud(string text, double weight, ChartColor? color = null) : base(ChartDefinitionType.WordCloud, text) {
        Weight = weight;
        Color = color;
    }
}

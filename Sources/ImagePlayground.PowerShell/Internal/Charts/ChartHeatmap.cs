namespace ImagePlayground;

/// <summary>Heatmap chart definition.</summary>
public sealed class ChartHeatmap : ChartDefinition {
    /// <summary>Values of the heatmap.</summary>
    public double[,] Data { get; }

    /// <summary>Create a heatmap definition.</summary>
    /// <param name="name">Series name.</param>
    /// <param name="data">Heatmap values.</param>
    public ChartHeatmap(string name, double[,] data) : base(ChartDefinitionType.Heatmap, name) {
        Data = data;
    }
}


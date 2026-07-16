namespace ImagePlayground;

/// <summary>Base class for chart definitions.</summary>
public abstract class ChartDefinition {
    /// <summary>Chart name.</summary>
    public string Name { get; }

    /// <summary>Initializes a chart definition.</summary>
    /// <param name="name">Chart name.</param>
    protected ChartDefinition(string name) {
        Name = name;
    }
}

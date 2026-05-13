namespace ImagePlayground;

/// <summary>Base class for chart definitions.</summary>
public abstract class ChartDefinition {
    /// <summary>Chart type.</summary>
    public ChartDefinitionType Type { get; }

    /// <summary>Chart name.</summary>
    public string Name { get; }

    /// <summary>Initializes a chart definition.</summary>
    /// <param name="type">Chart type.</param>
    /// <param name="name">Chart name.</param>
    protected ChartDefinition(ChartDefinitionType type, string name) {
        Type = type;
        Name = name;
    }
}

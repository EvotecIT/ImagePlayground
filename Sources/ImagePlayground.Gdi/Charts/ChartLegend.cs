namespace ImagePlayground.Gdi.Charts;

/// <summary>Legend positions.</summary>
public enum ChartLegendPosition {
    /// <summary>No legend.</summary>
    None,
    /// <summary>Top of the chart.</summary>
    Top,
    /// <summary>Bottom of the chart.</summary>
    Bottom,
    /// <summary>Left of the chart.</summary>
    Left,
    /// <summary>Right of the chart.</summary>
    Right
}

/// <summary>Legend configuration.</summary>
public sealed class ChartLegend {
    /// <summary>Whether the legend is visible.</summary>
    public bool Show { get; set; } = true;

    /// <summary>Legend position.</summary>
    public ChartLegendPosition Position { get; set; } = ChartLegendPosition.Bottom;

    /// <summary>Spacing between legend items.</summary>
    public float ItemSpacing { get; set; } = 10f;

    /// <summary>Size of legend color box.</summary>
    public float SwatchSize { get; set; } = 12f;
}

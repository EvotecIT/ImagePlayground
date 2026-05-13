namespace ImagePlayground;

/// <summary>Histogram chart definition.</summary>
public sealed class ChartHistogram : ChartDefinition {
    /// <summary>Values for the histogram.</summary>
    public double[] Values { get; }

    /// <summary>Size of each bin.</summary>
    public int? BinSize { get; }

    /// <summary>Create a histogram definition.</summary>
    /// <param name="name">Series name.</param>
    /// <param name="values">Input values.</param>
    /// <param name="binSize">Optional bin size.</param>
    public ChartHistogram(string name, double[] values, int? binSize = null) : base(ChartDefinitionType.Histogram, name) {
        Values = values;
        BinSize = binSize;
    }
}


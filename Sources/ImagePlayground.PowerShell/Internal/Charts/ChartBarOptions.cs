namespace ImagePlayground;

/// <summary>Options for bar charts.</summary>
public sealed class ChartBarOptions {
    /// <summary>Whether to show values above bars.</summary>
    public bool ShowValuesAboveBars { get; }

    /// <summary>Initialize options for bar charts.</summary>
    /// <param name="showValuesAboveBars">Show value labels.</param>
    public ChartBarOptions(bool showValuesAboveBars) {
        ShowValuesAboveBars = showValuesAboveBars;
    }
}


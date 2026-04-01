namespace ImagePlayground.Gdi.Charts;

/// <summary>Axis configuration for charts.</summary>
public sealed class ChartAxis {
    /// <summary>Axis title.</summary>
    public string? Title { get; set; }

    /// <summary>Minimum value override.</summary>
    public double? Min { get; set; }

    /// <summary>Maximum value override.</summary>
    public double? Max { get; set; }

    /// <summary>Number of ticks for the axis.</summary>
    public int TickCount { get; set; } = 5;

    /// <summary>Format string for labels.</summary>
    public string LabelFormat { get; set; } = "0.##";

    /// <summary>Whether to show grid lines.</summary>
    public bool ShowGrid { get; set; } = true;
}

namespace ImagePlayground;

/// <summary>Content rendered in pie and donut slice labels.</summary>
public enum ChartPieLabelContent {
    /// <summary>Render the slice label.</summary>
    Label,
    /// <summary>Render the numeric value.</summary>
    Value,
    /// <summary>Render the percent of total.</summary>
    Percent,
    /// <summary>Render label and value.</summary>
    LabelAndValue,
    /// <summary>Render label and percent.</summary>
    LabelAndPercent
}

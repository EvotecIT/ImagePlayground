using ChartForgeX.Primitives;

namespace ImagePlayground;

/// <summary>Slope chart definition.</summary>
public sealed class ChartSlope : ChartDefinition {
    /// <summary>Start value.</summary>
    public double Start { get; }

    /// <summary>End value.</summary>
    public double End { get; }

    /// <summary>Optional start axis label.</summary>
    public string? StartLabel { get; }

    /// <summary>Optional end axis label.</summary>
    public string? EndLabel { get; }

    /// <summary>Slope color.</summary>
    public ChartColor? Color { get; }

    /// <summary>Create a slope chart definition.</summary>
    /// <param name="name">Series name.</param>
    /// <param name="start">Start value.</param>
    /// <param name="end">End value.</param>
    /// <param name="startLabel">Optional start axis label.</param>
    /// <param name="endLabel">Optional end axis label.</param>
    /// <param name="color">Optional slope color.</param>
    public ChartSlope(string name, double start, double end, string? startLabel = null, string? endLabel = null, ChartColor? color = null) : base(name) {
        Start = start;
        End = end;
        StartLabel = startLabel;
        EndLabel = endLabel;
        Color = color;
    }
}

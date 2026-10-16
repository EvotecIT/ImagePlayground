using System.Collections.Generic;
using ChartForgeX.Primitives;

namespace ImagePlayground;

/// <summary>Line chart definition.</summary>
public sealed class ChartLine : ChartDefinition {
    /// <summary>Line values.</summary>
    public IList<double> Value { get; }

    /// <summary>Line color.</summary>
    public ChartColor? Color { get; }

    /// <summary>Optional marker diameter in pixels. Null and zero suppress markers for this series.</summary>
    public float? MarkerSize { get; }

    /// <summary>Marker shape used when <see cref="MarkerSize"/> is greater than zero.</summary>
    public ChartMarkerShape MarkerShape { get; }

    /// <summary>Render the line using a smooth curve.</summary>
    public bool Smooth { get; }

    /// <summary>Create a line chart definition.</summary>
    /// <param name="name">Series name.</param>
    /// <param name="value">Line values.</param>
    /// <param name="color">Optional line color.</param>
    /// <param name="markerSize">Optional marker diameter. Null and zero suppress markers for this series.</param>
    /// <param name="smooth">Render as a smooth curve.</param>
    /// <param name="markerShape">Marker shape used when a marker size is set. None also suppresses markers.</param>
    public ChartLine(
        string name,
        IList<double> value,
        ChartColor? color = null,
        float? markerSize = null,
        bool smooth = false,
        ChartMarkerShape markerShape = ChartMarkerShape.Circle) : base(name) {
        Value = value;
        Color = color;
        MarkerSize = markerShape == ChartMarkerShape.None ? 0f : markerSize;
        MarkerShape = markerShape;
        Smooth = smooth;
    }
}

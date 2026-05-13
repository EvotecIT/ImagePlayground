using System.Collections.Generic;
using ChartForgeX.Primitives;

namespace ImagePlayground;

/// <summary>Line chart definition.</summary>
internal sealed class ChartLine : ChartDefinition {
    /// <summary>Line values.</summary>
    public IList<double> Value { get; }

    /// <summary>Line color.</summary>
    public ChartColor? Color { get; }

    /// <summary>Shape of markers used for data points.</summary>
    public ChartMarkerShape MarkerShape { get; }

    /// <summary>Optional size of the markers.</summary>
    public float? MarkerSize { get; }

    /// <summary>Render the line using a smooth curve.</summary>
    public bool Smooth { get; }

    /// <summary>Create a line chart definition.</summary>
    /// <param name="name">Series name.</param>
    /// <param name="value">Line values.</param>
    /// <param name="color">Optional line color.</param>
    /// <param name="markerShape">Marker shape.</param>
    /// <param name="markerSize">Optional marker size.</param>
    /// <param name="smooth">Render as a smooth curve.</param>
    public ChartLine(
        string name,
        IList<double> value,
        ChartColor? color = null,
        ChartMarkerShape markerShape = ChartMarkerShape.None,
        float? markerSize = null,
        bool smooth = false) : base(ChartDefinitionType.Line, name) {
        Value = value;
        Color = color;
        MarkerShape = markerShape;
        MarkerSize = markerSize;
        Smooth = smooth;
    }
}

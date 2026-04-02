using System.Drawing;

namespace ImagePlayground.Gdi.Charts;

/// <summary>Defines styling for charts.</summary>
public sealed class ChartStyle {
    /// <summary>Background color.</summary>
    public Color BackgroundColor { get; set; } = Color.White;

    /// <summary>Plot area background color.</summary>
    public Color PlotAreaColor { get; set; } = Color.WhiteSmoke;

    /// <summary>Axis line color.</summary>
    public Color AxisColor { get; set; } = Color.DimGray;

    /// <summary>Grid line color.</summary>
    public Color GridColor { get; set; } = Color.Gainsboro;

    /// <summary>Default text color.</summary>
    public Color TextColor { get; set; } = Color.Black;

    /// <summary>Font family name.</summary>
    public string FontFamilyName { get; set; } = "Segoe UI";

    /// <summary>Base font size in pixels.</summary>
    public float FontSize { get; set; } = 12f;

    /// <summary>Title font size in pixels.</summary>
    public float TitleFontSize { get; set; } = 16f;

    /// <summary>Legend font size in pixels.</summary>
    public float LegendFontSize { get; set; } = 11f;

    /// <summary>Controls anti-aliasing.</summary>
    public bool AntiAlias { get; set; } = true;
}

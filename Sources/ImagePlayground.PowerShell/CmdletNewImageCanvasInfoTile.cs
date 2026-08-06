using ChartForgeX.Composition;
using ChartForgeX.Primitives;
using ImagePlayground;
using System;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a positioned information tile for a visual canvas.</summary>
/// <example>
///   <summary>Create a status tile</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageCanvasInfoTile -X 72 -Y 180 -Width 360 -Height 150 -Icon API -Label 'Requests' -Value '12,840' -Detail '+12%' -MiniChartKind Area -MiniValues 8,9,10,12</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageCanvasInfoTile")]
[OutputType(typeof(VisualCanvasInfoTileLayer))]
public sealed class NewImageCanvasInfoTileCmdlet : PSCmdlet {
    /// <summary>Horizontal position in canvas design pixels.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public double X { get; set; }

    /// <summary>Vertical position in canvas design pixels.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public double Y { get; set; }

    /// <summary>Tile width.</summary>
    [Parameter(Mandatory = true, Position = 2)]
    public double Width { get; set; }

    /// <summary>Tile height.</summary>
    [Parameter(Mandatory = true, Position = 3)]
    public double Height { get; set; }

    /// <summary>Compact icon or symbol.</summary>
    [Parameter(Mandatory = true, Position = 4)]
    public string Icon { get; set; } = string.Empty;

    /// <summary>Tile label.</summary>
    [Parameter(Mandatory = true, Position = 5)]
    public string Label { get; set; } = string.Empty;

    /// <summary>Primary tile value.</summary>
    [Parameter(Mandatory = true, Position = 6)]
    public string Value { get; set; } = string.Empty;

    /// <summary>Optional supporting detail.</summary>
    [Parameter]
    public string Detail { get; set; } = string.Empty;

    /// <summary>Optional accent color.</summary>
    [Parameter]
    [ChartColorArgumentTransformation]
    public ChartColor? Accent { get; set; }

    /// <summary>Tile surface treatment.</summary>
    [Parameter]
    public VisualCanvasInfoTileSurfaceStyle SurfaceStyle { get; set; } = VisualCanvasInfoTileSurfaceStyle.Glass;

    /// <summary>Built-in icon treatment.</summary>
    [Parameter]
    public VisualCanvasInfoTileIconKind IconKind { get; set; } = VisualCanvasInfoTileIconKind.Text;

    /// <summary>Compact chart treatment.</summary>
    [Parameter]
    public VisualCanvasInfoTileMiniChartKind MiniChartKind { get; set; } = VisualCanvasInfoTileMiniChartKind.None;

    /// <summary>Values rendered by the compact chart.</summary>
    [Parameter]
    public double[] MiniValues { get; set; } = Array.Empty<double>();

    /// <summary>Optional compact chart maximum.</summary>
    [Parameter]
    public double MiniChartMaximum { get; set; }

    /// <summary>Optional progress from zero to one.</summary>
    [Parameter]
    [ValidateRange(0, 1)]
    public double Progress { get; set; }

    /// <summary>Text fitting policy.</summary>
    [Parameter]
    public VisualCanvasTextFitPolicy TextFitPolicy { get; set; } = VisualCanvasTextFitPolicy.Auto;

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var tile = new VisualCanvasInfoTileLayer(X, Y, Width, Height, Icon, Label, Value) {
            Detail = Detail,
            SurfaceStyle = SurfaceStyle,
            IconKind = IconKind,
            TextFitPolicy = TextFitPolicy
        };
        if (Accent.HasValue) tile.AccentOverride = ChartColorConverter.Convert(Accent);
        if (MyInvocation.BoundParameters.ContainsKey(nameof(Progress))) tile.Progress = Progress;
        var maximum = MyInvocation.BoundParameters.ContainsKey(nameof(MiniChartMaximum)) ? MiniChartMaximum : (double?)null;
        tile.WithMiniChart(MiniChartKind, MiniValues, maximum);
        WriteObject(tile);
    }
}

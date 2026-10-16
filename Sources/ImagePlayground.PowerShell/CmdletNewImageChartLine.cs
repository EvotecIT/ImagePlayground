using System.Management.Automation;
using ChartForgeX.Primitives;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates line chart data item.</summary>
/// <para>Use this cmdlet inside <c>New-ImageChart</c> to define a line-series dataset. Lines can share a chart with bar, area, scatter, and other line definitions.</para>
/// <para>Markers are configured per series, so one line can show markers while another does not.</para>
/// <example>
///   <summary>Create line-series data</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageChartLine -Name 'Sales' -Value 10,20,18,24 -Color Green -Smooth</code>
///   <para>Creates a smoothed line-series definition ready to be rendered by New-ImageChart.</para>
/// </example>
/// <example>
///   <summary>Mark one series and plot it on the secondary axis</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageChart -XLabel Mon, Tue, Wed, Thu -SecondaryYTitle 'Error rate (%)' -FilePath combo.png {
///     New-ImageChartBar -Name Requests -Value 410, 380, 455, 520
///     New-ImageChartLine -Name 'Error rate' -Value 1.2, 0.8, 2.4, 1.1 -Marker Diamond -MarkerSize 8 -SecondaryAxis
/// }</code>
///   <para>Combines bars and a marked line; the line uses the right-hand axis.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageChartLine")]
[OutputType(typeof(ChartLine))]
public sealed class NewImageChartLineCmdlet : PSCmdlet {
    private const float DefaultMarkerSize = 6f;

    /// <summary>Label for the line.</summary>
    [Alias("Label")]
    [Parameter(Mandatory = true, Position = 0)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Y values for the line.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public double[] Value { get; set; } = System.Array.Empty<double>();

    /// <summary>Line color.</summary>
    [Parameter]
    [ChartColorArgumentTransformation]
    public ChartColor? Color { get; set; }

    /// <summary>Marker drawn on each data point of this series.</summary>
    /// <para>None (the default) draws no markers. Circle, Square, Diamond, Triangle, Plus, Cross, and Star draw markers on this series only.</para>
    [Parameter]
    public ChartMarkerShape Marker { get; set; } = ChartMarkerShape.None;

    /// <summary>Marker diameter in pixels. Supplying it without -Marker draws circles.</summary>
    [Parameter]
    [ValidateRange(1D, 100D)]
    public double MarkerSize { get; set; } = DefaultMarkerSize;

    /// <summary>Render the line using a smooth curve.</summary>
    [Parameter]
    public SwitchParameter Smooth { get; set; }

    /// <summary>Plot the line against the secondary (right) Y axis. Use New-ImageChart -SecondaryYTitle to name that axis.</summary>
    [Parameter]
    public SwitchParameter SecondaryAxis { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var shape = Marker;
        if (shape == ChartMarkerShape.None && MyInvocation.BoundParameters.ContainsKey(nameof(MarkerSize)) && !MyInvocation.BoundParameters.ContainsKey(nameof(Marker))) {
            shape = ChartMarkerShape.Circle;
        }
        var markerSize = shape == ChartMarkerShape.None ? 0f : (float)MarkerSize;
        var line = new ChartLine(Name, Value, ChartColorConverter.Convert(Color), markerSize, Smooth.IsPresent, shape) {
            SecondaryAxis = SecondaryAxis.IsPresent
        };
        WriteObject(line);
    }
}

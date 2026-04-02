using System.Management.Automation;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates line chart data item.</summary>
/// <para>Use this cmdlet inside <c>New-ImageChart</c> to define a line-series dataset.</para>
/// <example>
///   <summary>Create line-series data</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageChartLine -Name 'Sales' -Value 10,20,18,24 -Color Green -Smooth</code>
///   <para>Creates a smoothed line-series definition ready to be rendered by New-ImageChart.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageChartLine")]
public sealed class NewImageChartLineCmdlet : PSCmdlet {
    /// <summary>Label for the line.</summary>
    [Alias("Label")]
    [Parameter(Mandatory = true, Position = 0)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Y values for the line.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public double[] Value { get; set; } = System.Array.Empty<double>();

    /// <summary>Line color.</summary>
    [Parameter]
    public SixLabors.ImageSharp.Color? Color { get; set; }

    /// <summary>Shape of markers placed on data points.</summary>
    [Parameter]
    public ScottPlot.MarkerShape Marker { get; set; } = ScottPlot.MarkerShape.None;

    /// <summary>Render the line using a smooth curve.</summary>
    [Parameter]
    public SwitchParameter Smooth { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ChartLine(Name, Value, Color, Marker, null, Smooth.IsPresent));
    }
}

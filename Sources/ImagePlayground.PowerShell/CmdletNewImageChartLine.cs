using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Creates line chart data item.</summary>
/// <example>
///   <summary>Create line data</summary>
///   <code>New-ImageChartLine -Name 'Sales' -Value 10,20 -Color Green</code>
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
        WriteObject(new ImagePlayground.Charts.ChartLine(Name, Value, Color, Marker, null, Smooth.IsPresent));
    }
}

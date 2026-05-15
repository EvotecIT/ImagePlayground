using System.Management.Automation;
using ChartForgeX.Primitives;
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
    [ChartColorArgumentTransformation]
    public ChartColor? Color { get; set; }

    /// <summary>Circle markers placed on data points.</summary>
    [Parameter]
    public SwitchParameter Marker { get; set; }

    /// <summary>Render the line using a smooth curve.</summary>
    [Parameter]
    public SwitchParameter Smooth { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        float? markerSize = Marker.IsPresent ? 6 : null;
        WriteObject(new ChartLine(Name, Value, ChartColorConverter.Convert(Color), markerSize, Smooth.IsPresent));
    }
}

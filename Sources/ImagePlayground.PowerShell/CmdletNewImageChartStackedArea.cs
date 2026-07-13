using System.Management.Automation;
using ChartForgeX.Primitives;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates stacked-area chart data item.</summary>
/// <example>
///   <summary>Create stacked-area data</summary>
///   <code>New-ImageChartStackedArea -Name 'Used' -Value 20,30,40 -Color Orange -Smooth</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageChartStackedArea")]
public sealed class NewImageChartStackedAreaCmdlet : PSCmdlet {
    /// <summary>Label for the area.</summary>
    [Alias("Label")]
    [Parameter(Mandatory = true, Position = 0)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Y values for the area.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public double[] Value { get; set; } = System.Array.Empty<double>();

    /// <summary>Area color.</summary>
    [Parameter]
    [ChartColorArgumentTransformation]
    public ChartColor? Color { get; set; }

    /// <summary>Render using a smooth curve.</summary>
    [Parameter]
    public SwitchParameter Smooth { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ChartStackedArea(Name, Value, ChartColorConverter.Convert(Color), Smooth.IsPresent));
    }
}

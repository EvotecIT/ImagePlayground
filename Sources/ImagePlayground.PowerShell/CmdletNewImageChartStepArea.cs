using System.Management.Automation;
using ChartForgeX.Primitives;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates step-area chart data item.</summary>
/// <example>
///   <summary>Create step-area data</summary>
///   <code>New-ImageChartStepArea -Name 'Capacity' -Value 4,8,7 -Color Green</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageChartStepArea")]
public sealed class NewImageChartStepAreaCmdlet : PSCmdlet {
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

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ChartStepArea(Name, Value, ChartColorConverter.Convert(Color)));
    }
}

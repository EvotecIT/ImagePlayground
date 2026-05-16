using System.Management.Automation;
using ChartForgeX.Primitives;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates step-line chart data item.</summary>
/// <example>
///   <summary>Create step-line data</summary>
///   <code>New-ImageChartStepLine -Name 'Requests' -Value 4,8,7 -Color Cyan</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageChartStepLine")]
public sealed class NewImageChartStepLineCmdlet : PSCmdlet {
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

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ChartStepLine(Name, Value, ChartColorConverter.Convert(Color)));
    }
}

using System.Management.Automation;
using ChartForgeX.Primitives;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates range-bar chart data item.</summary>
/// <example>
///   <summary>Create range-bar data</summary>
///   <code>New-ImageChartRangeBar -Name 'Maintenance' -X 1,2 -Start 2,4 -End 5,8 -Color Orange</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageChartRangeBar")]
public sealed class NewImageChartRangeBarCmdlet : PSCmdlet {
    /// <summary>Series label.</summary>
    [Alias("Label")]
    [Parameter(Mandatory = true, Position = 0)]
    public string Name { get; set; } = string.Empty;

    /// <summary>X or category values.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public double[] X { get; set; } = System.Array.Empty<double>();

    /// <summary>Interval start values.</summary>
    [Parameter(Mandatory = true, Position = 2)]
    public double[] Start { get; set; } = System.Array.Empty<double>();

    /// <summary>Interval end values.</summary>
    [Parameter(Mandatory = true, Position = 3)]
    public double[] End { get; set; } = System.Array.Empty<double>();

    /// <summary>Range bar color.</summary>
    [Parameter]
    [ChartColorArgumentTransformation]
    public ChartColor? Color { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ChartRangeBar(Name, X, Start, End, ChartColorConverter.Convert(Color)));
    }
}

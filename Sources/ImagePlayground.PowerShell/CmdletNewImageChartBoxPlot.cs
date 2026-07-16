using System.Management.Automation;
using ChartForgeX.Primitives;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates box-plot chart data item.</summary>
/// <example>
///   <summary>Create box-plot data</summary>
///   <code>New-ImageChartBoxPlot -Name 'Latency' -X 1 -Minimum 2 -Q1 4 -Median 6 -Q3 8 -Maximum 10</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageChartBoxPlot")]
public sealed class NewImageChartBoxPlotCmdlet : PSCmdlet {
    /// <summary>Series label.</summary>
    [Alias("Label")]
    [Parameter(Mandatory = true, Position = 0)]
    public string Name { get; set; } = string.Empty;

    /// <summary>X or category values.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public double[] X { get; set; } = System.Array.Empty<double>();

    /// <summary>Minimum whisker values.</summary>
    [Parameter(Mandatory = true)]
    public double[] Minimum { get; set; } = System.Array.Empty<double>();

    /// <summary>First quartile values.</summary>
    [Parameter(Mandatory = true)]
    public double[] Q1 { get; set; } = System.Array.Empty<double>();

    /// <summary>Median values.</summary>
    [Parameter(Mandatory = true)]
    public double[] Median { get; set; } = System.Array.Empty<double>();

    /// <summary>Third quartile values.</summary>
    [Parameter(Mandatory = true)]
    public double[] Q3 { get; set; } = System.Array.Empty<double>();

    /// <summary>Maximum whisker values.</summary>
    [Parameter(Mandatory = true)]
    public double[] Maximum { get; set; } = System.Array.Empty<double>();

    /// <summary>Box plot color.</summary>
    [Parameter]
    [ChartColorArgumentTransformation]
    public ChartColor? Color { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ChartBoxPlotSeries(Name, X, Minimum, Q1, Median, Q3, Maximum, ChartColorConverter.Convert(Color)));
    }
}

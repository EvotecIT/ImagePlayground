using System.Management.Automation;
using ChartForgeX.Primitives;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates horizontal bar chart data item.</summary>
/// <example>
///   <summary>Create horizontal bar data</summary>
///   <code>New-ImageChartHorizontalBar -Name 'Disk' -Value 70,30 -Color Blue</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageChartHorizontalBar")]
public sealed class NewImageChartHorizontalBarCmdlet : PSCmdlet {
    /// <summary>Label for the bar data.</summary>
    [Alias("Label")]
    [Parameter(Mandatory = true, Position = 0)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Values for the bars.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public double[] Value { get; set; } = System.Array.Empty<double>();

    /// <summary>Bar color.</summary>
    [Parameter]
    [ChartColorArgumentTransformation]
    public ChartColor? Color { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ChartHorizontalBar(Name, Value, ChartColorConverter.Convert(Color)));
    }
}

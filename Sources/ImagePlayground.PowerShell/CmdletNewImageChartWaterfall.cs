using System.Management.Automation;
using ChartForgeX.Primitives;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates waterfall chart data item.</summary>
/// <example>
///   <summary>Create waterfall data</summary>
///   <code>New-ImageChartWaterfall -Name 'Change' -Value 120,-24,42 -Labels Start,Churn,Expansion</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageChartWaterfall")]
public sealed class NewImageChartWaterfallCmdlet : PSCmdlet {
    /// <summary>Label for the waterfall series.</summary>
    [Alias("Label")]
    [Parameter(Mandatory = true, Position = 0)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Delta values.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public double[] Value { get; set; } = System.Array.Empty<double>();

    /// <summary>Step labels.</summary>
    [Parameter]
    public string[]? Labels { get; set; }

    /// <summary>Waterfall color.</summary>
    [Parameter]
    [ChartColorArgumentTransformation]
    public ChartColor? Color { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ChartWaterfall(Name, Value, Labels, ChartColorConverter.Convert(Color)));
    }
}

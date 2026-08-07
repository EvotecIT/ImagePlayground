using System.Management.Automation;
using ChartForgeX.Primitives;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates gauge chart data item.</summary>
/// <example>
///   <summary>Create a capacity gauge</summary>
///   <code>New-ImageChartGauge -Name 'Capacity' -Value 68 -Minimum 0 -Maximum 100 -Color Orange</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageChartGauge")]
public sealed class NewImageChartGaugeCmdlet : PSCmdlet {
    /// <summary>Label for the gauge.</summary>
    [Alias("Label")]
    [Parameter(Mandatory = true, Position = 0)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Gauge value.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public double Value { get; set; }

    /// <summary>Gauge minimum.</summary>
    [Parameter]
    public double Minimum { get; set; }

    /// <summary>Gauge maximum.</summary>
    [Parameter]
    public double Maximum { get; set; } = 100;

    /// <summary>Gauge color.</summary>
    [Parameter]
    [ChartColorArgumentTransformation]
    public ChartColor? Color { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ChartGauge(Name, Value, Minimum, Maximum, ChartColorConverter.Convert(Color)));
    }
}

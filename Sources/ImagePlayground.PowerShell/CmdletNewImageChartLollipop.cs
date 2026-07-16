using System.Management.Automation;
using ChartForgeX.Primitives;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates lollipop chart data item.</summary>
/// <example>
///   <summary>Create lollipop data</summary>
///   <code>New-ImageChartLollipop -Name 'Latency' -Value 12,18,9 -Color Purple</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageChartLollipop")]
public sealed class NewImageChartLollipopCmdlet : PSCmdlet {
    /// <summary>Label for the lollipop series.</summary>
    [Alias("Label")]
    [Parameter(Mandatory = true, Position = 0)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Lollipop values.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public double[] Value { get; set; } = System.Array.Empty<double>();

    /// <summary>Lollipop color.</summary>
    [Parameter]
    [ChartColorArgumentTransformation]
    public ChartColor? Color { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ChartLollipop(Name, Value, ChartColorConverter.Convert(Color)));
    }
}

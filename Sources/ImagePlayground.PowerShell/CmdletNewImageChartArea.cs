using System.Management.Automation;
using ChartForgeX.Primitives;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates area chart data item.</summary>
/// <example>
///   <summary>Create area data</summary>
///   <code>New-ImageChartArea -Name 'Sales' -Value 10,20 -Color Green</code>
/// </example>
/// <example>
///   <summary>Area behind bars on a secondary axis</summary>
///   <code>New-ImageChart -XLabel Q1, Q2, Q3 -SecondaryYTitle Margin -FilePath area.png { New-ImageChartBar -Name Revenue -Value 12, 18, 25; New-ImageChartArea -Name Margin -Value 21, 24, 28 -SecondaryAxis }</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageChartArea")]
[OutputType(typeof(ChartArea))]
public sealed class NewImageChartAreaCmdlet : PSCmdlet {
    /// <summary>Label for the area.</summary>
    [Alias("Label")]
    [Parameter(Mandatory = true, Position = 0)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Y values for the area.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public double[] Value { get; set; } = System.Array.Empty<double>();

    /// <summary>Fill color.</summary>
    [Parameter]
    [ChartColorArgumentTransformation]
    public ChartColor? Color { get; set; }

    /// <summary>Plot the area against the secondary (right) Y axis. Use New-ImageChart -SecondaryYTitle to name that axis.</summary>
    [Parameter]
    public SwitchParameter SecondaryAxis { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ChartArea(Name, Value, ChartColorConverter.Convert(Color)) { SecondaryAxis = SecondaryAxis.IsPresent });
    }
}

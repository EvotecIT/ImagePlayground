using System.Management.Automation;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates area chart data item.</summary>
/// <example>
///   <summary>Create area data</summary>
///   <code>New-ImageChartArea -Name 'Sales' -Value 10,20 -Color Green</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageChartArea")]
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
    public SixLabors.ImageSharp.Color? Color { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ChartArea(Name, Value, Color));
    }
}

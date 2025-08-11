using System.Management.Automation;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates polar chart data item.</summary>
/// <example>
///   <summary>Create polar data</summary>
///   <code>New-ImageChartPolar -Name 'Series1' -Angle 0,1.57 -Value 1,2 -Color Blue</code>
/// </example>
/// <example>
///   <summary>Create advanced polar data</summary>
///   <code>New-ImageChartPolar -Name 'Advanced' -Angle 0,1.57,3.14 -Value 1,2,1 -Color Red</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageChartPolar")]
public sealed class NewImageChartPolarCmdlet : PSCmdlet {
    /// <summary>Label for the series.</summary>
    [Alias("Label")]
    [Parameter(Mandatory = true, Position = 0)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Angle values for the series.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public double[] Angle { get; set; } = System.Array.Empty<double>();

    /// <summary>Radius values for the series.</summary>
    [Parameter(Mandatory = true, Position = 2)]
    public double[] Value { get; set; } = System.Array.Empty<double>();

    /// <summary>Series color.</summary>
    [Parameter]
    public SixLabors.ImageSharp.Color? Color { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ChartPolar(Name, Angle, Value, Color));
    }
}

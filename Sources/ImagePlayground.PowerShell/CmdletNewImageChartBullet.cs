using System.Management.Automation;
using ChartForgeX.Primitives;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates bullet chart data item.</summary>
/// <example>
///   <summary>Create bullet data</summary>
///   <code>New-ImageChartBullet -Name 'Servers' -Value 92 -Target 95 -RangeEnds 70,85 -Color Green</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageChartBullet")]
public sealed class NewImageChartBulletCmdlet : PSCmdlet {
    /// <summary>Label for the bullet row.</summary>
    [Alias("Label")]
    [Parameter(Mandatory = true, Position = 0)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Current value.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public double Value { get; set; }

    /// <summary>Target value.</summary>
    [Parameter(Mandatory = true, Position = 2)]
    public double Target { get; set; }

    /// <summary>Minimum scale value.</summary>
    [Parameter]
    public double Minimum { get; set; }

    /// <summary>Maximum scale value.</summary>
    [Parameter]
    public double Maximum { get; set; } = 100;

    /// <summary>Qualitative range end values.</summary>
    [Parameter]
    public double[]? RangeEnds { get; set; }

    /// <summary>Bullet color.</summary>
    [Parameter]
    [ChartColorArgumentTransformation]
    public ChartColor? Color { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ChartBullet(Name, Value, Target, Minimum, Maximum, RangeEnds, ChartColorConverter.Convert(Color)));
    }
}

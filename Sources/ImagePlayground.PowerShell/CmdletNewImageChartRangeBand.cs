using System.Management.Automation;
using ChartForgeX.Primitives;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates range-band chart data item.</summary>
/// <example>
///   <summary>Create range-band data</summary>
///   <code>New-ImageChartRangeBand -Name 'Expected' -X 1,2 -Lower 8,9 -Upper 12,15 -Area</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageChartRangeBand")]
public sealed class NewImageChartRangeBandCmdlet : PSCmdlet {
    /// <summary>Series label.</summary>
    [Alias("Label")]
    [Parameter(Mandatory = true, Position = 0)]
    public string Name { get; set; } = string.Empty;

    /// <summary>X or category values.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public double[] X { get; set; } = System.Array.Empty<double>();

    /// <summary>Lower values.</summary>
    [Parameter(Mandatory = true, Position = 2)]
    public double[] Lower { get; set; } = System.Array.Empty<double>();

    /// <summary>Upper values.</summary>
    [Parameter(Mandatory = true, Position = 3)]
    public double[] Upper { get; set; } = System.Array.Empty<double>();

    /// <summary>Range band color.</summary>
    [Parameter]
    [ChartColorArgumentTransformation]
    public ChartColor? Color { get; set; }

    /// <summary>Render the range as an area.</summary>
    [Parameter]
    public SwitchParameter Area { get; set; }

    /// <summary>Render range-area boundaries without smoothing.</summary>
    [Parameter]
    public SwitchParameter NoSmooth { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ChartRangeBandSeries(Name, X, Lower, Upper, ChartColorConverter.Convert(Color), Area.IsPresent, !NoSmooth.IsPresent));
    }
}

using System.Management.Automation;
using ChartForgeX.Primitives;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates slope chart data item.</summary>
/// <example>
///   <summary>Create slope data</summary>
///   <code>New-ImageChartSlope -Name 'Current' -Start 62 -End 71 -StartLabel Before -EndLabel After</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageChartSlope")]
public sealed class NewImageChartSlopeCmdlet : PSCmdlet {
    /// <summary>Series label.</summary>
    [Alias("Label")]
    [Parameter(Mandatory = true, Position = 0)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Start value.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public double Start { get; set; }

    /// <summary>End value.</summary>
    [Parameter(Mandatory = true, Position = 2)]
    public double End { get; set; }

    /// <summary>Start axis label.</summary>
    [Parameter]
    public string? StartLabel { get; set; }

    /// <summary>End axis label.</summary>
    [Parameter]
    public string? EndLabel { get; set; }

    /// <summary>Slope color.</summary>
    [Parameter]
    [ChartColorArgumentTransformation]
    public ChartColor? Color { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ChartSlope(Name, Start, End, StartLabel, EndLabel, ChartColorConverter.Convert(Color)));
    }
}

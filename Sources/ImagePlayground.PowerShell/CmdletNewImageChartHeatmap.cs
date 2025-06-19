using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Creates heatmap chart data item.</summary>
/// <example>
///   <summary>Create heatmap data</summary>
///   <code>New-ImageChartHeatmap -Name 'Matrix' -Matrix ((1,2),(3,4))</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageChartHeatmap")]
public sealed class NewImageChartHeatmapCmdlet : PSCmdlet {
    /// <summary>Label for the heatmap.</summary>
    [Alias("Label")]
    [Parameter(Mandatory = true, Position = 0)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Matrix data for the heatmap.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public double[,] Matrix { get; set; } = new double[0, 0];

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ImagePlayground.Charts.ChartHeatmap(Name, Matrix));
    }
}

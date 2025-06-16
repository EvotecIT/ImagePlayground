using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Creates pie chart data item.</summary>
[Cmdlet(VerbsCommon.New, "ImageChartPie")]
public sealed class NewImageChartPieCmdlet : PSCmdlet {
    /// <summary>Label for the slice.</summary>
    [Alias("Label")]
    [Parameter(Mandatory = true, Position = 0)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Value for the slice.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public double Value { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ImagePlayground.Charts.ChartPie(Name, Value));
    }
}

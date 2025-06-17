using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Creates radial gauge chart data item.</summary>
[Cmdlet(VerbsCommon.New, "ImageChartRadial")]
public sealed class NewImageChartRadialCmdlet : PSCmdlet {
    /// <summary>Label for the gauge.</summary>
    [Alias("Label")]
    [Parameter(Mandatory = true, Position = 0)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Value for the gauge.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public double Value { get; set; }

    /// <summary>Gauge color.</summary>
    [Parameter]
    public SixLabors.ImageSharp.Color? Color { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ImagePlayground.Charts.ChartRadial(Name, Value, Color));
    }
}

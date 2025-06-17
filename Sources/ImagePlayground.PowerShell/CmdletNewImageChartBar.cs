using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Creates bar chart data item.</summary>
[Cmdlet(VerbsCommon.New, "ImageChartBar")]
public sealed class NewImageChartBarCmdlet : PSCmdlet {
    /// <summary>Label for the bar data.</summary>
    [Alias("Label")]
    [Parameter(Mandatory = true, Position = 0)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Values for the bar.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public double[] Value { get; set; } = System.Array.Empty<double>();

    /// <summary>Bar color.</summary>
    [Parameter]
    public SixLabors.ImageSharp.Color? Color { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ImagePlayground.Charts.ChartBar(Name, Value, Color));
    }
}

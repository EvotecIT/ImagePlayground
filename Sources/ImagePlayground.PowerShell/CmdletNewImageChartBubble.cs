using System.Management.Automation;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates bubble chart data item.</summary>
/// <example>
///   <summary>Create bubble data</summary>
///   <code>New-ImageChartBubble -Name 'Series1' -X 1,2,3 -Y 4,5,6 -Size 10,20,30 -Color Blue</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageChartBubble")]
public sealed class NewImageChartBubbleCmdlet : PSCmdlet {
    /// <summary>Label for the bubble series.</summary>
    [Alias("Label")]
    [Parameter(Mandatory = true, Position = 0)]
    public string Name { get; set; } = string.Empty;

    /// <summary>X values for the series.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public double[] X { get; set; } = System.Array.Empty<double>();

    /// <summary>Y values for the series.</summary>
    [Parameter(Mandatory = true, Position = 2)]
    public double[] Y { get; set; } = System.Array.Empty<double>();

    /// <summary>Bubble sizes.</summary>
    [Parameter(Mandatory = true, Position = 3)]
    public double[] Size { get; set; } = System.Array.Empty<double>();

    /// <summary>Series color.</summary>
    [Parameter]
    public SixLabors.ImageSharp.Color? Color { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ChartBubble(Name, X, Y, Size, Color));
    }
}

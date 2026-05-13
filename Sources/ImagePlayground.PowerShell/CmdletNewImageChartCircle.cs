using System.Management.Automation;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates circle status chart data item.</summary>
[Cmdlet(VerbsCommon.New, "ImageChartCircle")]
public sealed class NewImageChartCircleCmdlet : PSCmdlet {
    /// <summary>Label for the circle.</summary>
    [Alias("Label")]
    [Parameter(Mandatory = true, Position = 0)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Circle value.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public double Value { get; set; }

    /// <summary>Circle minimum.</summary>
    [Parameter]
    public double Minimum { get; set; }

    /// <summary>Circle maximum.</summary>
    [Parameter]
    public double Maximum { get; set; } = 100;

    /// <summary>Circle color.</summary>
    [Parameter]
    public SixLabors.ImageSharp.Color? Color { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ChartCircle(Name, Value, Minimum, Maximum, ChartColorConverter.Convert(Color)));
    }
}

using System.Management.Automation;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates progress-bar chart row.</summary>
[Cmdlet(VerbsCommon.New, "ImageChartProgress")]
public sealed class NewImageChartProgressCmdlet : PSCmdlet {
    /// <summary>Label for the progress row.</summary>
    [Alias("Label")]
    [Parameter(Mandatory = true, Position = 0)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Progress value.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public double Value { get; set; }

    /// <summary>Progress color.</summary>
    [Parameter]
    public SixLabors.ImageSharp.Color? Color { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ChartProgress(Name, Value, ChartColorConverter.Convert(Color)));
    }
}

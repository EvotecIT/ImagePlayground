using System.Management.Automation;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates pictorial chart row.</summary>
[Cmdlet(VerbsCommon.New, "ImageChartPictorial")]
public sealed class NewImageChartPictorialCmdlet : PSCmdlet {
    /// <summary>Label for the pictorial row.</summary>
    [Alias("Label")]
    [Parameter(Mandatory = true, Position = 0)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Pictorial value.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public double Value { get; set; }

    /// <summary>Pictorial color.</summary>
    [Parameter]
    public SixLabors.ImageSharp.Color? Color { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ChartPictorial(Name, Value, ChartColorConverter.Convert(Color)));
    }
}

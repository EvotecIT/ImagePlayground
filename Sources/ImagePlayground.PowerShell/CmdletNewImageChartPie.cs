using System.Management.Automation;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates pie chart data item.</summary>
/// <example>
///   <summary>Create pie slice</summary>
///   <code>New-ImageChartPie -Name 'Chrome' -Value 60 -Color Red</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageChartPie")]
public sealed class NewImageChartPieCmdlet : PSCmdlet {
    /// <summary>Label for the slice.</summary>
    [Alias("Label")]
    [Parameter(Mandatory = true, Position = 0)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Value for the slice.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public double Value { get; set; }

    /// <summary>Slice color.</summary>
    [Parameter]
    public SixLabors.ImageSharp.Color? Color { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ChartPie(Name, Value, Color));
    }
}

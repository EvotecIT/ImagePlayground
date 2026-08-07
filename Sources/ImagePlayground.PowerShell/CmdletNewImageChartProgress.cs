using System.Management.Automation;
using ChartForgeX.Primitives;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates progress-bar chart row.</summary>
/// <example>
///   <summary>Create a deployment progress row</summary>
///   <code>New-ImageChartProgress -Name 'Deployment' -Value 78 -Color '#8B5CF6'</code>
/// </example>
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
    [ChartColorArgumentTransformation]
    public ChartColor? Color { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ChartProgress(Name, Value, ChartColorConverter.Convert(Color)));
    }
}

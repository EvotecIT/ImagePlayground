using System.Management.Automation;
using ChartForgeX.Primitives;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates treemap chart item.</summary>
/// <example>
///   <summary>Create treemap item</summary>
///   <code>New-ImageChartTreemap -Name 'Servers' -Value 42 -Color Cyan</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageChartTreemap")]
public sealed class NewImageChartTreemapCmdlet : PSCmdlet {
    /// <summary>Item label.</summary>
    [Alias("Label")]
    [Parameter(Mandatory = true, Position = 0)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Item value.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public double Value { get; set; }

    /// <summary>Item color.</summary>
    [Parameter]
    [ChartColorArgumentTransformation]
    public ChartColor? Color { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ChartTreemap(Name, Value, ChartColorConverter.Convert(Color)));
    }
}

using System.Management.Automation;
using ChartForgeX.Primitives;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates funnel chart item.</summary>
/// <example>
///   <summary>Create funnel item</summary>
///   <code>New-ImageChartFunnel -Name 'Trials' -Value 220 -Color Blue</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageChartFunnel")]
public sealed class NewImageChartFunnelCmdlet : PSCmdlet {
    /// <summary>Stage label.</summary>
    [Alias("Label")]
    [Parameter(Mandatory = true, Position = 0)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Stage value.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public double Value { get; set; }

    /// <summary>Stage color.</summary>
    [Parameter]
    [ChartColorArgumentTransformation]
    public ChartColor? Color { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ChartFunnel(Name, Value, ChartColorConverter.Convert(Color)));
    }
}

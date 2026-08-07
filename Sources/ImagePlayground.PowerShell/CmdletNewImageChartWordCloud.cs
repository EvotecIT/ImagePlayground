using System.Management.Automation;
using ChartForgeX.Primitives;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates word cloud chart term.</summary>
/// <example>
///   <summary>Create a weighted word-cloud term</summary>
///   <code>New-ImageChartWordCloud -Text 'PowerShell' -Weight 12 -Color '#2DD4BF'</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageChartWordCloud")]
public sealed class NewImageChartWordCloudCmdlet : PSCmdlet {
    /// <summary>Term text.</summary>
    [Alias("Name")]
    [Parameter(Mandatory = true, Position = 0)]
    public string Text { get; set; } = string.Empty;

    /// <summary>Term weight.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public double Weight { get; set; }

    /// <summary>Term color.</summary>
    [Parameter]
    [ChartColorArgumentTransformation]
    public ChartColor? Color { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ChartWordCloud(Text, Weight, ChartColorConverter.Convert(Color)));
    }
}

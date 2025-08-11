using System.Management.Automation;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates chart annotation data item.</summary>
/// <example>
///   <summary>Create annotation</summary>
///   <code>New-ImageChartAnnotation -X 1 -Y 2 -Text 'Hello' -Arrow</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageChartAnnotation")]
public sealed class NewImageChartAnnotationCmdlet : PSCmdlet {
    /// <summary>X coordinate for annotation.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public double X { get; set; }

    /// <summary>Y coordinate for annotation.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public double Y { get; set; }

    /// <summary>Annotation text.</summary>
    [Parameter(Mandatory = true, Position = 2)]
    public string Text { get; set; } = string.Empty;

    /// <summary>Display arrow.</summary>
    [Parameter]
    public SwitchParameter Arrow { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        WriteObject(new ChartAnnotation(X, Y, Text, Arrow.IsPresent));
    }
}

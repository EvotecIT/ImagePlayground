using System.Management.Automation;
using ImagePlayground;

namespace ImagePlayground.PowerShell;

/// <summary>Creates chart annotation data item.</summary>
/// <para>Use this cmdlet with <c>New-ImageChart</c> to highlight notable points on a generated chart.</para>
/// <example>
///   <summary>Create a chart annotation</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageChartAnnotation -X 3 -Y 61 -Text 'Peak usage' -Arrow</code>
///   <para>Creates an annotation definition that can be passed to New-ImageChart via -Annotation or -AnnotationsDefinition.</para>
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

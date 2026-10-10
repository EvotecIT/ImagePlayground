using ChartForgeX.Composition;
using ChartForgeX.Primitives;
using ChartForgeX.Typography;
using ImagePlayground;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a positioned text layer for a visual canvas.</summary>
/// <example>
///   <summary>Create a social-preview heading</summary>
///   <prefix>PS&gt; </prefix>
///   <code>New-ImageCanvasText -X 72 -Y 70 -Width 900 -Text 'Release 3.3' -FontSize 56 -Color White -Emphasized</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageCanvasText")]
[OutputType(typeof(VisualCanvasTextLayer))]
public sealed class NewImageCanvasTextCmdlet : PSCmdlet {
    /// <summary>Horizontal position in canvas design pixels.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public double X { get; set; }

    /// <summary>Vertical position in canvas design pixels.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public double Y { get; set; }

    /// <summary>Text layout width.</summary>
    [Parameter(Mandatory = true, Position = 2)]
    public double Width { get; set; }

    /// <summary>Text to render.</summary>
    [Parameter(Mandatory = true, Position = 3)]
    public string Text { get; set; } = string.Empty;

    /// <summary>Font size in canvas design pixels.</summary>
    [Parameter]
    [ValidateRange(1, 1000)]
    public double FontSize { get; set; } = 32;

    /// <summary>Explicit text color. Omit this parameter to follow the canvas foreground; explicitly supplied white, transparent, or custom colors are retained.</summary>
    [Parameter]
    [ChartColorArgumentTransformation]
    public ChartColor Color { get; set; } = ChartColor.White;

    /// <summary>Text alignment within the layer width.</summary>
    [Parameter]
    public TextAlignment Alignment { get; set; } = TextAlignment.Left;

    /// <summary>Use the emphasized text treatment.</summary>
    [Parameter]
    public SwitchParameter Emphasized { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var layer = MyInvocation.BoundParameters.ContainsKey(nameof(Color))
            ? new VisualCanvasTextLayer(X, Y, Width, Text, FontSize, Color)
            : new VisualCanvasTextLayer(X, Y, Width, Text, FontSize);
        layer.Alignment = Alignment;
        layer.Emphasized = Emphasized.IsPresent;
        WriteObject(layer);
    }
}

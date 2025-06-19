using ImagePlayground;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Adds wrapped text to an image within a box.</summary>
/// <example>
///   <summary>Add text inside a box</summary>
///   <code>Add-ImageTextBox -FilePath in.png -OutputPath out.png -Text "Sample text" -X 10 -Y 10 -Width 100</code>
/// </example>
[Cmdlet(VerbsCommon.Add, "ImageTextBox")]
public sealed class AddImageTextBoxCmdlet : PSCmdlet {
    /// <summary>Source image path.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Destination image path.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public string OutputPath { get; set; } = string.Empty;

    /// <summary>Text to add.</summary>
    [Parameter(Mandatory = true, Position = 2)]
    public string Text { get; set; } = string.Empty;

    /// <summary>X coordinate.</summary>
    [Parameter(Mandatory = true, Position = 3)]
    public float X { get; set; }

    /// <summary>Y coordinate.</summary>
    [Parameter(Mandatory = true, Position = 4)]
    public float Y { get; set; }

    /// <summary>Width of the text box.</summary>
    [Parameter(Mandatory = true, Position = 5)]
    public float Width { get; set; }

    /// <summary>Height of the text box.</summary>
    [Parameter(Position = 6)]
    public float Height { get; set; } = 0f;

    /// <summary>Text color.</summary>
    [Parameter]
    public SixLabors.ImageSharp.Color Color { get; set; } = SixLabors.ImageSharp.Color.Black;

    /// <summary>Font size.</summary>
    [Parameter]
    public float FontSize { get; set; } = 16f;

    /// <summary>Font family.</summary>
    [Parameter]
    public string FontFamily { get; set; } = "Arial";

    /// <summary>Horizontal alignment.</summary>
    [Parameter]
    public SixLabors.Fonts.HorizontalAlignment HorizontalAlignment { get; set; } = SixLabors.Fonts.HorizontalAlignment.Left;

    /// <summary>Vertical alignment.</summary>
    [Parameter]
    public SixLabors.Fonts.VerticalAlignment VerticalAlignment { get; set; } = SixLabors.Fonts.VerticalAlignment.Top;

    /// <summary>Color of shadow.</summary>
    [Parameter]
    public SixLabors.ImageSharp.Color? ShadowColor { get; set; }

    /// <summary>X offset for shadow.</summary>
    [Parameter]
    public float ShadowOffsetX { get; set; } = 0f;

    /// <summary>Y offset for shadow.</summary>
    [Parameter]
    public float ShadowOffsetY { get; set; } = 0f;

    /// <summary>Outline color.</summary>
    [Parameter]
    public SixLabors.ImageSharp.Color? OutlineColor { get; set; }

    /// <summary>Outline width.</summary>
    [Parameter]
    public float OutlineWidth { get; set; } = 0f;

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var filePath = Helpers.ResolvePath(FilePath);
        if (!File.Exists(filePath)) {
            WriteWarning($"Add-ImageTextBox - File {FilePath} not found. Please check the path.");
            return;
        }

        var output = Helpers.ResolvePath(OutputPath);
        ImagePlayground.ImageHelper.AddTextBox(
            filePath,
            output,
            X,
            Y,
            Text,
            Width,
            Height,
            Color,
            FontSize,
            FontFamily,
            HorizontalAlignment,
            VerticalAlignment,
            ShadowColor,
            ShadowOffsetX,
            ShadowOffsetY,
            OutlineColor,
            OutlineWidth);
    }
}

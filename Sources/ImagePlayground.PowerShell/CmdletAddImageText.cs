using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Adds text to an image and saves the result.</summary>
/// <example>
///   <summary>Add watermark text</summary>
///   <code>Add-ImageText -FilePath in.png -OutputPath out.png -Text "Sample" -X 10 -Y 10</code>
/// </example>
[Cmdlet(VerbsCommon.Add, "ImageText")]
public sealed class AddImageTextCmdlet : PSCmdlet {
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

    /// <summary>Text color.</summary>
    [Parameter]
    public SixLabors.ImageSharp.Color Color { get; set; } = SixLabors.ImageSharp.Color.Black;

    /// <summary>Font size.</summary>
    [Parameter]
    public float FontSize { get; set; } = 16f;

    /// <summary>Font family.</summary>
    [Parameter]
    public string FontFamily { get; set; } = "Arial";

    /// <inheritdoc />
    protected override void ProcessRecord() {
        if (!File.Exists(FilePath)) {
            WriteWarning($"Add-ImageText - File {FilePath} not found. Please check the path.");
            return;
        }

        ImagePlayground.ImageHelper.AddText(FilePath, OutputPath, X, Y, Text, Color, FontSize, FontFamily);
    }
}

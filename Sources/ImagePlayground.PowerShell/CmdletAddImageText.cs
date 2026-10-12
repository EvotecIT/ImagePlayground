using ImagePlayground;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>
/// Adds text to an image at the provided coordinates and writes the updated
/// image to disk.
/// </summary>
/// <example>
///   <summary>Add watermark text</summary>
///   <code>Add-ImageText -FilePath in.png -OutputPath out.png -Text "Sample" -X 10 -Y 10</code>
/// </example>
/// <example>
///   <summary>Edit an image and save it</summary>
///   <code>
/// $img = Get-Image -FilePath in.png
/// try {
///     $img.AddText(10, 10, 'Sample', [OfficeIMO.Drawing.OfficeColor]::Black, 24)
///     Save-Image -Image $img -FilePath out.png
/// } finally {
///     $img.Dispose()
/// }
/// </code>
/// </example>
[Cmdlet(VerbsCommon.Add, "ImageText")]
public sealed class AddImageTextCmdlet : ImageCmdlet {
    /// <summary>Source image path.</summary>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0)]
    [ValidateNotNullOrEmpty]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Destination image path.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    [ValidateNotNullOrEmpty]
    public string OutputPath { get; set; } = string.Empty;

    /// <summary>Text to add.</summary>
    [Parameter(Mandatory = true, Position = 2)]
    [ValidateNotNullOrEmpty]
    public string Text { get; set; } = string.Empty;

    /// <summary>X coordinate.</summary>
    [Parameter(Mandatory = true, Position = 3)]
    [ValidateRange(0, float.MaxValue)]
    public float X { get; set; }

    /// <summary>Y coordinate.</summary>
    [Parameter(Mandatory = true, Position = 4)]
    [ValidateRange(0, float.MaxValue)]
    public float Y { get; set; }

    /// <summary>Text color.</summary>
    [Parameter]
    public OfficeIMO.Drawing.OfficeColor Color { get; set; } = OfficeIMO.Drawing.OfficeColor.Black;

    /// <summary>Font size.</summary>
    [Parameter]
    public float FontSize { get; set; } = 16f;

    /// <summary>Font family.</summary>
    [Parameter]
    public string FontFamily { get; set; } = "Arial";

    /// <summary>Color of shadow.</summary>
    [Parameter]
    public OfficeIMO.Drawing.OfficeColor? ShadowColor { get; set; }

    /// <summary>X offset for shadow.</summary>
    [Parameter]
    public float ShadowOffsetX { get; set; } = 0f;

    /// <summary>Y offset for shadow.</summary>
    [Parameter]
    public float ShadowOffsetY { get; set; } = 0f;

    /// <summary>Outline color.</summary>
    [Parameter]
    public OfficeIMO.Drawing.OfficeColor? OutlineColor { get; set; }

    /// <summary>Outline width.</summary>
    [Parameter]
    public float OutlineWidth { get; set; } = 0f;

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var filePath = ResolveExistingFilePath(FilePath, "AddImageTextFileNotFound", FilePath);

        using (var img = ImagePlayground.Image.Load(filePath)) {
            if (X >= img.Width || Y >= img.Height) {
                var message = $"Coordinates ({X},{Y}) exceed image bounds {img.Width}x{img.Height}.";
                var ex = new ArgumentOutOfRangeException(nameof(X), message);
                ThrowTerminatingError(new ErrorRecord(ex, "InvalidCoordinates", ErrorCategory.InvalidArgument, this));
            }
        }

        var output = PowerShellPathResolver.ResolveFileSystemPath(this, OutputPath);
        ImagePlayground.ImageHelper.AddText(
            filePath,
            output,
            X,
            Y,
            Text,
            Color,
            FontSize,
            FontFamily,
            ShadowColor,
            ShadowOffsetX,
            ShadowOffsetY,
            OutlineColor,
            OutlineWidth);
    }
}

using System;
using System.IO;
using System.Management.Automation;
using ChartForgeX.Composition;
using ChartForgeX.Primitives;
using ChartForgeX.VisualArtifacts;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a deterministic text or image watermark for ChartForgeX visual artifacts.</summary>
/// <example>
///   <summary>Create a centered confidential watermark</summary>
///   <prefix>PS&gt; </prefix>
///   <code>$mark = New-ImageVisualWatermark -Text CONFIDENTIAL -Anchor Center -RotationDegrees -28 -Opacity 0.15</code>
///   <para>The watermark can be passed to Export-ImageVisualArtifact or New-ImageTopology.</para>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageVisualWatermark", DefaultParameterSetName = TextSet)]
[OutputType(typeof(VisualWatermark))]
public sealed class NewImageVisualWatermarkCmdlet : ImageCmdlet {
    private const string TextSet = "Text";
    private const string ImageSet = "Image";

    /// <para>Watermark text.</para>
    [Parameter(Mandatory = true, Position = 0, ParameterSetName = TextSet)]
    public string Text { get; set; } = string.Empty;

    /// <para>Raster watermark image path.</para>
    [Parameter(Mandatory = true, Position = 0, ParameterSetName = ImageSet)]
    public string ImagePath { get; set; } = string.Empty;

    /// <para>Watermark anchor on the artifact canvas.</para>
    [Parameter]
    public VisualCanvasAnchor Anchor { get; set; } = VisualCanvasAnchor.BottomRight;

    /// <para>Horizontal offset from the selected anchor.</para>
    [Parameter]
    public double OffsetX { get; set; }

    /// <para>Vertical offset from the selected anchor.</para>
    [Parameter]
    public double OffsetY { get; set; }

    /// <para>Inset from anchored canvas edges.</para>
    [Parameter]
    [ValidateRange(0D, 10000D)]
    public double Padding { get; set; } = 24D;

    /// <para>Watermark opacity from zero to one.</para>
    [Parameter]
    [ValidateRange(0D, 1D)]
    public double Opacity { get; set; } = 0.18D;

    /// <para>Clockwise watermark rotation in degrees.</para>
    [Parameter]
    public double RotationDegrees { get; set; }

    /// <para>Positive watermark scale.</para>
    [Parameter]
    [ValidateRange(0.01D, 100D)]
    public double Scale { get; set; } = 1D;

    /// <para>Text color.</para>
    [Parameter(ParameterSetName = TextSet)]
    [ChartColorArgumentTransformation]
    public ChartColor Color { get; set; } = ChartColor.FromHex("#64748B");

    /// <para>Base text size in pixels.</para>
    [Parameter(ParameterSetName = TextSet)]
    [ValidateRange(1D, 1000D)]
    public double FontSize { get; set; } = 28D;

    /// <para>Render repeated watermark tiles across the canvas.</para>
    [Parameter]
    public SwitchParameter Repeat { get; set; }

    /// <para>Horizontal spacing between repeated watermark anchors.</para>
    [Parameter]
    [ValidateRange(1D, 10000D)]
    public double RepeatSpacingX { get; set; } = 180D;

    /// <para>Vertical spacing between repeated watermark anchors.</para>
    [Parameter]
    [ValidateRange(1D, 10000D)]
    public double RepeatSpacingY { get; set; } = 120D;

    /// <inheritdoc />
    protected override void ProcessRecord() {
        VisualWatermark watermark;
        if (ParameterSetName == ImageSet) {
            string path = ResolveExistingFilePath(ImagePath, "NewImageVisualWatermarkFileNotFound", ImagePath, "Watermark image");
            watermark = VisualWatermark.FromImage(File.ReadAllBytes(path), ResolveMimeType(path));
        } else {
            watermark = VisualWatermark.FromText(Text);
            watermark.Color = Color;
            watermark.FontSize = FontSize;
        }
        watermark.Anchor = Anchor;
        watermark.OffsetX = OffsetX;
        watermark.OffsetY = OffsetY;
        watermark.Padding = Padding;
        watermark.Opacity = Opacity;
        watermark.RotationDegrees = RotationDegrees;
        watermark.Scale = Scale;
        watermark.Repeat = Repeat.IsPresent;
        watermark.RepeatSpacingX = RepeatSpacingX;
        watermark.RepeatSpacingY = RepeatSpacingY;
        WriteObject(watermark);
    }

    private static string ResolveMimeType(string path) {
        string extension = Path.GetExtension(path);
        if (extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase) || extension.Equals(".jpeg", StringComparison.OrdinalIgnoreCase)) return "image/jpeg";
        if (extension.Equals(".bmp", StringComparison.OrdinalIgnoreCase)) return "image/bmp";
        if (extension.Equals(".gif", StringComparison.OrdinalIgnoreCase)) return "image/gif";
        if (extension.Equals(".tif", StringComparison.OrdinalIgnoreCase) || extension.Equals(".tiff", StringComparison.OrdinalIgnoreCase)) return "image/tiff";
        return "image/png";
    }
}

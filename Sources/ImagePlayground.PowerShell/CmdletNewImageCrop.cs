using ImagePlayground;
using System.IO;
using System.Management.Automation;
using SixLabors.ImageSharp;

namespace ImagePlayground.PowerShell;

/// <summary>
/// Creates a cropped version of an image using rectangular, circular or
/// polygonal areas.
/// </summary>
/// <example>
///   <summary>Crop using rectangle</summary>
///   <code>New-ImageCrop -FilePath in.png -OutputPath out.png -X 10 -Y 10 -Width 100 -Height 100</code>
/// </example>
/// <example>
///   <summary>Crop using circle</summary>
///   <code>New-ImageCrop -FilePath in.png -OutputPath out.png -CenterX 50 -CenterY 50 -Radius 25</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageCrop", DefaultParameterSetName = ParameterSetRectangle)]
public sealed class NewImageCropCmdlet : PSCmdlet {
    private const string ParameterSetRectangle = "Rectangle";
    private const string ParameterSetCircle = "Circle";
    private const string ParameterSetPolygon = "Polygon";

    /// <summary>Path to the image being cropped.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Where to save the cropped image.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public string OutputPath { get; set; } = string.Empty;

    /// <summary>X coordinate for rectangle cropping.</summary>
    [Parameter(ParameterSetName = ParameterSetRectangle)]
    public int X { get; set; }

    /// <summary>Y coordinate for rectangle cropping.</summary>
    [Parameter(ParameterSetName = ParameterSetRectangle)]
    public int Y { get; set; }

    /// <summary>Width of the rectangle.</summary>
    [Parameter(ParameterSetName = ParameterSetRectangle)]
    public int Width { get; set; }

    /// <summary>Height of the rectangle.</summary>
    [Parameter(ParameterSetName = ParameterSetRectangle)]
    public int Height { get; set; }

    /// <summary>X coordinate of the circle center.</summary>
    [Parameter(ParameterSetName = ParameterSetCircle)]
    public float CenterX { get; set; }

    /// <summary>Y coordinate of the circle center.</summary>
    [Parameter(ParameterSetName = ParameterSetCircle)]
    public float CenterY { get; set; }

    /// <summary>Radius of the circle.</summary>
    [Parameter(ParameterSetName = ParameterSetCircle)]
    public float Radius { get; set; }

    /// <summary>Points describing a polygon.</summary>
    [Parameter(ParameterSetName = ParameterSetPolygon)]
    public PointF[] Points { get; set; } = System.Array.Empty<PointF>();

    /// <summary>Open the cropped file after creation.</summary>
    [Parameter]
    public SwitchParameter Open { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var filePath = Helpers.ResolvePath(FilePath);
        if (!File.Exists(filePath)) {
            WriteWarning($"New-ImageCrop - File {FilePath} not found. Please check the path.");
            return;
        }
        var output = Helpers.ResolvePath(OutputPath);

        if (ParameterSetName == ParameterSetCircle) {
            ImageHelper.CropCircle(filePath, output, CenterX, CenterY, Radius);
        } else if (ParameterSetName == ParameterSetPolygon) {
            if (Points.Length < 3) {
                WriteWarning("New-ImageCrop - At least three points are required for polygon crop.");
                return;
            }
            ImageHelper.CropPolygon(filePath, output, Points);
        } else {
            var rect = new Rectangle(X, Y, Width, Height);
            ImageHelper.Crop(filePath, output, rect);
        }

        if (Open.IsPresent) {
            Helpers.Open(output, true);
        }
    }
}

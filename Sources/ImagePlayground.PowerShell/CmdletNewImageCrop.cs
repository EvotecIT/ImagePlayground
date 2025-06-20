using ImagePlayground;
using System.IO;
using System.Management.Automation;
using SixLabors.ImageSharp;

namespace ImagePlayground.PowerShell;

[Cmdlet(VerbsCommon.New, "ImageCrop", DefaultParameterSetName = ParameterSetRectangle)]
public sealed class NewImageCropCmdlet : PSCmdlet {
    private const string ParameterSetRectangle = "Rectangle";
    private const string ParameterSetCircle = "Circle";
    private const string ParameterSetPolygon = "Polygon";

    [Parameter(Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    [Parameter(Mandatory = true, Position = 1)]
    public string OutputPath { get; set; } = string.Empty;

    [Parameter(ParameterSetName = ParameterSetRectangle)]
    public int X { get; set; }

    [Parameter(ParameterSetName = ParameterSetRectangle)]
    public int Y { get; set; }

    [Parameter(ParameterSetName = ParameterSetRectangle)]
    public int Width { get; set; }

    [Parameter(ParameterSetName = ParameterSetRectangle)]
    public int Height { get; set; }

    [Parameter(ParameterSetName = ParameterSetCircle)]
    public float CenterX { get; set; }

    [Parameter(ParameterSetName = ParameterSetCircle)]
    public float CenterY { get; set; }

    [Parameter(ParameterSetName = ParameterSetCircle)]
    public float Radius { get; set; }

    [Parameter(ParameterSetName = ParameterSetPolygon)]
    public PointF[] Points { get; set; } = System.Array.Empty<PointF>();

    [Parameter]
    public SwitchParameter Open { get; set; }

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

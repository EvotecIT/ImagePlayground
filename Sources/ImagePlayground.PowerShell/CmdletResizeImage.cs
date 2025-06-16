using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>
/// Cmdlet that resizes an image by width/height or percentage.
/// </summary>
[Cmdlet(VerbsCommon.Resize, "Image", DefaultParameterSetName = ParameterSetHeightWidth)]
public sealed class ResizeImageCmdlet : Cmdlet {
    private const string ParameterSetHeightWidth = "HeightWidth";
    private const string ParameterSetPercentage = "Percentage";

    /// <summary>Path to the source image.</summary>
    [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSetHeightWidth)]
    [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSetPercentage)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Destination file path.</summary>
    [Parameter(Mandatory = true, Position = 1, ParameterSetName = ParameterSetHeightWidth)]
    [Parameter(Mandatory = true, Position = 1, ParameterSetName = ParameterSetPercentage)]
    public string OutputPath { get; set; } = string.Empty;

    /// <summary>New width of the image.</summary>
    [Parameter(ParameterSetName = ParameterSetHeightWidth)]
    public int Width { get; set; }

    /// <summary>New height of the image.</summary>
    [Parameter(ParameterSetName = ParameterSetHeightWidth)]
    public int Height { get; set; }

    /// <summary>Percentage based resize.</summary>
    [Parameter(ParameterSetName = ParameterSetPercentage)]
    public int Percentage { get; set; }

    /// <summary>Ignore aspect ratio.</summary>
    [Parameter(ParameterSetName = ParameterSetHeightWidth)]
    public SwitchParameter DontRespectAspectRatio { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        if (!File.Exists(FilePath)) {
            WriteWarning($"Resize-Image - File {FilePath} not found. Please check the path.");
            return;
        }

        if (ParameterSetName == ParameterSetPercentage) {
            ImagePlayground.ImageHelper.Resize(FilePath, OutputPath, Percentage);
            return;
        }

        bool widthBound = MyInvocation.BoundParameters.ContainsKey(nameof(Width));
        bool heightBound = MyInvocation.BoundParameters.ContainsKey(nameof(Height));

        if (!widthBound && !heightBound) {
            WriteWarning("Resize-Image - Please specify Width or Height or Percentage.");
            return;
        }

        int? width = widthBound ? Width : (int?)null;
        int? height = heightBound ? Height : (int?)null;
        bool keepAspect = !DontRespectAspectRatio.IsPresent;

        ImagePlayground.ImageHelper.Resize(FilePath, OutputPath, width, height, keepAspect);
    }
}

using ImagePlayground;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Creates a rounded avatar image.</summary>
/// <example>
///   <summary>Generate an avatar and open it</summary>
///   <code>New-ImageAvatar -FilePath user.jpg -OutputPath avatar.png -Open</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageAvatar", DefaultParameterSetName = ParameterSetPath)]
public sealed class NewImageAvatarCmdlet : PSCmdlet {
    private const string ParameterSetPath = "Path";
    private const string ParameterSetStream = "Stream";

    /// <summary>Path to the input image.</summary>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0, ParameterSetName = ParameterSetPath)]
    [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSetStream)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Destination path when saving to disk.</summary>
    [Parameter(Mandatory = true, Position = 1, ParameterSetName = ParameterSetPath)]
    public string OutputPath { get; set; } = string.Empty;

    /// <summary>Stream that receives the avatar image.</summary>
    [Parameter(Mandatory = true, Position = 1, ParameterSetName = ParameterSetStream)]
    public Stream OutputStream { get; set; } = Stream.Null;

    /// <summary>Width of the avatar.</summary>
    [Parameter]
    [ValidateRange(1, 1000)]
    public int Width { get; set; } = 200;

    /// <summary>Height of the avatar.</summary>
    [Parameter]
    [ValidateRange(1, 1000)]
    public int Height { get; set; } = 200;

    /// <summary>Corner radius for rounding.</summary>
    [Parameter]
    public float CornerRadius { get; set; } = 20f;

    /// <summary>Open the avatar after saving.</summary>
    [Parameter]
    public SwitchParameter Open { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var filePath = Helpers.ResolvePath(FilePath);
        if (!File.Exists(filePath)) {
            WriteWarning($"New-ImageAvatar - File {FilePath} not found. Please check the path.");
            return;
        }

        if (ParameterSetName == ParameterSetStream) {
            ImagePlayground.ImageHelper.Avatar(filePath, OutputStream, Width, Height, CornerRadius);
            OutputStream.Position = 0;
        } else {
            var output = Helpers.ResolvePath(OutputPath);
            Helpers.CreateParentDirectory(output);
            ImagePlayground.ImageHelper.Avatar(filePath, output, Width, Height, CornerRadius);
            if (Open.IsPresent) {
                ImagePlayground.Helpers.Open(output, true);
            }
        }
    }
}

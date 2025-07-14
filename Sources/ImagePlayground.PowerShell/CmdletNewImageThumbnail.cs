using ImagePlayground;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Creates thumbnails for all images in a directory.</summary>
/// <example>
///   <summary>Create 64x64 thumbnails</summary>
///   <code>New-ImageThumbnail -DirectoryPath images -OutputDirectory thumbs -Width 64 -Height 64</code>
/// </example>
[Cmdlet(VerbsCommon.New, "ImageThumbnail")]
public sealed class NewImageThumbnailCmdlet : PSCmdlet {
    /// <summary>Directory containing images.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public string DirectoryPath { get; set; } = string.Empty;

    /// <summary>Destination directory for thumbnails.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public string OutputDirectory { get; set; } = string.Empty;

    /// <summary>Thumbnail width.</summary>
    [Parameter]
    [ValidateRange(1, 10000)]
    public int Width { get; set; } = 100;

    /// <summary>Thumbnail height.</summary>
    [Parameter]
    [ValidateRange(1, 10000)]
    public int Height { get; set; } = 100;

    /// <summary>Ignore aspect ratio.</summary>
    [Parameter]
    public SwitchParameter DontRespectAspectRatio { get; set; }

    /// <summary>Resampling algorithm.</summary>
    [Parameter]
    public Sampler? Sampler { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var dir = Helpers.ResolvePath(DirectoryPath);
        if (!Directory.Exists(dir)) {
            WriteWarning($"New-ImageThumbnail - Directory {DirectoryPath} not found. Please check the path.");
            return;
        }
        var output = Helpers.ResolvePath(OutputDirectory);
        ImagePlayground.ImageHelper.GenerateThumbnails(dir, output, Width, Height, !DontRespectAspectRatio.IsPresent, Sampler);
    }
}

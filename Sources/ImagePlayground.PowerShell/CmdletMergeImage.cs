using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Merges two images and saves the result.</summary>
/// <example>
///   <summary>Merge vertically</summary>
///   <code>Merge-Image -FilePath img1.png -FilePathToMerge img2.png -FilePathOutput out.png</code>
/// </example>
[Cmdlet(VerbsData.Merge, "Image")]
public sealed class MergeImageCmdlet : PSCmdlet {
    /// <summary>Base image path.</summary>
    /// <para>Must exist.</para>
    [Parameter(Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Second image path.</summary>
    /// <para>Must exist.</para>
    [Parameter(Mandatory = true, Position = 1)]
    public string FilePathToMerge { get; set; } = string.Empty;

    /// <summary>Output file path.</summary>
    [Parameter(Mandatory = true, Position = 2)]
    public string FilePathOutput { get; set; } = string.Empty;

    /// <summary>Resize images to fit before merging.</summary>
    [Parameter]
    public SwitchParameter ResizeToFit { get; set; }

    /// <summary>Placement of the second image.</summary>
    [Parameter]
    public ImagePlayground.ImagePlacement Placement { get; set; } = ImagePlayground.ImagePlacement.Bottom;

    /// <inheritdoc />
    protected override void ProcessRecord() {
        string src1 = ImagePlayground.Helpers.ResolvePath(FilePath);
        string src2 = ImagePlayground.Helpers.ResolvePath(FilePathToMerge);
        string dest = ImagePlayground.Helpers.ResolvePath(FilePathOutput);
        if (!File.Exists(src1)) {
            WriteWarning($"Merge-Image - File {FilePath} not found. Please check the path.");
            return;
        }
        if (!File.Exists(src2)) {
            WriteWarning($"Merge-Image - File {FilePathToMerge} not found. Please check the path.");
            return;
        }

        ImagePlayground.ImageHelper.Combine(src1, src2, dest, ResizeToFit.IsPresent, Placement);
    }
}

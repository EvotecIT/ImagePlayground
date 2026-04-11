using ImagePlayground;
using System.IO;
using System.Management.Automation;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;

namespace ImagePlayground.PowerShell;

/// <summary>Removes EXIF metadata from an image.</summary>
/// <example>
///   <summary>Remove specific tag</summary>
///   <code>Remove-ImageExif -FilePath img.jpg -ExifTag ExifIFD.DateTimeOriginal</code>
/// </example>
/// <example>
///   <summary>Remove all tags</summary>
///   <code>Remove-ImageExif -FilePath img.jpg -All</code>
/// </example>
[Cmdlet(VerbsCommon.Remove, "ImageExif", DefaultParameterSetName = ParameterSetTag)]
public sealed class RemoveImageExifCmdlet : PSCmdlet {
    private const string ParameterSetTag = "Tag";
    private const string ParameterSetAll = "All";

    /// <summary>Path to the image file.</summary>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0, ParameterSetName = ParameterSetTag)]
    [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSetAll)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Optional output path.</summary>
    [Parameter(Position = 1, ParameterSetName = ParameterSetTag)]
    [Parameter(Position = 1, ParameterSetName = ParameterSetAll)]
    public string? FilePathOutput { get; set; }

    /// <summary>Tags to remove.</summary>
    [Parameter(Mandatory = true, ParameterSetName = ParameterSetTag)]
    public ExifTag[] ExifTag { get; set; } = null!;

    /// <summary>Remove all tags.</summary>
    [Parameter(Mandatory = true, ParameterSetName = ParameterSetAll)]
    public SwitchParameter All { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var filePath = Helpers.ResolvePath(FilePath);
        if (!File.Exists(filePath)) {
            WriteWarning($"Remove-ImageExif - File not found: {FilePath}");
            return;
        }

        var output = string.IsNullOrWhiteSpace(FilePathOutput) ? filePath : Helpers.ResolvePath(FilePathOutput!);
        Helpers.CreateParentDirectory(output);
        WriteVerbose($"Saving image to {output}");

        if (ParameterSetName == ParameterSetAll) {
            ImagePlayground.Image.ClearExifValues(filePath, output);
        } else {
            ImagePlayground.Image.RemoveExifValues(filePath, output, ExifTag);
        }

        WriteVerbose($"Saved image to {output}");
    }
}

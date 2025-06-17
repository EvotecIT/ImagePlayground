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
    [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSetTag)]
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
        string src = ImagePlayground.Helpers.ResolvePath(FilePath);
        string dest = string.IsNullOrWhiteSpace(FilePathOutput) ? src : ImagePlayground.Helpers.ResolvePath(FilePathOutput!);
        if (!File.Exists(src)) {
            WriteWarning($"Remove-ImageExif - File not found: {FilePath}");
            return;
        }

        using var img = ImagePlayground.Image.Load(src);
        if (ParameterSetName == ParameterSetAll) {
            img.ClearExifValues();
        } else {
            img.RemoveExifValues(ExifTag);
        }

        img.Save(dest);
    }
}


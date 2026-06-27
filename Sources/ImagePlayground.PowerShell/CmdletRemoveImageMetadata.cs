using ImagePlayground;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Removes metadata from an image.</summary>
/// <example>
///   <summary>Save a copy without metadata</summary>
///   <code>Remove-ImageMetadata -FilePath in.jpg -OutputPath out.jpg</code>
/// </example>
[Cmdlet(VerbsCommon.Remove, "ImageMetadata")]
public sealed class RemoveImageMetadataCmdlet : ImageCmdlet {
    /// <summary>Source image file.</summary>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Destination image path.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public string OutputPath { get; set; } = string.Empty;

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var filePath = ResolveExistingFilePath(FilePath, "RemoveImageMetadataFileNotFound", FilePath);
        var output = Helpers.ResolvePath(OutputPath);
        ImageHelper.RemoveMetadata(filePath, output);
    }
}

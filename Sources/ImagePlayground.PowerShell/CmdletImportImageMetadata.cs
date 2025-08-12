using ImagePlayground;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Imports metadata into an image.</summary>
/// <example>
///   <summary>Apply metadata from file</summary>
///   <code>Import-ImageMetadata -FilePath img.jpg -MetadataPath meta.json</code>
/// </example>
/// <example>
///   <summary>Save to new file</summary>
///   <code>Import-ImageMetadata -FilePath img.jpg -MetadataPath meta.json -OutputPath out.jpg</code>
/// </example>
[Cmdlet(VerbsData.Import, "ImageMetadata")]
public sealed class ImportImageMetadataCmdlet : PSCmdlet {
    /// <summary>Image to update.</summary>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>JSON metadata file.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public string MetadataPath { get; set; } = string.Empty;

    /// <summary>Destination image path. Defaults to FilePath.</summary>
    [Parameter(Position = 2)]
    public string? OutputPath { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var filePath = Helpers.ResolvePath(FilePath);
        if (!File.Exists(filePath)) {
            WriteWarning($"Import-ImageMetadata - File {FilePath} not found. Please check the path.");
            return;
        }

        var metaPath = Helpers.ResolvePath(MetadataPath);
        if (!File.Exists(metaPath)) {
            WriteWarning($"Import-ImageMetadata - Metadata file {MetadataPath} not found. Please check the path.");
            return;
        }

        var output = string.IsNullOrWhiteSpace(OutputPath) ? filePath : Helpers.ResolvePath(OutputPath!);
        Helpers.CreateParentDirectory(output);
        var options = new ImageHelper.ImportMetadataOptions(filePath, metaPath, output);
        ImageHelper.ImportMetadata(options);
    }
}

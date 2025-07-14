using ImagePlayground;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Exports metadata from an image.</summary>
/// <example>
///   <summary>Get metadata JSON string</summary>
///   <code>Export-ImageMetadata -FilePath in.jpg</code>
/// </example>
/// <example>
///   <summary>Save metadata to a file</summary>
///   <code>Export-ImageMetadata -FilePath in.jpg -OutputPath meta.json</code>
/// </example>
[Cmdlet(VerbsData.Export, "ImageMetadata")]
[OutputType(typeof(string))]
public sealed class ExportImageMetadataCmdlet : PSCmdlet {
    /// <summary>Source image file.</summary>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Optional path to write metadata JSON.</summary>
    [Parameter(Position = 1)]
    public string? OutputPath { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var filePath = Helpers.ResolvePath(FilePath);
        if (!File.Exists(filePath)) {
            WriteWarning($"Export-ImageMetadata - File {FilePath} not found. Please check the path.");
            return;
        }

        string json = ImageHelper.ExportMetadata(filePath);
        if (string.IsNullOrWhiteSpace(OutputPath)) {
            WriteObject(json);
        } else {
            var output = Helpers.ResolvePath(OutputPath!);
            File.WriteAllText(output, json);
        }
    }
}

using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>
/// Cmdlet that converts an image to another format.
/// </summary>
[Cmdlet(VerbsData.ConvertTo, "Image")]
public sealed class ConvertToImageCmdlet : PSCmdlet {
    /// <summary>Path to the source image.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Destination file path including extension.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public string OutputPath { get; set; } = string.Empty;

    /// <inheritdoc />
    protected override void ProcessRecord() {
        if (!File.Exists(FilePath)) {
            WriteWarning($"ConvertTo-Image - File {FilePath} not found. Please check the path.");
            return;
        }

        ImagePlayground.ImageHelper.ConvertTo(FilePath, OutputPath);
    }
}
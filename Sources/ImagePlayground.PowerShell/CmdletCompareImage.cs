using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Compares two images and optionally saves a difference mask.</summary>
/// <example>
///   <summary>Compare two images</summary>
///   <code>Compare-Image -FilePath img1.png -FilePathToCompare img2.png</code>
/// </example>
[Cmdlet(VerbsData.Compare, "Image")]
public sealed class CompareImageCmdlet : PSCmdlet {
    /// <summary>First image path.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Second image path.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public string FilePathToCompare { get; set; } = string.Empty;

    /// <summary>Output path for difference mask.</summary>
    [Parameter(Position = 2)]
    public string? OutputPath { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        if (!File.Exists(FilePath)) {
            WriteWarning($"Compare-Image - File {FilePath} not found. Please check the path.");
            return;
        }
        if (!File.Exists(FilePathToCompare)) {
            WriteWarning($"Compare-Image - File {FilePathToCompare} not found. Please check the path.");
            return;
        }

        if (!string.IsNullOrWhiteSpace(OutputPath)) {
            ImagePlayground.ImageHelper.Compare(FilePath, FilePathToCompare, OutputPath);
        } else {
            var result = ImagePlayground.ImageHelper.Compare(FilePath, FilePathToCompare);
            WriteObject(result);
        }
    }
}

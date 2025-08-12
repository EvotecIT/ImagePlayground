using ImagePlayground;
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
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Second image path.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public string FilePathToCompare { get; set; } = string.Empty;

    /// <summary>Output path for difference mask.</summary>
    [Parameter(Position = 2)]
    public string? OutputPath { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var filePath = Helpers.ResolvePath(FilePath);
        if (!File.Exists(filePath)) {
            WriteWarning($"Compare-Image - File {FilePath} not found. Please check the path.");
            return;
        }
        var compare = Helpers.ResolvePath(FilePathToCompare);
        if (!File.Exists(compare)) {
            WriteWarning($"Compare-Image - File {FilePathToCompare} not found. Please check the path.");
            return;
        }

        if (!string.IsNullOrWhiteSpace(OutputPath)) {
            var output = Helpers.ResolvePath(OutputPath);
            Helpers.CreateParentDirectory(output);
            ImagePlayground.ImageHelper.Compare(filePath, compare, output);
        } else {
            var result = ImagePlayground.ImageHelper.Compare(filePath, compare);
            WriteObject(result);
        }
    }
}

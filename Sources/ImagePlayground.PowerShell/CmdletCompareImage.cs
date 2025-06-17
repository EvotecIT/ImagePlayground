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
        string src1 = ImagePlayground.Helpers.ResolvePath(FilePath);
        string src2 = ImagePlayground.Helpers.ResolvePath(FilePathToCompare);
        if (!File.Exists(src1)) {
            WriteWarning($"Compare-Image - File {FilePath} not found. Please check the path.");
            return;
        }
        if (!File.Exists(src2)) {
            WriteWarning($"Compare-Image - File {FilePathToCompare} not found. Please check the path.");
            return;
        }

        if (!string.IsNullOrWhiteSpace(OutputPath)) {
            string dest = ImagePlayground.Helpers.ResolvePath(OutputPath);
            ImagePlayground.ImageHelper.Compare(src1, src2, dest);
        } else {
            var result = ImagePlayground.ImageHelper.Compare(src1, src2);
            WriteObject(result);
        }
    }
}

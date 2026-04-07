using ImagePlayground;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Compares two images and optionally saves a difference mask.</summary>
/// <para>When OutputPath is omitted, the cmdlet writes the comparison result to the pipeline instead of creating a file.</para>
/// <example>
///   <summary>Compare two images</summary>
///   <prefix>PS&gt; </prefix>
///   <code>Compare-Image -FilePath img1.png -FilePathToCompare img2.png</code>
/// </example>
[Cmdlet(VerbsData.Compare, "Image")]
public sealed class CompareImageCmdlet : ImageCmdlet {
    /// <summary>First image path.</summary>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Second image path.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public string FilePathToCompare { get; set; } = string.Empty;

    /// <summary>Output path for difference mask.</summary>
    /// <para>When provided, the cmdlet writes a visual difference image instead of only returning the comparison result.</para>
    [Parameter(Position = 2)]
    public string? OutputPath { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var filePath = ResolveExistingFilePath(FilePath, "CompareImageSourceNotFound", FilePath);
        var compare = ResolveExistingFilePath(FilePathToCompare, "CompareImageTargetNotFound", FilePathToCompare);

        var outputPath = OutputPath;
        if (!string.IsNullOrWhiteSpace(outputPath)) {
            var output = Helpers.ResolvePath(outputPath!);
            ImagePlayground.ImageHelper.Compare(filePath, compare, output);
        } else {
            var result = ImagePlayground.ImageHelper.Compare(filePath, compare);
            WriteObject(result);
        }
    }
}

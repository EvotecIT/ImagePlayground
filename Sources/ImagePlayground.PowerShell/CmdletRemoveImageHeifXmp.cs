using ImagePlayground;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Removes the XMP metadata packet from a HEIF or HEIC file.</summary>
/// <para>Requires an existing HEIF XMP item with a single writable file extent.</para>
/// <example>
///   <summary>Clear the HEIC XMP packet</summary>
///   <code>Remove-ImageHeifXmp -FilePath photo.heic</code>
/// </example>
[Cmdlet(VerbsCommon.Remove, "ImageHeifXmp")]
public sealed class RemoveImageHeifXmpCmdlet : PSCmdlet {
    /// <summary>Path to the HEIF or HEIC file.</summary>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Optional output path.</summary>
    /// <para>When not specified the source file is overwritten.</para>
    [Parameter(Position = 1)]
    public string? FilePathOutput { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var filePath = Helpers.ResolvePath(FilePath);
        if (!File.Exists(filePath)) {
            WriteWarning($"Remove-ImageHeifXmp - File {FilePath} not found. Please check the path.");
            return;
        }

        var output = string.IsNullOrWhiteSpace(FilePathOutput)
            ? filePath
            : Helpers.ResolvePath(FilePathOutput!);
        ImagePlayground.Image.RemoveHeifXmp(filePath, output);
    }
}

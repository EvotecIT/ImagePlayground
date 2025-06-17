using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Loads an image from disk.</summary>
/// <example>
///   <summary>Read an image</summary>
///   <code>$img = Get-Image -FilePath sample.png</code>
/// </example>
[Cmdlet(VerbsCommon.Get, "Image")]
public sealed class GetImageCmdlet : PSCmdlet {
    /// <summary>Path to the image file.</summary>
    /// <para>The file must exist.</para>
    [Parameter(Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <inheritdoc />
    protected override void ProcessRecord() {
        string fullPath = ImagePlayground.Helpers.ResolvePath(FilePath);
        if (!File.Exists(fullPath)) {
            WriteWarning($"Get-Image - File {FilePath} not found. Please check the path.");
            return;
        }

        var img = ImagePlayground.Image.Load(fullPath);
        WriteObject(img);
    }
}

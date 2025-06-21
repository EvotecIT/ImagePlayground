using ImagePlayground;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Loads an image from disk.</summary>
/// <para>Returns a SixLabors.ImageSharp.Image object for further processing.</para>
/// <example>
///   <summary>Read an image</summary>
///   <code>$img = Get-Image -FilePath sample.png</code>
/// </example>
/// <example>
///   <summary>Check dimensions</summary>
///   <code>(Get-Image -FilePath sample.png).Width</code>
/// </example>
[Cmdlet(VerbsCommon.Get, "Image")]
public sealed class GetImageCmdlet : PSCmdlet {
    /// <summary>Path to the image file.</summary>
    /// <para>The file must exist.</para>
    [Parameter(Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var filePath = Helpers.ResolvePath(FilePath);
        if (!File.Exists(filePath)) {
            WriteWarning($"Get-Image - File {FilePath} not found. Please check the path.");
            return;
        }

        var img = ImagePlayground.Image.Load(filePath);
        WriteObject(img);
    }
}

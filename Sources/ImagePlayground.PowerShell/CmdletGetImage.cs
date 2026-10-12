using ImagePlayground;
using System.IO;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Loads an image from disk.</summary>
/// <para>Returns an editable <see cref="ImagePlayground.Image"/>. Save it with Save-Image after applying transformations, and dispose it when finished.</para>
/// <example>
///   <summary>Read an image</summary>
///   <code>$img = Get-Image -FilePath sample.png</code>
/// </example>
/// <example>
///   <summary>Check dimensions</summary>
///   <code>(Get-Image -FilePath sample.png).Width</code>
/// </example>
[Cmdlet(VerbsCommon.Get, "Image")]
[OutputType(typeof(ImagePlayground.Image))]
public sealed class GetImageCmdlet : ImageCmdlet {
    /// <summary>Path to the image file.</summary>
    /// <para>The file must exist.</para>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var filePath = ResolveExistingFilePath(FilePath, "GetImageFileNotFound", FilePath);
        var img = ImagePlayground.Image.Load(filePath);
        WriteObject(img);
    }
}

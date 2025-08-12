using ImagePlayground;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Saves an image to disk.</summary>
/// <example>
///   <summary>Overwrite the source file</summary>
///   <code>Save-Image -Image $img</code>
/// </example>
/// <example>
///   <summary>Save as JPEG with quality 80</summary>
///   <code>Save-Image -Image $img -FilePath out.jpg -Quality 80</code>
/// </example>
[Cmdlet(VerbsData.Save, "Image")]
public sealed class SaveImageCmdlet : PSCmdlet {
    /// <summary>Image object to save.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public ImagePlayground.Image Image { get; set; } = null!;

    /// <summary>Optional path for the new file.</summary>
    [Parameter(ValueFromPipeline = true, Position = 1)]
    public string? FilePath { get; set; }

    /// <summary>Quality for JPEG or WEBP images.</summary>
    [Parameter]
    public int? Quality { get; set; }

    /// <summary>Compression level for PNG images.</summary>
    [Parameter]
    public int? CompressionLevel { get; set; }

    /// <summary>Return the image as a stream instead of saving.</summary>
    [Parameter]
    public SwitchParameter AsStream { get; set; }

    /// <summary>Open file after saving.</summary>
    [Parameter]
    public SwitchParameter Open { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        if (!string.IsNullOrWhiteSpace(FilePath)) {
            var output = Helpers.ResolvePath(FilePath);
            Helpers.CreateParentDirectory(output);
            Image.Save(output, Open.IsPresent, Quality, CompressionLevel);
        } else if (AsStream.IsPresent) {
            WriteObject(Image.ToStream(Quality, CompressionLevel));
        } else {
            Image.Save("", Open.IsPresent, Quality, CompressionLevel);
        }
    }
}

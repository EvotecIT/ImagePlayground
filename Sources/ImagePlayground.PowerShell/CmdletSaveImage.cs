using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Saves an image to disk.</summary>
/// <example>
///   <summary>Overwrite the source file</summary>
///   <code>Save-Image -Image $img</code>
/// </example>
[Cmdlet(VerbsData.Save, "Image")]
public sealed class SaveImageCmdlet : PSCmdlet {
    /// <summary>Image object to save.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public ImagePlayground.Image Image { get; set; } = null!;

    /// <summary>Optional path for the new file.</summary>
    [Parameter(Position = 1)]
    public string? FilePath { get; set; }

    [Parameter]
    public SwitchParameter AsStream { get; set; }

    /// <summary>Open file after saving.</summary>
    [Parameter]
    public SwitchParameter Open { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        if (!string.IsNullOrWhiteSpace(FilePath)) {
            string outPath = ImagePlayground.Helpers.ResolvePath(FilePath);
            Image.Save(outPath, Open.IsPresent);
        } else if (AsStream.IsPresent) {
            WriteObject(Image.ToStream());
        } else {
            Image.Save(Open.IsPresent);
        }
    }
}

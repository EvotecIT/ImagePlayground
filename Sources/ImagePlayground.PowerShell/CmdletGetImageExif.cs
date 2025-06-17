using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;

namespace ImagePlayground.PowerShell;

/// <summary>Gets EXIF metadata from an image.</summary>
/// <example>
///   <summary>Return raw EXIF values</summary>
///   <code>Get-ImageExif -FilePath img.jpg</code>
/// </example>
/// <example>
///   <summary>Return translated EXIF as properties</summary>
///   <code>Get-ImageExif -FilePath img.jpg -Translate</code>
/// </example>
[Cmdlet(VerbsCommon.Get, "ImageExif")]
public sealed class GetImageExifCmdlet : PSCmdlet {
    /// <summary>Path to the image file.</summary>
    [Parameter(Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Return dictionary with tag names and values.</summary>
    [Parameter]
    public SwitchParameter Translate { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        string fullPath = ImagePlayground.Helpers.ResolvePath(FilePath);
        if (!File.Exists(fullPath)) {
            WriteWarning($"Get-ImageExif - File not found: {FilePath}");
            return;
        }

        using var img = ImagePlayground.Image.Load(fullPath);
        IReadOnlyList<IExifValue> values = img.GetExifValues();

        if (Translate.IsPresent) {
            var obj = new PSObject();
            foreach (IExifValue v in values) {
                obj.Properties.Add(new PSNoteProperty(v.Tag.ToString(), v.GetValue()));
            }
            WriteObject(obj);
        } else {
            WriteObject(values, true);
        }
    }
}


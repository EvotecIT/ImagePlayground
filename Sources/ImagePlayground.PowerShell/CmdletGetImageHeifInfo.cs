using ImagePlayground;
using OfficeIMO.Drawing;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Gets HEIF container metadata without decoding image pixels.</summary>
/// <para>Returns an OfficeIMO.Drawing.OfficeHeifImageInfo containing brands, primary item information, item types, EXIF presence, and image dimensions when declared by HEIF item properties.</para>
/// <example>
///   <summary>Inspect a HEIC file</summary>
///   <code>Get-ImageHeifInfo -FilePath photo.heic</code>
/// </example>
[Cmdlet(VerbsCommon.Get, "ImageHeifInfo")]
[OutputType(typeof(OfficeHeifImageInfo))]
public sealed class GetImageHeifInfoCmdlet : ImageCmdlet {
    /// <summary>Path to the HEIF or HEIC file.</summary>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var filePath = ResolveExistingFilePath(FilePath, "GetImageHeifInfoFileNotFound", FilePath);
        WriteObject(ImagePlayground.Image.GetHeifInfo(filePath));
    }
}

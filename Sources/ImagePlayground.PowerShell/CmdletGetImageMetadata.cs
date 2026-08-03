using ImagePlayground;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Gets supported metadata profiles and provenance indicators from an image.</summary>
/// <para>The result includes resolution, EXIF, XMP, ICC, IPTC, and lightweight C2PA provenance detection where supported.</para>
/// <para>C2PA presence does not by itself mean that an image was made with AI, and this command does not cryptographically validate C2PA claims.</para>
/// <example>
///   <summary>Inspect all supported image metadata</summary>
///   <code>Get-ImageMetadata -FilePath image.png</code>
/// </example>
/// <example>
///   <summary>Check metadata for AI provenance indicators</summary>
///   <code>$metadata = Get-ImageMetadata -FilePath image.png
/// $metadata.Provenance.HasC2paManifest
/// $metadata.Provenance.HasXmpAiDeclaration</code>
/// </example>
[Cmdlet(VerbsCommon.Get, "ImageMetadata")]
[OutputType(typeof(ImageMetadataInfo))]
public sealed class GetImageMetadataCmdlet : ImageCmdlet {
    /// <summary>Path to the image file.</summary>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <inheritdoc />
    protected override void ProcessRecord() {
        string filePath = ResolveExistingFilePath(FilePath, "GetImageMetadataFileNotFound", FilePath);
        WriteObject(ImageHelper.InspectMetadata(filePath));
    }
}
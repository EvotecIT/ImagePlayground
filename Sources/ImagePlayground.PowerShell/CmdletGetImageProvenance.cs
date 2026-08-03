using ImagePlayground;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Gets embedded C2PA containers and direct XMP generative-AI declarations from an image.</summary>
/// <para>This command does not interpret the active C2PA manifest or cryptographically validate C2PA signatures, trust chains, or asset hashes.</para>
/// <para>A C2PA container records provenance but does not by itself mean that the image was made with AI. Use a conforming C2PA validator to inspect its active claim.</para>
/// <example>
///   <summary>Check whether an image declares AI provenance</summary>
///   <code>Get-ImageProvenance -FilePath image.png</code>
/// </example>
/// <example>
///   <summary>Remove all metadata when direct XMP metadata declares generative AI</summary>
///   <code>$info = Get-ImageProvenance -FilePath image.png
/// if ($info.HasXmpAiDeclaration) {
///     Remove-ImageMetadata -FilePath image.png -OutputPath image-clean.png
/// }</code>
/// </example>
[Cmdlet(VerbsCommon.Get, "ImageProvenance")]
public sealed class GetImageProvenanceCmdlet : ImageCmdlet {
    /// <summary>Path to the image file.</summary>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <inheritdoc />
    protected override void ProcessRecord() {
        string filePath = ResolveExistingFilePath(FilePath, "GetImageProvenanceFileNotFound", FilePath);
        WriteObject(ImageHelper.InspectProvenance(filePath));
    }
}
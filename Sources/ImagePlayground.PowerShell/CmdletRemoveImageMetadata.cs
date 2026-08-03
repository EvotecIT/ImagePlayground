using ImagePlayground;
using System.Management.Automation;

namespace ImagePlayground.PowerShell;

/// <summary>Removes selected metadata from an image.</summary>
/// <para>JPEG and PNG files are rewritten without re-encoding their compressed image data. Choose individual metadata families, or use All for the compatibility behavior that removes every supported family. HEIF and HEIC cleanup is limited to EXIF and XMP.</para>
/// <example>
///   <summary>Save a copy without metadata</summary>
///   <code>Remove-ImageMetadata -FilePath in.jpg -OutputPath out.jpg -All</code>
/// </example>
/// <example>
///   <summary>Remove C2PA Content Credentials but preserve EXIF, XMP, IPTC, and ICC metadata</summary>
///   <code>Remove-ImageMetadata -FilePath in.jpg -OutputPath out.jpg -MetadataType C2pa</code>
/// </example>
[Cmdlet(VerbsCommon.Remove, "ImageMetadata", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.Medium)]
public sealed class RemoveImageMetadataCmdlet : ImageCmdlet {
    /// <summary>Source image file.</summary>
    [Parameter(ValueFromPipeline = true, Mandatory = true, Position = 0)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>Destination image path.</summary>
    [Parameter(Mandatory = true, Position = 1)]
    public string OutputPath { get; set; } = string.Empty;

    /// <summary>Metadata families to remove. When omitted, all supported metadata is removed for backward compatibility.</summary>
    [Parameter]
    public ImageMetadataType[]? MetadataType { get; set; }

    /// <summary>Remove every metadata family supported by the image format.</summary>
    [Parameter]
    public SwitchParameter All { get; set; }

    /// <summary>Return a result describing which metadata was removed and whether re-encoding occurred.</summary>
    [Parameter]
    public SwitchParameter PassThru { get; set; }

    /// <inheritdoc />
    protected override void ProcessRecord() {
        var filePath = ResolveExistingFilePath(FilePath, "RemoveImageMetadataFileNotFound", FilePath);
        var output = Helpers.ResolvePath(OutputPath);
        if (All && MetadataType is { Length: > 0 }) {
            ThrowTerminatingError(new ErrorRecord(
                new ArgumentException("All and MetadataType cannot be used together."),
                "RemoveImageMetadataConflictingSelection",
                ErrorCategory.InvalidArgument,
                FilePath));
        }

        ImageMetadataType selected = All || MetadataType is null || MetadataType.Length == 0
            ? ImageMetadataType.All
            : CombineMetadataTypes(MetadataType);
        if (!ShouldProcess(output, $"Remove {selected} metadata from '{filePath}'")) {
            return;
        }

        var result = ImageHelper.RemoveMetadata(new ImageMetadataRemovalOptions(filePath, output) {
            MetadataTypes = selected
        });
        if (PassThru) {
            WriteObject(result);
        }
    }

    private static ImageMetadataType CombineMetadataTypes(IEnumerable<ImageMetadataType> metadataTypes) {
        ImageMetadataType selected = ImageMetadataType.None;
        foreach (ImageMetadataType metadataType in metadataTypes) {
            selected |= metadataType;
        }

        return selected;
    }
}
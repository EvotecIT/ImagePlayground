namespace ImagePlayground;

/// <summary>
/// Identifies metadata families that can be removed from an image.
/// </summary>
[Flags]
public enum ImageMetadataType {
    /// <summary>Do not remove metadata.</summary>
    None = 0,

    /// <summary>Exchangeable Image File Format metadata.</summary>
    Exif = 1,

    /// <summary>Extensible Metadata Platform metadata.</summary>
    Xmp = 2,

    /// <summary>International Press Telecommunications Council metadata.</summary>
    Iptc = 4,

    /// <summary>International Color Consortium color profile.</summary>
    Icc = 8,

    /// <summary>C2PA Content Credentials manifest store.</summary>
    C2pa = 16,

    /// <summary>All metadata families supported by the image format.</summary>
    All = Exif | Xmp | Iptc | Icc | C2pa
}

/// <summary>
/// Configures selective image metadata removal.
/// </summary>
public sealed class ImageMetadataRemovalOptions {
    /// <summary>
    /// Creates metadata removal options.
    /// </summary>
    /// <param name="filePath">Source image path.</param>
    /// <param name="outputPath">Destination image path.</param>
    public ImageMetadataRemovalOptions(string filePath, string outputPath) {
        FilePath = filePath;
        OutputPath = outputPath;
    }

    /// <summary>Source image path.</summary>
    public string FilePath { get; }

    /// <summary>Destination image path.</summary>
    public string OutputPath { get; }

    /// <summary>Metadata families to remove.</summary>
    public ImageMetadataType MetadataTypes { get; set; } = ImageMetadataType.All;
}

/// <summary>
/// Reports the outcome of selective image metadata removal.
/// </summary>
public sealed class ImageMetadataRemovalResult {
    internal ImageMetadataRemovalResult(
        string filePath,
        string outputPath,
        ImageMetadataType requestedMetadataTypes,
        ImageMetadataType removedMetadataTypes,
        bool wasReencoded,
        long originalLength,
        long outputLength) {
        FilePath = filePath;
        OutputPath = outputPath;
        RequestedMetadataTypes = requestedMetadataTypes;
        RemovedMetadataTypes = removedMetadataTypes;
        WasReencoded = wasReencoded;
        OriginalLength = originalLength;
        OutputLength = outputLength;
    }

    /// <summary>Resolved source image path.</summary>
    public string FilePath { get; }

    /// <summary>Resolved destination image path.</summary>
    public string OutputPath { get; }

    /// <summary>Metadata families requested for removal.</summary>
    public ImageMetadataType RequestedMetadataTypes { get; }

    /// <summary>Metadata families that were present and removed.</summary>
    public ImageMetadataType RemovedMetadataTypes { get; }

    /// <summary>Whether the image was decoded and encoded again.</summary>
    public bool WasReencoded { get; }

    /// <summary>Source file size in bytes.</summary>
    public long OriginalLength { get; }

    /// <summary>Destination file size in bytes.</summary>
    public long OutputLength { get; }

    /// <summary>Whether at least one requested metadata family was removed.</summary>
    public bool Changed => RemovedMetadataTypes != ImageMetadataType.None;
}
using System.IO;

namespace ImagePlayground;

/// <summary>
/// Provides HEIF container metadata operations.
/// </summary>
public partial class Image {
    private const string HeifXmpReadNotSupportedMessage = "The HEIF/HEIC file declares an XMP item, but the XMP payload could not be read.";
    private const string HeifXmpWriteNotSupportedMessage = "Updating HEIF/HEIC XMP requires an existing XMP item with a single writable file extent. Creating a brand-new HEIF XMP item is not supported yet.";

    /// <summary>
    /// Gets HEIF container metadata without decoding image pixels.
    /// </summary>
    /// <param name="filePath">Path to a HEIF or HEIC file.</param>
    /// <returns>Basic HEIF container and item metadata.</returns>
    public static HeifImageInfo GetHeifInfo(string filePath) {
        string fullPath = Helpers.ResolvePath(filePath);
        if (!Helpers.IsHeifExtension(fullPath)) {
            throw new NotSupportedException("HEIF metadata can only be read from .heic or .heif files.");
        }

        if (!File.Exists(fullPath)) {
            throw new FileNotFoundException("HEIF file was not found.", fullPath);
        }

        if (!HeifMetadataReader.TryReadInfo(fullPath, out HeifImageInfo? info) || info is null) {
            throw new InvalidDataException("The HEIF container metadata could not be read.");
        }

        return info;
    }

    /// <summary>
    /// Gets the XMP metadata packet from a HEIF or HEIC file when the container declares one.
    /// </summary>
    /// <param name="filePath">Path to a HEIF or HEIC file.</param>
    /// <returns>The UTF-8 XMP metadata packet, or <c>null</c> when no XMP item is present.</returns>
    public static string? GetHeifXmp(string filePath) {
        string fullPath = Helpers.ResolvePath(filePath);
        if (!Helpers.IsHeifExtension(fullPath)) {
            throw new NotSupportedException("HEIF XMP metadata can only be read from .heic or .heif files.");
        }

        if (!File.Exists(fullPath)) {
            throw new FileNotFoundException("HEIF file was not found.", fullPath);
        }

        if (HeifMetadataReader.TryReadXmp(fullPath, out string? xmp)) {
            return xmp;
        }

        if (HeifMetadataReader.HasXmpItem(fullPath)) {
            throw new NotSupportedException(HeifXmpReadNotSupportedMessage);
        }

        return null;
    }

    /// <summary>
    /// Sets the XMP metadata packet on a HEIF or HEIC file when an existing writable XMP item is present.
    /// </summary>
    /// <param name="filePath">Path to a HEIF or HEIC file.</param>
    /// <param name="filePathOutput">Optional output path. When omitted, the input file is overwritten.</param>
    /// <param name="xmp">The UTF-8 XMP metadata packet to write.</param>
    public static void SetHeifXmp(string filePath, string? filePathOutput, string xmp) {
        if (xmp is null) {
            throw new ArgumentNullException(nameof(xmp));
        }

        WriteHeifXmp(filePath, filePathOutput, xmp);
    }

    /// <summary>
    /// Clears the XMP metadata packet from a HEIF or HEIC file when an existing writable XMP item is present.
    /// </summary>
    /// <param name="filePath">Path to a HEIF or HEIC file.</param>
    /// <param name="filePathOutput">Optional output path. When omitted, the input file is overwritten.</param>
    public static void RemoveHeifXmp(string filePath, string? filePathOutput) =>
        WriteHeifXmp(filePath, filePathOutput, null);

    private static void WriteHeifXmp(string filePath, string? filePathOutput, string? xmp) {
        string fullPath = Helpers.ResolvePath(filePath);
        if (!Helpers.IsHeifExtension(fullPath)) {
            throw new NotSupportedException("HEIF XMP metadata can only be updated for .heic or .heif files.");
        }

        if (!File.Exists(fullPath)) {
            throw new FileNotFoundException("HEIF file was not found.", fullPath);
        }

        string outputPath = string.IsNullOrWhiteSpace(filePathOutput)
            ? fullPath
            : Helpers.ResolvePath(filePathOutput!);

        if (!HeifMetadataReader.TryWriteXmp(fullPath, outputPath, xmp)) {
            throw new NotSupportedException(HeifXmpWriteNotSupportedMessage);
        }
    }
}

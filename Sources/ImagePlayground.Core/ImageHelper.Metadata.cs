using System.IO;
using System.Text.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SixLabors.ImageSharp.Metadata.Profiles.Icc;
using SixLabors.ImageSharp.Metadata.Profiles.Iptc;
using SixLabors.ImageSharp.Metadata.Profiles.Xmp;

namespace ImagePlayground;
/// <summary>
/// Provides helper methods for image manipulation.
/// </summary>
public partial class ImageHelper {
    private const string HeifExifWriteNotSupportedMessage = "Updating HEIF/HEIC EXIF requires an existing EXIF item with a single writable file extent. Creating a brand-new HEIF EXIF item is not supported yet.";
    private const string HeifXmpWriteNotSupportedMessage = "Updating HEIF/HEIC XMP requires an existing XMP item with a single writable file extent. Creating a brand-new HEIF XMP item is not supported yet.";

    /// <summary>
    /// Serialization model for image metadata.
    /// </summary>
    private sealed class SerializedImageMetadata {
        /// <summary>Horizontal resolution.</summary>
        public double HorizontalResolution { get; set; }

        /// <summary>Vertical resolution.</summary>
        public double VerticalResolution { get; set; }

        /// <summary>Resolution measurement units.</summary>
        public PixelResolutionUnit ResolutionUnits { get; set; }

        /// <summary>Serialized Exif profile.</summary>
        public byte[]? ExifProfile { get; set; }

        /// <summary>Serialized XMP profile.</summary>
        public byte[]? XmpProfile { get; set; }

        /// <summary>Serialized ICC profile.</summary>
        public byte[]? IccProfile { get; set; }

        /// <summary>Serialized IPTC profile.</summary>
        public byte[]? IptcProfile { get; set; }
    }

    /// <summary>Options for importing metadata.</summary>
    public sealed class ImportMetadataOptions {
        /// <summary>Source image path.</summary>
        public string FilePath { get; }

        /// <summary>JSON metadata file.</summary>
        public string MetadataPath { get; }

        /// <summary>Destination image path.</summary>
        public string? OutputPath { get; }

        /// <summary>Create import options.</summary>
        /// <param name="filePath">Path to the source image.</param>
        /// <param name="metadataPath">Path to the metadata file.</param>
        /// <param name="outputPath">Destination image path.</param>
        public ImportMetadataOptions(string filePath, string metadataPath, string? outputPath = null) {
            FilePath = filePath;
            MetadataPath = metadataPath;
            OutputPath = outputPath;
        }
    }
    /// <summary>
    /// Exports metadata from an image to a JSON string.
    /// </summary>
    /// <param name="filePath">Path to the source image.</param>
    /// <returns>Serialized metadata.</returns>
    public static string ExportMetadata(string filePath) {
        string fullPath = Helpers.ResolvePath(filePath);
        if (Helpers.IsHeifExtension(fullPath)) {
            var heifData = new SerializedImageMetadata {
                ExifProfile = HeifMetadataReader.TryReadExifProfile(fullPath, out ExifProfile? heifProfile)
                    ? heifProfile?.ToByteArray()
                    : null,
                XmpProfile = HeifMetadataReader.TryReadXmp(fullPath, out string? heifXmp) && heifXmp is not null
                    ? Encoding.UTF8.GetBytes(heifXmp)
                    : null
            };
            return JsonSerializer.Serialize(heifData, new JsonSerializerOptions { WriteIndented = true });
        }

        using var img = Image.Load(fullPath);
        var meta = img.Metadata;
        var data = new SerializedImageMetadata {
            HorizontalResolution = meta.HorizontalResolution,
            VerticalResolution = meta.VerticalResolution,
            ResolutionUnits = meta.ResolutionUnits,
            ExifProfile = meta.ExifProfile?.ToByteArray(),
            XmpProfile = meta.XmpProfile?.ToByteArray(),
            IccProfile = meta.IccProfile?.ToByteArray(),
            IptcProfile = meta.IptcProfile?.Data
        };
        return JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
    }

    /// <summary>
    /// Exports metadata from an image and writes it to a file.
    /// </summary>
    /// <param name="filePath">Path to the source image.</param>
    /// <param name="outFilePath">Destination file for metadata.</param>
    public static void ExportMetadata(string filePath, string outFilePath) {
        string json = ExportMetadata(filePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);
        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outFullPath)!);
        File.WriteAllText(outFullPath, json);
    }

    /// <summary>
    /// Imports metadata using the specified <see cref="ImportMetadataOptions"/>.
    /// </summary>
    /// <param name="options">Import options.</param>
    public static void ImportMetadata(ImportMetadataOptions options) {
        if (options is null) {
            throw new ArgumentNullException(nameof(options));
        }

        string output = string.IsNullOrWhiteSpace(options.OutputPath)
            ? options.FilePath
            : options.OutputPath!;

        ImportMetadata(options.FilePath, options.MetadataPath, output);
    }

    /// <summary>
    /// Imports metadata from a JSON file and saves the updated image.
    /// </summary>
    /// <param name="filePath">Source image path.</param>
    /// <param name="metadataFilePath">Path to JSON metadata file.</param>
    /// <param name="outFilePath">Destination image path.</param>
    public static void ImportMetadata(string filePath, string metadataFilePath, string outFilePath) {
        string fullPath = Helpers.ResolvePath(filePath);
        string metaFullPath = Helpers.ResolvePath(metadataFilePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);
        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outFullPath)!);

        string json = File.ReadAllText(metaFullPath);
        SerializedImageMetadata? data;

        try {
            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;
            if (root.ValueKind != JsonValueKind.Object ||
                !root.TryGetProperty(nameof(SerializedImageMetadata.HorizontalResolution), out JsonElement hRes) || hRes.ValueKind != JsonValueKind.Number ||
                !root.TryGetProperty(nameof(SerializedImageMetadata.VerticalResolution), out JsonElement vRes) || vRes.ValueKind != JsonValueKind.Number ||
                !root.TryGetProperty(nameof(SerializedImageMetadata.ResolutionUnits), out JsonElement unit)) {
                throw new InvalidDataException("Metadata file is invalid");
            }

            if (unit.ValueKind == JsonValueKind.Number) {
                int intVal = unit.GetInt32();
                object val = Enum.ToObject(typeof(PixelResolutionUnit), intVal);
                if (!Enum.IsDefined(typeof(PixelResolutionUnit), val)) {
                    throw new InvalidDataException("Metadata file is invalid");
                }
            } else if (unit.ValueKind == JsonValueKind.String) {
                string? text = unit.GetString();
                if (!Enum.TryParse(text, true, out PixelResolutionUnit _)) {
                    throw new InvalidDataException("Metadata file is invalid");
                }
            } else {
                throw new InvalidDataException("Metadata file is invalid");
            }
        } catch (JsonException ex) {
            throw new InvalidDataException("Metadata file is invalid", ex);
        }

        data = JsonSerializer.Deserialize<SerializedImageMetadata>(json);
        if (data is null) {
            throw new InvalidDataException("Metadata file is invalid");
        }

        if (Helpers.IsHeifExtension(fullPath)) {
            ImportHeifMetadata(fullPath, outFullPath, data);
            return;
        }

        using var img = Image.Load(fullPath);
        img.Metadata.HorizontalResolution = data.HorizontalResolution;
        img.Metadata.VerticalResolution = data.VerticalResolution;
        img.Metadata.ResolutionUnits = data.ResolutionUnits;
        img.Metadata.ExifProfile = data.ExifProfile != null ? new ExifProfile(data.ExifProfile) : null;
        img.Metadata.XmpProfile = data.XmpProfile != null ? new XmpProfile(data.XmpProfile) : null;
        img.Metadata.IccProfile = data.IccProfile != null ? new IccProfile(data.IccProfile) : null;
        img.Metadata.IptcProfile = data.IptcProfile != null ? new IptcProfile(data.IptcProfile) : null;
        img.Save(outFullPath);
    }

    /// <summary>
    /// Removes all metadata profiles from an image and saves it to the specified path.
    /// </summary>
    /// <param name="filePath">Source image path.</param>
    /// <param name="outFilePath">Destination image path.</param>
    public static void RemoveMetadata(string filePath, string outFilePath) {
        string fullPath = Helpers.ResolvePath(filePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);
        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outFullPath)!);

        if (Helpers.IsHeifExtension(fullPath)) {
            RemoveHeifMetadata(fullPath, outFullPath);
            return;
        }

        using var img = Image.Load(fullPath);
        var meta = img.Metadata;
        meta.ExifProfile = null;
        meta.XmpProfile = null;
        meta.IccProfile = null;
        meta.IptcProfile = null;
        img.Save(outFullPath);
    }

    private static void ImportHeifMetadata(string fullPath, string outFullPath, SerializedImageMetadata data) {
        string currentPath = fullPath;
        bool wroteMetadata = false;
        var temporaryFiles = new List<string>();

        try {
            if (data.ExifProfile is not null || HeifMetadataReader.HasExifItem(currentPath)) {
                ExifProfile? heifProfile = data.ExifProfile != null
                    ? new ExifProfile(data.ExifProfile)
                    : null;
                string temporaryPath = GetTemporaryHeifPath(outFullPath);
                temporaryFiles.Add(temporaryPath);
                if (!HeifMetadataReader.TryWriteExifProfile(currentPath, temporaryPath, heifProfile)) {
                    throw new NotSupportedException(HeifExifWriteNotSupportedMessage);
                }

                currentPath = temporaryPath;
                wroteMetadata = true;
            }

            if (data.XmpProfile is not null || HeifMetadataReader.HasXmpItem(currentPath)) {
                string? xmp = data.XmpProfile != null
                    ? Encoding.UTF8.GetString(data.XmpProfile)
                    : null;
                string temporaryPath = GetTemporaryHeifPath(outFullPath);
                temporaryFiles.Add(temporaryPath);
                if (!HeifMetadataReader.TryWriteXmp(currentPath, temporaryPath, xmp)) {
                    throw new NotSupportedException(HeifXmpWriteNotSupportedMessage);
                }

                currentPath = temporaryPath;
                wroteMetadata = true;
            }

            if (wroteMetadata) {
                File.Copy(currentPath, outFullPath, true);
            } else {
                CopyIfDifferent(fullPath, outFullPath);
            }
        } finally {
            DeleteTemporaryFiles(temporaryFiles);
        }
    }

    private static void RemoveHeifMetadata(string fullPath, string outFullPath) {
        string currentPath = fullPath;
        bool wroteMetadata = false;
        var temporaryFiles = new List<string>();

        try {
            if (HeifMetadataReader.HasExifItem(currentPath)) {
                string temporaryPath = GetTemporaryHeifPath(outFullPath);
                temporaryFiles.Add(temporaryPath);
                if (HeifMetadataReader.TryWriteExifProfile(currentPath, temporaryPath, null)) {
                    currentPath = temporaryPath;
                    wroteMetadata = true;
                }
            }

            if (HeifMetadataReader.HasXmpItem(currentPath)) {
                string temporaryPath = GetTemporaryHeifPath(outFullPath);
                temporaryFiles.Add(temporaryPath);
                if (HeifMetadataReader.TryWriteXmp(currentPath, temporaryPath, null)) {
                    currentPath = temporaryPath;
                    wroteMetadata = true;
                }
            }

            if (wroteMetadata) {
                File.Copy(currentPath, outFullPath, true);
            } else {
                CopyIfDifferent(fullPath, outFullPath);
            }
        } finally {
            DeleteTemporaryFiles(temporaryFiles);
        }
    }

    private static string GetTemporaryHeifPath(string outFullPath) =>
        Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}{Path.GetExtension(outFullPath)}");

    private static void DeleteTemporaryFiles(List<string> temporaryFiles) {
        foreach (string temporaryFile in temporaryFiles) {
            if (File.Exists(temporaryFile)) {
                File.Delete(temporaryFile);
            }
        }
    }

    private static void CopyIfDifferent(string fullPath, string outFullPath) {
        if (fullPath.Equals(outFullPath, StringComparison.OrdinalIgnoreCase)) {
            return;
        }

        File.Copy(fullPath, outFullPath, true);
    }
}

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
    /// <summary>
    /// Exports metadata from an image to a JSON string.
    /// </summary>
    /// <param name="filePath">Path to the source image.</param>
    /// <returns>Serialized metadata.</returns>
    public static string ExportMetadata(string filePath) {
        string fullPath = Helpers.ResolvePath(filePath);
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
        File.WriteAllText(outFullPath, json);
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

        string json = File.ReadAllText(metaFullPath);
        SerializedImageMetadata? data = JsonSerializer.Deserialize<SerializedImageMetadata>(json);
        if (data is null) {
            throw new InvalidDataException("Metadata file is invalid");
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
}

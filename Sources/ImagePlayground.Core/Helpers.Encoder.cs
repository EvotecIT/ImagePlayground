using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Pbm;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Tga;
using SixLabors.ImageSharp.Formats.Tiff;
using SixLabors.ImageSharp.Formats.Webp;

namespace ImagePlayground;

/// <summary>
/// Image encoding helper methods.
/// </summary>
public static partial class Helpers {
    /// <summary>
    /// Returns an image encoder instance appropriate for the given file extension.
    /// </summary>
    /// <param name="extension">File extension including the leading dot.</param>
    /// <param name="quality">Optional quality value for lossy formats.</param>
    /// <param name="compressionLevel">Optional compression level for PNG/WebP.</param>
    /// <returns>Configured image encoder.</returns>
    public static IImageEncoder GetEncoder(string extension, int? quality, int? compressionLevel) {
        extension = extension.ToLowerInvariant();
        return extension switch {
            ".png" => new PngEncoder {
                CompressionLevel = compressionLevel.HasValue
                    ? (PngCompressionLevel)Math.Max(0, Math.Min(9, compressionLevel.Value))
                    : PngCompressionLevel.DefaultCompression
            },
            ".jpg" => new JpegEncoder {
                Quality = quality.HasValue
                    ? Math.Max(0, Math.Min(100, quality.Value))
                    : 75
            },
            ".jpeg" => new JpegEncoder {
                Quality = quality.HasValue
                    ? Math.Max(0, Math.Min(100, quality.Value))
                    : 75
            },
            ".bmp" => new BmpEncoder(),
            ".gif" => new GifEncoder(),
            ".pbm" => new PbmEncoder(),
            ".tga" => new TgaEncoder(),
            ".tiff" => new TiffEncoder(),
            ".webp" => new WebpEncoder {
                Quality = quality.HasValue
                    ? Math.Max(0, Math.Min(100, quality.Value))
                    : 75
            },
            _ => throw new UnknownImageFormatException(
                $"Image format not supported. Supported extensions: {string.Join(", ", SupportedExtensions)}"),
        };
    }
}

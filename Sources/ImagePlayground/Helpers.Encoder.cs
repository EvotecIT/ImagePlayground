namespace ImagePlayground;

/// <summary>Shared image encoder settings used by file and stream entry points.</summary>
public static partial class Helpers {
    /// <summary>Resolves a supported file extension to its image format.</summary>
    public static ImageType GetImageType(string extension) => extension.ToLowerInvariant() switch {
        ".png" => ImageType.Png, ".jpg" or ".jpeg" => ImageType.Jpeg, ".bmp" => ImageType.Bmp,
        ".gif" => ImageType.Gif, ".pbm" => ImageType.Pbm, ".tga" => ImageType.Tga,
        ".tif" or ".tiff" => ImageType.Tiff, ".webp" => ImageType.WebP, ".ico" => ImageType.Icon,
        _ => throw new NotSupportedException("The image extension is not supported.")
    };

    /// <summary>Resolves the shared encoder format for a still image.</summary>
    public static OfficeImageExportFormat GetEncoder(ImageType type) => type switch {
        ImageType.Png => OfficeImageExportFormat.Png, ImageType.Jpeg => OfficeImageExportFormat.Jpeg,
        ImageType.Bmp => OfficeImageExportFormat.Bmp, ImageType.Pbm => OfficeImageExportFormat.Pbm,
        ImageType.Tga => OfficeImageExportFormat.Tga, ImageType.Tiff => OfficeImageExportFormat.Tiff,
        ImageType.WebP => OfficeImageExportFormat.Webp, ImageType.Icon => OfficeImageExportFormat.Icon,
        ImageType.Gif => throw new ArgumentException("GIF frames are encoded by the shared animation encoder.", nameof(type)),
        _ => throw new ArgumentOutOfRangeException(nameof(type))
    };

    /// <summary>Resolves the shared encoder format from a file extension.</summary>
    public static OfficeImageExportFormat GetEncoder(string extension) => GetEncoder(GetImageType(extension));

    internal static OfficeImageFormat GetContainerFormat(ImageType type) =>
        type == ImageType.Gif ? OfficeImageFormat.Gif : GetEncoder(type).GetContainerFormat();

    /// <summary>Creates format-specific encoding options, clamping quality and compression controls to their supported ranges.</summary>
    public static OfficeRasterEncodingOptions GetEncodingOptions(ImageType type, int? quality, int? compressionLevel) {
        if (quality.HasValue) quality = Math.Max(1, Math.Min(100, quality.Value));
        if (compressionLevel.HasValue) compressionLevel = Math.Max(0, Math.Min(9, compressionLevel.Value));
        var options = new OfficeRasterEncodingOptions();
        if (quality.HasValue) options.Jpeg.Quality = quality.Value;
        if (type == ImageType.WebP && quality.HasValue) {
            options.Webp.Mode = OfficeWebpEncodingMode.Lossy;
            options.Webp.Quality = quality.Value;
        }
        if (compressionLevel.HasValue) options.Png.Compression = compressionLevel.Value == 0 ? OfficePngCompression.Stored : compressionLevel.Value <= 3 ? OfficePngCompression.Fastest : OfficePngCompression.Optimal;
        return options;
    }
}
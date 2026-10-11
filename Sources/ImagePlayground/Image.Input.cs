namespace ImagePlayground;

/// <summary>Bounded decoding and source-format evidence from the shared raster owner.</summary>
public partial class Image {
    /// <summary>Loads all image frames and editable primary-image metadata from a file.</summary>
    /// <remarks>Filename extensions do not override the detected source format. Default loading retains stored orientation for AutoOrient.</remarks>
    public static Image Load(string filePath, OfficeRasterDecodeOptions? options = null) {
        string fullPath = Helpers.ResolvePath(filePath);
        using var stream = File.OpenRead(fullPath);
        var image = Load(stream, options);
        image._filePath = fullPath;
        return image;
    }

    /// <summary>Loads all frames and primary-image metadata from encoded bytes.</summary>
    /// <remarks>Default loading retains stored orientation for AutoOrient. The detected supported source format becomes the default stream-output format.</remarks>
    public static Image Load(byte[] bytes, OfficeRasterDecodeOptions? options = null) {
        if (bytes == null) { throw new ArgumentNullException(nameof(bytes)); }
        var effective = options?.Clone() ?? new OfficeRasterDecodeOptions { ApplyExifOrientation = false };
        OfficeRasterFrames frames = OfficeRasterImageDecoder.DecodeFrames(bytes, effective, out var info);
        OfficeImageMetadata metadata = OfficeImageMetadata.Read(bytes, effective.CancellationToken);
        if (info.OrientationNormalized) { metadata.SetExifValue(OfficeExifTag.Orientation, (ushort)1); }
        return new Image { _frames = frames, _metadata = metadata, _sourceFormat = info.Format, _sourceIsAnimation = info.Container?.IsAnimated == true, _imageType = GetDefaultImageType(info.Format) };
    }

    /// <summary>Loads encoded bytes with an explicit default output format.</summary>
    /// <remarks>This selection changes default stream export, while SourceFormat continues to describe the decoded bytes.</remarks>
    public static Image Load(byte[] bytes, ImageType imageType) {
        if (!Enum.IsDefined(typeof(ImageType), imageType)) { throw new ArgumentOutOfRangeException(nameof(imageType)); }
        var image = Load(bytes);
        image._imageType = imageType;
        return image;
    }

    /// <summary>Reads a bounded image from the stream's current position without closing it.</summary>
    public static Image Load(Stream stream, OfficeRasterDecodeOptions? options = null) {
        if (stream == null) { throw new ArgumentNullException(nameof(stream)); }
        var effective = options?.Clone() ?? new OfficeRasterDecodeOptions { ApplyExifOrientation = false };
        byte[] bytes = OfficeRasterImageDecoder.ReadEncodedBytes(stream, effective);
        return Load(bytes, effective);
    }

    /// <summary>Loads a file asynchronously while observing cancellation during reading and decoding.</summary>
    public static Task<Image> LoadAsync(string filePath, CancellationToken cancellationToken = default) =>
        LoadAsync(filePath, new OfficeRasterDecodeOptions { ApplyExifOrientation = false, CancellationToken = cancellationToken });

    /// <summary>Loads a file asynchronously with an independent snapshot of owned decoder options.</summary>
    public static Task<Image> LoadAsync(string filePath, OfficeRasterDecodeOptions options) {
        if (options == null) { throw new ArgumentNullException(nameof(options)); }
        string fullPath = Helpers.ResolvePath(filePath);
        var effective = options.Clone();
        return Task.Run(() => Load(fullPath, effective), effective.CancellationToken);
    }

    private static ImageType GetDefaultImageType(OfficeImageFormat format) => format switch {
        OfficeImageFormat.Jpeg => ImageType.Jpeg,
        OfficeImageFormat.Gif => ImageType.Gif,
        OfficeImageFormat.Bmp => ImageType.Bmp,
        OfficeImageFormat.Tiff => ImageType.Tiff,
        OfficeImageFormat.Icon => ImageType.Icon,
        OfficeImageFormat.Webp => ImageType.WebP,
        OfficeImageFormat.Tga => ImageType.Tga,
        OfficeImageFormat.PortableMap => ImageType.Pbm,
        _ => ImageType.Png
    };
}

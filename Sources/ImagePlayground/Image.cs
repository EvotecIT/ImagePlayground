using ChartForgeX.Raster;
using ChartForgeX.Primitives;

namespace ImagePlayground;

/// <summary>A managed image with editable raster frames and metadata.</summary>
/// <remarks>Dispose the wrapper after use. Raster buffers belong to this wrapper; clone them when retaining an independent image.</remarks>
public partial class Image : IDisposable {
    private OfficeRasterFrames _frames = new OfficeRasterFrames(new[] { new OfficeRasterFrame(new OfficeRasterImage(1, 1)) });
    private OfficeImageMetadata _metadata = new OfficeImageMetadata();
    private string _filePath = string.Empty;
    private ImageType _imageType = ImageType.Png;
    private bool _disposed;
    private OfficeRasterImage _image => Raster;

    /// <summary>Width of the first frame in pixels.</summary>
    public int Width => Raster.Width;
    /// <summary>Height of the first frame in pixels.</summary>
    public int Height => Raster.Height;
    /// <summary>Resolved path associated with a loaded or created image, or an empty string for pixel and byte inputs.</summary>
    public string FilePath { get { EnsureUsable(); return _filePath; } }
    /// <summary>Editable metadata whose profiles are preserved when the output format supports them.</summary>
    public OfficeImageMetadata Metadata { get { EnsureUsable(); return _metadata; } }
    /// <summary>Decoded frames, TIFF pages, or icon resolutions, including animation timing and play count.</summary>
    public OfficeRasterFrames Frames { get { EnsureUsable(); return _frames; } }
    /// <summary>Managed RGBA pixels of the first frame.</summary>
    public OfficeRasterImage Raster { get { EnsureUsable(); return _frames[0].Image; } }

    /// <summary>Reports metadata profile families the selected output format cannot preserve.</summary>
    /// <remarks>The current metadata remains unchanged. Save retains the supported families; lossless metadata editing remains strict about unsupported profiles. TIFF-relative opaque fields must be explicitly removed or replaced before raster re-encoding.</remarks>
    public OfficeImageMetadataProfileKinds GetEncodingMetadataOmissions(ImageType type) {
        EnsureUsable();
        _metadata.PrepareForEncoding(Helpers.GetContainerFormat(type), out var omittedProfiles);
        return omittedProfiles;
    }

    /// <summary>Creates an image wrapper around an existing raster buffer without copying its pixels.</summary>
    public static Image FromRaster(OfficeRasterImage raster, ImageType imageType = ImageType.Png) {
        if (raster == null) {
            throw new ArgumentNullException(nameof(raster));
        }
        return FromFrames(new OfficeRasterFrames(new[] { new OfficeRasterFrame(raster) }), imageType);
    }

    /// <summary>Creates a wrapper around an explicit collection of independent frames or pages.</summary>
    public static Image FromFrames(OfficeRasterFrames frames, ImageType imageType = ImageType.Png) {
        if (frames == null) {
            throw new ArgumentNullException(nameof(frames));
        }
        return new Image { _frames = frames, _imageType = imageType };
    }

    /// <summary>Creates an independent copy of pixels, frame timing, and metadata.</summary>
    public Image Clone() {
        EnsureUsable();
        var clone = FromFrames(_frames.Transform(frame => frame.Clone()), _imageType);
        clone._filePath = _filePath;
        clone._metadata = _metadata.Clone();
        return clone;
    }

    /// <summary>Loads a managed image wrapper from a file.</summary>
    public static Image Load(string filePath) {
        string fullPath = Helpers.ResolvePath(filePath);
        var image = Load(Helpers.ReadEncodedFile(fullPath), GetLoadedImageType(fullPath));
        image._filePath = fullPath;
        return image;
    }

    /// <summary>Decodes all frames from encoded image bytes and captures their editable metadata.</summary>
    public static Image Load(byte[] bytes, ImageType imageType = ImageType.Png) {
        if (bytes == null) {
            throw new ArgumentNullException(nameof(bytes));
        }
        if (!OfficeRasterImageDecoder.TryDecodeFrames(bytes, new OfficeRasterDecodeOptions { ApplyExifOrientation = false }, out var frames) || frames == null) {
            throw new InvalidDataException("The image format or encoded content cannot be decoded by the managed raster engine.");
        }
        return new Image { _frames = frames, _imageType = imageType, _metadata = OfficeImageMetadata.Read(bytes) };
    }

    /// <summary>Loads an image asynchronously while observing cancellation during decoding.</summary>
    public static Task<Image> LoadAsync(string filePath, CancellationToken cancellationToken = default) {
        string fullPath = Helpers.ResolvePath(filePath);
        return Task.Run(() => {
            byte[] bytes = Helpers.ReadEncodedFile(fullPath, cancellationToken);
            var options = new OfficeRasterDecodeOptions { CancellationToken = cancellationToken, ApplyExifOrientation = false };
            if (!OfficeRasterImageDecoder.TryDecodeFrames(bytes, options, out var frames) || frames == null) {
                throw new InvalidDataException("The image format or encoded content cannot be decoded by the managed raster engine.");
            }
            return new Image { _frames = frames, _imageType = GetLoadedImageType(fullPath), _filePath = fullPath, _metadata = OfficeImageMetadata.Read(bytes, cancellationToken) };
        }, cancellationToken);
    }

    /// <summary>Loads an image for further managed processing.</summary>
    public static Image GetImage(string filePath) => Load(filePath);

    /// <summary>Initializes a transparent image and associates its output path.</summary>
    public void Create(string filePath, int width, int height) {
        EnsureUsable();
        _filePath = Helpers.ResolvePath(filePath);
        _imageType = Helpers.GetImageType(Path.GetExtension(_filePath));
        _frames = new OfficeRasterFrames(new[] { new OfficeRasterFrame(new OfficeRasterImage(width, height)) });
        _metadata = new OfficeImageMetadata();
    }

    /// <summary>Compares the first frames, returning pixel metrics and a difference image.</summary>
    public OfficeRasterComparisonResult Compare(Image imageToCompare) {
        if (imageToCompare == null) {
            throw new ArgumentNullException(nameof(imageToCompare));
        }
        return OfficeRasterComparison.Compare(Raster, imageToCompare.Raster);
    }

    /// <summary>Compares the first frame with an image file.</summary>
    public OfficeRasterComparisonResult Compare(string filePathToCompare) {
        using var other = Load(filePathToCompare);
        return Compare(other);
    }

    /// <summary>Saves a PNG difference mask between two first frames.</summary>
    public void Compare(Image imageToCompare, string filePathToSave) {
        using var mask = FromRaster(Compare(imageToCompare).DifferenceImage);
        string output = Helpers.ResolvePath(filePathToSave);
        Helpers.CreateParentDirectory(output);
        OfficeImageFileWriter.WriteAllBytes(output, mask.Encode(ImageType.Png, null, null));
    }

    /// <summary>Saves a PNG difference mask against another image file.</summary>
    public void Compare(string filePathToCompare, string filePathToSave) {
        using var other = Load(filePathToCompare);
        Compare(other, filePathToSave);
    }

    /// <summary>Encodes the image to a file, preserving animation when writing GIF or PNG.</summary>
    /// <remarks>Metadata profile families supported by the destination are retained. Call GetEncodingMetadataOmissions to inspect families the destination cannot carry before saving. Completed bytes are staged beside the destination and atomically replace it.</remarks>
    public void Save(string filePath = "", bool openImage = false, int? quality = null, int? compressionLevel = null) {
        string fullPath = ResolveOutputPath(filePath);
        byte[] bytes = Encode(Helpers.GetImageType(Path.GetExtension(fullPath)), quality, compressionLevel);
        Helpers.CreateParentDirectory(fullPath);
        OfficeImageFileWriter.WriteAllBytes(fullPath, bytes);
        Helpers.Open(fullPath, openImage);
    }

    /// <summary>Encodes and saves the image while observing cancellation.</summary>
    /// <remarks>Metadata preservation follows the destination format. Call GetEncodingMetadataOmissions before saving when profile omissions must be reviewed. Cancellation and staging failures preserve the destination until the atomic commit.</remarks>
    public async Task SaveAsync(string filePath = "", bool openImage = false, int? quality = null, int? compressionLevel = null, CancellationToken cancellationToken = default) {
        string fullPath = ResolveOutputPath(filePath);
        byte[] bytes = await Task.Run(() => Encode(Helpers.GetImageType(Path.GetExtension(fullPath)), quality, compressionLevel, cancellationToken), cancellationToken).ConfigureAwait(false);
        cancellationToken.ThrowIfCancellationRequested();
        Helpers.CreateParentDirectory(fullPath);
        await OfficeImageFileWriter.WriteAllBytesAsync(fullPath, bytes, cancellationToken).ConfigureAwait(false);
        Helpers.Open(fullPath, openImage);
    }

    /// <summary>Overwrites the associated image path and optionally opens it.</summary>
    public void Save(bool openImage) => Save("", openImage);

    /// <summary>Writes encoded bytes to a caller-owned stream and rewinds it when seekable.</summary>
    /// <remarks>Seekable streams are replaced from position zero and truncated to the encoded length. Nonseekable streams receive bytes at their current position.</remarks>
    public void Save(Stream stream, int? quality = null, int? compressionLevel = null) {
        if (stream == null) {
            throw new ArgumentNullException(nameof(stream));
        }
        byte[] bytes = Encode(_imageType, quality, compressionLevel);
        if (stream.CanSeek) {
            stream.Position = 0;
        }
        stream.Write(bytes, 0, bytes.Length);
        if (stream.CanSeek) {
            stream.SetLength(bytes.LongLength);
            stream.Position = 0;
        }
    }

    /// <summary>Returns a new writable, expandable memory stream containing the encoded image, positioned at zero.</summary>
    public MemoryStream ToStream(int? quality = null, int? compressionLevel = null) {
        byte[] bytes = Encode(_imageType, quality, compressionLevel);
        var stream = new MemoryStream(bytes.Length);
        stream.Write(bytes, 0, bytes.Length);
        stream.Position = 0;
        return stream;
    }

    private byte[] Encode(ImageType type, int? quality, int? compressionLevel, CancellationToken cancellationToken = default) {
        EnsureUsable();
        cancellationToken.ThrowIfCancellationRequested();
        OfficeImageMetadata metadata = _metadata.PrepareForEncoding(Helpers.GetContainerFormat(type), out _);
        byte[] bytes;
        if (type == ImageType.Gif || (type == ImageType.Png && _frames.Count > 1)) {
            var frames = ToAnimationFrames(_frames, cancellationToken);
            bytes = RasterAnimationEncoder.Encode(frames, type == ImageType.Gif ? RasterAnimationFormat.Gif : RasterAnimationFormat.Apng, new RasterAnimationOptions { PlayCount = _frames.PlayCount }, cancellationToken);
        } else {
            if (_frames.Count > 1 && type != ImageType.Tiff && type != ImageType.Icon) {
                throw new NotSupportedException("This output format cannot preserve multiple frames. Select a frame explicitly before saving.");
            }
            var options = Helpers.GetEncodingOptions(type, quality, compressionLevel);
            if (type == ImageType.Tiff) {
                options.Tiff.Resolution = metadata.Resolution;
            } else {
                options.DpiX = metadata.PhysicalDpiX ?? 96D;
                options.DpiY = metadata.PhysicalDpiY ?? 96D;
            }
            bytes = type == ImageType.Icon
                ? OfficeIconEncoder.Encode(_frames.Select(frame => frame.Image).ToArray(), options, cancellationToken)
                : type == ImageType.Tiff && _frames.Count > 1
                    ? OfficeTiffCodec.EncodePages(_frames.Select(frame => frame.Image).ToArray(), options.Tiff, cancellationToken)
                    : OfficeRasterImageEncoder.Encode(Raster, Helpers.GetEncoder(type), options, maximumEncodedBytes: 128L * 1024 * 1024, cancellationToken);
        }
        cancellationToken.ThrowIfCancellationRequested();
        if (type == ImageType.Jpeg || type == ImageType.Png || type == ImageType.WebP || type == ImageType.Tiff || type == ImageType.Bmp || type == ImageType.Gif) {
            bytes = OfficeImageMetadata.Apply(bytes, metadata, cancellationToken);
        }
        return bytes;
    }

    private string ResolveOutputPath(string filePath) {
        EnsureUsable();
        string path = string.IsNullOrEmpty(filePath) ? _filePath : Helpers.ResolvePath(filePath);
        if (string.IsNullOrEmpty(path)) {
            throw new InvalidOperationException("An output path is required for an image created from pixels or bytes.");
        }
        return path;
    }

    private static ImageType GetLoadedImageType(string filePath) {
        try { return Helpers.GetImageType(Path.GetExtension(filePath)); }
        catch (NotSupportedException) { return ImageType.Png; }
    }

    private void Apply(Func<OfficeRasterImage, OfficeRasterImage> transform, Func<OfficeRasterImage, (int Width, int Height)>? outputSize = null, long additionalRetainedBytes = 0) {
        EnsureUsable();
        _frames = _frames.Transform(transform, outputSize, additionalRetainedBytes: additionalRetainedBytes);
    }

    private void EnsureUsable() {
        if (_disposed) {
            throw new ObjectDisposedException(nameof(Image));
        }
    }

    /// <summary>Releases references to managed pixel buffers. Later image access throws.</summary>
    public void Dispose() {
        if (_disposed) { return; }
        _disposed = true;
        _frames = null!;
        _metadata = null!;
    }
}

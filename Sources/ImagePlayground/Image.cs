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
    private OfficeImageFormat _sourceFormat = OfficeImageFormat.Unknown;
    private bool _sourceIsAnimation;
    private bool _disposed;
    private OfficeRasterImage _image => Raster;

    /// <summary>Width of the first frame in pixels.</summary>
    public int Width => Raster.Width;
    /// <summary>Height of the first frame in pixels.</summary>
    public int Height => Raster.Height;
    /// <summary>Resolved path associated with a loaded or created image, or an empty string for pixel and byte inputs.</summary>
    public string FilePath { get { EnsureUsable(); return _filePath; } }
    /// <summary>Detected source container, independent of its filename; pixel-created images have an unknown source format.</summary>
    public OfficeImageFormat SourceFormat { get { EnsureUsable(); return _sourceFormat; } }
    /// <summary>Supported mapped format used by default stream export; unsupported source containers fall back to PNG.</summary>
    public ImageType DefaultOutputFormat { get { EnsureUsable(); return _imageType; } }

    /// <summary>Editable metadata whose profiles are preserved when the output format supports them.</summary>
    public OfficeImageMetadata Metadata { get { EnsureUsable(); return _metadata; } }
    /// <summary>Decoded frames, TIFF pages, or icon resolutions, including animation timing and play count.</summary>
    public OfficeRasterFrames Frames { get { EnsureUsable(); return _frames; } }
    /// <summary>Managed RGBA pixels of the first frame.</summary>
    public OfficeRasterImage Raster { get { EnsureUsable(); return _frames[0].Image; } }

    /// <summary>Reports metadata profile families the selected output format cannot preserve.</summary>
    /// <remarks>This is a preflight profile-family estimate; Encode returns the completed output evidence. The current metadata remains unchanged. Save retains the supported families; lossless metadata editing remains strict about unsupported profiles. TIFF-relative opaque fields must be explicitly removed or replaced before raster re-encoding.</remarks>
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

    /// <summary>Creates a wrapper retaining the supplied frames and their mutable pixel buffers without copying.</summary>
    public static Image FromFrames(OfficeRasterFrames frames, ImageType imageType = ImageType.Png) {
        if (frames == null) {
            throw new ArgumentNullException(nameof(frames));
        }
        return new Image { _frames = frames, _imageType = imageType };
    }

    /// <summary>Creates an independent copy of pixels, frame timing, and metadata.</summary>
    public Image Clone(CancellationToken cancellationToken = default) {
        EnsureUsable();
        var clone = FromFrames(_frames.Transform(frame => frame.Clone(), cancellationToken: cancellationToken), _imageType);
        clone._filePath = _filePath;
        clone._sourceFormat = _sourceFormat;
        clone._sourceIsAnimation = _sourceIsAnimation;
        clone._metadata = _metadata.Clone();
        return clone;
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
        _sourceFormat = OfficeImageFormat.Unknown;
        _sourceIsAnimation = false;
    }

    /// <summary>Compares the first frames, returning pixel metrics and a difference image.</summary>
    public OfficeRasterComparisonResult Compare(Image imageToCompare, CancellationToken cancellationToken = default) {
        if (imageToCompare == null) {
            throw new ArgumentNullException(nameof(imageToCompare));
        }
        return OfficeRasterComparison.Compare(Raster, imageToCompare.Raster, cancellationToken);
    }

    /// <summary>Compares the first frame with an image file.</summary>
    public OfficeRasterComparisonResult Compare(string filePathToCompare, CancellationToken cancellationToken = default) {
        using var other = Load(filePathToCompare, new OfficeRasterDecodeOptions { CancellationToken = cancellationToken, ApplyExifOrientation = false });
        return Compare(other, cancellationToken);
    }

    /// <summary>Saves a PNG difference mask between two first frames.</summary>
    public void Compare(Image imageToCompare, string filePathToSave, CancellationToken cancellationToken = default) {
        using var mask = FromRaster(Compare(imageToCompare, cancellationToken).DifferenceImage);
        string output = Helpers.ResolvePath(filePathToSave);
        Helpers.CreateParentDirectory(output);
        OfficeImageFileWriter.WriteAllBytes(output, mask.Encode(ImageType.Png, null, cancellationToken).EncodedBytes, cancellationToken);
    }

    /// <summary>Saves a PNG difference mask against another image file.</summary>
    public void Compare(string filePathToCompare, string filePathToSave, CancellationToken cancellationToken = default) {
        using var other = Load(filePathToCompare, new OfficeRasterDecodeOptions { CancellationToken = cancellationToken, ApplyExifOrientation = false });
        Compare(other, filePathToSave, cancellationToken);
    }

    private string ResolveOutputPath(string filePath) {
        EnsureUsable();
        string path = string.IsNullOrEmpty(filePath) ? _filePath : Helpers.ResolvePath(filePath);
        if (string.IsNullOrEmpty(path)) {
            throw new InvalidOperationException("An output path is required for an image created from pixels or bytes.");
        }
        return path;
    }

    private void Apply(Func<OfficeRasterImage, OfficeRasterImage> transform, Func<OfficeRasterImage, (int Width, int Height)>? outputSize = null, long additionalRetainedBytes = 0, CancellationToken cancellationToken = default) {
        EnsureUsable();
        _frames = _frames.Transform(transform, outputSize, cancellationToken, additionalRetainedBytes);
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

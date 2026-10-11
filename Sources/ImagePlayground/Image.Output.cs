using ChartForgeX.Raster;

namespace ImagePlayground;

/// <summary>Format-explicit export through the shared metadata and raster encoders.</summary>
public partial class Image {
    /// <summary>Saves using the destination extension, optionally opening the completed file.</summary>
    /// <remarks>Supported metadata is preserved. Completed bytes atomically replace the destination.</remarks>
    public void Save(string filePath = "", bool openImage = false, int? quality = null, int? compressionLevel = null) {
        string fullPath = ResolveOutputPath(filePath);
        ImageType format = Helpers.GetImageType(Path.GetExtension(fullPath));
        Save(fullPath, Helpers.GetEncodingOptions(format, quality, compressionLevel), openImage);
    }

    /// <summary>Saves using the destination extension and owned encoder options.</summary>
    /// <remarks>Cancellation before the atomic commit preserves an existing destination.</remarks>
    public void Save(string filePath, OfficeRasterEncodingOptions? options, bool openImage = false, CancellationToken cancellationToken = default) {
        string fullPath = ResolveOutputPath(filePath);
        byte[] bytes = Encode(Helpers.GetImageType(Path.GetExtension(fullPath)), options, cancellationToken).EncodedBytes;
        Helpers.CreateParentDirectory(fullPath);
        OfficeImageFileWriter.WriteAllBytes(fullPath, bytes, cancellationToken);
        Helpers.Open(fullPath, openImage);
    }

    /// <summary>Saves asynchronously using the destination extension and simple encoder controls.</summary>
    public Task SaveAsync(string filePath = "", bool openImage = false, int? quality = null, int? compressionLevel = null, CancellationToken cancellationToken = default) {
        string fullPath = ResolveOutputPath(filePath);
        ImageType format = Helpers.GetImageType(Path.GetExtension(fullPath));
        return SaveAsync(fullPath, Helpers.GetEncodingOptions(format, quality, compressionLevel), openImage, cancellationToken);
    }

    /// <summary>Encodes and atomically saves with owned options while observing cancellation.</summary>
    public async Task SaveAsync(string filePath, OfficeRasterEncodingOptions? options, bool openImage = false, CancellationToken cancellationToken = default) {
        string fullPath = ResolveOutputPath(filePath);
        var effective = options?.Clone();
        byte[] bytes = await Task.Run(() => Encode(Helpers.GetImageType(Path.GetExtension(fullPath)), effective, cancellationToken).EncodedBytes, cancellationToken).ConfigureAwait(false);
        cancellationToken.ThrowIfCancellationRequested();
        Helpers.CreateParentDirectory(fullPath);
        await OfficeImageFileWriter.WriteAllBytesAsync(fullPath, bytes, cancellationToken).ConfigureAwait(false);
        Helpers.Open(fullPath, openImage);
    }

    /// <summary>Overwrites the associated image path and optionally opens it.</summary>
    public void Save(bool openImage) => Save("", openImage);

    /// <summary>Writes in the image's default encoding format to a caller-owned stream.</summary>
    /// <remarks>Seekable streams are replaced, truncated, and rewound. Nonseekable streams receive bytes at their current position.</remarks>
    public void Save(Stream stream, int? quality = null, int? compressionLevel = null) =>
        Save(stream, _imageType, Helpers.GetEncodingOptions(_imageType, quality, compressionLevel));

    /// <summary>Writes an explicit format with owned options to a caller-owned stream.</summary>
    /// <remarks>The stream remains open. Seekable streams are replaced, truncated, and rewound; a write failure or cancellation during writing may leave partial output.</remarks>
    public void Save(Stream stream, ImageType format, OfficeRasterEncodingOptions? options = null, CancellationToken cancellationToken = default) {
        if (stream == null) { throw new ArgumentNullException(nameof(stream)); }
        if (!stream.CanWrite) { throw new NotSupportedException("Image output requires a writable stream."); }
        byte[] bytes = Encode(format, options, cancellationToken).EncodedBytes;
        cancellationToken.ThrowIfCancellationRequested();
        if (stream.CanSeek) { stream.Position = 0; }
        const int blockSize = 81920;
        for (int offset = 0; offset < bytes.Length; offset += blockSize) {
            cancellationToken.ThrowIfCancellationRequested();
            stream.Write(bytes, offset, Math.Min(blockSize, bytes.Length - offset));
        }
        if (stream.CanSeek) { stream.SetLength(bytes.LongLength); stream.Position = 0; }
    }

    /// <summary>Returns a writable, expandable encoded stream at position zero using the image's default format.</summary>
    public MemoryStream ToStream(int? quality = null, int? compressionLevel = null) =>
        ToStream(_imageType, Helpers.GetEncodingOptions(_imageType, quality, compressionLevel));

    /// <summary>Returns a writable, expandable encoded stream at position zero using an explicit format.</summary>
    public MemoryStream ToStream(ImageType format, OfficeRasterEncodingOptions? options = null, CancellationToken cancellationToken = default) {
        byte[] bytes = Encode(format, options, cancellationToken).EncodedBytes;
        var stream = new MemoryStream(bytes.Length);
        stream.Write(bytes, 0, bytes.Length);
        stream.Position = 0;
        return stream;
    }

    /// <summary>Prepares encoded bytes and the actual metadata projection and omission evidence without writing output.</summary>
    /// <remarks>The result owns its byte array. The image pixels and editable metadata remain unchanged.</remarks>
    public OfficeRasterEncodingResult Encode(ImageType format, OfficeRasterEncodingOptions? options = null, CancellationToken cancellationToken = default) {
        EnsureUsable();
        cancellationToken.ThrowIfCancellationRequested();
        options = options?.Clone();
        if (format == ImageType.Gif || (format == ImageType.Png && (_frames.Count > 1 || _sourceIsAnimation || _frames.PlayCount != 1 || _frames[0].Duration > TimeSpan.Zero))) {
            var frames = ToAnimationFrames(_frames, cancellationToken);
            byte[] encoded = RasterAnimationEncoder.Encode(frames,
                format == ImageType.Gif ? RasterAnimationFormat.Gif : RasterAnimationFormat.Apng,
                new RasterAnimationOptions {
                    PlayCount = _frames.PlayCount,
                    PngCompressionLevel = format == ImageType.Png ? GetAnimationPngCompressionLevel(options) : 6
                }, cancellationToken);
            return _metadata.ApplyForEncoding(encoded, options, cancellationToken: cancellationToken);
        }
        return OfficeRasterImageEncoder.EncodeWithMetadata(_frames, Helpers.GetEncoder(format), _metadata, options,
            cancellationToken: cancellationToken);
    }

    private static int GetAnimationPngCompressionLevel(OfficeRasterEncodingOptions? options) => options?.Png.Compression switch {
        OfficePngCompression.Stored => 0,
        OfficePngCompression.Fastest => 1,
        OfficePngCompression.Optimal => 6,
        null => 6,
        _ => throw new ArgumentOutOfRangeException(nameof(options), "Unsupported PNG compression setting.")
    };

}

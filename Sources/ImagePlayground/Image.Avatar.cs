namespace ImagePlayground;

/// <summary>Avatar and transparent corner workflows.</summary>
public partial class Image {
    /// <summary>Fits the image to the requested canvas with centered cropping and rounded corners.</summary>
    public void Avatar(int width, int height, float cornerRadius, CancellationToken cancellationToken = default) {
        EnsureUsable();
        _frames = CreateAvatarFrames(width, height, cornerRadius, cancellationToken);
    }
    /// <summary>Saves a rounded avatar without changing this image's pixels or metadata.</summary>
    public void SaveAsAvatar(string filePath, int width, int height, float cornerRadius, CancellationToken cancellationToken = default) {
        using var clone = CreateAvatarCopy(width, height, cornerRadius, cancellationToken);
        clone.Save(filePath, options: null, cancellationToken: cancellationToken);
    }
    /// <summary>Writes a PNG rounded avatar to a caller-owned stream without changing this image.</summary>
    public void SaveAsAvatar(Stream stream, int width, int height, float cornerRadius, CancellationToken cancellationToken = default) {
        if (stream == null) {
            throw new ArgumentNullException(nameof(stream));
        }
        using var clone = CreateAvatarCopy(width, height, cornerRadius, cancellationToken);
        clone.Save(stream, ImageType.Png, cancellationToken: cancellationToken);
    }
    /// <summary>Saves a circular avatar without changing this image's pixels or metadata.</summary>
    public void SaveAsCircularAvatar(string filePath, int size, CancellationToken cancellationToken = default) => SaveAsAvatar(filePath, size, size, size / 2f, cancellationToken);
    /// <summary>Writes a PNG circular avatar to a caller-owned stream without changing this image.</summary>
    public void SaveAsCircularAvatar(Stream stream, int size, CancellationToken cancellationToken = default) => SaveAsAvatar(stream, size, size, size / 2f, cancellationToken);

    private Image CreateAvatarCopy(int width, int height, float cornerRadius, CancellationToken cancellationToken = default) {
        EnsureUsable();
        return new Image {
            _frames = CreateAvatarFrames(width, height, cornerRadius, cancellationToken),
            _metadata = _metadata.Clone(),
            _filePath = _filePath,
            _imageType = _imageType,
            _sourceFormat = _sourceFormat,
            _sourceIsAnimation = _sourceIsAnimation
        };
    }

    private OfficeRasterFrames CreateAvatarFrames(int width, int height, float cornerRadius, CancellationToken cancellationToken = default) {
        if (width <= 0) {
            throw new ArgumentOutOfRangeException(nameof(width));
        }
        if (height <= 0) {
            throw new ArgumentOutOfRangeException(nameof(height));
        }
        var options = new OfficeRasterResizeOptions { Width = width, Height = height, Fit = OfficeImageFit.Cover };
        long avatarBytes = checked((long)width * height * 4);
        long additionalWorkingBytes = 0;
        foreach (var frame in _frames) {
            var plan = OfficeRasterResampler.PlanResize(frame.Image.Width, frame.Image.Height, options, cancellationToken);
            // The sequence reserves source and final pixels; masking also retains the fitted image and mask.
            additionalWorkingBytes = Math.Max(additionalWorkingBytes, Math.Max(plan.AdditionalWorkingBytes, avatarBytes * 2));
        }
        return _frames.Transform(source => {
            var resized = OfficeRasterResampler.Resize(source, options, cancellationToken);
            return OfficeRasterTransforms.MaskRoundedRectangle(resized, cornerRadius, cancellationToken);
        }, _ => (width, height), cancellationToken, additionalWorkingBytes);
    }
}

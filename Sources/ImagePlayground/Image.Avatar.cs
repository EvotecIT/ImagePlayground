namespace ImagePlayground;

/// <summary>Avatar and transparent corner workflows.</summary>
public partial class Image {
    /// <summary>Fits the image to the requested canvas with centered cropping and rounded corners.</summary>
    public void Avatar(int width, int height, float cornerRadius) {
        EnsureUsable();
        _frames = CreateAvatarFrames(width, height, cornerRadius);
    }
    /// <summary>Saves a rounded avatar without changing this image's pixels or metadata.</summary>
    public void SaveAsAvatar(string filePath, int width, int height, float cornerRadius) {
        using var clone = CreateAvatarCopy(width, height, cornerRadius);
        clone.Save(filePath);
    }
    /// <summary>Writes a PNG rounded avatar to a caller-owned stream without changing this image.</summary>
    public void SaveAsAvatar(Stream stream, int width, int height, float cornerRadius) {
        if (stream == null) {
            throw new ArgumentNullException(nameof(stream));
        }
        using var clone = CreateAvatarCopy(width, height, cornerRadius);
        clone._imageType = ImageType.Png;
        clone.Save(stream);
    }
    /// <summary>Saves a circular avatar without changing this image's pixels or metadata.</summary>
    public void SaveAsCircularAvatar(string filePath, int size) => SaveAsAvatar(filePath, size, size, size / 2f);
    /// <summary>Writes a PNG circular avatar to a caller-owned stream without changing this image.</summary>
    public void SaveAsCircularAvatar(Stream stream, int size) => SaveAsAvatar(stream, size, size, size / 2f);

    private Image CreateAvatarCopy(int width, int height, float cornerRadius) {
        EnsureUsable();
        return new Image {
            _frames = CreateAvatarFrames(width, height, cornerRadius),
            _metadata = _metadata.Clone(),
            _filePath = _filePath,
            _imageType = _imageType
        };
    }

    private OfficeRasterFrames CreateAvatarFrames(int width, int height, float cornerRadius) {
        if (width <= 0) {
            throw new ArgumentOutOfRangeException(nameof(width));
        }
        if (height <= 0) {
            throw new ArgumentOutOfRangeException(nameof(height));
        }
        long avatarBytes = checked((long)width * height * 4);
        long additionalWorkingBytes = 0;
        foreach (var frame in _frames) {
            var resizedSize = GetAvatarResizeDimensions(frame.Image, width, height);
            long resizeWorkingBytes = OfficeRasterResampler.GetResizeWorkingSetBytes(frame.Image.Width, frame.Image.Height,
                resizedSize.Width, resizedSize.Height, OfficeRasterResamplingMode.Bicubic);
            long sourceBytes = (long)frame.Image.Width * frame.Image.Height * 4;
            long resizedBytes = (long)resizedSize.Width * resizedSize.Height * 4;
            // The frame plan already reserves the source and final avatar. At mask time the resized,
            // cropped and mask buffers can remain live together; the resizer plans its own scratch.
            additionalWorkingBytes = Math.Max(additionalWorkingBytes,
                Math.Max(resizeWorkingBytes - sourceBytes - avatarBytes, checked(resizedBytes + avatarBytes * 2)));
        }
        return _frames.Transform(source => {
            var resizedSize = GetAvatarResizeDimensions(source, width, height);
            var resized = OfficeRasterResampler.Resize(source, resizedSize.Width, resizedSize.Height, OfficeRasterResamplingMode.Bicubic);
            var cropped = OfficeRasterTransforms.Crop(resized, (resized.Width - width) / 2, (resized.Height - height) / 2, width, height);
            return OfficeRasterTransforms.MaskRoundedRectangle(cropped, cornerRadius);
        }, _ => (width, height), additionalRetainedBytes: additionalWorkingBytes);
    }

    private static (int Width, int Height) GetAvatarResizeDimensions(OfficeRasterImage source, int width, int height) {
        double scale = Math.Max(width / (double)source.Width, height / (double)source.Height);
        return (Math.Max(width, checked((int)Math.Ceiling(source.Width * scale))),
            Math.Max(height, checked((int)Math.Ceiling(source.Height * scale))));
    }
}

namespace ImagePlayground;

/// <summary>Composites image pixels without exposing a third-party image object.</summary>
public partial class Image {
    /// <summary>Draws the first frame of an image file over every target frame.</summary>
    public void AddImage(string filePath, int x, int y, float opacity, CancellationToken cancellationToken = default) {
        OfficeRasterImage firstFrame;
        using (var image = Load(filePath, new OfficeRasterDecodeOptions { ApplyExifOrientation = false, CancellationToken = cancellationToken })) {
            firstFrame = image.Raster;
        }
        AddImage(firstFrame, x, y, opacity, cancellationToken);
    }
    /// <summary>Draws a caller-supplied managed raster over every target frame.</summary>
    public void AddImage(OfficeRasterImage image, int x, int y, float opacity, CancellationToken cancellationToken = default) => AddImage(image, new OfficePoint(x, y), opacity, cancellationToken);
    /// <summary>Draws a caller-supplied managed raster at the requested position and opacity.</summary>
    public void AddImage(OfficeRasterImage image, OfficePoint location, float opacity, CancellationToken cancellationToken = default) {
        EnsureUsable();
        cancellationToken.ThrowIfCancellationRequested();
        if (image == null) {
            throw new ArgumentNullException(nameof(image));
        }
        if (opacity < 0 || opacity > 1 || float.IsNaN(opacity)) {
            throw new ArgumentOutOfRangeException(nameof(opacity));
        }
        if (opacity == 0) {
            return;
        }
        long overlayBytes = _frames.Any(frame => ReferenceEquals(frame.Image, image))
            ? 0
            : (long)image.Width * image.Height * 4;
        Apply(source => {
            var result = source.Clone();
            new OfficeRasterCanvas(result, font: null, fonts: null, cancellationToken: cancellationToken).DrawAffineImage(image, OfficeTransform.Translate(location.X, location.Y), opacity);
            return result;
        }, additionalRetainedBytes: overlayBytes, cancellationToken: cancellationToken);
    }
}

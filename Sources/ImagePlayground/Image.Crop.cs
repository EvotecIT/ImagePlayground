namespace ImagePlayground;

/// <summary>Cropping and transparency masks over managed image frames.</summary>
public partial class Image {
    /// <summary>Crops each frame to an exact in-bounds pixel rectangle.</summary>
    public void Crop(Rectangle rectangle, CancellationToken cancellationToken = default) => Apply(image => OfficeRasterTransforms.Crop(image, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, cancellationToken), _ => (rectangle.Width, rectangle.Height), cancellationToken: cancellationToken);
    /// <summary>Keeps pixels inside the requested circular region without changing canvas dimensions.</summary>
    public void CropCircle(float centerX, float centerY, float radius, CancellationToken cancellationToken = default) => Apply(image => OfficeRasterTransforms.MaskEllipse(image, centerX-radius, centerY-radius, radius*2, radius*2, cancellationToken), cancellationToken: cancellationToken);
    /// <summary>Keeps pixels inside a polygon without changing canvas dimensions.</summary>
    public void CropPolygon(params OfficePoint[] points) => CropPolygon(points, default);
    /// <summary>Keeps pixels inside a polygon while observing cancellation.</summary>
    public void CropPolygon(IReadOnlyList<OfficePoint> points, CancellationToken cancellationToken) => Apply(image => OfficeRasterTransforms.MaskPolygon(image, points, cancellationToken), cancellationToken: cancellationToken);
}

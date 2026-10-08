namespace ImagePlayground;

/// <summary>Cropping and transparency masks over managed image frames.</summary>
public partial class Image {
    /// <summary>Crops each frame to an exact in-bounds pixel rectangle.</summary>
    public void Crop(Rectangle rectangle) => Apply(image => OfficeRasterTransforms.Crop(image, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height), _ => (rectangle.Width, rectangle.Height));
    /// <summary>Keeps pixels inside the requested circular region without changing canvas dimensions.</summary>
    public void CropCircle(float centerX, float centerY, float radius) => Apply(image => OfficeRasterTransforms.MaskEllipse(image, centerX-radius, centerY-radius, radius*2, radius*2));
    /// <summary>Keeps pixels inside a polygon without changing canvas dimensions.</summary>
    public void CropPolygon(params OfficePoint[] points) => Apply(image => OfficeRasterTransforms.MaskPolygon(image, points));
}
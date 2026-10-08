namespace ImagePlayground;

/// <summary>Composites image pixels without exposing a third-party image object.</summary>
public partial class Image {
    /// <summary>Draws the first frame of an image file over every target frame.</summary>
    public void AddImage(string filePath, int x, int y, float opacity) {
        using var image = Load(filePath); AddImage(image.Raster, x, y, opacity);
    }
    /// <summary>Draws a caller-supplied managed raster over every target frame.</summary>
    public void AddImage(OfficeRasterImage image, int x, int y, float opacity) => AddImage(image, new OfficePoint(x, y), opacity);
    /// <summary>Draws a caller-supplied managed raster at the requested position and opacity.</summary>
    public void AddImage(OfficeRasterImage image, OfficePoint location, float opacity) => Apply(source => {
        if (image == null) {
            throw new ArgumentNullException(nameof(image));
        }
        if (opacity < 0 || opacity > 1 || float.IsNaN(opacity)) {
            throw new ArgumentOutOfRangeException(nameof(opacity));
        }
        var result = source.Clone();
        new OfficeRasterCanvas(result).DrawAffineImage(image, OfficeTransform.Translate(location.X, location.Y), opacity);
        return result;
    });
}
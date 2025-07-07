using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ImagePlayground;
/// <summary>
/// Provides image manipulation operations.
/// </summary>
public partial class Image : System.IDisposable {
    /// <summary>
    /// Crops the image to a circular region.
    /// </summary>
    /// <param name="centerX">Center X coordinate.</param>
    /// <param name="centerY">Center Y coordinate.</param>
    /// <param name="radius">Radius of the circle.</param>
    public void CropCircle(float centerX, float centerY, float radius) {
        var circle = new EllipsePolygon(centerX, centerY, radius);
        ApplyClip(circle);
    }

    /// <summary>
    /// Crops the image to the specified polygon.
    /// </summary>
    /// <param name="points">Polygon vertices.</param>
    public void CropPolygon(params PointF[] points) {
        var polygon = new Polygon(new LinearLineSegment(points));
        ApplyClip(polygon);
    }

    private void ApplyClip(IPath shape) {
        using var clone = _image.Clone(ctx => { });
        _image.Mutate(x => {
            x.Clear(Color.Transparent);
            x.Clip(shape, ctx => ctx.DrawImage(clone, 1f));
        });
    }
}

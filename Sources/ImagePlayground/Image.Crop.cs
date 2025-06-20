using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ImagePlayground;
public partial class Image : System.IDisposable {
    public void CropCircle(float centerX, float centerY, float radius) {
        var circle = new EllipsePolygon(centerX, centerY, radius);
        ApplyClip(circle);
    }

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

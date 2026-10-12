namespace ImagePlayground;

/// <summary>Drawing operations over managed image frames.</summary>
public partial class Image {
    /// <summary>Composites a color behind each image frame.</summary>
    public void BackgroundColor(OfficeColor color, CancellationToken cancellationToken = default) => Apply(source => {
        var result = new OfficeRasterImage(source.Width, source.Height, color);
        new OfficeRasterCanvas(result, font: null, fonts: null, cancellationToken: cancellationToken).DrawImage(source, 0, 0, source.Width, source.Height);
        return result;
    }, cancellationToken: cancellationToken);
    /// <summary>Draws a polyline using the supplied points.</summary>
    public void DrawLines(OfficeColor color, float thickness, params OfficePoint[] points) => DrawLines(color, thickness, points, default);
    /// <summary>Draws a polyline while observing cancellation.</summary>
    public void DrawLines(OfficeColor color, float thickness, IReadOnlyList<OfficePoint> points, CancellationToken cancellationToken) => Apply(source => {
        if (points == null || points.Count < 2) { throw new ArgumentException("At least two points are required.", nameof(points)); }
        var result = source.Clone(); var canvas = new OfficeRasterCanvas(result, font: null, fonts: null, cancellationToken: cancellationToken);
        for (int i = 1; i < points.Count; i++) {
            canvas.DrawLine(points[i-1].X, points[i-1].Y, points[i].X, points[i].Y, color, thickness);
        }
        return result;
    }, cancellationToken: cancellationToken);
    /// <summary>Draws the closed outline of a polygon.</summary>
    public void DrawPolygon(OfficeColor color, float thickness, params OfficePoint[] points) => DrawPolygon(color, thickness, points, default);
    /// <summary>Draws a polygon outline while observing cancellation.</summary>
    public void DrawPolygon(OfficeColor color, float thickness, IReadOnlyList<OfficePoint> points, CancellationToken cancellationToken) => Apply(source => {
        var result = source.Clone(); new OfficeRasterCanvas(result, font: null, fonts: null, cancellationToken: cancellationToken).DrawPolygon(points, color, thickness); return result;
    }, cancellationToken: cancellationToken);
    /// <summary>Replaces all pixels with the supplied color.</summary>
    public void Fill(OfficeColor color, CancellationToken cancellationToken = default) => Apply(source => new OfficeRasterImage(source.Width, source.Height, color), cancellationToken: cancellationToken);
    /// <summary>Fills a rectangular region using alpha compositing.</summary>
    public void Fill(OfficeColor color, Rectangle rectangle, CancellationToken cancellationToken = default) => Apply(source => {
        var result = source.Clone(); new OfficeRasterCanvas(result, font: null, fonts: null, cancellationToken: cancellationToken).FillRectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, color); return result;
    }, cancellationToken: cancellationToken);
}
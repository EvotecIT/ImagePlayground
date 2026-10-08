namespace ImagePlayground;

/// <summary>Drawing operations over managed image frames.</summary>
public partial class Image {
    /// <summary>Composites a color behind each image frame.</summary>
    public void BackgroundColor(OfficeColor color) => Apply(source => {
        var result = new OfficeRasterImage(source.Width, source.Height, color);
        new OfficeRasterCanvas(result).DrawImage(source, 0, 0, source.Width, source.Height);
        return result;
    });
    /// <summary>Draws a polyline using the supplied points.</summary>
    public void DrawLines(OfficeColor color, float thickness, params OfficePoint[] points) => Apply(source => {
        if (points == null || points.Length < 2) throw new ArgumentException("At least two points are required.", nameof(points));
        var result = source.Clone(); var canvas = new OfficeRasterCanvas(result);
        for (int i = 1; i < points.Length; i++) canvas.DrawLine(points[i-1].X, points[i-1].Y, points[i].X, points[i].Y, color, thickness);
        return result;
    });
    /// <summary>Draws the closed outline of a polygon.</summary>
    public void DrawPolygon(OfficeColor color, float thickness, params OfficePoint[] points) => Apply(source => {
        var result = source.Clone(); new OfficeRasterCanvas(result).DrawPolygon(points, color, thickness); return result;
    });
    /// <summary>Replaces all pixels with the supplied color.</summary>
    public void Fill(OfficeColor color) => Apply(source => new OfficeRasterImage(source.Width, source.Height, color));
    /// <summary>Fills a rectangular region using alpha compositing.</summary>
    public void Fill(OfficeColor color, Rectangle rectangle) => Apply(source => {
        var result = source.Clone(); new OfficeRasterCanvas(result).FillRectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, color); return result;
    });
}
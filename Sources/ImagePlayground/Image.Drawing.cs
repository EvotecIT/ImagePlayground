using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;

namespace ImagePlayground;

/// <summary>
/// Provides drawing operations for images.
/// </summary>
public partial class Image : IDisposable {
    /// <summary>
    /// Fills the background with the specified <paramref name="color"/>.
    /// </summary>
    /// <param name="color">Fill color.</param>
    public void BackgroundColor(Color color) {
        _image.Mutate(x => x.BackgroundColor(color));
    }

    /// <summary>
    /// Draws a line ending at <paramref name="pointF"/> using the specified <paramref name="color"/> and <paramref name="thickness"/>.
    /// </summary>
    /// <param name="color">Line color.</param>
    /// <param name="thickness">Line thickness.</param>
    /// <param name="pointF">End point of the line.</param>
    public void DrawLines(Color color, float thickness, PointF pointF) {
        _image.Mutate(x => x.DrawLine(color, thickness, pointF));
    }

    /// <summary>
    /// Draws a polygon with the specified parameters.
    /// </summary>
    /// <param name="color">Polygon color.</param>
    /// <param name="thickness">Outline thickness.</param>
    /// <param name="pointF">Polygon vertex.</param>
    public void DrawPolygon(Color color, float thickness, PointF pointF) {
        _image.Mutate(x => x.DrawPolygon(color, thickness, pointF));
    }

    /// <summary>
    /// Fills the image with <paramref name="color"/>.
    /// </summary>
    /// <param name="color">Fill color.</param>
    public void Fill(Color color) {
        _image.Mutate(x => x.Fill(color));
    }

    /// <summary>
    /// Fills the specified <paramref name="rectangle"/> with <paramref name="color"/>.
    /// </summary>
    /// <param name="color">Fill color.</param>
    /// <param name="rectangle">Target rectangle.</param>
    public void Fill(Color color, Rectangle rectangle) {
        _image.Mutate(x => x.Fill(color, rectangle));
    }
}


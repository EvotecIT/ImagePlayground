using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;


namespace ImagePlayground;
/// <summary>
/// Provides image manipulation operations.
/// </summary>
public partial class Image : IDisposable {
    /// <summary>
    /// Draws another image onto the current image from a file path.
    /// </summary>
    /// <param name="filePath">Path to the source image.</param>
    /// <param name="x">X coordinate where the image will be placed.</param>
    /// <param name="y">Y coordinate where the image will be placed.</param>
    /// <param name="opacity">Opacity of the drawn image.</param>
    public void AddImage(string filePath, int x, int y, float opacity) {
        string fullPath = Helpers.ResolvePath(filePath);

        var location = new Point(x, y);
        using (var image = SixLabors.ImageSharp.Image.Load(fullPath)) {
            _image.Mutate(mx => mx.DrawImage(image, location, opacity));
        }
    }
    /// <summary>
    /// Draws another image onto the current image.
    /// </summary>
    /// <param name="image">Image to draw.</param>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    /// <param name="opacity">Opacity of the drawn image.</param>
    public void AddImage(SixLabors.ImageSharp.Image image, int x, int y, float opacity) {
        var location = new Point(x, y);
        _image.Mutate(mx => mx.DrawImage(image, location, opacity));
    }

    /// <summary>
    /// Draws another image onto the current image at the given location.
    /// </summary>
    /// <param name="image">Image to draw.</param>
    /// <param name="location">Target location.</param>
    /// <param name="opacity">Opacity of the drawn image.</param>
    public void AddImage(SixLabors.ImageSharp.Image image, Point location, float opacity) {
        _image.Mutate(mx => mx.DrawImage(image, location, opacity));
    }
}

using System.IO;
using SixLabors.ImageSharp;

namespace ImagePlayground;
/// <summary>
/// Provides helper methods for image manipulation.
/// </summary>
/// <remarks>
/// Useful for quickly cropping or clipping images without manual steps.
/// </remarks>
public partial class ImageHelper {
    /// <summary>
    /// Crops an image to the specified rectangle and saves the result.
    /// </summary>
    /// <param name="filePath">Source image path.</param>
    /// <param name="outFilePath">Destination image path.</param>
    /// <param name="rectangle">Rectangle to crop.</param>
    public static void Crop(string filePath, string outFilePath, Rectangle rectangle) {
        string fullPath = Helpers.ResolvePath(filePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);

        using var img = Image.Load(fullPath);
        img.Crop(rectangle);
        img.Save(outFullPath);
    }

    /// <summary>
    /// Crops an image to a circle and saves the result.
    /// </summary>
    /// <param name="filePath">Source image path.</param>
    /// <param name="outFilePath">Destination image path.</param>
    /// <param name="centerX">Circle center X.</param>
    /// <param name="centerY">Circle center Y.</param>
    /// <param name="radius">Circle radius.</param>
    public static void CropCircle(string filePath, string outFilePath, float centerX, float centerY, float radius) {
        string fullPath = Helpers.ResolvePath(filePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);

        using var img = Image.Load(fullPath);
        img.CropCircle(centerX, centerY, radius);
        img.Save(outFullPath);
    }

    /// <summary>
    /// Crops an image using a polygon and saves the result.
    /// </summary>
    /// <param name="filePath">Source image path.</param>
    /// <param name="outFilePath">Destination image path.</param>
    /// <param name="points">Polygon points.</param>
    public static void CropPolygon(string filePath, string outFilePath, params PointF[] points) {
        string fullPath = Helpers.ResolvePath(filePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);

        using var img = Image.Load(fullPath);
        img.CropPolygon(points);
        img.Save(outFullPath);
    }
}

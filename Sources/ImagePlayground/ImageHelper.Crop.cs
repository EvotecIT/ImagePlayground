using System.IO;
using SixLabors.ImageSharp;

namespace ImagePlayground;
public partial class ImageHelper {
    public static void Crop(string filePath, string outFilePath, Rectangle rectangle) {
        string fullPath = Helpers.ResolvePath(filePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);

        using var img = Image.Load(fullPath);
        img.Crop(rectangle);
        img.Save(outFullPath);
    }

    public static void CropCircle(string filePath, string outFilePath, float centerX, float centerY, float radius) {
        string fullPath = Helpers.ResolvePath(filePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);

        using var img = Image.Load(fullPath);
        img.CropCircle(centerX, centerY, radius);
        img.Save(outFullPath);
    }

    public static void CropPolygon(string filePath, string outFilePath, params PointF[] points) {
        string fullPath = Helpers.ResolvePath(filePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);

        using var img = Image.Load(fullPath);
        img.CropPolygon(points);
        img.Save(outFullPath);
    }
}

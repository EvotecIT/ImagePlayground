using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ImagePlayground {
    public partial class ImageHelper {
        public static void WatermarkImage(string filePath, string outFilePath, string watermarkFilePath, Image.WatermarkPlacement placement, float opacity = 1f, float padding = 18f, int rotate = 0, FlipMode flipMode = FlipMode.None, int watermarkPercentage = 100) {
            string fullPath = Path.GetFullPath(filePath);
            string outFullPath = Path.GetFullPath(outFilePath);
            using var img = Image.Load(fullPath);
            img.WatermarkImage(watermarkFilePath, placement, opacity, padding, rotate, flipMode, watermarkPercentage);
            img.Save(outFullPath);
        }

        public static void WatermarkImage(string filePath, string outFilePath, string watermarkFilePath, int x, int y, float opacity = 1f, int rotate = 0, FlipMode flipMode = FlipMode.None, int watermarkPercentage = 100) {
            string fullPath = Path.GetFullPath(filePath);
            string outFullPath = Path.GetFullPath(outFilePath);
            using var img = Image.Load(fullPath);
            img.WatermarkImage(watermarkFilePath, x, y, opacity, rotate, flipMode, watermarkPercentage);
            img.Save(outFullPath);
        }
    }
}

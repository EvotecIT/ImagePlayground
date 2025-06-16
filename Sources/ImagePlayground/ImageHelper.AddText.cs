using System.IO;
using SixLabors.ImageSharp;

namespace ImagePlayground {
    public partial class ImageHelper {
        public static void AddText(string filePath, string outFilePath, float x, float y, string text, Color color, float fontSize = 16f, string fontFamilyName = "Arial") {
            string fullPath = Path.GetFullPath(filePath);
            string outFullPath = Path.GetFullPath(outFilePath);

            using var img = Image.Load(fullPath);
            img.AddText(x, y, text, color, fontSize, fontFamilyName);
            img.Save(outFullPath);
        }
    }
}

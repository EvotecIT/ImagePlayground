using System.IO;
using SixLabors.ImageSharp;

namespace ImagePlayground {
    public partial class ImageHelper {
        public static void Avatar(string filePath, string outFilePath, int width, int height, float cornerRadius) {
            string fullPath = Path.GetFullPath(filePath);
            string outFullPath = Path.GetFullPath(outFilePath);

            using var img = Image.Load(fullPath);
            img.SaveAsAvatar(outFullPath, width, height, cornerRadius);
        }
    }
}

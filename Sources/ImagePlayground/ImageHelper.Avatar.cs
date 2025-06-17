using System.IO;
using SixLabors.ImageSharp;

namespace ImagePlayground {
    public partial class ImageHelper {
        public static void Avatar(string filePath, string outFilePath, int width, int height, float cornerRadius) {
            string fullPath = Helpers.ResolvePath(filePath);
            string outFullPath = Helpers.ResolvePath(outFilePath);

            using var img = Image.Load(fullPath);
            img.SaveAsAvatar(outFullPath, width, height, cornerRadius);
        }

        public static void Avatar(string filePath, Stream outStream, int width, int height, float cornerRadius) {
            string fullPath = Helpers.ResolvePath(filePath);

            using var img = Image.Load(fullPath);
            img.SaveAsAvatar(outStream, width, height, cornerRadius);
        }

        public static void AvatarCircular(string filePath, string outFilePath, int size) {
            string fullPath = Helpers.ResolvePath(filePath);
            string outFullPath = Helpers.ResolvePath(outFilePath);

            using var img = Image.Load(fullPath);
            img.SaveAsCircularAvatar(outFullPath, size);
        }

        public static void AvatarCircular(string filePath, Stream outStream, int size) {
            string fullPath = Helpers.ResolvePath(filePath);

            using var img = Image.Load(fullPath);
            img.SaveAsCircularAvatar(outStream, size);
        }
    }
}

using System;
using System.IO;

namespace ImagePlayground {
    public partial class ImageHelper {
        public static string ConvertToBase64(string filePath) {
            string fullPath = Helpers.ResolvePath(filePath);
            using var stream = File.OpenRead(fullPath);
            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            return Convert.ToBase64String(ms.ToArray());
        }

        public static void ConvertFromBase64(string base64, string outFilePath) {
            string outFullPath = Helpers.ResolvePath(outFilePath);
            var bytes = Convert.FromBase64String(base64);
            File.WriteAllBytes(outFullPath, bytes);
        }
    }
}

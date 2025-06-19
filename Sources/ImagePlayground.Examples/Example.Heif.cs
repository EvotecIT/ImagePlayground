using System;
using SixLabors.ImageSharp;

namespace ImagePlayground.Examples {
    internal partial class Example {
        public static void HeifAvif(string folderPath) {
            Console.WriteLine("[*] Loading HEIF/AVIF image");
            string src = System.IO.Path.Combine(folderPath, "photo.heic");
            string dest = System.IO.Path.Combine(folderPath, "photo.png");
            using (var image = Image.Load(src)) {
                image.Save(dest);
            }
        }
    }
}

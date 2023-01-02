using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagePlayground.Examples {
    internal partial class Example {
        public static void ImageEXIF(string folderPath) {
            Console.WriteLine("[*] Comparing two images - showing output");
            string filePath1 = System.IO.Path.Combine(folderPath, "IMG_4539.jpeg");

            var image = ImagePlayground.Image.Load(filePath1);
            if (image.Metadata.ExifProfile != null) {
                Console.WriteLine(image.Metadata.ExifProfile.Values);
            }

            string filePath2 = System.IO.Path.Combine(folderPath, "IMG_4539.HEIC");

            image = ImagePlayground.Image.Load(filePath2);
            Console.WriteLine(image.Metadata.ExifProfile.Values);

        }
    }
}

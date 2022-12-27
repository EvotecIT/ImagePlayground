using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagePlayground.Examples {
    internal partial class Example {
        public static void ResizeImage(string folderPath) {
            Console.WriteLine("[*] Resizing - JPG");
            string filePath = System.IO.Path.Combine(folderPath, "KulekWSluchawkach.jpg");
            string filePathOut = System.IO.Path.Combine(folderPath, "KulekWSluchawkach-Resized.jpg");
            ImageHelper.Resize(filePath, filePathOut, 50, 50);

            Console.WriteLine("[*] Resizing - ICO");
            string filePathIco = System.IO.Path.Combine(folderPath, "QRCode1.ico");
            string filePathOutPng = System.IO.Path.Combine(folderPath, "QRCode2.png");
            ImageHelper.ConvertTo(filePathIco, filePathOutPng);

            string filePathOutTemporary = System.IO.Path.Combine(folderPath, "QRCode1-Temporary.png");
            ImageHelper.Resize(filePathOutPng, filePathOutTemporary, 50, 50);

            string filePathOutIco = System.IO.Path.Combine(folderPath, "QRCode1-Temporary.ico");
            ImageHelper.ConvertTo(filePathOutTemporary, filePathOutIco);
        }

        public static void ConvertTo(string folderPath) {
            Console.WriteLine("[*] Converting JPG to PNG");
            string filePath = System.IO.Path.Combine(folderPath, "KulekWSluchawkach.jpg");
            string filePathOut = System.IO.Path.Combine(folderPath, "KulekWSluchawkach.png");
            ImageHelper.ConvertTo(filePath, filePathOut);
            Console.WriteLine("[*] Converting JPG to BMP");
            filePathOut = System.IO.Path.Combine(folderPath, "KulekWSluchawkach.bmp");
            ImageHelper.ConvertTo(filePath, filePathOut);
            Console.WriteLine("[*] Converting JPG to WEBP");
            filePathOut = System.IO.Path.Combine(folderPath, "KulekWSluchawkach.webp");
            ImageHelper.ConvertTo(filePath, filePathOut);
            Console.WriteLine("[*] Converting JPG to ICO");
            filePathOut = System.IO.Path.Combine(folderPath, "KulekWSluchawkach.ico");
            ImageHelper.ConvertTo(filePath, filePathOut);
        }

    }
}
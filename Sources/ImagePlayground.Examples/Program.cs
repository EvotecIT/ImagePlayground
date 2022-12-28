using System;
using System.IO;
using SixLabors.ImageSharp;

namespace ImagePlayground.Examples {
    internal class Program {
        private static void Setup(string path) {
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            } else {
                // Directory.Delete(path, true);
                // Directory.CreateDirectory(path);
            }
        }
        static void Main(string[] args) {
            string folderPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Images");
            Setup(folderPath);

            //Example.QRCode(folderPath);
            //Example.BarCodes1(folderPath);
            //Example.Charts1(folderPath);
            //Example.ResizeImage(folderPath);
            //Example.ConvertTo(folderPath);

            //Example.ImageModifications1(folderPath);
            //Example.ImageModifications2(folderPath);
            Example.ImageTextWatermark(folderPath);
            Example.ImageBGInfo(folderPath);
            // lets open up the folder with the images
            //Console.WriteLine("\npress any key to exit the process...");
            //Console.ReadKey();
            System.Diagnostics.Process.Start("explorer.exe", folderPath);
        }
    }
}
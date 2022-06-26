using System;
using System.Drawing.Imaging;
using System.IO;
using ImagePlayground;

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

            Example_QRCode(folderPath);
            Example_BarCode(folderPath);

            System.Diagnostics.Process.Start("explorer.exe", folderPath);
        }

        private static void Example_BarCode(string folderPath) {
            Console.WriteLine("[*] Creating Barcode code - PNG");
            //string filePath = System.IO.Path.Combine(folderPath, "QR.png");

            //BarCode.GenerateQR("Hello world!", filePath, Barcoder.Renderer.Image.ImageFormat.Png);

            //filePath = System.IO.Path.Combine(folderPath, "QR.jpg");

            //BarCode.GenerateQR("Hello world!", filePath, Barcoder.Renderer.Image.ImageFormat.Jpeg);

            //filePath = System.IO.Path.Combine(folderPath, "QR.bmp");

            //BarCode.GenerateQR("Hello world!", filePath, Barcoder.Renderer.Image.ImageFormat.Bmp);

            string filePath = System.IO.Path.Combine(folderPath, "Barcode.png");
            BarCode.Generate(BarCode.BarcodeTypes.Code93, "FOO/BAR/12345", filePath);

            BarCode.Read(@"C:\Users\przemyslaw.klys\source\repos\barcodereader-imagesharp\BarcodeReader.ImageSharp.UnitTests\codes\barcode.png");
        }

        private static void Example_QRCode(string folderPath) {
            Console.WriteLine("[*] Creating QR code - JPG");
            string filePath = System.IO.Path.Combine(folderPath, "QRCode1.jpg");
            QrCode.Generate("https://evotec.xyz", filePath);

            Console.WriteLine("[*] Creating QR code - ICO");
            filePath = System.IO.Path.Combine(folderPath, "QRCode1.ico");
            QrCode.Generate("https://evotec.xyz", filePath);

            Console.WriteLine("[*] Creating QR code - PNG (transparent)");
            filePath = System.IO.Path.Combine(folderPath, "QRCode1.png");
            QrCode.Generate("https://evotec.xyz", filePath, true);

            Console.WriteLine("[*] Creating QR WIFI code - PNG (transparent)");
            filePath = System.IO.Path.Combine(folderPath, "QRCodeWifi.png");
            QrCode.GenerateWiFi("myWifi", "password0!A", filePath, true);
        }
    }
}

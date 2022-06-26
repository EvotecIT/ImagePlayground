using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Barcoder.Renderer.Image;
using System.Drawing;
using System.Drawing.Imaging;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace ImagePlayground.ExampleNET {
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

            string filePath = System.IO.Path.Combine(folderPath, "BarcodeEAN7.png");
            BarCode.Generate(BarCode.BarcodeTypes.EAN, "96385074", filePath);



            BarCode.Read(filePath);

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

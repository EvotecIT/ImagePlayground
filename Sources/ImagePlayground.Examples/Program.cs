using System;
using System.IO;

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
            Example_Chart(folderPath);

            Console.WriteLine("\npress any key to exit the process...");

            // basic use of "Console.ReadKey()" method
            Console.ReadKey();

            System.Diagnostics.Process.Start("explorer.exe", folderPath);
        }

        private static void Example_BarCode(string folderPath) {
            //string filePath = System.IO.Path.Combine(folderPath, "QR.png");

            //BarCode.GenerateQR("Hello world!", filePath, Barcoder.Renderer.Image.ImageFormat.Png);

            //filePath = System.IO.Path.Combine(folderPath, "QR.jpg");

            //BarCode.GenerateQR("Hello world!", filePath, Barcoder.Renderer.Image.ImageFormat.Jpeg);

            //filePath = System.IO.Path.Combine(folderPath, "QR.bmp");

            //BarCode.GenerateQR("Hello world!", filePath, Barcoder.Renderer.Image.ImageFormat.Bmp);

            Console.WriteLine("[*] Creating Barcode EAN13 - PNG");
            string filePath = System.IO.Path.Combine(folderPath, "BarcodeEAN13.png");
            BarCode.Generate(BarCode.BarcodeTypes.EAN, "901234123457", filePath);

            Console.WriteLine("[*] Creating Barcode EAN8 - PNG");
            filePath = System.IO.Path.Combine(folderPath, "BarcodeEAN7.png");
            BarCode.Generate(BarCode.BarcodeTypes.EAN, "96385074", filePath);

            Console.WriteLine("[*] Reading Barcode code - PNG: ");
            var read = BarCode.Read(filePath);
            Console.Write(read.Message);

        }

        private static void Example_QRCode(string folderPath) {
            Console.WriteLine("[*] Creating QR code - JPG");
            string filePath = System.IO.Path.Combine(folderPath, "QRCode1.jpg");
            QrCode.Generate("https://evotec.xyz", filePath);

            Console.WriteLine("[*] Reading QR code - JPG: ");
            var read = QrCode.Read(filePath);
            Console.Write(read.Message);
            Console.WriteLine();

            Console.WriteLine("[*] Creating QR code - ICO");
            filePath = System.IO.Path.Combine(folderPath, "QRCode1.ico");
            QrCode.Generate("https://evotec.xyz", filePath);

            Console.WriteLine("[*] Creating QR code - PNG (transparent)");
            filePath = System.IO.Path.Combine(folderPath, "QRCode1.png");
            QrCode.Generate("https://evotec.xyz", filePath, true);

            Console.WriteLine("[*] Reading QR code - PNG: ");
            read = QrCode.Read(filePath);
            Console.Write(read.Message);
            Console.WriteLine();

            Console.WriteLine("[*] Creating QR WIFI code - PNG (transparent)");
            filePath = System.IO.Path.Combine(folderPath, "QRCodeWifi.png");
            QrCode.GenerateWiFi("myWifi", "password0!A", filePath, true);

            Console.WriteLine("[*] Reading QR code (WIFI) - PNG: ");
            read = QrCode.Read(filePath);
            Console.Write(read.Message);
            Console.WriteLine();
        }

        private static void Example_Chart(string folderPath) {
            var plt = new ScottPlot.Plot(600, 400);

            //// create sample data
            //double[] values = { 26, 20, 23, 7, 16 };

            //// add a bar graph to the plot
            //plt.AddBar(values);

            //// adjust axis limits so there is no padding below the bar graph
            //plt.SetAxisLimits(yMin: 0);


            //string filePath = System.IO.Path.Combine(folderPath, "Chart.png");

            //plt.SaveFig(filePath);
        }

    }
}
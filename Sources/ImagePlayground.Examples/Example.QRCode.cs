using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagePlayground.Examples {
    internal partial class Example {
        public static void QRCode(string folderPath) {
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

    }
}

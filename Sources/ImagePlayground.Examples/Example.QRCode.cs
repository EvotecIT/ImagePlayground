using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ImagePlayground.Examples;
internal partial class Example {
    public static void QRCode(string folderPath) {
        Trace.WriteLine("[*] Creating QR code - JPG");
        string filePath = System.IO.Path.Combine(folderPath, "QRCode1.jpg");
        QrCode.Generate("https://evotec.xyz", filePath);

        Trace.WriteLine("[*] Reading QR code - JPG: ");
        var read = QrCode.Read(filePath);
        Console.Write(read.Message);
        Trace.WriteLine(string.Empty);

        Trace.WriteLine("[*] Creating QR code - ICO");
        filePath = System.IO.Path.Combine(folderPath, "QRCode1.ico");
        QrCode.Generate("https://evotec.xyz", filePath);

        Trace.WriteLine("[*] Creating QR code - PNG (transparent)");
        filePath = System.IO.Path.Combine(folderPath, "QRCode1.png");
        QrCode.Generate("https://evotec.xyz", filePath, true);

        Trace.WriteLine("[*] Reading QR code - PNG: ");
        read = QrCode.Read(filePath);
        Console.Write(read.Message);
        Trace.WriteLine(string.Empty);

        Trace.WriteLine("[*] Creating QR WIFI code - PNG (transparent)");
        filePath = System.IO.Path.Combine(folderPath, "QRCodeWifi.png");
        QrCode.GenerateWiFi("myWifi", "password0!A", filePath, true);

        Trace.WriteLine("[*] Reading QR code (WIFI) - PNG: ");
        read = QrCode.Read(filePath);
        Console.Write(read.Message);
        Trace.WriteLine(string.Empty);
    }

}

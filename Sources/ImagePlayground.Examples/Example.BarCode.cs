using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ImagePlayground.Examples;

internal partial class Example {
    public static void BarCodes1(string folderPath) {
        //string filePath = System.IO.Path.Combine(folderPath, "QR.png");

        //BarCode.GenerateQR("Hello world!", filePath, Barcoder.Renderer.Image.ImageFormat.Png);

        //filePath = System.IO.Path.Combine(folderPath, "QR.jpg");

        //BarCode.GenerateQR("Hello world!", filePath, Barcoder.Renderer.Image.ImageFormat.Jpeg);

        //filePath = System.IO.Path.Combine(folderPath, "QR.bmp");

        //BarCode.GenerateQR("Hello world!", filePath, Barcoder.Renderer.Image.ImageFormat.Bmp);

        Trace.WriteLine("[*] Creating Barcode EAN13 - PNG");
        string filePath = System.IO.Path.Combine(folderPath, "BarcodeEAN13.png");
        BarCode.Generate(BarCode.BarcodeTypes.EAN, "901234123457", filePath);

        Trace.WriteLine("[*] Creating Barcode EAN8 - PNG");
        filePath = System.IO.Path.Combine(folderPath, "BarcodeEAN7.png");
        BarCode.Generate(BarCode.BarcodeTypes.EAN, "96385074", filePath);

        Trace.WriteLine("[*] Reading Barcode code - PNG: ");
        var read = BarCode.Read(filePath);
        Console.Write(read.Message);
    }

    public static void DataMatrixSample(string folderPath) {
        Trace.WriteLine("[*] Creating Data Matrix barcode - PNG");
        string filePath = System.IO.Path.Combine(folderPath, "DataMatrix.png");
        BarCode.Generate(BarCode.BarcodeTypes.DataMatrix, "DataMatrixExample", filePath);

        Trace.WriteLine("[*] Reading Data Matrix barcode:");
        var read = BarCode.Read(filePath);
        Trace.WriteLine(read.Message);
    }

    public static void Pdf417Sample(string folderPath) {
        Trace.WriteLine("[*] Creating PDF417 barcode - PNG");
        string filePath = System.IO.Path.Combine(folderPath, "Pdf417.png");
        BarCode.Generate(BarCode.BarcodeTypes.PDF417, "Pdf417Example", filePath);

        Trace.WriteLine("[*] Reading PDF417 barcode:");
        var read = BarCode.Read(filePath);
        Trace.WriteLine(read.Message);
    }
}


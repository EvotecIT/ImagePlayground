﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagePlayground.Examples {
    internal partial class Example {
        public static void BarCodes1(string folderPath) {
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
    }
}

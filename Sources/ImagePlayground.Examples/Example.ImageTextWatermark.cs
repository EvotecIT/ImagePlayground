using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;

namespace ImagePlayground.Examples;
internal partial class Example {
    public static void ImageTextWatermark(string folderPath) {
        Console.WriteLine("[*] Creating Text Watermark");
        string filePath = System.IO.Path.Combine(folderPath, "PrzemyslawKlysAndKulkozaurr.jpg");
        string waterMark = System.IO.Path.Combine(folderPath, "LogoEvotec.png");
        string targetPath = System.IO.Path.Combine(folderPath, "PrzemyslawKlysAndKulkozaurr_TextWatermark.png");
        using (var image = Image.Load(filePath)) {
            // image.Watermark("Evotec Example", Color.Red, 10, 0, 15, 30);
            image.Watermark("Evotec Services Bottom Left", Image.WatermarkPlacement.BottomLeft, Color.DarkCyan);
            image.Watermark("Evotec Services Bottom Right", Image.WatermarkPlacement.BottomRight, Color.DarkCyan);
            image.Watermark("Evotec Services Top Left", Image.WatermarkPlacement.TopLeft, Color.DarkCyan);
            image.Watermark("Evotec Services Top Right", Image.WatermarkPlacement.TopRight, Color.DarkCyan);
            //image.Watermark("Evotec Services Middle", Image.WatermarkPlacement.Middle, Color.DarkCyan);

            image.WatermarkImage(waterMark, Image.WatermarkPlacement.Middle, 0.5f);

            image.Save(targetPath);
        }
    }


    public static void ImageBGInfo(string folderPath) {
        Console.WriteLine("[*] Creating Text Watermark");
        string filePath = System.IO.Path.Combine(folderPath, "PrzemyslawKlysAndKulkozaurr.jpg");
        string targetPath = System.IO.Path.Combine(folderPath, "PrzemyslawKlysAndKulkozaurr_BGInfo.png");
        using (var image = Image.Load(filePath)) {
            // image.Watermark("Evotec Example", Color.Red, 10, 0, 15, 30);
            image.AddText(500, 100, "Host Name: ", Color.DarkCyan, 20);
            image.AddText(800, 100, Environment.MachineName, Color.DarkCyan, 20);

            // get uptime
            image.AddText(500, 140, "Boot Time: ", Color.DarkCyan, 20);
            var bootTime = System.Diagnostics.Process.GetCurrentProcess().StartTime;
            image.AddText(800, 140, bootTime.ToString(), Color.DarkCyan, 20);
            // get cpu type 
            image.AddText(500, 180, "CPU Type: ", Color.DarkCyan, 20);
            image.AddText(800, 180, Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER"), Color.DarkCyan, 30);

            image.Save(targetPath);
        }
    }
}
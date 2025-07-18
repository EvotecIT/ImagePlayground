﻿using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;

namespace ImagePlayground.Examples;
internal partial class Example {
    /// <summary>
    /// Demonstrates applying text and image watermarks.
    /// </summary>
    /// <param name="folderPath">Folder containing images for watermarking.</param>
    public static void ImageTextWatermark(string folderPath) {
        Console.WriteLine("[*] Creating Text Watermark");
        string filePath = System.IO.Path.Combine(folderPath, "PrzemyslawKlysAndKulkozaurr.jpg");
        string waterMark = System.IO.Path.Combine(folderPath, "LogoEvotec.png");
        string targetPath = System.IO.Path.Combine(folderPath, "PrzemyslawKlysAndKulkozaurr_TextWatermark.png");
        using (var image = Image.Load(filePath)) {
            // image.Watermark("Evotec Example", Color.Red, 10, 0, 15, 30);
            image.Watermark("Evotec Services Bottom Left", WatermarkPlacement.BottomLeft, Color.DarkCyan);
            image.Watermark("Evotec Services Bottom Right", WatermarkPlacement.BottomRight, Color.DarkCyan);
            image.Watermark("Evotec Services Top Left", WatermarkPlacement.TopLeft, Color.DarkCyan);
            image.Watermark("Evotec Services Top Right", WatermarkPlacement.TopRight, Color.DarkCyan);
            //image.Watermark("Evotec Services Middle", Image.WatermarkPlacement.Middle, Color.DarkCyan);

            image.WatermarkImage(waterMark, WatermarkPlacement.Middle, 0.5f);

            image.Save(targetPath);
        }
    }


    /// <summary>
    /// Adds basic system information as text overlay.
    /// </summary>
    /// <param name="folderPath">Folder containing the source image.</param>
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
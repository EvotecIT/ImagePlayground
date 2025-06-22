using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Diagnostics;

namespace ImagePlayground.Examples;
internal partial class Example {
    public static void ImageModifications1(string folderPath) {
        Trace.WriteLine("[*] Manipulating Image - JPG");
        string filePath = System.IO.Path.Combine(folderPath, "LogoEvotec.png");
        string targetPath = System.IO.Path.Combine(folderPath, "LogoEvotec_Flipped.png");
        using (var image = Image.Load(filePath)) {
            Trace.WriteLine("[+] Flipping vertically");
            image.Flip(FlipMode.Vertical);
            Trace.WriteLine("[+] Resizing 1000x1000");
            image.Resize(1000, 1000);
            Trace.WriteLine("[+] Changing background color");
            image.BackgroundColor(Color.Red);
            image.Save(targetPath);
        }

        Trace.WriteLine("[+] In use: " + Helpers.IsFileLocked(filePath));
        Trace.WriteLine("[+] In use: " + Helpers.IsFileLocked(targetPath));
    }

    public static void ImageModifications2(string folderPath) {
        Trace.WriteLine("[*] Manipulating Image - JPG");
        string filePath = System.IO.Path.Combine(folderPath, "PrzemyslawKlysAndKulkozaurr.jpg");
        string targetPath = System.IO.Path.Combine(folderPath, "PrzemyslawKlysAndKulkozaurr_GrayScale.png");
        using (var image = Image.Load(filePath)) {
            image.Grayscale(GrayscaleMode.Bt709);
            image.Save(targetPath);
        }

        targetPath = System.IO.Path.Combine(folderPath, "PrzemyslawKlysAndKulkozaurr_Contrast.png");
        using (var image = Image.Load(filePath)) {
            image.Contrast(50);
            image.Save(targetPath);
        }

        targetPath = System.IO.Path.Combine(folderPath, "PrzemyslawKlysAndKulkozaurr_BW.png");
        using (var image = Image.Load(filePath)) {
            image.BlackWhite();
            image.Save(targetPath);
        }

        targetPath = System.IO.Path.Combine(folderPath, "PrzemyslawKlysAndKulkozaurr_BokehBlur.png");
        using (var image = Image.Load(filePath)) {
            image.BokehBlur();
            image.Save(targetPath);
        }

        targetPath = System.IO.Path.Combine(folderPath, "PrzemyslawKlysAndKulkozaurr_AdaptiveThreshold.png");
        using (var image = Image.Load(filePath)) {
            image.AdaptiveThreshold();
            image.Save(targetPath);
        }

        targetPath = System.IO.Path.Combine(folderPath, "PrzemyslawKlysAndKulkozaurr_BoxBlur.png");
        using (var image = Image.Load(filePath)) {
            image.BoxBlur();
            image.Save(targetPath);
        }

        targetPath = System.IO.Path.Combine(folderPath, "PrzemyslawKlysAndKulkozaurr_Watermark.png");
        using (var image = Image.Load(filePath)) {
            // image.Watermark("Evotec Example", Color.Red, 10, 0, 15, 30);
            //image.Watermark();
            image.Save(targetPath);
        }


        Trace.WriteLine("[+] In use: " + Helpers.IsFileLocked(filePath));
        Trace.WriteLine("[+] In use: " + Helpers.IsFileLocked(targetPath));
    }
}
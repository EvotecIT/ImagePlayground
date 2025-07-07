using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;

namespace ImagePlayground.Examples;
internal partial class Example {
    /// <summary>
    /// Applies several modifications to demonstrate basic image processing.
    /// </summary>
    /// <param name="folderPath">Folder containing the source image.</param>
    public static void ImageModifications1(string folderPath) {
        Console.WriteLine("[*] Manipulating Image - JPG");
        string filePath = System.IO.Path.Combine(folderPath, "LogoEvotec.png");
        string targetPath = System.IO.Path.Combine(folderPath, "LogoEvotec_Flipped.png");
        using (var image = Image.Load(filePath)) {
            Console.WriteLine("[+] Flipping vertically");
            image.Flip(FlipMode.Vertical);
            Console.WriteLine("[+] Resizing 1000x1000");
            image.Resize(1000, 1000);
            Console.WriteLine("[+] Changing background color");
            image.BackgroundColor(Color.Red);
            image.Save(targetPath);
        }

        Console.WriteLine("[+] In use: " + Helpers.IsFileLocked(filePath));
        Console.WriteLine("[+] In use: " + Helpers.IsFileLocked(targetPath));
    }

    /// <summary>
    /// Shows additional image manipulation examples.
    /// </summary>
    /// <param name="folderPath">Folder containing the sample image.</param>
    public static void ImageModifications2(string folderPath) {
        Console.WriteLine("[*] Manipulating Image - JPG");
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


        Console.WriteLine("[+] In use: " + Helpers.IsFileLocked(filePath));
        Console.WriteLine("[+] In use: " + Helpers.IsFileLocked(targetPath));
    }
}
using System;

namespace ImagePlayground.Examples;
internal partial class Example {
    /// <summary>
    /// Demonstrates blur and sharpen helpers.
    /// </summary>
    /// <param name="folderPath">Folder containing the sample image.</param>
    public static void BlurAndSharpen(string folderPath) {
        Console.WriteLine("[*] Blurring and sharpening");
        string src = System.IO.Path.Combine(folderPath, "LogoEvotec.png");
        string blur = System.IO.Path.Combine(folderPath, "LogoEvotec_Blur.png");
        string sharp = System.IO.Path.Combine(folderPath, "LogoEvotec_Sharp.png");
        ImageHelper.Blur(src, blur, 5);
        ImageHelper.Sharpen(src, sharp, 2);
    }
}

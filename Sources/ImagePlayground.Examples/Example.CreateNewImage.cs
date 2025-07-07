using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagePlayground.Examples;
internal partial class Example {
    /// <summary>
    /// Creates blank images with predefined resolutions.
    /// </summary>
    /// <param name="folderPath">Folder where images will be created.</param>
    public static void CreateNewImage(string folderPath) {
        Console.WriteLine("[*] Creating new image");
        string filePath1 = System.IO.Path.Combine(folderPath, "NewImage_3840x2160.jpg");
        ImageHelper.Create(filePath1, 3840, 2160, Color.Black, false);

        string filePath2 = System.IO.Path.Combine(folderPath, "NewImage_1920x1080.jpg");
        ImageHelper.Create(filePath2, 1920, 1080, Color.Black, false);
    }

}

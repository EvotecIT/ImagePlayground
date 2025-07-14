using System;
using System.IO;

namespace ImagePlayground.Examples;
internal partial class Example {
    /// <summary>
    /// Demonstrates exporting and importing metadata on an image.
    /// </summary>
    /// <param name="folderPath">Folder containing the sample image.</param>
    public static void MetadataRoundTrip(string folderPath) {
        Console.WriteLine("[*] Exporting metadata");
        string filePath = Path.Combine(folderPath, "KulekWSluchawkach.jpg");
        string metaPath = Path.Combine(folderPath, "KulekWSluchawkach.json");
        ImageHelper.ExportMetadata(filePath, metaPath);

        Console.WriteLine("[*] Importing metadata back");
        var options = new ImageHelper.ImportMetadataOptions(filePath, metaPath, filePath);
        ImageHelper.ImportMetadata(options);
    }
}
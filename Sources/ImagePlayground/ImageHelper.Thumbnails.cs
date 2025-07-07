using System;
using System.IO;
using SixLabors.ImageSharp;

namespace ImagePlayground;
/// <summary>
/// Provides helper methods for image manipulation.
/// </summary>
public partial class ImageHelper {
    /// <summary>
    /// Generates resized copies for all images found in <paramref name="directoryPath"/>.
    /// </summary>
    /// <param name="directoryPath">Folder containing the source images.</param>
    /// <param name="outputDirectory">Destination folder for the thumbnails.</param>
    /// <param name="width">Thumbnail width.</param>
    /// <param name="height">Thumbnail height.</param>
    /// <param name="keepAspectRatio">Whether to maintain aspect ratio.</param>
    /// <param name="sampler">Optional sampler algorithm.</param>
    public static void GenerateThumbnails(string directoryPath, string outputDirectory, int width, int height, bool keepAspectRatio = true, Image.Sampler? sampler = null) {
        string inputDir = Helpers.ResolvePath(directoryPath);
        string outDir = Helpers.ResolvePath(outputDirectory);

        if (!Directory.Exists(inputDir)) {
            throw new DirectoryNotFoundException($"Input directory not found: {directoryPath}");
        }

        if (!Directory.Exists(outDir)) {
            Directory.CreateDirectory(outDir);
        }

        foreach (var file in Directory.EnumerateFiles(inputDir)) {
            try {
                using var inStream = File.OpenRead(file);
                using SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(inStream);
                Resize(image, width, height, keepAspectRatio, sampler);
                string destPath = Path.Combine(outDir, Path.GetFileName(file));
                try {
                    image.Save(destPath);
                } catch (Exception ex) when (ex is SixLabors.ImageSharp.UnknownImageFormatException || ex is NotSupportedException) {
                    File.Copy(file, destPath, true);
                }
            } catch (SixLabors.ImageSharp.UnknownImageFormatException) {
            }
        }
    }
}

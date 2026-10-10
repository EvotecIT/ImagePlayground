using System;
using System.IO;

namespace ImagePlayground;
/// <summary>
/// Provides helper methods for image manipulation.
/// </summary>
public partial class ImageHelper {
    /// <summary>
    /// Generates resized copies for all images found in <paramref name="directoryPath"/>.
    /// </summary>
    /// <remarks>Decodable inputs whose extension has no encoder are copied unchanged. Invalid image inputs are skipped; failures while resizing or encoding a supported output are propagated.</remarks>
    /// <param name="directoryPath">Folder containing the source images.</param>
    /// <param name="outputDirectory">Destination folder for the thumbnails.</param>
    /// <param name="width">Thumbnail width.</param>
    /// <param name="height">Thumbnail height.</param>
    /// <param name="keepAspectRatio">Whether to maintain aspect ratio.</param>
    /// <param name="sampler">Optional sampler algorithm.</param>
    public static void GenerateThumbnails(string directoryPath, string outputDirectory, int width, int height, bool keepAspectRatio = true, Sampler? sampler = null) {
        string inputDir = Helpers.ResolvePath(directoryPath);
        string outDir = Helpers.ResolvePath(outputDirectory);

        if (!Directory.Exists(inputDir)) {
            throw new DirectoryNotFoundException($"Input directory not found: {directoryPath}");
        }

        if (!Directory.Exists(outDir)) {
            Directory.CreateDirectory(outDir);
        }

        foreach (var file in Directory.EnumerateFiles(inputDir)) {
            string destPath = Path.Combine(outDir, Path.GetFileName(file));
            bool canEncode = true;
            try {
                Helpers.GetImageType(Path.GetExtension(destPath));
            } catch (NotSupportedException) {
                canEncode = false;
            }
            Image image;
            try {
                image = Image.Load(file);
            } catch (InvalidDataException) {
                continue;
            }
            using (image) {
                if (!canEncode) {
                    File.Copy(file, destPath, true);
                    continue;
                }
                image.Resize(width, height, keepAspectRatio, sampler);
                image.Save(destPath);
            }
        }
    }
}
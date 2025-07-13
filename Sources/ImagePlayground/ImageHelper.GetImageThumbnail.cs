using System;
using System.IO;
using System.Security.Cryptography;
using SixLabors.ImageSharp;

namespace ImagePlayground;
/// <summary>
/// Provides helper methods for image manipulation.
/// </summary>
/// <remarks>
/// Thumbnails are cached in the user's temporary directory for quick retrieval.
/// </remarks>
public partial class ImageHelper {
    private static string GetThumbnailCacheDirectory() {
        string dir = Path.Combine(Path.GetTempPath(), "ImagePlayground", "thumbnails");
        if (!Directory.Exists(dir)) {
            Directory.CreateDirectory(dir);
        }
        return dir;
    }

    private static string ComputeFileHash(string path) {
        using var stream = File.OpenRead(path);
        using var sha = SHA256.Create();
        byte[] hash = sha.ComputeHash(stream);
        return BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();
    }

    /// <summary>
    /// Gets a thumbnail for the specified image. The thumbnail is cached based on the image hash.
    /// </summary>
    /// <param name="filePath">Path to the image file.</param>
    /// <param name="width">Thumbnail width.</param>
    /// <param name="height">Thumbnail height.</param>
    /// <param name="keepAspectRatio">Whether to maintain aspect ratio.</param>
    /// <param name="sampler">Optional sampler algorithm.</param>
    /// <returns>Thumbnail image.</returns>
    public static Image GetImageThumbnail(string filePath, int width, int height, bool keepAspectRatio = true, Image.Sampler? sampler = null) {
        string fullPath = Helpers.ResolvePath(filePath);
        string hash = ComputeFileHash(fullPath);
        string cacheDir = GetThumbnailCacheDirectory();
        string thumbPath = Path.Combine(cacheDir, $"{hash}_{width}x{height}.png");

        if (!File.Exists(thumbPath)) {
            Resize(fullPath, thumbPath, width, height, keepAspectRatio, sampler);
        }

        return Image.Load(thumbPath);
    }

    /// <summary>
    /// Removes all cached thumbnails.
    /// </summary>
    public static void ClearThumbnailCache() {
        string cacheDir = Path.Combine(Path.GetTempPath(), "ImagePlayground", "thumbnails");
        if (Directory.Exists(cacheDir)) {
            Directory.Delete(cacheDir, true);
        }
    }
}

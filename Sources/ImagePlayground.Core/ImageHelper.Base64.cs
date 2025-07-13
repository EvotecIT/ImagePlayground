using System;
using System.IO;

namespace ImagePlayground;
/// <summary>
/// Provides helper methods for image manipulation.
/// </summary>
public partial class ImageHelper {
    /// <summary>
    /// Reads an image file and returns its Base64 representation.
    /// </summary>
    /// <param name="filePath">Path to the source image.</param>
    /// <returns>Base64 encoded string.</returns>
    public static string ConvertToBase64(string filePath) {
        string fullPath = Helpers.ResolvePath(filePath);
        using var stream = File.OpenRead(fullPath);
        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        return Convert.ToBase64String(ms.ToArray());
    }

    /// <summary>
    /// Writes a Base64 encoded image to disk.
    /// </summary>
    /// <param name="base64">Base64 encoded image content.</param>
    /// <param name="outFilePath">Destination file path.</param>
    public static void ConvertFromBase64(string base64, string outFilePath) {
        string outFullPath = Helpers.ResolvePath(outFilePath);
        var bytes = Convert.FromBase64String(base64);
        File.WriteAllBytes(outFullPath, bytes);
    }
}

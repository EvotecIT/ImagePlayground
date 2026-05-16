using System;
using System.IO;

namespace ImagePlayground;

/// <summary>
/// Provides helper methods for converting images to and from Base64 strings.
/// </summary>
public partial class ImageHelper {
    /// <summary>
    /// Reads an image file and returns its Base64 representation.
    /// </summary>
    /// <param name="filePath">Path to the source image.</param>
    /// <returns>Base64 encoded string.</returns>
    /// <example>
    /// <code>string base64 = ImageHelper.ConvertToBase64("input.png");</code>
    /// </example>
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
    /// <example>
    /// <code>ImageHelper.ConvertFromBase64(base64String, "output.png");</code>
    /// </example>
    public static void ConvertFromBase64(string base64, string outFilePath) {
        string outFullPath = Helpers.ResolvePath(outFilePath);
        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outFullPath)!);
        var bytes = Convert.FromBase64String(base64);
        File.WriteAllBytes(outFullPath, bytes);
    }
}

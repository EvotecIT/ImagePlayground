using System;
using System.Diagnostics;
using System.IO;

namespace ImagePlayground;

/// <summary>
/// General helper utilities for the library.
/// </summary>
public static partial class Helpers {
    /// <summary>
    /// List of supported file extensions for image encoding.
    /// </summary>
    public static readonly string[] SupportedExtensions = new[] {
        ".png",
        ".jpg",
        ".jpeg",
        ".bmp",
        ".gif",
        ".pbm",
        ".tga",
        ".tiff",
        ".webp"
    };
    /// <summary>
    /// Converts a <see cref="SixLabors.ImageSharp.Color"/> to a 6 character hex string.
    /// </summary>
    /// <param name="c">Color value to convert.</param>
    /// <returns>Hex string without alpha component.</returns>
    public static string ToHexColor(this SixLabors.ImageSharp.Color c) {
        return c.ToHex().Remove(6);
    }

    /// <summary>
    /// Opens the specified file using the OS default application if <paramref name="open"/> is <c>true</c>.
    /// </summary>
    /// <param name="filePath">Path to the file.</param>
    /// <param name="open">Whether to open the file.</param>
    public static void Open(string filePath, bool open) {
        if (!open) {
            return;
        }

        if (!File.Exists(filePath)) {
            Console.Error.WriteLine($"Unable to open {filePath}: file does not exist.");
            return;
        }

        ProcessStartInfo startInfo = new(filePath) {
            UseShellExecute = true
        };

        try {
            Process.Start(startInfo);
        } catch (Exception ex) {
            Console.Error.WriteLine($"Unable to open {filePath}: {ex.Message}");
        }
    }

    /// <summary>
    /// Checks whether the specified file is locked by another process.
    /// </summary>
    /// <param name="file">File to test.</param>
    /// <returns><c>true</c> if the file cannot be opened exclusively.</returns>
    public static bool IsFileLocked(this FileInfo file) {
        try {
            using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None)) {
                stream.Close();
            }
        } catch (IOException) {
            //the file is unavailable because it is:
            //still being written to
            //or being processed by another thread
            //or does not exist (has already been processed)
            return true;
        }

        //file is not locked
        return false;
    }

    /// <summary>
    /// Determines if the file specified by path is locked by another process.
    /// </summary>
    /// <param name="fileName">Path to the file.</param>
    /// <returns><c>true</c> if the file cannot be opened exclusively.</returns>
    public static bool IsFileLocked(this string fileName) {
        try {
            var file = new FileInfo(fileName);
            using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None)) {
                stream.Close();
            }
        } catch (IOException) {
            //the file is unavailable because it is:
            //still being written to
            //or being processed by another thread
            //or does not exist (has already been processed)
            return true;
        }

        //file is not locked
        return false;
    }
}

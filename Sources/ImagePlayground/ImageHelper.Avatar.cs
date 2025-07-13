using System.IO;
using SixLabors.ImageSharp;

namespace ImagePlayground;
/// <summary>
/// Provides helper methods for image manipulation.
/// </summary>
/// <remarks>
/// Avatar helpers simplify creating square cropped images with rounded corners.
/// </remarks>
public partial class ImageHelper {
    /// <summary>
    /// Converts an image file to an avatar and saves the result.
    /// </summary>
    /// <param name="filePath">Source image path.</param>
    /// <param name="outFilePath">Destination file path.</param>
    /// <param name="width">Width of the avatar.</param>
    /// <param name="height">Height of the avatar.</param>
    /// <param name="cornerRadius">Radius of the rounded corners.</param>
    public static void Avatar(string filePath, string outFilePath, int width, int height, float cornerRadius) {
        string fullPath = Helpers.ResolvePath(filePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);

        using var img = Image.Load(fullPath);
        img.SaveAsAvatar(outFullPath, width, height, cornerRadius);
    }

    /// <summary>
    /// Converts an image file to an avatar and writes it to a stream.
    /// </summary>
    /// <param name="filePath">Source image path.</param>
    /// <param name="outStream">Destination stream.</param>
    /// <param name="width">Width of the avatar.</param>
    /// <param name="height">Height of the avatar.</param>
    /// <param name="cornerRadius">Radius of the rounded corners.</param>
    public static void Avatar(string filePath, Stream outStream, int width, int height, float cornerRadius) {
        string fullPath = Helpers.ResolvePath(filePath);

        using var img = Image.Load(fullPath);
        img.SaveAsAvatar(outStream, width, height, cornerRadius);
    }

    /// <summary>
    /// Converts an image file to a circular avatar and saves the result.
    /// </summary>
    /// <param name="filePath">Source image path.</param>
    /// <param name="outFilePath">Destination file path.</param>
    /// <param name="size">Diameter of the avatar.</param>
    public static void AvatarCircular(string filePath, string outFilePath, int size) {
        string fullPath = Helpers.ResolvePath(filePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);

        using var img = Image.Load(fullPath);
        img.SaveAsCircularAvatar(outFullPath, size);
    }

    /// <summary>
    /// Converts an image file to a circular avatar and writes it to a stream.
    /// </summary>
    /// <param name="filePath">Source image path.</param>
    /// <param name="outStream">Destination stream.</param>
    /// <param name="size">Diameter of the avatar.</param>
    public static void AvatarCircular(string filePath, Stream outStream, int size) {
        string fullPath = Helpers.ResolvePath(filePath);

        using var img = Image.Load(fullPath);
        img.SaveAsCircularAvatar(outStream, size);
    }
}

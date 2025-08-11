using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ImagePlayground;
/// <summary>
/// Provides helper methods for image manipulation.
/// </summary>
public partial class ImageHelper {
    /// <summary>
    /// Rotates an image using a <see cref="RotateMode"/>.
    /// </summary>
    /// <param name="filePath">Source image path.</param>
    /// <param name="outFilePath">Destination image path.</param>
    /// <param name="rotateMode">Rotation mode.</param>
    public static void Rotate(string filePath, string outFilePath, RotateMode rotateMode) {
        string fullPath = Helpers.ResolvePath(filePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);
        Directory.CreateDirectory(Path.GetDirectoryName(outFullPath)!);
        using var img = Image.Load(fullPath);
        img.Rotate(rotateMode);
        img.Save(outFullPath);
    }

    /// <summary>
    /// Rotates an image by an arbitrary number of <paramref name="degrees"/>.
    /// </summary>
    /// <param name="filePath">Source image path.</param>
    /// <param name="outFilePath">Destination image path.</param>
    /// <param name="degrees">Angle in degrees.</param>
    public static void Rotate(string filePath, string outFilePath, float degrees) {
        string fullPath = Helpers.ResolvePath(filePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);
        Directory.CreateDirectory(Path.GetDirectoryName(outFullPath)!);
        using var img = Image.Load(fullPath);
        img.Rotate(degrees);
        img.Save(outFullPath);
    }

    /// <summary>
    /// Asynchronously rotates an image using a <see cref="RotateMode"/>.
    /// </summary>
    public static async Task RotateAsync(string filePath, string outFilePath, RotateMode rotateMode) {
        string fullPath = Helpers.ResolvePath(filePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);
        Directory.CreateDirectory(Path.GetDirectoryName(outFullPath)!);
        await Task.Run(() => {
            using var img = Image.Load(fullPath);
            img.Rotate(rotateMode);
            img.Save(outFullPath);
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously rotates an image by an arbitrary number of <paramref name="degrees"/>.
    /// </summary>
    public static async Task RotateAsync(string filePath, string outFilePath, float degrees) {
        string fullPath = Helpers.ResolvePath(filePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);
        Directory.CreateDirectory(Path.GetDirectoryName(outFullPath)!);
        await Task.Run(() => {
            using var img = Image.Load(fullPath);
            img.Rotate(degrees);
            img.Save(outFullPath);
        }).ConfigureAwait(false);
    }
}

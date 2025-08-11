using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp;

namespace ImagePlayground;
/// <summary>
/// Provides helper methods for image manipulation.
/// </summary>
public partial class ImageHelper {
    /// <summary>
    /// Sharpens an image using Gaussian algorithm and saves the result.
    /// </summary>
    /// <param name="filePath">Source image path.</param>
    /// <param name="outFilePath">Destination image path.</param>
    /// <param name="sigma">Sharpen strength.</param>
    public static void Sharpen(string filePath, string outFilePath, float? sigma = null) {
        string fullPath = Helpers.ResolvePath(filePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);
        Directory.CreateDirectory(Path.GetDirectoryName(outFullPath)!);
        using var img = Image.Load(fullPath);
        img.GaussianSharpen(sigma);
        img.Save(outFullPath);
    }

    /// <summary>
    /// Asynchronously sharpens an image using Gaussian algorithm and saves the result.
    /// </summary>
    public static async Task SharpenAsync(string filePath, string outFilePath, float? sigma = null) {
        string fullPath = Helpers.ResolvePath(filePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);
        Directory.CreateDirectory(Path.GetDirectoryName(outFullPath)!);
        await Task.Run(() => {
            using var img = Image.Load(fullPath);
            img.GaussianSharpen(sigma);
            img.Save(outFullPath);
        }).ConfigureAwait(false);
    }
}
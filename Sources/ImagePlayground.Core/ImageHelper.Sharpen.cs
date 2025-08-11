using System;
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
    /// Applies a Gaussian sharpen filter to an image and saves the result.
    /// </summary>
    /// <param name="filePath">Source image path.</param>
    /// <param name="outFilePath">Destination image path.</param>
    /// <param name="amount">Sharpen amount.</param>
    public static void Sharpen(string filePath, string outFilePath, float amount) {
        if (amount <= 0) {
            throw new ArgumentOutOfRangeException(nameof(amount));
        }
        string fullPath = Helpers.ResolvePath(filePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);
        Directory.CreateDirectory(Path.GetDirectoryName(outFullPath)!);
        using var img = Image.Load(fullPath);
        img.GaussianSharpen(amount);
        img.Save(outFullPath);
    }

    /// <summary>
    /// Asynchronously applies a Gaussian sharpen filter to an image and saves the result.
    /// </summary>
    public static async Task SharpenAsync(string filePath, string outFilePath, float amount) {
        if (amount <= 0) {
            throw new ArgumentOutOfRangeException(nameof(amount));
        }
        string fullPath = Helpers.ResolvePath(filePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);
        Directory.CreateDirectory(Path.GetDirectoryName(outFullPath)!);
        await Task.Run(() => {
            using var img = Image.Load(fullPath);
            img.GaussianSharpen(amount);
            img.Save(outFullPath);
        }).ConfigureAwait(false);
    }
}

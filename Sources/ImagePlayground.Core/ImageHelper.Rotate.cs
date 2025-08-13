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
    public static async Task RotateAsync(string filePath, string outFilePath, RotateMode rotateMode, CancellationToken cancellationToken = default) {
        cancellationToken.ThrowIfCancellationRequested();
        string fullPath = Helpers.ResolvePath(filePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);
        Directory.CreateDirectory(Path.GetDirectoryName(outFullPath)!);
        using SixLabors.ImageSharp.Image img = await SixLabors.ImageSharp.Image.LoadAsync(fullPath, cancellationToken).ConfigureAwait(false);
        img.Mutate(x => x.Rotate(rotateMode));
        await img.SaveAsync(outFullPath, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously rotates an image by an arbitrary number of <paramref name="degrees"/>.
    /// </summary>
    public static async Task RotateAsync(string filePath, string outFilePath, float degrees, CancellationToken cancellationToken = default) {
        cancellationToken.ThrowIfCancellationRequested();
        string fullPath = Helpers.ResolvePath(filePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);
        Directory.CreateDirectory(Path.GetDirectoryName(outFullPath)!);
        using SixLabors.ImageSharp.Image img = await SixLabors.ImageSharp.Image.LoadAsync(fullPath, cancellationToken).ConfigureAwait(false);
        img.Mutate(x => x.Rotate(degrees));
        await img.SaveAsync(outFullPath, cancellationToken).ConfigureAwait(false);
    }
}

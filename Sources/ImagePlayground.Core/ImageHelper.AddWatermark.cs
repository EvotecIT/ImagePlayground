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
    /// Adds an image watermark at one of the predefined placements.
    /// </summary>
    /// <param name="filePath">Source image path.</param>
    /// <param name="outFilePath">Destination path.</param>
    /// <param name="watermarkFilePath">Watermark image path.</param>
    /// <param name="placement">Placement of the watermark.</param>
    /// <param name="opacity">Opacity of the watermark.</param>
    /// <param name="padding">Padding around the watermark.</param>
    /// <param name="rotate">Rotation in degrees.</param>
    /// <param name="flipMode">Flip mode applied to watermark.</param>
    /// <param name="watermarkPercentage">Size of the watermark relative to the base image.</param>
    public static void WatermarkImage(string filePath, string outFilePath, string watermarkFilePath, WatermarkPlacement placement, float opacity = 1f, float padding = 18f, int rotate = 0, FlipMode flipMode = FlipMode.None, int watermarkPercentage = 20) {
        string fullPath = Helpers.ResolvePath(filePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);
        using var img = Image.Load(fullPath);
        img.WatermarkImage(watermarkFilePath, placement, opacity, padding, rotate, flipMode, watermarkPercentage);
        img.Save(outFullPath);
    }

    /// <summary>
    /// Asynchronously adds an image watermark at one of the predefined placements.
    /// </summary>
    public static async Task WatermarkImageAsync(string filePath, string outFilePath, string watermarkFilePath, WatermarkPlacement placement, float opacity = 1f, float padding = 18f, int rotate = 0, FlipMode flipMode = FlipMode.None, int watermarkPercentage = 20) {
        string fullPath = Helpers.ResolvePath(filePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);
        await Task.Run(() => {
            using var img = Image.Load(fullPath);
            img.WatermarkImage(watermarkFilePath, placement, opacity, padding, rotate, flipMode, watermarkPercentage);
            img.Save(outFullPath);
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Adds an image watermark at exact coordinates.
    /// </summary>
    /// <param name="filePath">Source image path.</param>
    /// <param name="outFilePath">Destination path.</param>
    /// <param name="watermarkFilePath">Watermark image path.</param>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    /// <param name="opacity">Opacity of the watermark.</param>
    /// <param name="rotate">Rotation in degrees.</param>
    /// <param name="flipMode">Flip mode applied to watermark.</param>
    /// <param name="watermarkPercentage">Size of the watermark relative to the base image.</param>
    public static void WatermarkImage(string filePath, string outFilePath, string watermarkFilePath, int x, int y, float opacity = 1f, int rotate = 0, FlipMode flipMode = FlipMode.None, int watermarkPercentage = 20) {
        string fullPath = Helpers.ResolvePath(filePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);
        using var img = Image.Load(fullPath);
        img.WatermarkImage(watermarkFilePath, x, y, opacity, rotate, flipMode, watermarkPercentage);
        img.Save(outFullPath);
    }

    /// <summary>
    /// Asynchronously adds an image watermark at exact coordinates.
    /// </summary>
    public static async Task WatermarkImageAsync(string filePath, string outFilePath, string watermarkFilePath, int x, int y, float opacity = 1f, int rotate = 0, FlipMode flipMode = FlipMode.None, int watermarkPercentage = 20) {
        string fullPath = Helpers.ResolvePath(filePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);
        await Task.Run(() => {
            using var img = Image.Load(fullPath);
            img.WatermarkImage(watermarkFilePath, x, y, opacity, rotate, flipMode, watermarkPercentage);
            img.Save(outFullPath);
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Tiles the watermark image across the target image.
    /// </summary>
    /// <param name="filePath">Source image path.</param>
    /// <param name="outFilePath">Destination path.</param>
    /// <param name="watermarkFilePath">Watermark image path.</param>
    /// <param name="spacing">Spacing between watermark tiles.</param>
    /// <param name="opacity">Opacity of the watermark.</param>
    /// <param name="rotate">Rotation in degrees.</param>
    /// <param name="flipMode">Flip mode applied to watermark.</param>
    /// <param name="watermarkPercentage">Size of each watermark relative to the base image.</param>
    public static void WatermarkImageTiled(string filePath, string outFilePath, string watermarkFilePath, int spacing, float opacity = 1f, int rotate = 0, FlipMode flipMode = FlipMode.None, int watermarkPercentage = 20) {
        string fullPath = Helpers.ResolvePath(filePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);
        using var img = Image.Load(fullPath);
        img.WatermarkImageTiled(watermarkFilePath, spacing, opacity, rotate, flipMode, watermarkPercentage);
        img.Save(outFullPath);
    }

    /// <summary>
    /// Asynchronously tiles the watermark image across the target image.
    /// </summary>
    public static async Task WatermarkImageTiledAsync(string filePath, string outFilePath, string watermarkFilePath, int spacing, float opacity = 1f, int rotate = 0, FlipMode flipMode = FlipMode.None, int watermarkPercentage = 20) {
        string fullPath = Helpers.ResolvePath(filePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);
        await Task.Run(() => {
            using var img = Image.Load(fullPath);
            img.WatermarkImageTiled(watermarkFilePath, spacing, opacity, rotate, flipMode, watermarkPercentage);
            img.Save(outFullPath);
        }).ConfigureAwait(false);
    }
}

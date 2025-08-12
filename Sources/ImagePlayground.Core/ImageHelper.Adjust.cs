using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp;

namespace ImagePlayground;
/// <summary>
/// Provides helper methods for image manipulation.
/// </summary>
public partial class ImageHelper {
    /// <summary>
    /// Adjusts image properties and saves the result.
    /// </summary>
    /// <param name="filePath">Source image path.</param>
    /// <param name="outFilePath">Destination image path.</param>
    /// <param name="brightness">Brightness adjustment factor.</param>
    /// <param name="contrast">Contrast adjustment factor.</param>
    /// <param name="lightness">Lightness adjustment factor.</param>
    /// <param name="opacity">Opacity adjustment factor.</param>
    /// <param name="saturation">Saturation adjustment factor.</param>
    /// <param name="sepia">Sepia adjustment factor.</param>
    public static void Adjust(string filePath, string outFilePath, float? brightness = null, float? contrast = null, float? lightness = null, float? opacity = null, float? saturation = null, float? sepia = null) {
        string fullPath = Helpers.ResolvePath(filePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);
        Directory.CreateDirectory(Path.GetDirectoryName(outFullPath)!);
        using var img = Image.Load(fullPath);
        if (brightness.HasValue) { img.Brightness(brightness.Value); }
        if (contrast.HasValue) { img.Contrast(contrast.Value); }
        if (lightness.HasValue) { img.Lightness(lightness.Value); }
        if (opacity.HasValue) { img.Opacity(opacity.Value); }
        if (saturation.HasValue) { img.Saturate(saturation.Value); }
        if (sepia.HasValue) { img.Sepia(sepia.Value); }
        img.Save(outFullPath);
    }

    /// <summary>
    /// Asynchronously adjusts image properties and saves the result.
    /// </summary>
    public static async Task AdjustAsync(string filePath, string outFilePath, float? brightness = null, float? contrast = null, float? lightness = null, float? opacity = null, float? saturation = null, float? sepia = null) {
        string fullPath = Helpers.ResolvePath(filePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);
        Directory.CreateDirectory(Path.GetDirectoryName(outFullPath)!);
        await Task.Run(() => {
            using var img = Image.Load(fullPath);
            if (brightness.HasValue) { img.Brightness(brightness.Value); }
            if (contrast.HasValue) { img.Contrast(contrast.Value); }
            if (lightness.HasValue) { img.Lightness(lightness.Value); }
            if (opacity.HasValue) { img.Opacity(opacity.Value); }
            if (saturation.HasValue) { img.Saturate(saturation.Value); }
            if (sepia.HasValue) { img.Sepia(sepia.Value); }
            img.Save(outFullPath);
        }).ConfigureAwait(false);
    }
}
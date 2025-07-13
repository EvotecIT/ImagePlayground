using System.IO;
using Codeuctivity.ImageSharpCompare;

namespace ImagePlayground;
/// <summary>
/// Provides helper methods for image manipulation.
/// </summary>
/// <remarks>
/// Comparison helpers rely on ImageSharpCompare to detect visual differences between files.
/// </remarks>
public partial class ImageHelper {

    /// <summary>
    /// Compares two images and returns the difference result.
    /// </summary>
    /// <param name="filePath">Path to the first image.</param>
    /// <param name="filePathToCompare">Path to the image to compare against.</param>
    /// <returns>Comparison result.</returns>
    public static ICompareResult Compare(string filePath, string filePathToCompare) {
        string fullPath = Helpers.ResolvePath(filePath);
        string fullPathToCompare = Helpers.ResolvePath(filePathToCompare);

        return ImageSharpCompare.CalcDiff(fullPath, fullPathToCompare);
    }

    /// <summary>
    /// Compares two images and saves the difference mask.
    /// </summary>
    /// <param name="filePath">Path to the first image.</param>
    /// <param name="filePathToCompare">Path to the image to compare against.</param>
    /// <param name="filePathToSave">Destination path for the difference mask.</param>
    public static void Compare(string filePath, string filePathToCompare, string filePathToSave) {
        string fullPath = Helpers.ResolvePath(filePath);
        string fullPathToCompare = Helpers.ResolvePath(filePathToCompare);
        string outFullPath = Helpers.ResolvePath(filePathToSave);

        using (var fileStreamDifferenceMask = File.Create(outFullPath)) {
            using (var maskImage = ImageSharpCompare.CalcDiffMaskImage(fullPath, fullPathToCompare)) {
                SixLabors.ImageSharp.ImageExtensions.SaveAsPng(maskImage, fileStreamDifferenceMask);
            }
        }
    }
}

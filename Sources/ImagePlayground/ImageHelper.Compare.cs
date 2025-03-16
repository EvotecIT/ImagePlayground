using System.IO;
using Codeuctivity.ImageSharpCompare;
using SixLabors.ImageSharp;

namespace ImagePlayground {
    public partial class ImageHelper {

        public static ICompareResult Compare(string filePath, string filePathToCompare) {
            string fullPath = Path.GetFullPath(filePath);
            string fullPathToCompare = Path.GetFullPath(filePathToCompare);

            // bool isEqual = ImageSharpCompare.ImagesAreEqual(fullPath, fullPathToCompare);
            return ImageSharpCompare.CalcDiff(fullPath, fullPathToCompare);
        }

        public static void Compare(string filePath, string filePathToCompare, string filePathToSave) {
            string fullPath = Path.GetFullPath(filePath);
            string fullPathToCompare = Path.GetFullPath(filePathToCompare);
            string outFullPath = Path.GetFullPath(filePathToSave);

            using var fileStreamDifferenceMask = File.Create(outFullPath);
            using var maskImage = ImageSharpCompare.CalcDiffMaskImage(fullPath, fullPathToCompare);
            ImageExtensions.SaveAsPng(maskImage, fileStreamDifferenceMask);
        }
    }
}

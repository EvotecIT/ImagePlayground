using System.IO;
using Codeuctivity.ImageSharpCompare;

namespace ImagePlayground {
    public partial class ImageHelper {

        public static ICompareResult Compare(string filePath, string filePathToCompare) {
            string fullPath = Helpers.ResolvePath(filePath);
            string fullPathToCompare = Helpers.ResolvePath(filePathToCompare);

            return ImageSharpCompare.CalcDiff(fullPath, fullPathToCompare);
        }

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
}
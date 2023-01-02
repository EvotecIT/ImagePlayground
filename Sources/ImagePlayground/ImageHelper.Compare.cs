using System.IO;
using Codeuctivity.ImageSharpCompare;

namespace ImagePlayground {
    public partial class ImageHelper {

        public static ICompareResult Compare(string filePath, string filePathToCompare) {
            bool isEqual = ImageSharpCompare.ImagesAreEqual(filePath, filePathToCompare);
            ICompareResult calcDiff = ImageSharpCompare.CalcDiff(filePath, filePathToCompare);
            return calcDiff;
        }

        public static void Compare(string filePath, string filePathToCompare, string filePathToSave) {
            using (var fileStreamDifferenceMask = File.Create(filePathToSave)) {
                using (var maskImage = ImageSharpCompare.CalcDiffMaskImage(filePath, filePathToCompare)) {
                    SixLabors.ImageSharp.ImageExtensions.SaveAsPng(maskImage, fileStreamDifferenceMask);
                }
            }
        }
    }
}
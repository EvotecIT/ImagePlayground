using System.IO;
using Codeuctivity.ImageSharpCompare;

namespace ImagePlayground {
    public partial class ImageHelper {

        public static ICompareResult Compare(string filePath, string filePathToCompare) {
            string fullPath = System.IO.Path.GetFullPath(filePath);
            string fullPathToCompare = System.IO.Path.GetFullPath(filePathToCompare);

            bool isEqual = ImageSharpCompare.ImagesAreEqual(fullPath, fullPathToCompare);
            ICompareResult calcDiff = ImageSharpCompare.CalcDiff(fullPath, fullPathToCompare);
            return calcDiff;
        }

        public static void Compare(string filePath, string filePathToCompare, string filePathToSave) {
            string fullPath = System.IO.Path.GetFullPath(filePath);
            string fullPathToCompare = System.IO.Path.GetFullPath(filePathToCompare);
            string outFullPath = System.IO.Path.GetFullPath(filePathToSave);

            using (var fileStreamDifferenceMask = File.Create(outFullPath)) {
                using (var maskImage = ImageSharpCompare.CalcDiffMaskImage(fullPath, fullPathToCompare)) {
                    SixLabors.ImageSharp.ImageExtensions.SaveAsPng(maskImage, fileStreamDifferenceMask);
                }
            }
        }
    }
}
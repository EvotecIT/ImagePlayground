namespace ImagePlayground;

/// <summary>Managed image comparison file workflows.</summary>
public partial class ImageHelper {
    /// <summary>Compares the first frames of two files and returns observable pixel metrics.</summary>
    public static OfficeRasterComparisonResult Compare(string filePath, string filePathToCompare) {
        using var image = Image.Load(filePath); return image.Compare(filePathToCompare);
    }
    /// <summary>Compares two files and saves a PNG difference mask.</summary>
    public static void Compare(string filePath, string filePathToCompare, string filePathToSave) {
        using var image = Image.Load(filePath); image.Compare(filePathToCompare, filePathToSave);
    }
}
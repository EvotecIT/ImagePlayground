using ChartForgeX.Composition;

namespace ImagePlayground;

/// <summary>File workflows for arranging a mosaic of images.</summary>
public partial class ImageHelper {
    /// <summary>Creates a mosaic from the first frames of input images using fixed-size tiles.</summary>
    public static void Mosaic(IEnumerable<string> filePaths, string outFilePath, int columns, int tileWidth, int tileHeight) {
        if (filePaths == null) throw new ArgumentNullException(nameof(filePaths));
        if (columns <= 0) throw new ArgumentOutOfRangeException(nameof(columns));
        if (tileWidth <= 0 || tileHeight <= 0) throw new ArgumentOutOfRangeException(nameof(tileWidth));
        string[] files = filePaths.ToArray();
        if (files.Length == 0) throw new ArgumentException("At least one image path is required.", nameof(filePaths));
        int rows = checked((files.Length + columns - 1) / columns);
        var composition = ImageComposition.CreateTransparent(checked(columns * tileWidth), checked(rows * tileHeight));
        for (int i = 0; i < files.Length; i++) {
            using var image = Image.Load(files[i]); image.Resize(tileWidth, tileHeight, false);
            composition.DrawImage(ToChartImage(image.Raster), i % columns * tileWidth, i / columns * tileHeight, tileWidth, tileHeight);
        }
        using var output = Image.FromRaster(ToOfficeImage(composition.ToImage())); output.Save(outFilePath);
    }
}
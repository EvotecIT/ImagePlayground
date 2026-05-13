using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ImagePlayground;
/// <summary>
/// Provides helper methods for image manipulation.
/// </summary>
public partial class ImageHelper {
    /// <summary>
    /// Creates a mosaic from multiple images.
    /// </summary>
    /// <param name="filePaths">Collection of image paths.</param>
    /// <param name="outFilePath">Destination path for the mosaic.</param>
    /// <param name="columns">Number of columns in the mosaic.</param>
    /// <param name="tileWidth">Width of each tile.</param>
    /// <param name="tileHeight">Height of each tile.</param>
    public static void Mosaic(IEnumerable<string> filePaths, string outFilePath, int columns, int tileWidth, int tileHeight) {
        if (filePaths == null) {
            throw new ArgumentNullException(nameof(filePaths));
        }
        if (columns <= 0) {
            throw new ArgumentOutOfRangeException(nameof(columns));
        }
        if (tileWidth <= 0) {
            throw new ArgumentOutOfRangeException(nameof(tileWidth));
        }
        if (tileHeight <= 0) {
            throw new ArgumentOutOfRangeException(nameof(tileHeight));
        }
        string[] files = filePaths.ToArray();
        if (files.Length == 0) {
            throw new ArgumentException("At least one file path must be provided.", nameof(filePaths));
        }
        string outFullPath = Helpers.ResolvePath(outFilePath);
        Helpers.CreateParentDirectory(outFullPath);
        int rows = (int)Math.Ceiling(files.Length / (double)columns);
        using Image<Rgba32> output = new Image<Rgba32>(columns * tileWidth, rows * tileHeight);
        for (int i = 0; i < files.Length; i++) {
            string full = Helpers.ResolvePath(files[i]);
            using var inStream = File.OpenRead(full);
            using SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(inStream);
            Resize(image, tileWidth, tileHeight, false);
            int x = (i % columns) * tileWidth;
            int y = (i / columns) * tileHeight;
            output.Mutate(ctx => ctx.DrawImage(image, new Point(x, y), 1f));
        }
        output.Save(outFullPath);
    }
}

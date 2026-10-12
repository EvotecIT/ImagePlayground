using ChartForgeX.Raster;

namespace ImagePlayground;

/// <summary>Animated GIF workflows backed by the shared animation encoder.</summary>
public static class Gif {
    /// <summary>Encodes the first frame of each input image, preserving order and repeating indefinitely.</summary>
    public static void Generate(IEnumerable<string> sourceImages, string filePath, int frameDelay = 100) {
        if (sourceImages == null) {
            throw new ArgumentNullException(nameof(sourceImages));
        }
        var source = new OfficeRasterFrames(ReadFrames(), playCount: 0);
        var frames = Image.ToAnimationFrames(source);
        string fullPath = Helpers.ResolvePath(filePath); Helpers.CreateParentDirectory(fullPath);
        OfficeImageFileWriter.WriteAllBytes(fullPath, RasterAnimationEncoder.Encode(frames, RasterAnimationFormat.Gif, new RasterAnimationOptions { PlayCount = 0 }));

        IEnumerable<OfficeRasterFrame> ReadFrames() {
            foreach (string path in sourceImages) {
                using var image = Image.Load(path);
                yield return new OfficeRasterFrame(image.Raster, TimeSpan.FromMilliseconds(Math.Max(10, frameDelay)));
            }
        }
    }
}

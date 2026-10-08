namespace ImagePlayground;

public static partial class Helpers {
    /// <summary>Reads encoded image bytes within the shared raster engine's resource limits.</summary>
    internal static byte[] ReadEncodedFile(string filePath, CancellationToken cancellationToken = default) {
        using var stream = File.OpenRead(filePath);
        return OfficeRasterImageDecoder.ReadEncodedBytes(stream, new OfficeRasterDecodeOptions { CancellationToken = cancellationToken });
    }
}
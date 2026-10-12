using ChartForgeX.Raster;

namespace ImagePlayground;

public partial class Image {
    // This consumer boundary retains both the Core pixels and independent Chart frame copies.
    // Individual decode, transform, and encode operations have their own working-buffer budgets.
    private const long MaximumAnimationInteropBytes = 256L * 1024 * 1024;

    internal static RasterAnimationFrame[] ToAnimationFrames(OfficeRasterFrames source, CancellationToken cancellationToken = default) {
        long retainedBytes = 65536;
        foreach (var frame in source) {
            cancellationToken.ThrowIfCancellationRequested();
            if (frame.Image.Width != source[0].Image.Width || frame.Image.Height != source[0].Image.Height) {
                throw new ArgumentException("Animation frames must share canvas dimensions. Resize or pad the frames before saving.", nameof(source));
            }
            retainedBytes = checked(retainedBytes + (long)frame.Image.Width * frame.Image.Height * 8 + 192);
            if (retainedBytes > MaximumAnimationInteropBytes) {
                throw new ArgumentException("Retaining raster pixels and animation frame copies exceeds 256 MiB. Reduce the canvas dimensions or frame count.", nameof(source));
            }
        }
        var frames = new RasterAnimationFrame[source.Count];
        for (int i = 0; i < frames.Length; i++) {
            cancellationToken.ThrowIfCancellationRequested();
            var frame = source[i];
            frames[i] = new RasterAnimationFrame(ImageHelper.ToChartImage(frame.Image), frame.Duration);
        }
        return frames;
    }
}
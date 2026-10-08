using ChartForgeX.Raster;

namespace ImagePlayground;

/// <summary>Explicit independent pixel snapshots for neutral composition and animation APIs.</summary>
public partial class Image {
    /// <summary>Copies the first frame into a neutral ChartForgeX pixel buffer.</summary>
    /// <remarks>Later mutations or disposal of either image do not affect the other pixel buffer.</remarks>
    public RgbaImage ToRgbaImage() => ImageHelper.ToChartImage(Raster);

    /// <summary>Copies neutral ChartForgeX pixels into an editable image.</summary>
    /// <remarks>The caller retains its pixel buffer. This image has empty metadata and no source file.</remarks>
    public static Image FromRgbaImage(RgbaImage image, ImageType imageType = ImageType.Png) =>
        FromRaster(ImageHelper.ToOfficeImage(image), imageType);
}

namespace ImagePlayground;

/// <summary>Pixel filters delegated to the shared managed raster engine.</summary>
public partial class Image {
    /// <summary>Thresholds pixels relative to their local luminance.</summary>
    public void AdaptiveThreshold(CancellationToken cancellationToken = default) {
        EnsureUsable();
        cancellationToken.ThrowIfCancellationRequested();
        long scratch = _frames.Max(frame => OfficeRasterFilters.EstimateAdaptiveThresholdAdditionalWorkingBytes(frame.Image.Width, frame.Image.Height));
        Apply(image => OfficeRasterFilters.AdaptiveThreshold(image, cancellationToken: cancellationToken), additionalRetainedBytes: scratch, cancellationToken: cancellationToken);
    }
    /// <summary>Converts pixels to black or white at the midpoint of their luminance.</summary>
    public void BlackWhite(CancellationToken cancellationToken = default) => Apply(image => OfficeRasterFilters.Threshold(image, cancellationToken: cancellationToken), cancellationToken: cancellationToken);
    /// <summary>Multiplies the RGB brightness by the requested factor.</summary>
    public void Brightness(float amount, CancellationToken cancellationToken = default) => Apply(image => OfficeRasterFilters.Brightness(image, amount, cancellationToken: cancellationToken), cancellationToken: cancellationToken);
    /// <summary>Applies a circular bokeh blur.</summary>
    public void BokehBlur(CancellationToken cancellationToken = default) => Apply(image => OfficeRasterFilters.BokehBlur(image, cancellationToken: cancellationToken), cancellationToken: cancellationToken);
    /// <summary>Applies a box blur.</summary>
    public void BoxBlur(CancellationToken cancellationToken = default) {
        EnsureUsable();
        cancellationToken.ThrowIfCancellationRequested();
        long scratch = _frames.Max(frame => OfficeRasterFilters.EstimateBoxBlurAdditionalWorkingBytes(frame.Image.Width, frame.Image.Height));
        Apply(image => OfficeRasterFilters.BoxBlur(image, cancellationToken: cancellationToken), additionalRetainedBytes: scratch, cancellationToken: cancellationToken);
    }
    /// <summary>Adjusts contrast around the midpoint of the RGB range.</summary>
    public void Contrast(float amount, CancellationToken cancellationToken = default) => Apply(image => OfficeRasterFilters.Contrast(image, amount, cancellationToken: cancellationToken), cancellationToken: cancellationToken);
    /// <summary>Applies monochrome error diffusion dithering.</summary>
    public void Dither(CancellationToken cancellationToken = default) => Apply(image => OfficeRasterFilters.Dither(image, cancellationToken: cancellationToken), cancellationToken: cancellationToken);
    /// <summary>Transforms normalized RGBA channels with the supplied matrix.</summary>
    public void Filter(OfficeRasterColorMatrix colorMatrix, CancellationToken cancellationToken = default) => Apply(image => OfficeRasterFilters.ColorMatrix(image, colorMatrix, cancellationToken: cancellationToken), cancellationToken: cancellationToken);
    /// <summary>Applies a Gaussian blur with the requested standard deviation.</summary>
    public void GaussianBlur(float? sigma = null, CancellationToken cancellationToken = default) {
        EnsureUsable();
        cancellationToken.ThrowIfCancellationRequested();
        double amount = sigma ?? 3;
        long scratch = _frames.Max(frame => OfficeRasterFilters.EstimateGaussianBlurAdditionalWorkingBytes(frame.Image.Width, frame.Image.Height, amount));
        Apply(image => OfficeRasterFilters.GaussianBlur(image, amount, cancellationToken: cancellationToken), additionalRetainedBytes: scratch, cancellationToken: cancellationToken);
    }
    /// <summary>Sharpens using a Gaussian unsharp mask.</summary>
    public void GaussianSharpen(float? sigma = null, CancellationToken cancellationToken = default) {
        EnsureUsable();
        cancellationToken.ThrowIfCancellationRequested();
        double amount = sigma ?? 3;
        long scratch = _frames.Max(frame => OfficeRasterFilters.EstimateGaussianSharpenAdditionalWorkingBytes(frame.Image.Width, frame.Image.Height, amount));
        Apply(image => OfficeRasterFilters.GaussianSharpen(image, amount, cancellationToken: cancellationToken), additionalRetainedBytes: scratch, cancellationToken: cancellationToken);
    }
    /// <summary>Equalizes the luminance histogram.</summary>
    public void HistogramEqualization(CancellationToken cancellationToken = default) => Apply(image => OfficeRasterFilters.HistogramEqualization(image, cancellationToken: cancellationToken), cancellationToken: cancellationToken);
    /// <summary>Rotates hue in degrees.</summary>
    public void Hue(float degrees, CancellationToken cancellationToken = default) => Apply(image => OfficeRasterFilters.Hue(image, degrees, cancellationToken: cancellationToken), cancellationToken: cancellationToken);
    /// <summary>Converts to grayscale while preserving alpha.</summary>
    public void Grayscale(OfficeRasterGrayscaleMode grayscaleMode = OfficeRasterGrayscaleMode.Bt709, CancellationToken cancellationToken = default) => Apply(image => OfficeRasterFilters.Grayscale(image, grayscaleMode, cancellationToken: cancellationToken), cancellationToken: cancellationToken);
    /// <summary>Applies the shared Kodachrome color transform.</summary>
    public void Kodachrome(CancellationToken cancellationToken = default) => Apply(image => OfficeRasterFilters.Kodachrome(image, cancellationToken: cancellationToken), cancellationToken: cancellationToken);
    /// <summary>Adjusts HSL lightness.</summary>
    public void Lightness(float amount, CancellationToken cancellationToken = default) => Apply(image => OfficeRasterFilters.Lightness(image, amount, cancellationToken: cancellationToken), cancellationToken: cancellationToken);
    /// <summary>Applies the shared Lomograph color transform.</summary>
    public void Lomograph(CancellationToken cancellationToken = default) => Apply(image => OfficeRasterFilters.Lomograph(image, cancellationToken: cancellationToken), cancellationToken: cancellationToken);
    /// <summary>Inverts RGB channels while preserving alpha.</summary>
    public void Invert(CancellationToken cancellationToken = default) => Apply(image => OfficeRasterFilters.Invert(image, cancellationToken: cancellationToken), cancellationToken: cancellationToken);
    /// <summary>Multiplies pixel opacity by the requested factor.</summary>
    public void Opacity(float amount, CancellationToken cancellationToken = default) => Apply(image => OfficeRasterFilters.Opacity(image, amount, cancellationToken: cancellationToken), cancellationToken: cancellationToken);
    /// <summary>Applies the shared Polaroid color transform.</summary>
    public void Polaroid(CancellationToken cancellationToken = default) => Apply(image => OfficeRasterFilters.Polaroid(image, cancellationToken: cancellationToken), cancellationToken: cancellationToken);
    /// <summary>Pixelates using blocks of four pixels.</summary>
    public void Pixelate(CancellationToken cancellationToken = default) => Pixelate(4, cancellationToken);
    /// <summary>Pixelates with the requested block size.</summary>
    public void Pixelate(int size, CancellationToken cancellationToken = default) => Apply(image => OfficeRasterFilters.Pixelate(image, size, cancellationToken: cancellationToken), cancellationToken: cancellationToken);
    /// <summary>Applies an oil painting filter.</summary>
    public void OilPaint(CancellationToken cancellationToken = default) => OilPaint(10, 15, cancellationToken);
    /// <summary>Applies an oil painting filter with explicit levels and brush size.</summary>
    public void OilPaint(int levels, int brushSize, CancellationToken cancellationToken = default) => Apply(image => OfficeRasterFilters.OilPaint(image, levels, brushSize, cancellationToken: cancellationToken), cancellationToken: cancellationToken);
    /// <summary>Adjusts saturation by the requested factor.</summary>
    public void Saturate(float amount, CancellationToken cancellationToken = default) => Apply(image => OfficeRasterFilters.Saturate(image, amount, cancellationToken: cancellationToken), cancellationToken: cancellationToken);
    /// <summary>Applies the full sepia transform.</summary>
    public void Sepia(CancellationToken cancellationToken = default) => Sepia(1, cancellationToken);
    /// <summary>Blends the sepia transform by the requested amount.</summary>
    public void Sepia(float amount, CancellationToken cancellationToken = default) => Apply(image => OfficeRasterFilters.Sepia(image, amount, cancellationToken: cancellationToken), cancellationToken: cancellationToken);
    /// <summary>Darkens the image towards its edges.</summary>
    public void Vignette(CancellationToken cancellationToken = default) => Apply(image => OfficeRasterFilters.Vignette(image, cancellationToken: cancellationToken), cancellationToken: cancellationToken);
    /// <summary>Blends a color towards the image edges.</summary>
    public void Vignette(OfficeColor color, CancellationToken cancellationToken = default) => Apply(image => OfficeRasterFilters.Vignette(image, color, cancellationToken: cancellationToken), cancellationToken: cancellationToken);
}

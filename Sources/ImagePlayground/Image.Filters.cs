namespace ImagePlayground;

/// <summary>Pixel filters delegated to the shared managed raster engine.</summary>
public partial class Image {
    /// <summary>Thresholds pixels relative to their local luminance.</summary>
    public void AdaptiveThreshold() => Apply(image => OfficeRasterFilters.AdaptiveThreshold(image));
    /// <summary>Converts pixels to black or white at the midpoint of their luminance.</summary>
    public void BlackWhite() => Apply(image => OfficeRasterFilters.Threshold(image));
    /// <summary>Multiplies the RGB brightness by the requested factor.</summary>
    public void Brightness(float amount) => Apply(image => OfficeRasterFilters.Brightness(image, amount));
    /// <summary>Applies a circular bokeh blur.</summary>
    public void BokehBlur() => Apply(image => OfficeRasterFilters.BokehBlur(image));
    /// <summary>Applies a box blur.</summary>
    public void BoxBlur() => Apply(image => OfficeRasterFilters.BoxBlur(image));
    /// <summary>Adjusts contrast around the midpoint of the RGB range.</summary>
    public void Contrast(float amount) => Apply(image => OfficeRasterFilters.Contrast(image, amount));
    /// <summary>Applies monochrome error diffusion dithering.</summary>
    public void Dither() => Apply(image => OfficeRasterFilters.Dither(image));
    /// <summary>Transforms normalized RGBA channels with the supplied matrix.</summary>
    public void Filter(OfficeRasterColorMatrix colorMatrix) => Apply(image => OfficeRasterFilters.ColorMatrix(image, colorMatrix));
    /// <summary>Applies a Gaussian blur with the requested standard deviation.</summary>
    public void GaussianBlur(float? sigma) => Apply(image => OfficeRasterFilters.GaussianBlur(image, sigma ?? 3));
    /// <summary>Sharpens using a Gaussian unsharp mask.</summary>
    public void GaussianSharpen(float? sigma) => Apply(image => OfficeRasterFilters.GaussianSharpen(image, sigma ?? 3));
    /// <summary>Equalizes the luminance histogram.</summary>
    public void HistogramEqualization() => Apply(image => OfficeRasterFilters.HistogramEqualization(image));
    /// <summary>Rotates hue in degrees.</summary>
    public void Hue(float degrees) => Apply(image => OfficeRasterFilters.Hue(image, degrees));
    /// <summary>Converts to grayscale while preserving alpha.</summary>
    public void Grayscale(OfficeRasterGrayscaleMode grayscaleMode = OfficeRasterGrayscaleMode.Bt709) => Apply(image => OfficeRasterFilters.Grayscale(image, grayscaleMode));
    /// <summary>Applies the shared Kodachrome color transform.</summary>
    public void Kodachrome() => Apply(image => OfficeRasterFilters.Kodachrome(image));
    /// <summary>Adjusts HSL lightness.</summary>
    public void Lightness(float amount) => Apply(image => OfficeRasterFilters.Lightness(image, amount));
    /// <summary>Applies the shared Lomograph color transform.</summary>
    public void Lomograph() => Apply(image => OfficeRasterFilters.Lomograph(image));
    /// <summary>Inverts RGB channels while preserving alpha.</summary>
    public void Invert() => Apply(image => OfficeRasterFilters.Invert(image));
    /// <summary>Multiplies pixel opacity by the requested factor.</summary>
    public void Opacity(float amount) => Apply(image => OfficeRasterFilters.Opacity(image, amount));
    /// <summary>Applies the shared Polaroid color transform.</summary>
    public void Polaroid() => Apply(image => OfficeRasterFilters.Polaroid(image));
    /// <summary>Pixelates using blocks of four pixels.</summary>
    public void Pixelate() => Pixelate(4);
    /// <summary>Pixelates with the requested block size.</summary>
    public void Pixelate(int size) => Apply(image => OfficeRasterFilters.Pixelate(image, size));
    /// <summary>Applies an oil painting filter.</summary>
    public void OilPaint() => OilPaint(10, 15);
    /// <summary>Applies an oil painting filter with explicit levels and brush size.</summary>
    public void OilPaint(int levels, int brushSize) => Apply(image => OfficeRasterFilters.OilPaint(image, levels, brushSize));
    /// <summary>Adjusts saturation by the requested factor.</summary>
    public void Saturate(float amount) => Apply(image => OfficeRasterFilters.Saturate(image, amount));
    /// <summary>Applies the full sepia transform.</summary>
    public void Sepia() => Sepia(1);
    /// <summary>Blends the sepia transform by the requested amount.</summary>
    public void Sepia(float amount) => Apply(image => OfficeRasterFilters.Sepia(image, amount));
    /// <summary>Darkens the image towards its edges.</summary>
    public void Vignette() => Apply(image => OfficeRasterFilters.Vignette(image));
    /// <summary>Blends a color towards the image edges.</summary>
    public void Vignette(OfficeColor color) => Apply(image => OfficeRasterFilters.Vignette(image, color));
}
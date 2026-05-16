using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ImagePlayground;

/// <summary>
/// Provides image filter operations.
/// </summary>
public partial class Image : IDisposable {
    /// <summary>
    /// Applies an adaptive threshold filter to the current image.
    /// </summary>
    public void AdaptiveThreshold() {
        _image.Mutate(x => x.AdaptiveThreshold());
    }

    /// <summary>
    /// Converts the image to black and white.
    /// </summary>
    public void BlackWhite() {
        _image.Mutate(x => x.BlackWhite());
    }

    /// <summary>
    /// Adjusts brightness by the specified <paramref name="amount"/>.
    /// </summary>
    /// <param name="amount">Brightness adjustment factor.</param>
    public void Brightness(float amount) {
        _image.Mutate(x => x.Brightness(amount));
    }

    /// <summary>
    /// Applies a bokeh blur effect.
    /// </summary>
    public void BokehBlur() {
        _image.Mutate(x => x.BokehBlur());
    }

    /// <summary>
    /// Applies a simple box blur.
    /// </summary>
    public void BoxBlur() {
        _image.Mutate(x => x.BoxBlur());
    }

    /// <summary>
    /// Adjusts contrast by the specified <paramref name="amount"/>.
    /// </summary>
    /// <param name="amount">Contrast adjustment factor.</param>
    public void Contrast(float amount) {
        _image.Mutate(x => x.Contrast(amount));
    }

    /// <summary>
    /// Applies a dithering effect.
    /// </summary>
    public void Dither() {
        _image.Mutate(x => x.Dither());
    }

    /// <summary>
    /// Applies a color <paramref name="colorMatrix"/> filter.
    /// </summary>
    /// <param name="colorMatrix">Color matrix to apply.</param>
    public void Filter(ColorMatrix colorMatrix) {
        _image.Mutate(x => x.Filter(colorMatrix));
    }

    /// <summary>
    /// Applies a Gaussian blur using the optional <paramref name="sigma"/> value.
    /// </summary>
    /// <param name="sigma">Blur radius.</param>
    public void GaussianBlur(float? sigma) {
        if (sigma != null) {
            _image.Mutate(x => x.GaussianBlur(sigma.Value));
        } else {
            _image.Mutate(x => x.GaussianBlur());
        }
    }

    /// <summary>
    /// Sharpens the image using a Gaussian algorithm.
    /// </summary>
    /// <param name="sigma">Sharpen strength.</param>
    public void GaussianSharpen(float? sigma) {
        if (sigma != null) {
            _image.Mutate(x => x.GaussianSharpen(sigma.Value));
        } else {
            _image.Mutate(x => x.GaussianSharpen());
        }
    }

    /// <summary>
    /// Performs histogram equalization on the image.
    /// </summary>
    public void HistogramEqualization() {
        _image.Mutate(x => x.HistogramEqualization());
    }

    /// <summary>
    /// Shifts hue by the specified <paramref name="degrees"/>.
    /// </summary>
    /// <param name="degrees">Hue rotation in degrees.</param>
    public void Hue(float degrees) {
        _image.Mutate(x => x.Hue(degrees));
    }

    /// <summary>
    /// Converts the image to grayscale using the specified <paramref name="grayscaleMode"/>.
    /// </summary>
    /// <param name="grayscaleMode">Grayscale conversion mode.</param>
    public void Grayscale(GrayscaleMode grayscaleMode = GrayscaleMode.Bt709) {
        _image.Mutate(x => x.Grayscale(grayscaleMode));
    }

    /// <summary>
    /// Applies a Kodachrome color filter.
    /// </summary>
    public void Kodachrome() {
        _image.Mutate(x => x.Kodachrome());
    }

    /// <summary>
    /// Adjusts lightness by <paramref name="amount"/>.
    /// </summary>
    /// <param name="amount">Lightness factor.</param>
    public void Lightness(float amount) {
        _image.Mutate(x => x.Lightness(amount));
    }

    /// <summary>
    /// Applies a lomograph effect.
    /// </summary>
    public void Lomograph() {
        _image.Mutate(x => x.Lomograph());
    }

    /// <summary>
    /// Inverts the colors of the image.
    /// </summary>
    public void Invert() {
        _image.Mutate(x => x.Invert());
    }

    /// <summary>
    /// Changes opacity by the given <paramref name="amount"/>.
    /// </summary>
    /// <param name="amount">Opacity factor.</param>
    public void Opacity(float amount) {
        _image.Mutate(x => x.Opacity(amount));
    }

    /// <summary>
    /// Applies a Polaroid style filter.
    /// </summary>
    public void Polaroid() {
        _image.Mutate(x => x.Polaroid());
    }

    /// <summary>
    /// Pixelates the image with a default size.
    /// </summary>
    public void Pixelate() {
        _image.Mutate(x => x.Pixelate());
    }

    /// <summary>
    /// Pixelates the image using the specified <paramref name="size"/>.
    /// </summary>
    /// <param name="size">Pixel block size.</param>
    public void Pixelate(int size) {
        _image.Mutate(x => x.Pixelate(size));
    }

    /// <summary>
    /// Applies an oil paint effect using default options.
    /// </summary>
    public void OilPaint() {
        _image.Mutate(x => x.OilPaint());
    }

    /// <summary>
    /// Applies an oil paint effect with the given parameters.
    /// </summary>
    /// <param name="levels">Number of intensity levels.</param>
    /// <param name="brushSize">Brush size.</param>
    public void OilPaint(int levels, int brushSize) {
        _image.Mutate(x => x.OilPaint(levels, brushSize));
    }

    /// <summary>
    /// Changes saturation by the specified <paramref name="amount"/>.
    /// </summary>
    /// <param name="amount">Saturation factor.</param>
    public void Saturate(float amount) {
        _image.Mutate(x => x.Saturate(amount));
    }

    /// <summary>
    /// Applies a sepia tone effect.
    /// </summary>
    public void Sepia() {
        _image.Mutate(x => x.Sepia());
    }

    /// <summary>
    /// Applies a sepia tone effect with the given <paramref name="amount"/>.
    /// </summary>
    /// <param name="amount">Sepia intensity.</param>
    public void Sepia(float amount) {
        _image.Mutate(x => x.Sepia(amount));
    }

    /// <summary>
    /// Adds a vignette effect using default options.
    /// </summary>
    public void Vignette() {
        _image.Mutate(x => x.Vignette());
    }

    /// <summary>
    /// Adds a vignette effect using the specified <paramref name="color"/>.
    /// </summary>
    /// <param name="color">Vignette color.</param>
    public void Vignette(Color color) {
        _image.Mutate(x => x.Vignette(color));
    }
}


using System;
using System.IO;
using System.Numerics;
using Codeuctivity.ImageSharpCompare;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Metadata;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Extensions.Transforms;

namespace ImagePlayground;

/// <summary>
/// Represents an image loaded using ImageSharp and exposes basic manipulation helpers.
/// </summary>
public partial class Image : IDisposable {
    private SixLabors.ImageSharp.Image _image;
    private string _filePath;

    /// <summary>Gets the width of the image.</summary>
    public int Width => _image.Width;

    /// <summary>Gets the height of the image.</summary>
    public int Height => _image.Height;

    /// <summary>Gets the path the image was loaded from.</summary>
    public string FilePath => _filePath;

    /// <summary>Gets the metadata associated with the image.</summary>
    public ImageMetadata Metadata => _image.Metadata;

    /// <summary>Gets information about the underlying pixel type.</summary>
    public PixelTypeInfo PixelType => _image.PixelType;

    /// <summary>Gets the frame collection for multi-frame images.</summary>
    public ImageFrameCollection Frames => _image.Frames;



    /// <summary>
    /// Applies an adaptive threshold filter to the current image.
    /// </summary>
    public void AdaptiveThreshold() {
        _image.Mutate(x => x.AdaptiveThreshold());
    }

    /// <summary>
    /// Automatically rotates the image according to EXIF orientation data.
    /// </summary>
    public void AutoOrient() {
        _image.Mutate(x => x.AutoOrient());
    }

    /// <summary>
    /// Fills the background with the specified <paramref name="color"/>.
    /// </summary>
    /// <param name="color">Fill color.</param>
    public void BackgroundColor(Color color) {
        _image.Mutate(x => x.BackgroundColor(color));
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
    /// Compares the current image with <paramref name="imageToCompare"/>.
    /// </summary>
    /// <param name="imageToCompare">Image to compare with.</param>
    /// <returns>Comparison result.</returns>
    public ICompareResult Compare(Image imageToCompare) {
        return ImageSharpCompare.CalcDiff(_image, imageToCompare._image);
    }

    /// <summary>
    /// Compares the current image with the file at <paramref name="filePathToCompare"/>.
    /// </summary>
    /// <param name="filePathToCompare">Path to the image to compare with.</param>
    /// <returns>Comparison result.</returns>
    public ICompareResult Compare(string filePathToCompare) {
        string fullPath = Helpers.ResolvePath(filePathToCompare);

        using (var imageToCompare = GetImage(fullPath)) {
            return ImageSharpCompare.CalcDiff(_image, imageToCompare);
        }
    }

    /// <summary>
    /// Creates a difference mask between this image and <paramref name="imageToCompare"/> and saves it.
    /// </summary>
    /// <param name="imageToCompare">Image to compare with.</param>
    /// <param name="filePathToSave">Output path for the mask image.</param>
    public void Compare(Image imageToCompare, string filePathToSave) {
        string outFullPath = Helpers.ResolvePath(filePathToSave);
        using (var fileStreamDifferenceMask = File.Create(outFullPath)) {
            using (var maskImage = ImageSharpCompare.CalcDiffMaskImage(_image, imageToCompare._image)) {
                SixLabors.ImageSharp.ImageExtensions.SaveAsPng(maskImage, fileStreamDifferenceMask);
            }
        }
    }

    /// <summary>
    /// Creates a difference mask between this image and the file at <paramref name="filePathToCompare"/> and saves it.
    /// </summary>
    /// <param name="filePathToCompare">Path to the image to compare with.</param>
    /// <param name="filePathToSave">Output path for the mask image.</param>
    public void Compare(string filePathToCompare, string filePathToSave) {
        string fullPath = Helpers.ResolvePath(filePathToCompare);
        string outFullPath = Helpers.ResolvePath(filePathToSave);

        using (var fileStreamDifferenceMask = File.Create(outFullPath)) {
            using (var imageToCompare = GetImage(fullPath)) {
                using (var maskImage = ImageSharpCompare.CalcDiffMaskImage(_image, imageToCompare)) {
                    SixLabors.ImageSharp.ImageExtensions.SaveAsPng(maskImage, fileStreamDifferenceMask);
                }
            }
        }
    }

    /// <summary>
    /// Crops the image to the specified <paramref name="rectangle"/>.
    /// </summary>
    /// <param name="rectangle">Crop rectangle.</param>
    public void Crop(Rectangle rectangle) {
        _image.Mutate(x => x.Crop(rectangle));
    }

    /// <summary>
    /// Applies a dithering effect.
    /// </summary>
    public void Dither() {
        _image.Mutate(x => x.Dither());
    }

    /// <summary>
    /// Draws a line ending at <paramref name="pointF"/> using the specified <paramref name="color"/> and <paramref name="thickness"/>.
    /// </summary>
    /// <param name="color">Line color.</param>
    /// <param name="thickness">Line thickness.</param>
    /// <param name="pointF">End point of the line.</param>
    public void DrawLines(Color color, float thickness, PointF pointF) {
        _image.Mutate(x => x.DrawLine(color, thickness, pointF));
    }

    /// <summary>
    /// Draws a polygon with the specified parameters.
    /// </summary>
    /// <param name="color">Polygon color.</param>
    /// <param name="thickness">Outline thickness.</param>
    /// <param name="pointF">Polygon vertex.</param>
    public void DrawPolygon(Color color, float thickness, PointF pointF) {
        _image.Mutate(x => x.DrawPolygon(color, thickness, pointF));
    }

    /// <summary>
    /// Applies a color <paramref name="colorMatrix"/> filter.
    /// </summary>
    /// <param name="colorMatrix">Color matrix to apply.</param>
    public void Filter(ColorMatrix colorMatrix) {
        _image.Mutate(x => x.Filter(colorMatrix));
    }

    /// <summary>
    /// Fills the image with <paramref name="color"/>.
    /// </summary>
    /// <param name="color">Fill color.</param>
    public void Fill(Color color) {
        _image.Mutate(x => x.Fill(color));
    }

    /// <summary>
    /// Fills the specified <paramref name="rectangle"/> with <paramref name="color"/>.
    /// </summary>
    /// <param name="color">Fill color.</param>
    /// <param name="rectangle">Target rectangle.</param>
    public void Fill(Color color, Rectangle rectangle) {
        _image.Mutate(x => x.Fill(color, rectangle));
    }

    /// <summary>
    /// Flips the image using the provided <paramref name="flipMode"/>.
    /// </summary>
    /// <param name="flipMode">Flip mode.</param>
    public void Flip(FlipMode flipMode) {
        _image.Mutate(x => x.Flip(flipMode));
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
    /// Rotates the image using the specified <paramref name="rotateMode"/>.
    /// </summary>
    /// <param name="rotateMode">Rotation mode.</param>
    public void Rotate(RotateMode rotateMode) {
        _image.Mutate(x => x.Rotate(rotateMode));
    }

    /// <summary>
    /// Rotates the image by an arbitrary number of <paramref name="degrees"/>.
    /// </summary>
    /// <param name="degrees">Angle in degrees.</param>
    public void Rotate(float degrees) {
        _image.Mutate(x => x.Rotate(degrees: degrees));
    }

    /// <summary>
    /// Rotates and flips the image in a single operation.
    /// </summary>
    /// <param name="rotateMode">Rotation mode.</param>
    /// <param name="flipMode">Flip mode.</param>
    public void RotateFlip(RotateMode rotateMode, FlipMode flipMode) {
        _image.Mutate(x => x.RotateFlip(rotateMode, flipMode));
    }

    /// <summary>
    /// Resizes the image to the specified dimensions.
    /// </summary>
    /// <param name="width">New width.</param>
    /// <param name="height">New height.</param>
    /// <param name="keepAspectRatio">Maintain aspect ratio if possible.</param>
    public void Resize(int? width, int? height, bool keepAspectRatio = true) {
        if (width == null && height == null) {
            return;
        }

        var options = new ResizeOptions();
        if (keepAspectRatio && (width == null || height == null)) {
            options.Mode = ResizeMode.Max;
            options.Size = new Size(width ?? 0, height ?? 0);
        } else if (keepAspectRatio) {
            options.Mode = ResizeMode.Max;
            options.Size = new Size(width ?? _image.Width, height ?? _image.Height);
        } else {
            options.Mode = ResizeMode.Stretch;
            options.Size = new Size(width ?? _image.Width, height ?? _image.Height);
        }

        _image.Mutate(x => x.Resize(options));
    }

    /// <summary>
    /// Resizes the image by a <paramref name="percentage"/> of the current size.
    /// </summary>
    /// <param name="percentage">Scale percentage.</param>
    public void Resize(int percentage) {
        if (percentage <= 0) {
            throw new ArgumentOutOfRangeException(nameof(percentage));
        }

        int width = _image.Width * percentage / 100;
        int height = _image.Height * percentage / 100;
        var options = new ResizeOptions {
            Mode = ResizeMode.Stretch,
            Size = new Size(width, height)
        };
        _image.Mutate(x => x.Resize(options));
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
    /// Skews the image by the specified angles.
    /// </summary>
    /// <param name="degreesX">Skew angle on the X axis.</param>
    /// <param name="degreesY">Skew angle on the Y axis.</param>
    public void Skew(float degreesX, float degreesY) {
        _image.Mutate(x => x.Skew(degreesX, degreesY));
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

    /// <summary>
    /// Loads an <see cref="SixLabors.ImageSharp.Image"/> from disk.
    /// </summary>
    /// <param name="filePath">Path to the image file.</param>
    /// <returns>The loaded image.</returns>
    public static SixLabors.ImageSharp.Image GetImage(string filePath) {
        string fullPath = Helpers.ResolvePath(filePath);
        using (var inStream = System.IO.File.OpenRead(fullPath)) {
            return SixLabors.ImageSharp.Image.Load(inStream);
        }
    }

    /// <summary>
    /// Creates a new blank image and assigns the provided <paramref name="filePath"/>.
    /// </summary>
    /// <param name="filePath">Path where the image will be saved.</param>
    /// <param name="width">Image width.</param>
    /// <param name="height">Image height.</param>
    public void Create(string filePath, int width, int height) {
        _filePath = filePath;
        _image = new Image<Rgba32>(width, height);
    }

    /// <summary>
    /// Loads an image from the specified path.
    /// </summary>
    /// <param name="filePath">Path to the image file.</param>
    /// <returns>Loaded <see cref="Image"/> instance.</returns>
    public static Image Load(string filePath) {
        string fullPath = Helpers.ResolvePath(filePath);

        using var stream = System.IO.File.OpenRead(fullPath);
        Image image = new Image {
            _filePath = fullPath,
            _image = SixLabors.ImageSharp.Image.Load(stream)
        };

        return image;
    }

    /// <summary>
    /// Saves the image to disk.
    /// </summary>
    /// <param name="filePath">Target path or empty to overwrite the original file.</param>
    /// <param name="openImage">Whether to open the saved file.</param>
    /// <param name="quality">Optional quality for lossy formats.</param>
    /// <param name="compressionLevel">Optional compression level for PNG/WebP.</param>
    public void Save(string filePath = "", bool openImage = false, int? quality = null, int? compressionLevel = null) {
        if (filePath == "") {
            filePath = _filePath;
        } else {
            filePath = Helpers.ResolvePath(filePath);
        }
        var encoder = Helpers.GetEncoder(System.IO.Path.GetExtension(filePath), quality, compressionLevel);
        _image.Save(filePath, encoder);
        Helpers.Open(filePath, openImage);
    }

    /// <summary>
    /// Saves the image overwriting the original file and optionally opens it.
    /// </summary>
    /// <param name="openImage">Whether to open the saved file.</param>
    public void Save(bool openImage) {
        Save("", openImage, null, null);
    }

    /// <summary>
    /// Saves the image to the provided <paramref name="stream"/>.
    /// </summary>
    /// <param name="stream">Destination stream.</param>
    /// <param name="quality">Optional quality for lossy formats.</param>
    /// <param name="compressionLevel">Optional compression level for PNG/WebP.</param>
    public void Save(Stream stream, int? quality = null, int? compressionLevel = null) {
        string extension = System.IO.Path.GetExtension(_filePath)?.ToLowerInvariant();
        var encoder = Helpers.GetEncoder(extension, quality, compressionLevel);
        _image.Save(stream, encoder);
        stream.Seek(0, SeekOrigin.Begin);
    }

    /// <summary>
    /// Converts the image to a memory stream.
    /// </summary>
    /// <param name="quality">Optional quality for lossy formats.</param>
    /// <param name="compressionLevel">Optional compression level for PNG/WebP.</param>
    /// <returns>Stream containing the image data.</returns>
    public MemoryStream ToStream(int? quality = null, int? compressionLevel = null) {
        var ms = new MemoryStream();
        Save(ms, quality, compressionLevel);
        return ms;
    }

    /// <summary>
    /// Disposes the underlying image resources.
    /// </summary>
    public void Dispose() {
        if (_image != null) {
            _image.Dispose();
        }
    }
}

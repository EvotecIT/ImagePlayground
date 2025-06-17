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

namespace ImagePlayground {
    public partial class Image : IDisposable {
        private SixLabors.ImageSharp.Image _image;
        private string _filePath;
        public int Width => _image.Width;
        public int Height => _image.Height;
        public string FilePath => _filePath;
        public ImageMetadata Metadata => _image.Metadata;
        public PixelTypeInfo PixelType => _image.PixelType;
        public ImageFrameCollection Frames => _image.Frames;

        public enum Sampler {
            NearestNeighbor,
            Box,
            Triangle,
            Hermite,
            Lanczos2,
            Lanczos3,
            Lanczos5,
            Lanczos8,
            MitchellNetravali,
            CatmullRom,
            Robidoux,
            RobidouxSharp,
            Spline,
            Welch,
        }

        public void AdaptiveThreshold() {
            _image.Mutate(x => x.AdaptiveThreshold());
        }

        public void AutoOrient() {
            _image.Mutate(x => x.AutoOrient());
        }

        public void BackgroundColor(Color color) {
            _image.Mutate(x => x.BackgroundColor(color));
        }

        public void BlackWhite() {
            _image.Mutate(x => x.BlackWhite());
        }

        public void Brightness(float amount) {
            _image.Mutate(x => x.Brightness(amount));
        }

        public void BokehBlur() {
            _image.Mutate(x => x.BokehBlur());
        }

        public void BoxBlur() {
            _image.Mutate(x => x.BoxBlur());
        }

        public void Contrast(float amount) {
            _image.Mutate(x => x.Contrast(amount));
        }

        public ICompareResult Compare(Image imageToCompare) {
            bool isEqual = ImageSharpCompare.ImagesAreEqual(_image, imageToCompare._image);
            ICompareResult calcDiff = ImageSharpCompare.CalcDiff(_image, imageToCompare._image);
            return calcDiff;
        }

        public ICompareResult Compare(string filePathToCompare) {
            string fullPath = Helpers.ResolvePath(filePathToCompare);

            using (var imageToCompare = GetImage(fullPath)) {
                bool isEqual = ImageSharpCompare.ImagesAreEqual(_image, imageToCompare);
                ICompareResult calcDiff = ImageSharpCompare.CalcDiff(_image, imageToCompare);
                return calcDiff;
            }
        }

        public void Compare(Image imageToCompare, string filePathToSave) {
            string outFullPath = Helpers.ResolvePath(filePathToSave);
            using (var fileStreamDifferenceMask = File.Create(outFullPath)) {
                using (var maskImage = ImageSharpCompare.CalcDiffMaskImage(_image, imageToCompare._image)) {
                    SixLabors.ImageSharp.ImageExtensions.SaveAsPng(maskImage, fileStreamDifferenceMask);
                }
            }
        }

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

        public void Crop(Rectangle rectangle) {
            _image.Mutate(x => x.Crop(rectangle));
        }
        public void Dither() {
            _image.Mutate(x => x.Dither());
        }

        public void DrawLines(Color color, float thickness, PointF pointF) {
            _image.Mutate(x => x.DrawLine(color, thickness, pointF));
        }

        public void DrawPolygon(Color color, float thickness, PointF pointF) {
            _image.Mutate(x => x.DrawPolygon(color, thickness, pointF));
        }

        public void Filter(ColorMatrix colorMatrix) {
            _image.Mutate(x => x.Filter(colorMatrix));
        }

        public void Fill(Color color) {
            _image.Mutate(x => x.Fill(color));
        }

        public void Fill(Color color, Rectangle rectangle) {
            _image.Mutate(x => x.Fill(color, rectangle));
        }

        public void Flip(FlipMode flipMode) {
            _image.Mutate(x => x.Flip(flipMode));
        }

        public void GaussianBlur(float? sigma) {
            if (sigma != null) {
                _image.Mutate(x => x.GaussianBlur(sigma.Value));
            } else {
                _image.Mutate(x => x.GaussianBlur());
            }
        }

        public void GaussianSharpen(float? sigma) {
            if (sigma != null) {
                _image.Mutate(x => x.GaussianSharpen(sigma.Value));
            } else {
                _image.Mutate(x => x.GaussianSharpen());
            }
        }
        public void HistogramEqualization() {
            _image.Mutate(x => x.HistogramEqualization());
        }

        public void Hue(float degrees) {
            _image.Mutate(x => x.Hue(degrees));
        }

        public void Grayscale(GrayscaleMode grayscaleMode = GrayscaleMode.Bt709) {
            _image.Mutate(x => x.Grayscale(grayscaleMode));
        }

        public void Kodachrome() {
            _image.Mutate(x => x.Kodachrome());
        }

        public void Lightness(float amount) {
            _image.Mutate(x => x.Lightness(amount));
        }
        public void Lomograph() {
            _image.Mutate(x => x.Lomograph());
        }

        public void Invert() {
            _image.Mutate(x => x.Invert());
        }

        public void Opacity(float amount) {
            _image.Mutate(x => x.Opacity(amount));
        }

        public void Polaroid() {
            _image.Mutate(x => x.Polaroid());
        }

        public void Pixelate() {
            _image.Mutate(x => x.Pixelate());
        }

        public void Pixelate(int size) {
            _image.Mutate(x => x.Pixelate(size));
        }

        public void OilPaint() {
            _image.Mutate(x => x.OilPaint());
        }

        public void OilPaint(int levels, int brushSize) {
            _image.Mutate(x => x.OilPaint(levels, brushSize));
        }

        public void Rotate(RotateMode rotateMode) {
            _image.Mutate(x => x.Rotate(rotateMode));
        }

        public void Rotate(float degrees) {
            _image.Mutate(x => x.Rotate(degrees: degrees));
        }

        public void RotateFlip(RotateMode rotateMode, FlipMode flipMode) {
            _image.Mutate(x => x.RotateFlip(rotateMode, flipMode));
        }

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

        public void Saturate(float amount) {
            _image.Mutate(x => x.Saturate(amount));
        }

        public void Sepia() {
            _image.Mutate(x => x.Sepia());
        }

        public void Sepia(float amount) {
            _image.Mutate(x => x.Sepia(amount));
        }

        public void Skew(float degreesX, float degreesY) {
            _image.Mutate(x => x.Skew(degreesX, degreesY));
        }

        public void Vignette() {
            _image.Mutate(x => x.Vignette());
        }

        public void Vignette(Color color) {
            _image.Mutate(x => x.Vignette(color));
        }

        public static SixLabors.ImageSharp.Image GetImage(string filePath) {
            string fullPath = Helpers.ResolvePath(filePath);
            using (var inStream = System.IO.File.OpenRead(fullPath)) {
                return SixLabors.ImageSharp.Image.Load(inStream);
            }
        }

        public void Create(string filePath, int width, int height) {
            _filePath = filePath;
            _image = new Image<Rgba32>(width, height);
        }

        public static Image Load(string filePath) {
            string fullPath = Helpers.ResolvePath(filePath);

            Image image = new Image {
                _filePath = fullPath,
                _image = SixLabors.ImageSharp.Image.Load(fullPath)
            };

            return image;
        }

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

        public void Save(bool openImage) {
            Save("", openImage, null, null);
        }

        public void Save(Stream stream, int? quality = null, int? compressionLevel = null) {
            string extension = System.IO.Path.GetExtension(_filePath)?.ToLowerInvariant();
            var encoder = Helpers.GetEncoder(extension, quality, compressionLevel);
            _image.Save(stream, encoder);
            stream.Seek(0, SeekOrigin.Begin);
        }

        public MemoryStream ToStream(int? quality = null, int? compressionLevel = null) {
            var ms = new MemoryStream();
            Save(ms, quality, compressionLevel);
            return ms;
        }

        public void Dispose() {
            if (_image != null) {
                _image.Dispose();
            }
        }
    }
}

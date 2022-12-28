using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Metadata;
using SixLabors.ImageSharp.Processing;

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

        public void Crop(Rectangle rectangle) {
            _image.Mutate(x => x.Crop(rectangle));
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

        public void Hue(float degrees) {
            _image.Mutate(x => x.Hue(degrees));
        }

        public void Grayscale(GrayscaleMode grayscaleMode = GrayscaleMode.Bt709) {
            _image.Mutate(x => x.Grayscale(grayscaleMode));
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

        public void Resize(int width, int height) {
            _image.Mutate(x => x.Resize(width, height));
        }

        public void Resize(int percentage) {
            _image.Mutate(x => x.Resize(_image.Width * percentage / 100, _image.Height * percentage / 100));
        }

        public void Saturate(float amount) {
            _image.Mutate(x => x.Saturate(amount));
        }

        public static SixLabors.ImageSharp.Image GetImage(string filePath) {
            var inStream = System.IO.File.OpenRead(filePath);
            using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(inStream)) {
                return image;
            }
        }

        public static Image Load(string filePath) {
            Image image = new Image();
            image._filePath = filePath;

            var inStream = System.IO.File.OpenRead(filePath);
            image._image = SixLabors.ImageSharp.Image.Load(inStream);
            inStream.Close();
            inStream.Dispose();

            return image;
        }

        public void Save(string filePath = "", bool openImage = false) {
            if (filePath == "") {
                filePath = _filePath;
            }
            _image.Save(filePath);
            Helpers.Open(filePath, openImage);
        }

        public void Save(bool openImage) {
            Save("", openImage);
        }

        public void Dispose() {
            if (_image != null) {
                _image.Dispose();
            }
        }
    }
}

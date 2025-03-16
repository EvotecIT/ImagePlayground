using System;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using FontStyle = SixLabors.Fonts.FontStyle;
using PointF = SixLabors.ImageSharp.PointF;
using Path = System.IO.Path;
using SLImage = SixLabors.ImageSharp.Image;

namespace ImagePlayground {
    public partial class Image : IDisposable {

        public enum WatermarkPlacement {
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight,
            Middle,
        }

        public void Watermark(string text, float x, float y, Color color, float fontSize = 16f, string fontFamilyName = "Arial", float padding = 18f) {
            if (!SystemFonts.TryGet(fontFamilyName, out var fontFamily)) {
                throw new Exception($"Couldn't find font {fontFamilyName}");
            }

            var font = fontFamily.CreateFont(fontSize, FontStyle.Regular);
            var pointF = new PointF(x, y);
            _image.Mutate(mx => mx.DrawText(text, font, color, pointF));

        }

        public void Watermark(string text, WatermarkPlacement placement, Color color, float fontSize = 16f, string fontFamilyName = "Arial", float padding = 18f) {
            var textSize = GetTextSize(text, fontSize, fontFamilyName);

            switch (placement)
            {
                case WatermarkPlacement.TopLeft:
                    Watermark(text, padding, padding, color, fontSize, fontFamilyName, padding);
                    break;
                case WatermarkPlacement.TopRight:
                    Watermark(text, _image.Width - textSize.Width - padding, padding, color, fontSize, fontFamilyName, padding);
                    break;
                case WatermarkPlacement.BottomLeft:
                    Watermark(text, padding, _image.Height - textSize.Height - padding, color, fontSize, fontFamilyName, padding);
                    break;
                case WatermarkPlacement.BottomRight:
                    Watermark(text, _image.Width - textSize.Width - padding, _image.Height - textSize.Height - padding, color, fontSize, fontFamilyName, padding);
                    break;
                case WatermarkPlacement.Middle:
                    Watermark(text, (_image.Width - textSize.Width) / 2, (_image.Height - textSize.Height) / 2, color, fontSize, fontFamilyName, padding);
                    break;
            }
        }

        public void WatermarkImage(string filePath, WatermarkPlacement placement, float opacity = 1f, float padding = 18f, int rotate = 0, FlipMode flipMode = FlipMode.None, int watermarkPercentage = 20) {
            string fullPath = Path.GetFullPath(filePath);
            var location = new Point(0, 0);
            using var image = SLImage.Load(fullPath);
            var watermarkWidth = _image.Width * watermarkPercentage / 100;
            var watermarkHeight = watermarkWidth * image.Height / image.Width;

            // rotate watermark
            if (rotate == 0)
            {
                image.Mutate(mx => mx.Resize(watermarkWidth, watermarkHeight).Flip(flipMode));
            }
            else
            {
                image.Mutate(mx => mx.Resize(watermarkWidth, watermarkHeight).Flip(flipMode).Rotate(rotate));
            }
            switch (placement)
            {
                case WatermarkPlacement.TopLeft:
                    location = new Point((int)padding, (int)padding);
                    break;
                case WatermarkPlacement.TopRight:
                    location = new Point((int)(_image.Width - image.Width - padding), (int)padding);
                    break;
                case WatermarkPlacement.BottomLeft:
                    location = new Point((int)padding, (int)(_image.Height - image.Height - padding));
                    break;
                case WatermarkPlacement.BottomRight:
                    location = new Point((int)(_image.Width - image.Width - padding), (int)(_image.Height - image.Height - padding));
                    break;
                case WatermarkPlacement.Middle:
                    location = new Point((_image.Width - image.Width) / 2, (_image.Height - image.Height) / 2);
                    break;
            }
            AddImage(image, location, opacity);
        }

        public void WatermarkImage(string filePath, int x, int y, float opacity = 1f, int rotate = 0, FlipMode flipMode = FlipMode.None, int watermarkPercentage = 20) {
            string fullPath = Path.GetFullPath(filePath);

            var location = new Point(x, y);
            using var image = SLImage.Load(fullPath);
            var watermarkWidth = _image.Width * watermarkPercentage / 100;
            var watermarkHeight = watermarkWidth * image.Height / image.Width;

            // rotate watermark
            if (rotate == 0)
            {
                image.Mutate(mx => mx.Resize(watermarkWidth, watermarkHeight).Flip(flipMode));
            }
            else
            {
                image.Mutate(mx => mx.Resize(watermarkWidth, watermarkHeight).Flip(flipMode).Rotate(rotate));
            }
            AddImage(image, location, opacity);
        }
    }
}

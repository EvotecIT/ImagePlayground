using System;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using FontStyle = SixLabors.Fonts.FontStyle;
using PointF = SixLabors.ImageSharp.PointF;

namespace ImagePlayground;
public partial class Image : IDisposable {

    public enum WatermarkPlacement {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        Middle,
    }

    public void Watermark(string text, float x, float y, SixLabors.ImageSharp.Color color, float fontSize = 16f, string fontFamilyName = "Arial", float padding = 18f) {
        if (!SystemFonts.TryGet(fontFamilyName, out var fontFamily)) {
            throw new Exception($"Couldn't find font {fontFamilyName}");
        }

        var font = fontFamily.CreateFont(fontSize, FontStyle.Regular);
        //var styles = fontFamily.GetAvailableStyles();
        //var families = SystemFonts.Families;

        var pointF = new PointF(x, y);
        _image.Mutate(mx => mx.DrawText(text, font, color, pointF));

    }

    public void Watermark(string text, WatermarkPlacement placement, SixLabors.ImageSharp.Color color, float fontSize = 16f, string fontFamilyName = "Arial", float padding = 18f) {
        var textSize = GetTextSize(text, fontSize, fontFamilyName);

        if (placement == WatermarkPlacement.TopLeft) {
            Watermark(text, padding, padding, color, fontSize, fontFamilyName, padding);
        } else if (placement == WatermarkPlacement.TopRight) {
            Watermark(text, _image.Width - textSize.Width - padding, padding, color, fontSize, fontFamilyName, padding);
        } else if (placement == WatermarkPlacement.BottomLeft) {
            Watermark(text, padding, _image.Height - textSize.Height - padding, color, fontSize, fontFamilyName, padding);
        } else if (placement == WatermarkPlacement.BottomRight) {
            Watermark(text, _image.Width - textSize.Width - padding, _image.Height - textSize.Height - padding, color, fontSize, fontFamilyName, padding);
        } else if (placement == WatermarkPlacement.Middle) {
            Watermark(text, (_image.Width - textSize.Width) / 2, (_image.Height - textSize.Height) / 2, color, fontSize, fontFamilyName, padding);
        }
    }

    /// <summary>
    /// Adds an image watermark using a predefined placement.
    /// </summary>
    /// <param name="filePath">Path to the watermark image.</param>
    /// <param name="placement">Placement for the watermark.</param>
    /// <param name="opacity">Opacity of the watermark.</param>
    /// <param name="padding">Padding around the watermark.</param>
    /// <param name="rotate">Rotation angle.</param>
    /// <param name="flipMode">Flip mode for the watermark.</param>
    /// <param name="watermarkPercentage">Size of the watermark in percent (1-100).</param>
    public void WatermarkImage(string filePath, WatermarkPlacement placement, float opacity = 1f, float padding = 18f, int rotate = 0, FlipMode flipMode = FlipMode.None, int watermarkPercentage = 20) {
        string fullPath = Helpers.ResolvePath(filePath);
        if (watermarkPercentage < 1 || watermarkPercentage > 100) {
            throw new ArgumentOutOfRangeException(nameof(watermarkPercentage), "Watermark percentage must be between 1 and 100.");
        }

        var location = new Point(0, 0);
        using (var image = SixLabors.ImageSharp.Image.Load(fullPath)) {
            var watermarkWidth = image.Width * watermarkPercentage / 100;
            var watermarkHeight = image.Height * watermarkPercentage / 100;

            if (watermarkPercentage != 100 || rotate != 0 || flipMode != FlipMode.None) {
                image.Mutate(mx => {
                    if (watermarkPercentage != 100) {
                        mx.Resize(watermarkWidth, watermarkHeight);
                    }

                    if (flipMode != FlipMode.None) {
                        mx.Flip(flipMode);
                    }

                    if (rotate != 0) {
                        mx.Rotate(rotate);
                    }
                });
            }

            if (placement == WatermarkPlacement.TopLeft) {
                location = new Point((int)padding, (int)padding);
            } else if (placement == WatermarkPlacement.TopRight) {
                location = new Point((int)(_image.Width - image.Width - padding), (int)padding);
            } else if (placement == WatermarkPlacement.BottomLeft) {
                location = new Point((int)padding, (int)(_image.Height - image.Height - padding));
            } else if (placement == WatermarkPlacement.BottomRight) {
                location = new Point((int)(_image.Width - image.Width - padding), (int)(_image.Height - image.Height - padding));
            } else if (placement == WatermarkPlacement.Middle) {
                location = new Point((int)((_image.Width - image.Width) / 2), (int)((_image.Height - image.Height) / 2));
            }
            AddImage(image, location, opacity);
        }
    }

    /// <summary>
    /// Adds an image watermark at the specified coordinates.
    /// </summary>
    /// <param name="filePath">Path to the watermark image.</param>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    /// <param name="opacity">Opacity of the watermark.</param>
    /// <param name="rotate">Rotation angle.</param>
    /// <param name="flipMode">Flip mode for the watermark.</param>
    /// <param name="watermarkPercentage">Size of the watermark in percent (1-100).</param>
    public void WatermarkImage(string filePath, int x, int y, float opacity = 1f, int rotate = 0, FlipMode flipMode = FlipMode.None, int watermarkPercentage = 20) {
        string fullPath = Helpers.ResolvePath(filePath);
        if (watermarkPercentage < 1 || watermarkPercentage > 100) {
            throw new ArgumentOutOfRangeException(nameof(watermarkPercentage), "Watermark percentage must be between 1 and 100.");
        }

        var location = new Point(x, y);
        using (var image = SixLabors.ImageSharp.Image.Load(fullPath)) {
            var watermarkWidth = image.Width * watermarkPercentage / 100;
            var watermarkHeight = image.Height * watermarkPercentage / 100;

            // apply changes
            if (watermarkPercentage != 100 || rotate != 0 || flipMode != FlipMode.None) {
                image.Mutate(mx => {
                    if (watermarkPercentage != 100) {
                        mx.Resize(watermarkWidth, watermarkHeight);
                    }

                    if (flipMode != FlipMode.None) {
                        mx.Flip(flipMode);
                    }

                    if (rotate != 0) {
                        mx.Rotate(rotate);
                    }
                });
            }
            AddImage(image, location, opacity);
        }
    }

    public void WatermarkImageTiled(string filePath, int spacing, float opacity = 1f, int rotate = 0, FlipMode flipMode = FlipMode.None, int watermarkPercentage = 20) {
        string fullPath = Helpers.ResolvePath(filePath);
        if (watermarkPercentage < 1 || watermarkPercentage > 100) {
            throw new ArgumentOutOfRangeException(nameof(watermarkPercentage), "Watermark percentage must be between 1 and 100.");
        }

        using (var image = SixLabors.ImageSharp.Image.Load(fullPath)) {
            var watermarkWidth = image.Width * watermarkPercentage / 100;
            var watermarkHeight = image.Height * watermarkPercentage / 100;

            if (watermarkPercentage != 100 || rotate != 0 || flipMode != FlipMode.None) {
                image.Mutate(mx => {
                    if (watermarkPercentage != 100) {
                        mx.Resize(watermarkWidth, watermarkHeight);
                    }

                    if (flipMode != FlipMode.None) {
                        mx.Flip(flipMode);
                    }

                    if (rotate != 0) {
                        mx.Rotate(rotate);
                    }
                });
            }

            for (int y = spacing; y < _image.Height; y += image.Height + spacing) {
                for (int x = spacing; x < _image.Width; x += image.Width + spacing) {
                    AddImage(image, x, y, opacity);
                }
            }
        }
    }
}

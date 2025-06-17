using System;
using System.Linq;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;

namespace ImagePlayground {
    public partial class Image : IDisposable {
        public FontRectangle GetTextSize(string text, float fontSize, string fontFamilyName) {
            if (!SixLabors.Fonts.SystemFonts.TryGet(fontFamilyName, out var fontFamily)) {
                if (!SixLabors.Fonts.SystemFonts.TryGet("DejaVu Sans", out fontFamily)) {
                    fontFamily = SixLabors.Fonts.SystemFonts.Collection.Families.First();
                }
            }
            var font = fontFamily.CreateFont(fontSize, FontStyle.Regular);
            var options = new TextOptions(font) { Dpi = 72, KerningMode = KerningMode.Auto };
            var textSize = TextMeasurer.MeasureSize(text, options);
            return textSize;
        }

        public void AddText(float x, float y, string text, SixLabors.ImageSharp.Color color, float fontSize = 16f, string fontFamilyName = "Arial", SixLabors.ImageSharp.Color? shadowColor = null, float shadowOffsetX = 0f, float shadowOffsetY = 0f, SixLabors.ImageSharp.Color? outlineColor = null, float outlineWidth = 0f) {
            if (!SixLabors.Fonts.SystemFonts.TryGet(fontFamilyName, out var fontFamily)) {
                if (!SixLabors.Fonts.SystemFonts.TryGet("DejaVu Sans", out fontFamily)) {
                    fontFamily = SixLabors.Fonts.SystemFonts.Collection.Families.First();
                }
            }
            var font = fontFamily.CreateFont(fontSize, FontStyle.Regular);
            var pointF = new PointF(x, y);
            _image.Mutate(mx => {
                if (shadowColor != null) {
                    var shadowPoint = new PointF(x + shadowOffsetX, y + shadowOffsetY);
                    mx.DrawText(text, font, shadowColor.Value, shadowPoint);
                }
                if (outlineColor != null && outlineWidth > 0f) {
                    mx.DrawText(text, font, Brushes.Solid(outlineColor.Value), Pens.Solid(outlineColor.Value, outlineWidth), pointF);
                }
                mx.DrawText(text, font, color, pointF);
            });
        }
    }
}

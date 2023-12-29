using System;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;

namespace ImagePlayground {
    public partial class Image : IDisposable {
        public FontRectangle GetTextSize(string text, float fontSize, string fontFamilyName) {
            if (!SixLabors.Fonts.SystemFonts.TryGet(fontFamilyName, out var fontFamily)) {
                throw new Exception($"Couldn't find font {fontFamilyName}");
            }
            var font = fontFamily.CreateFont(fontSize, FontStyle.Regular);
            var options = new TextOptions(font) { Dpi = 72, KerningMode = KerningMode.Auto };
            var textSize = TextMeasurer.Measure(text, options);
            return textSize;
        }

        public void AddText(float x, float y, string text, SixLabors.ImageSharp.Color color, float fontSize = 16f, string fontFamilyName = "Arial") {
            if (!SixLabors.Fonts.SystemFonts.TryGet(fontFamilyName, out var fontFamily)) {
                throw new Exception($"Couldn't find font {fontFamilyName}");
            }
            var font = fontFamily.CreateFont(fontSize, FontStyle.Regular);
            var pointF = new PointF(x, y);
            _image.Mutate(mx => mx.DrawText(text, font, color, pointF));
        }
    }
}

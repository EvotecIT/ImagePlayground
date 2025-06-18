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
            var options = new SixLabors.Fonts.TextOptions(font) { Dpi = 72, KerningMode = KerningMode.Auto };
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

        public void AddTextBox(float x, float y, string text, float boxWidth, SixLabors.ImageSharp.Color color, float boxHeight = 0f, float fontSize = 16f, string fontFamilyName = "Arial", SixLabors.Fonts.HorizontalAlignment horizontalAlignment = SixLabors.Fonts.HorizontalAlignment.Left, SixLabors.Fonts.VerticalAlignment verticalAlignment = SixLabors.Fonts.VerticalAlignment.Top, SixLabors.ImageSharp.Color? shadowColor = null, float shadowOffsetX = 0f, float shadowOffsetY = 0f, SixLabors.ImageSharp.Color? outlineColor = null, float outlineWidth = 0f) {
            if (!SixLabors.Fonts.SystemFonts.TryGet(fontFamilyName, out var fontFamily)) {
                if (!SixLabors.Fonts.SystemFonts.TryGet("DejaVu Sans", out fontFamily)) {
                    fontFamily = SixLabors.Fonts.SystemFonts.Collection.Families.First();
                }
            }

            var font = fontFamily.CreateFont(fontSize, FontStyle.Regular);
            var options = new SixLabors.ImageSharp.Drawing.Processing.RichTextOptions(font) {
                Dpi = 72,
                WrappingLength = boxWidth,
                HorizontalAlignment = horizontalAlignment,
                VerticalAlignment = verticalAlignment,
                Origin = new PointF(x, y),
            };

            _image.Mutate(mx => {
                if (boxHeight > 0) {
                    var rect = new RectangularPolygon(x, y, boxWidth, boxHeight);
                    mx.Clip(rect, ctx => DrawTextInternal(ctx, options, text, color, shadowColor, shadowOffsetX, shadowOffsetY, outlineColor, outlineWidth));
                } else {
                    DrawTextInternal(mx, options, text, color, shadowColor, shadowOffsetX, shadowOffsetY, outlineColor, outlineWidth);
                }
            });
        }

        private static void DrawTextInternal(IImageProcessingContext ctx, SixLabors.ImageSharp.Drawing.Processing.RichTextOptions options, string text, SixLabors.ImageSharp.Color color, SixLabors.ImageSharp.Color? shadowColor, float shadowOffsetX, float shadowOffsetY, SixLabors.ImageSharp.Color? outlineColor, float outlineWidth) {
            if (shadowColor != null) {
                var shadowOptions = options;
                shadowOptions.Origin = new PointF(options.Origin.X + shadowOffsetX, options.Origin.Y + shadowOffsetY);
                ctx.DrawText(shadowOptions, text, shadowColor.Value);
            }
            if (outlineColor != null && outlineWidth > 0f) {
                ctx.DrawText(options, text, Brushes.Solid(outlineColor.Value), Pens.Solid(outlineColor.Value, outlineWidth));
            }
            ctx.DrawText(options, text, color);
        }
    }
}

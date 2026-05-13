using System;
using System.Linq;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;

namespace ImagePlayground;
/// <summary>
/// Provides image manipulation operations.
/// </summary>
public partial class Image : IDisposable {
    /// <summary>
    /// Calculates the dimensions required to render <paramref name="text"/> using the specified font.
    /// </summary>
    /// <param name="text">The text to measure.</param>
    /// <param name="fontSize">Font size in points.</param>
    /// <param name="fontFamilyName">Name of the font family to use.</param>
    /// <returns>The measured text rectangle.</returns>
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

    /// <summary>
    /// Draws a text string at the specified location.
    /// </summary>
    /// <param name="x">The x-coordinate of the text origin.</param>
    /// <param name="y">The y-coordinate of the text origin.</param>
    /// <param name="text">The text to render.</param>
    /// <param name="color">Text color.</param>
    /// <param name="fontSize">Font size in points.</param>
    /// <param name="fontFamilyName">Font family name.</param>
    /// <param name="shadowColor">Optional shadow color.</param>
    /// <param name="shadowOffsetX">Horizontal shadow offset.</param>
    /// <param name="shadowOffsetY">Vertical shadow offset.</param>
    /// <param name="outlineColor">Optional outline color.</param>
    /// <param name="outlineWidth">Outline thickness.</param>
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

    /// <summary>
    /// Draws text confined to a bounding box.
    /// </summary>
    /// <param name="x">X coordinate of the bounding box origin.</param>
    /// <param name="y">Y coordinate of the bounding box origin.</param>
    /// <param name="text">Text to render.</param>
    /// <param name="boxWidth">Width of the text box.</param>
    /// <param name="color">Text color.</param>
    /// <param name="fontSize">Font size in points.</param>
    /// <param name="fontFamilyName">Font family name.</param>
    /// <param name="horizontalAlignment">Horizontal alignment within the box.</param>
    /// <param name="verticalAlignment">Vertical alignment within the box.</param>
    /// <param name="shadowColor">Optional shadow color.</param>
    /// <param name="shadowOffsetX">Horizontal shadow offset.</param>
    /// <param name="shadowOffsetY">Vertical shadow offset.</param>
    /// <param name="outlineColor">Optional outline color.</param>
    /// <param name="outlineWidth">Outline thickness.</param>
    public void AddTextBox(float x, float y, string text, float boxWidth, SixLabors.ImageSharp.Color color, float fontSize = 16f, string fontFamilyName = "Arial", SixLabors.Fonts.HorizontalAlignment horizontalAlignment = SixLabors.Fonts.HorizontalAlignment.Left, SixLabors.Fonts.VerticalAlignment verticalAlignment = SixLabors.Fonts.VerticalAlignment.Top, SixLabors.ImageSharp.Color? shadowColor = null, float shadowOffsetX = 0f, float shadowOffsetY = 0f, SixLabors.ImageSharp.Color? outlineColor = null, float outlineWidth = 0f) =>
        AddTextBox(x, y, text, boxWidth, 0f, color, fontSize, fontFamilyName, horizontalAlignment, verticalAlignment, shadowColor, shadowOffsetX, shadowOffsetY, outlineColor, outlineWidth);

    /// <summary>
    /// Draws text confined to a bounding box with explicit height.
    /// </summary>
    /// <param name="x">X coordinate of the box.</param>
    /// <param name="y">Y coordinate of the box.</param>
    /// <param name="text">Text to render.</param>
    /// <param name="boxWidth">Box width.</param>
    /// <param name="boxHeight">Box height.</param>
    /// <param name="color">Text color.</param>
    /// <param name="fontSize">Font size in points.</param>
    /// <param name="fontFamilyName">Font family name.</param>
    /// <param name="horizontalAlignment">Horizontal alignment inside the box.</param>
    /// <param name="verticalAlignment">Vertical alignment inside the box.</param>
    /// <param name="shadowColor">Optional shadow color.</param>
    /// <param name="shadowOffsetX">Horizontal shadow offset.</param>
    /// <param name="shadowOffsetY">Vertical shadow offset.</param>
    /// <param name="outlineColor">Optional outline color.</param>
    /// <param name="outlineWidth">Outline thickness.</param>
    public void AddTextBox(float x, float y, string text, float boxWidth, float boxHeight, SixLabors.ImageSharp.Color color, float fontSize = 16f, string fontFamilyName = "Arial", SixLabors.Fonts.HorizontalAlignment horizontalAlignment = SixLabors.Fonts.HorizontalAlignment.Left, SixLabors.Fonts.VerticalAlignment verticalAlignment = SixLabors.Fonts.VerticalAlignment.Top, SixLabors.ImageSharp.Color? shadowColor = null, float shadowOffsetX = 0f, float shadowOffsetY = 0f, SixLabors.ImageSharp.Color? outlineColor = null, float outlineWidth = 0f) {
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

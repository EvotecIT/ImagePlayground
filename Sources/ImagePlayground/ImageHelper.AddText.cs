using System.IO;
using SixLabors.ImageSharp;

namespace ImagePlayground;
public partial class ImageHelper {
    public static void AddText(string filePath, string outFilePath, float x, float y, string text, Color color, float fontSize = 16f, string fontFamilyName = "Arial", Color? shadowColor = null, float shadowOffsetX = 0f, float shadowOffsetY = 0f, Color? outlineColor = null, float outlineWidth = 0f) {
        string fullPath = Helpers.ResolvePath(filePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);

        using var img = Image.Load(fullPath);
        img.AddText(x, y, text, color, fontSize, fontFamilyName, shadowColor, shadowOffsetX, shadowOffsetY, outlineColor, outlineWidth);
        img.Save(outFullPath);
    }

    public static void AddTextBox(string filePath, string outFilePath, float x, float y, string text, float boxWidth, Color color, float fontSize = 16f, string fontFamilyName = "Arial", SixLabors.Fonts.HorizontalAlignment horizontalAlignment = SixLabors.Fonts.HorizontalAlignment.Left, SixLabors.Fonts.VerticalAlignment verticalAlignment = SixLabors.Fonts.VerticalAlignment.Top, Color? shadowColor = null, float shadowOffsetX = 0f, float shadowOffsetY = 0f, Color? outlineColor = null, float outlineWidth = 0f) =>
        AddTextBox(filePath, outFilePath, x, y, text, boxWidth, 0f, color, fontSize, fontFamilyName, horizontalAlignment, verticalAlignment, shadowColor, shadowOffsetX, shadowOffsetY, outlineColor, outlineWidth);

    public static void AddTextBox(string filePath, string outFilePath, float x, float y, string text, float boxWidth, float boxHeight, Color color, float fontSize = 16f, string fontFamilyName = "Arial", SixLabors.Fonts.HorizontalAlignment horizontalAlignment = SixLabors.Fonts.HorizontalAlignment.Left, SixLabors.Fonts.VerticalAlignment verticalAlignment = SixLabors.Fonts.VerticalAlignment.Top, Color? shadowColor = null, float shadowOffsetX = 0f, float shadowOffsetY = 0f, Color? outlineColor = null, float outlineWidth = 0f) {
        string fullPath = Helpers.ResolvePath(filePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);

        using var img = Image.Load(fullPath);
        img.AddTextBox(x, y, text, boxWidth, boxHeight, color, fontSize, fontFamilyName, horizontalAlignment, verticalAlignment, shadowColor, shadowOffsetX, shadowOffsetY, outlineColor, outlineWidth);
        img.Save(outFullPath);
    }
}

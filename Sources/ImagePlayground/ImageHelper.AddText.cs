using System.IO;
using SixLabors.ImageSharp;

namespace ImagePlayground;
/// <summary>
/// Provides helper methods for image manipulation.
/// </summary>
/// <remarks>
/// These methods operate on file paths and delegate actual drawing to the <see cref="Image"/> class.
/// </remarks>
public partial class ImageHelper {
    /// <summary>
    /// Adds text to an image file and saves the result.
    /// </summary>
    /// <param name="filePath">Source image path.</param>
    /// <param name="outFilePath">Destination image path.</param>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    /// <param name="text">Text to render.</param>
    /// <param name="color">Text color.</param>
    /// <param name="fontSize">Font size.</param>
    /// <param name="fontFamilyName">Font family name.</param>
    /// <param name="shadowColor">Optional shadow color.</param>
    /// <param name="shadowOffsetX">Shadow offset X.</param>
    /// <param name="shadowOffsetY">Shadow offset Y.</param>
    /// <param name="outlineColor">Optional outline color.</param>
    /// <param name="outlineWidth">Outline width.</param>
    public static void AddText(string filePath, string outFilePath, float x, float y, string text, Color color, float fontSize = 16f, string fontFamilyName = "Arial", Color? shadowColor = null, float shadowOffsetX = 0f, float shadowOffsetY = 0f, Color? outlineColor = null, float outlineWidth = 0f) {
        string fullPath = Helpers.ResolvePath(filePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);

        using var img = Image.Load(fullPath);
        img.AddText(x, y, text, color, fontSize, fontFamilyName, shadowColor, shadowOffsetX, shadowOffsetY, outlineColor, outlineWidth);
        img.Save(outFullPath);
    }

    /// <summary>
    /// Adds text within a bounding box to an image file and saves the result.
    /// </summary>
    /// <param name="filePath">Source image path.</param>
    /// <param name="outFilePath">Destination image path.</param>
    /// <param name="x">X coordinate of the box.</param>
    /// <param name="y">Y coordinate of the box.</param>
    /// <param name="text">Text to render.</param>
    /// <param name="boxWidth">Width of the box.</param>
    /// <param name="color">Text color.</param>
    /// <param name="fontSize">Font size.</param>
    /// <param name="fontFamilyName">Font family name.</param>
    /// <param name="horizontalAlignment">Horizontal alignment.</param>
    /// <param name="verticalAlignment">Vertical alignment.</param>
    /// <param name="shadowColor">Optional shadow color.</param>
    /// <param name="shadowOffsetX">Shadow offset X.</param>
    /// <param name="shadowOffsetY">Shadow offset Y.</param>
    /// <param name="outlineColor">Optional outline color.</param>
    /// <param name="outlineWidth">Outline width.</param>
    public static void AddTextBox(string filePath, string outFilePath, float x, float y, string text, float boxWidth, Color color, float fontSize = 16f, string fontFamilyName = "Arial", SixLabors.Fonts.HorizontalAlignment horizontalAlignment = SixLabors.Fonts.HorizontalAlignment.Left, SixLabors.Fonts.VerticalAlignment verticalAlignment = SixLabors.Fonts.VerticalAlignment.Top, Color? shadowColor = null, float shadowOffsetX = 0f, float shadowOffsetY = 0f, Color? outlineColor = null, float outlineWidth = 0f) =>
        AddTextBox(filePath, outFilePath, x, y, text, boxWidth, 0f, color, fontSize, fontFamilyName, horizontalAlignment, verticalAlignment, shadowColor, shadowOffsetX, shadowOffsetY, outlineColor, outlineWidth);

    /// <summary>
    /// Adds text within a bounding box to an image file and saves the result.
    /// </summary>
    /// <param name="filePath">Source image path.</param>
    /// <param name="outFilePath">Destination image path.</param>
    /// <param name="x">X coordinate of the box.</param>
    /// <param name="y">Y coordinate of the box.</param>
    /// <param name="text">Text to render.</param>
    /// <param name="boxWidth">Width of the box.</param>
    /// <param name="boxHeight">Height of the box.</param>
    /// <param name="color">Text color.</param>
    /// <param name="fontSize">Font size.</param>
    /// <param name="fontFamilyName">Font family name.</param>
    /// <param name="horizontalAlignment">Horizontal alignment.</param>
    /// <param name="verticalAlignment">Vertical alignment.</param>
    /// <param name="shadowColor">Optional shadow color.</param>
    /// <param name="shadowOffsetX">Shadow offset X.</param>
    /// <param name="shadowOffsetY">Shadow offset Y.</param>
    /// <param name="outlineColor">Optional outline color.</param>
    /// <param name="outlineWidth">Outline width.</param>
    public static void AddTextBox(string filePath, string outFilePath, float x, float y, string text, float boxWidth, float boxHeight, Color color, float fontSize = 16f, string fontFamilyName = "Arial", SixLabors.Fonts.HorizontalAlignment horizontalAlignment = SixLabors.Fonts.HorizontalAlignment.Left, SixLabors.Fonts.VerticalAlignment verticalAlignment = SixLabors.Fonts.VerticalAlignment.Top, Color? shadowColor = null, float shadowOffsetX = 0f, float shadowOffsetY = 0f, Color? outlineColor = null, float outlineWidth = 0f) {
        string fullPath = Helpers.ResolvePath(filePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);

        using var img = Image.Load(fullPath);
        img.AddTextBox(x, y, text, boxWidth, boxHeight, color, fontSize, fontFamilyName, horizontalAlignment, verticalAlignment, shadowColor, shadowOffsetX, shadowOffsetY, outlineColor, outlineWidth);
        img.Save(outFullPath);
    }
}

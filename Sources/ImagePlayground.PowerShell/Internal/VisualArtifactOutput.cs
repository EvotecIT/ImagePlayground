using System;
using System.IO;
using ChartForgeX.VisualArtifacts;

namespace ImagePlayground.PowerShell;

/// <summary>Writes a visual artifact to the format named by the destination extension.</summary>
/// <remarks>SVG, HTML, and PNG use the ChartForgeX renderers. Other raster formats (JPEG, WebP, GIF, BMP, TIFF, TGA)
/// render the same ChartForgeX pixels and encode them through the ImagePlayground image encoder.</remarks>
internal static class VisualArtifactOutput {
    internal const string SupportedExtensions = ".svg, .html, .htm, .png, .jpg, .jpeg, .webp, .gif, .bmp, .tif, .tiff, or .tga";

    public static bool IsSupported(string extension) => IsNative(extension) || TryGetRasterType(extension, out _);

    public static bool IsNative(string extension) =>
        IsSvg(extension) || IsHtml(extension) || extension.Equals(".png", StringComparison.OrdinalIgnoreCase);

    public static void Save(VisualArtifact artifact, string output, VisualArtifactRenderOptions? options = null) {
        if (artifact == null) throw new ArgumentNullException(nameof(artifact));
        options ??= new VisualArtifactRenderOptions();
        var extension = Path.GetExtension(output);
        var directory = Path.GetDirectoryName(output);
        if (!string.IsNullOrWhiteSpace(directory)) {
            Directory.CreateDirectory(directory!);
        }

        if (IsSvg(extension)) {
            artifact.SaveSvg(output, options);
        } else if (IsHtml(extension)) {
            artifact.SaveHtml(output, options);
        } else if (extension.Equals(".png", StringComparison.OrdinalIgnoreCase)) {
            artifact.SavePng(output, options);
        } else if (TryGetRasterType(extension, out var type)) {
            using var image = Image.FromRgbaImage(artifact.ToRgbaImage(options), type);
            image.Save(output);
        } else {
            throw new NotSupportedException("Visual output supports " + SupportedExtensions + " file extensions.");
        }
    }

    internal static bool IsSvg(string extension) => extension.Equals(".svg", StringComparison.OrdinalIgnoreCase);

    internal static bool IsHtml(string extension) =>
        extension.Equals(".html", StringComparison.OrdinalIgnoreCase) || extension.Equals(".htm", StringComparison.OrdinalIgnoreCase);

    private static bool TryGetRasterType(string extension, out ImageType type) {
        switch ((extension ?? string.Empty).ToLowerInvariant()) {
            case ".jpg":
            case ".jpeg":
                type = ImageType.Jpeg;
                return true;
            case ".webp":
                type = ImageType.WebP;
                return true;
            case ".gif":
                type = ImageType.Gif;
                return true;
            case ".bmp":
                type = ImageType.Bmp;
                return true;
            case ".tif":
            case ".tiff":
                type = ImageType.Tiff;
                return true;
            case ".tga":
                type = ImageType.Tga;
                return true;
            default:
                type = ImageType.Png;
                return false;
        }
    }
}

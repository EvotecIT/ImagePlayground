using ChartForgeX.Primitives;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ImagePlayground.PowerShell;

internal static class ChartColorConverter {
    public static ChartColor? Convert(Color? color) {
        if (!color.HasValue) {
            return null;
        }

        Rgba32 rgba = color.Value.ToPixel<Rgba32>();
        return ChartColor.FromRgba(rgba.R, rgba.G, rgba.B, rgba.A);
    }

    public static ChartColor[] Convert(Color[] colors) {
        var converted = new ChartColor[colors.Length];
        for (var i = 0; i < colors.Length; i++) {
            Rgba32 rgba = colors[i].ToPixel<Rgba32>();
            converted[i] = ChartColor.FromRgba(rgba.R, rgba.G, rgba.B, rgba.A);
        }
        return converted;
    }
}

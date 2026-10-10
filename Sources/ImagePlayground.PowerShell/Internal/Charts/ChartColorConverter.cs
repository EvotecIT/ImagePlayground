using ChartForgeX.Primitives;
using OfficeIMO.Drawing;

namespace ImagePlayground.PowerShell;

internal static class ChartColorConverter {
    public static ChartColor? Convert(ChartColor? color) => color;

    public static ChartColor[] Convert(ChartColor[] colors) => colors;

    public static ChartColor? Convert(OfficeColor? color) {
        if (!color.HasValue) {
            return null;
        }

        OfficeColor rgba = color.Value;
        return ChartColor.FromRgba(rgba.R, rgba.G, rgba.B, rgba.A);
    }

    public static ChartColor[] Convert(OfficeColor[] colors) {
        var converted = new ChartColor[colors.Length];
        for (var i = 0; i < colors.Length; i++) {
            OfficeColor rgba = colors[i];
            converted[i] = ChartColor.FromRgba(rgba.R, rgba.G, rgba.B, rgba.A);
        }
        return converted;
    }
}

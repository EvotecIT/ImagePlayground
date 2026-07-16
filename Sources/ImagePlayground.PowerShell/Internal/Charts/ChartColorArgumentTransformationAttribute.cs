using System;
using System.Collections;
using System.Globalization;
using System.Management.Automation;
using System.Reflection;
using ChartForgeX.Primitives;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ImagePlayground.PowerShell;

/// <summary>Converts PowerShell-friendly color values into ChartForgeX colors.</summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
internal sealed class ChartColorArgumentTransformationAttribute : ArgumentTransformationAttribute {
    public override object? Transform(EngineIntrinsics engineIntrinsics, object? inputData) {
        if (inputData == null) {
            return null;
        }

        if (inputData is IEnumerable enumerable && inputData is not string && inputData is not ChartColor && inputData is not Color) {
            var colors = new ArrayList();
            foreach (var item in enumerable) {
                colors.Add(ConvertOne(item));
            }

            return colors.ToArray(typeof(ChartColor));
        }

        return ConvertOne(inputData);
    }

    private static ChartColor ConvertOne(object? value) {
        if (value == null) {
            throw new PSArgumentException("Color value cannot be null.");
        }

        if (value is PSObject psObject) {
            value = psObject.BaseObject;
        }

        if (value is ChartColor chartColor) {
            return chartColor;
        }

        if (value is Color imageColor) {
            return FromImageSharp(imageColor);
        }

        if (value is string text) {
            return Parse(text);
        }

        throw new PSArgumentException("Color must be a ChartForgeX ChartColor, ImageSharp color, known color name, or hex value.");
    }

    private static ChartColor Parse(string value) {
        if (string.IsNullOrWhiteSpace(value)) {
            throw new PSArgumentException("Color value cannot be empty.");
        }

        if (TryParseWithChartForgeX(value, out var color)) {
            return color;
        }

        try {
            return ChartColor.FromHex(value);
        } catch (ArgumentException) {
            // Fall back to ImageSharp named colors while ChartForgeX named colors are unavailable in older packages.
        }

        if (Color.TryParse(value, out var imageColor)) {
            return FromImageSharp(imageColor);
        }

        throw new PSArgumentException(string.Format(CultureInfo.InvariantCulture, "Color '{0}' is not a valid ChartForgeX color name or hex value.", value));
    }

    private static bool TryParseWithChartForgeX(string value, out ChartColor color) {
        color = default;

        var parse = typeof(ChartColor).GetMethod("TryParse", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string), typeof(ChartColor).MakeByRefType() }, null);
        if (parse != null) {
            object?[] args = { value, default(ChartColor) };
            if (parse.Invoke(null, args) is true && args[1] is ChartColor parsed) {
                color = parsed;
                return true;
            }
        }

        var chartColors = typeof(ChartColor).Assembly.GetType("ChartForgeX.Primitives.ChartColors");
        var tryGet = chartColors?.GetMethod("TryGet", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string), typeof(ChartColor).MakeByRefType() }, null);
        if (tryGet != null) {
            object?[] args = { value, default(ChartColor) };
            if (tryGet.Invoke(null, args) is true && args[1] is ChartColor parsed) {
                color = parsed;
                return true;
            }
        }

        return false;
    }

    private static ChartColor FromImageSharp(Color color) {
        Rgba32 rgba = color.ToPixel<Rgba32>();
        return ChartColor.FromRgba(rgba.R, rgba.G, rgba.B, rgba.A);
    }
}

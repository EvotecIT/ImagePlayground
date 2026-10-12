using System;
using System.Collections;
using System.Globalization;
using System.Management.Automation;
using ChartForgeX.Primitives;
using OfficeIMO.Drawing;

namespace ImagePlayground.PowerShell;

/// <summary>Converts PowerShell-friendly color values into ChartForgeX colors.</summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
internal sealed class ChartColorArgumentTransformationAttribute : ArgumentTransformationAttribute {
    public override object? Transform(EngineIntrinsics engineIntrinsics, object? inputData) {
        if (inputData == null) {
            return null;
        }

        if (inputData is IEnumerable enumerable && inputData is not string && inputData is not ChartColor && inputData is not OfficeColor) {
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

        if (value is OfficeColor imageColor) {
            return FromOfficeColor(imageColor);
        }

        if (value is string text) {
            return Parse(text);
        }

        throw new PSArgumentException("Color must be a ChartForgeX ChartColor, OfficeIMO OfficeColor, known color name, or hex value.");
    }

    private static ChartColor Parse(string value) {
        if (string.IsNullOrWhiteSpace(value)) {
            throw new PSArgumentException("Color value cannot be empty.");
        }

        if (ChartColor.TryParse(value, out var color)) {
            return color;
        }

        throw new PSArgumentException(string.Format(CultureInfo.InvariantCulture, "Color '{0}' is not a valid ChartForgeX color name or hex value.", value));
    }

    private static ChartColor FromOfficeColor(OfficeColor color) =>
        ChartColor.FromRgba(color.R, color.G, color.B, color.A);
}

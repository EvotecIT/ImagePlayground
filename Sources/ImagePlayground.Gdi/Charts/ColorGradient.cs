using System.Drawing;
using ImagePlayground.Gdi;

namespace ImagePlayground.Gdi.Charts;

/// <summary>Defines a color stop for gradients.</summary>
public readonly struct ColorStop {
    /// <summary>Position from 0 to 1.</summary>
    public float Position { get; }

    /// <summary>Stop color.</summary>
    public Color Color { get; }

    /// <summary>Creates a color stop.</summary>
    public ColorStop(float position, Color color) {
        Position = GdiMath.Clamp(position, 0f, 1f);
        Color = color;
    }
}

/// <summary>Defines a linear color gradient.</summary>
public sealed class ColorGradient {
    /// <summary>Gradient stops.</summary>
    public IReadOnlyList<ColorStop> Stops { get; }

    /// <summary>Creates a gradient with the specified stops.</summary>
    public ColorGradient(IReadOnlyList<ColorStop> stops) {
        Stops = stops.Count == 0 ? new[] { new ColorStop(0f, Color.Blue), new ColorStop(1f, Color.Red) } : stops;
    }

    /// <summary>Returns a color for the normalized value.</summary>
    public Color GetColor(float t) {
        t = GdiMath.Clamp(t, 0f, 1f);
        var sorted = Stops.OrderBy(s => s.Position).ToArray();
        if (t <= sorted[0].Position) return sorted[0].Color;
        if (t >= sorted[sorted.Length - 1].Position) return sorted[sorted.Length - 1].Color;
        for (var i = 0; i < sorted.Length - 1; i++) {
            var a = sorted[i];
            var b = sorted[i + 1];
            if (t >= a.Position && t <= b.Position) {
                var range = b.Position - a.Position;
                var localT = range <= 0 ? 0f : (t - a.Position) / range;
                return Lerp(a.Color, b.Color, localT);
            }
        }
        return sorted[sorted.Length - 1].Color;
    }

    private static Color Lerp(Color a, Color b, float t) {
        var r = (int)Math.Round(a.R + (b.R - a.R) * t);
        var g = (int)Math.Round(a.G + (b.G - a.G) * t);
        var bl = (int)Math.Round(a.B + (b.B - a.B) * t);
        var al = (int)Math.Round(a.A + (b.A - a.A) * t);
        return Color.FromArgb(al, r, g, bl);
    }
}

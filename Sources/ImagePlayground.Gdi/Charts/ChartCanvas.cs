using System.Drawing;

namespace ImagePlayground.Gdi.Charts;

/// <summary>Defines canvas size and padding for chart rendering.</summary>
public sealed class ChartCanvas {
    /// <summary>Canvas width in pixels.</summary>
    public int Width { get; set; } = 800;

    /// <summary>Canvas height in pixels.</summary>
    public int Height { get; set; } = 450;

    /// <summary>Padding around the plot area.</summary>
    public ChartPadding Padding { get; set; } = new(60, 40, 40, 60);
}

/// <summary>Represents padding values in pixels.</summary>
public readonly struct ChartPadding {
    /// <summary>Left padding.</summary>
    public float Left { get; }

    /// <summary>Top padding.</summary>
    public float Top { get; }

    /// <summary>Right padding.</summary>
    public float Right { get; }

    /// <summary>Bottom padding.</summary>
    public float Bottom { get; }

    /// <summary>Creates a new padding definition.</summary>
    public ChartPadding(float left, float top, float right, float bottom) {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }

    /// <summary>Creates padding with the same value for all sides.</summary>
    public ChartPadding(float all) {
        Left = all;
        Top = all;
        Right = all;
        Bottom = all;
    }

    internal RectangleF Apply(RectangleF rect) {
        return new RectangleF(
            rect.X + Left,
            rect.Y + Top,
            Math.Max(0, rect.Width - Left - Right),
            Math.Max(0, rect.Height - Top - Bottom));
    }
}

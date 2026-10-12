namespace ImagePlayground;

/// <summary>A rectangular pixel region used by image cropping and drawing operations.</summary>
public readonly struct Rectangle {
    /// <summary>Creates a rectangular pixel region.</summary>
    public Rectangle(int x, int y, int width, int height) {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    /// <summary>Left edge in pixels.</summary>
    public int X { get; }
    /// <summary>Top edge in pixels.</summary>
    public int Y { get; }
    /// <summary>Width in pixels.</summary>
    public int Width { get; }
    /// <summary>Height in pixels.</summary>
    public int Height { get; }
}
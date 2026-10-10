namespace ImagePlayground;

/// <summary>Axis along which image pixels are mirrored.</summary>
public enum FlipMode {
    /// <summary>Keep the image orientation.</summary>
    None,
    /// <summary>Mirror the image from left to right.</summary>
    Horizontal,
    /// <summary>Mirror the image from top to bottom.</summary>
    Vertical
}

/// <summary>Clockwise rotations in quarter turns.</summary>
public enum RotateMode {
    /// <summary>Keep the image orientation.</summary>
    None = 0,
    /// <summary>Rotate clockwise by ninety degrees.</summary>
    Rotate90 = 90,
    /// <summary>Rotate clockwise by one hundred eighty degrees.</summary>
    Rotate180 = 180,
    /// <summary>Rotate clockwise by two hundred seventy degrees.</summary>
    Rotate270 = 270
}
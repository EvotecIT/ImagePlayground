using System;
using System.Collections.Generic;
using System.Text;

namespace ImagePlayground;

/// <summary>
/// Supported image container formats.
/// </summary>
public enum ImageTypes {
    /// <summary>Bitmap image format.</summary>
    Bmp,
    /// <summary>GIF image format.</summary>
    Gif,
    /// <summary>JPEG image format.</summary>
    Jpeg,
    /// <summary>Portable bitmap format.</summary>
    Pbm,
    /// <summary>PNG image format.</summary>
    Png,
    /// <summary>TGA image format.</summary>
    Tga,
    /// <summary>TIFF image format.</summary>
    Tiff,
    /// <summary>WebP image format.</summary>
    WebP
}

/// <summary>
/// Describes a relative placement for combining images.
/// </summary>
public enum ImagePlacement {
    /// <summary>Place image at the bottom of the canvas.</summary>
    Bottom,
    /// <summary>Place image on the right side.</summary>
    Right,
    /// <summary>Place image at the top of the canvas.</summary>
    Top,
    /// <summary>Place image on the left side.</summary>
    Left
}

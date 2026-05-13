namespace ImagePlayground;

/// <summary>
/// Resampling algorithms used when resizing images.
/// </summary>
public enum Sampler
{
    /// <summary>Nearest neighbour sampling.</summary>
    NearestNeighbor,
    /// <summary>Box resampling.</summary>
    Box,
    /// <summary>Triangle resampling.</summary>
    Triangle,
    /// <summary>Hermite resampling.</summary>
    Hermite,
    /// <summary>Lanczos with a radius of 2.</summary>
    Lanczos2,
    /// <summary>Lanczos with a radius of 3.</summary>
    Lanczos3,
    /// <summary>Lanczos with a radius of 5.</summary>
    Lanczos5,
    /// <summary>Lanczos with a radius of 8.</summary>
    Lanczos8,
    /// <summary>Mitchell–Netravali filter.</summary>
    MitchellNetravali,
    /// <summary>Catmull–Rom spline.</summary>
    CatmullRom,
    /// <summary>Robidoux filter.</summary>
    Robidoux,
    /// <summary>Sharper variant of Robidoux filter.</summary>
    RobidouxSharp,
    /// <summary>B-Spline resampling.</summary>
    Spline,
    /// <summary>Welch resampling.</summary>
    Welch
}

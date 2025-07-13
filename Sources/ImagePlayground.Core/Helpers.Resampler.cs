using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;

namespace ImagePlayground;
public static partial class Helpers {
    /// <summary>
    /// Maps the <see cref="Sampler"/> enumeration to a concrete resampler implementation.
    /// </summary>
    /// <param name="sampler">Sampler option.</param>
    /// <returns>Corresponding image resampler.</returns>
    public static IResampler GetResampler(Sampler sampler) =>
        sampler switch {
            Sampler.NearestNeighbor => KnownResamplers.NearestNeighbor,
            Sampler.Box => KnownResamplers.Box,
            Sampler.Triangle => KnownResamplers.Triangle,
            Sampler.Hermite => KnownResamplers.Hermite,
            Sampler.Lanczos2 => KnownResamplers.Lanczos2,
            Sampler.Lanczos3 => KnownResamplers.Lanczos3,
            Sampler.Lanczos5 => KnownResamplers.Lanczos5,
            Sampler.Lanczos8 => KnownResamplers.Lanczos8,
            Sampler.MitchellNetravali => KnownResamplers.MitchellNetravali,
            Sampler.CatmullRom => KnownResamplers.CatmullRom,
            Sampler.Robidoux => KnownResamplers.Robidoux,
            Sampler.RobidouxSharp => KnownResamplers.RobidouxSharp,
            Sampler.Spline => KnownResamplers.Spline,
            Sampler.Welch => KnownResamplers.Welch,
            _ => KnownResamplers.Bicubic,
        };
}

using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;

namespace ImagePlayground;
/// <summary>
/// Resolves sampler options to ImageSharp resampler instances.
/// </summary>
public static partial class Helpers {
    /// <summary>
    /// Maps the <see cref="Image.Sampler"/> enumeration to a concrete resampler implementation.
    /// </summary>
    /// <param name="sampler">Sampler option.</param>
    /// <returns>Corresponding image resampler.</returns>
    public static IResampler GetResampler(Image.Sampler sampler) =>
        sampler switch {
            Image.Sampler.NearestNeighbor => KnownResamplers.NearestNeighbor,
            Image.Sampler.Box => KnownResamplers.Box,
            Image.Sampler.Triangle => KnownResamplers.Triangle,
            Image.Sampler.Hermite => KnownResamplers.Hermite,
            Image.Sampler.Lanczos2 => KnownResamplers.Lanczos2,
            Image.Sampler.Lanczos3 => KnownResamplers.Lanczos3,
            Image.Sampler.Lanczos5 => KnownResamplers.Lanczos5,
            Image.Sampler.Lanczos8 => KnownResamplers.Lanczos8,
            Image.Sampler.MitchellNetravali => KnownResamplers.MitchellNetravali,
            Image.Sampler.CatmullRom => KnownResamplers.CatmullRom,
            Image.Sampler.Robidoux => KnownResamplers.Robidoux,
            Image.Sampler.RobidouxSharp => KnownResamplers.RobidouxSharp,
            Image.Sampler.Spline => KnownResamplers.Spline,
            Image.Sampler.Welch => KnownResamplers.Welch,
            _ => KnownResamplers.Bicubic,
        };
}

namespace ImagePlayground;

/// <summary>Maps public sampler selections to their shared raster kernels.</summary>
public static partial class Helpers {
    /// <summary>Resolves a sampler without substituting another kernel.</summary>
    public static OfficeRasterResamplingMode GetResampler(Sampler sampler) {
        if (!Enum.IsDefined(typeof(Sampler), sampler)) {
            throw new ArgumentOutOfRangeException(nameof(sampler));
        }
        return (OfficeRasterResamplingMode)Enum.Parse(typeof(OfficeRasterResamplingMode), sampler.ToString());
    }
}
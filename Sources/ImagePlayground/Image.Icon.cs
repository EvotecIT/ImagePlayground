namespace ImagePlayground;

/// <summary>Icon export using the shared managed ICO writer.</summary>
public partial class Image {
    /// <summary>Saves the first frame as an ICO containing the requested square resolutions.</summary>
    public void SaveAsIcon(string filePath, params int[] sizes) {
        EnsureUsable();
        if (sizes == null || sizes.Length == 0) { sizes = new[] { 16, 32, 48, 64, 128, 256 }; }
        int[] dimensions = sizes.Distinct().OrderBy(size => size).ToArray();
        if (dimensions.Any(size => size < 1 || size > 256)) { throw new ArgumentOutOfRangeException(nameof(sizes), "ICO resolutions must be between 1 and 256 pixels."); }
        var images = dimensions.Select(size => OfficeRasterResampler.Resize(Raster, size, size, OfficeRasterResamplingMode.Bicubic)).ToArray();
        byte[] encoded = OfficeIconEncoder.Encode(images);
        string fullPath = Helpers.ResolvePath(filePath);
        Helpers.CreateParentDirectory(fullPath);
        OfficeImageFileWriter.WriteAllBytes(fullPath, encoded);
    }
}

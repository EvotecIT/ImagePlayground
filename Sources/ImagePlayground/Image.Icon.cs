namespace ImagePlayground;

/// <summary>Icon export using the shared managed ICO writer.</summary>
public partial class Image {
    /// <summary>Saves the first frame as an ICO containing the requested square resolutions.</summary>
    public void SaveAsIcon(string filePath, params int[] sizes) => SaveAsIcon(filePath, sizes, default);
    /// <summary>Saves an ICO while observing cancellation during resizing, encoding and atomic writing.</summary>
    public void SaveAsIcon(string filePath, IReadOnlyList<int> sizes, CancellationToken cancellationToken) {
        EnsureUsable();
        cancellationToken.ThrowIfCancellationRequested();
        if (sizes == null || sizes.Count == 0) { sizes = new[] { 16, 32, 48, 64, 128, 256 }; }
        int[] dimensions = sizes.Distinct().OrderBy(size => size).ToArray();
        if (dimensions.Any(size => size < 1 || size > 256)) { throw new ArgumentOutOfRangeException(nameof(sizes), "ICO resolutions must be between 1 and 256 pixels."); }
        var images = dimensions.Select(size => OfficeRasterResampler.Resize(Raster, new OfficeRasterResizeOptions { Width = size, Height = size, Fit = OfficeImageFit.Stretch }, cancellationToken)).ToArray();
        byte[] encoded = OfficeRasterImageEncoder.EncodeWithMetadata(new OfficeRasterFrames(images.Select(image => new OfficeRasterFrame(image))), OfficeImageExportFormat.Icon, _metadata, cancellationToken: cancellationToken).EncodedBytes;
        string fullPath = Helpers.ResolvePath(filePath);
        Helpers.CreateParentDirectory(fullPath);
        OfficeImageFileWriter.WriteAllBytes(fullPath, encoded, cancellationToken);
    }
}

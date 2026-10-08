namespace ImagePlayground;

/// <summary>Avatar and transparent corner workflows.</summary>
public partial class Image {
    /// <summary>Fits the image to the requested canvas with centered cropping and rounded corners.</summary>
    public void Avatar(int width, int height, float cornerRadius) => Apply(source => {
        if (width <= 0 || height <= 0) throw new ArgumentOutOfRangeException(nameof(width));
        double scale = Math.Max(width / (double)source.Width, height / (double)source.Height);
        var resized = OfficeRasterResampler.Resize(source, Math.Max(width, checked((int)Math.Ceiling(source.Width*scale))), Math.Max(height, checked((int)Math.Ceiling(source.Height*scale))), OfficeRasterResamplingMode.Bicubic);
        var cropped = OfficeRasterTransforms.Crop(resized, (resized.Width-width)/2, (resized.Height-height)/2, width, height);
        return OfficeRasterTransforms.MaskRoundedRectangle(cropped, cornerRadius);
    }, _ => (width, height));
    /// <summary>Applies a rounded avatar and saves it to a file.</summary>
    public void SaveAsAvatar(string filePath, int width, int height, float cornerRadius) { Avatar(width,height,cornerRadius); Save(filePath); }
    /// <summary>Applies a rounded avatar and writes it to a caller-owned stream.</summary>
    public void SaveAsAvatar(Stream stream, int width, int height, float cornerRadius) { Avatar(width,height,cornerRadius); Save(stream); }
    /// <summary>Applies a circular avatar and saves it to a file.</summary>
    public void SaveAsCircularAvatar(string filePath, int size) { Avatar(size,size,size/2f); Save(filePath); }
    /// <summary>Applies a circular avatar and writes it to a caller-owned stream.</summary>
    public void SaveAsCircularAvatar(Stream stream, int size) { Avatar(size,size,size/2f); Save(stream); }
}
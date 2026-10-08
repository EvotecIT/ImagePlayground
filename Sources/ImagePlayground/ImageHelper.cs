using ChartForgeX.Composition;
using ChartForgeX.Primitives;
using ChartForgeX.Raster;

namespace ImagePlayground;

/// <summary>File workflows over the shared managed raster and composition engines.</summary>
public partial class ImageHelper {
    /// <summary>Converts a decoded image to the format selected by the output extension.</summary>
    public static void ConvertTo(string filePath, string outFilePath, int? quality = null, int? compressionLevel = null) {
        string input = Helpers.ResolvePath(filePath);
        string output = Helpers.ResolvePath(outFilePath);
        if (Path.GetExtension(output).Equals(".ico", StringComparison.OrdinalIgnoreCase) &&
            Path.GetExtension(input).Equals(".ico", StringComparison.OrdinalIgnoreCase)) {
            Helpers.CreateParentDirectory(output);
            File.Copy(input, output, true);
            return;
        }
        using var image = Image.Load(input);
        image.Save(output, quality: quality, compressionLevel: compressionLevel);
    }
    /// <summary>Resizes all image frames and saves the result, fitting within supplied bounds when preserving aspect ratio.</summary>
    public static void Resize(string filePath, string outFilePath, int? width, int? height, bool keepAspectRatio = true, Sampler? sampler = null) {
        using var image = Image.Load(filePath); image.Resize(width, height, keepAspectRatio, sampler); image.Save(outFilePath);
    }
    /// <summary>Loads, resizes, and saves while observing cancellation.</summary>
    public static async Task ResizeAsync(string filePath, string outFilePath, int? width, int? height, bool keepAspectRatio = true, Sampler? sampler = null, CancellationToken cancellationToken = default) {
        cancellationToken.ThrowIfCancellationRequested(); using var image = await Image.LoadAsync(filePath, cancellationToken).ConfigureAwait(false);
        image.Resize(width, height, keepAspectRatio, sampler); await image.SaveAsync(outFilePath, cancellationToken: cancellationToken).ConfigureAwait(false);
    }
    /// <summary>Returns independently resized raster pixels, fitting within supplied bounds when preserving aspect ratio; the input remains unchanged.</summary>
    public static OfficeRasterImage Resize(OfficeRasterImage image, int? width, int? height, bool keepAspectRatio = true, Sampler? sampler = null) {
        var dimensions = GetResizeDimensions(image, width, height, keepAspectRatio);
        return OfficeRasterResampler.Resize(image, dimensions.Width, dimensions.Height, sampler.HasValue ? Helpers.GetResampler(sampler.Value) : OfficeRasterResamplingMode.Bicubic);
    }

    internal static (int Width, int Height) GetResizeDimensions(OfficeRasterImage image, int? width, int? height, bool keepAspectRatio) {
        if (image == null) {
            throw new ArgumentNullException(nameof(image));
        }
        if (width <= 0 || height <= 0) {
            throw new ArgumentOutOfRangeException(nameof(width), "Requested dimensions must be positive.");
        }
        int outputWidth = width ?? image.Width, outputHeight = height ?? image.Height;
        if (keepAspectRatio) {
            if (width.HasValue && height.HasValue) {
                double scale = Math.Min(width.Value / (double)image.Width, height.Value / (double)image.Height);
                outputWidth = Math.Min(width.Value, Math.Max(1, checked((int)Math.Round(image.Width * scale))));
                outputHeight = Math.Min(height.Value, Math.Max(1, checked((int)Math.Round(image.Height * scale))));
            } else if (width.HasValue) {
                outputHeight = Math.Max(1, checked((int)Math.Round(width.Value * image.Height / (double)image.Width)));
            } else if (height.HasValue) {
                outputWidth = Math.Max(1, checked((int)Math.Round(height.Value * image.Width / (double)image.Height)));
            }
        }
        return (outputWidth, outputHeight);
    }
    /// <summary>Scales all image frames by a positive percentage and saves the result.</summary>
    public static void Resize(string filePath, string outFilePath, int percentage) {
        using var image = Image.Load(filePath); image.Resize(percentage); image.Save(outFilePath);
    }
    /// <summary>Scales and saves the image while observing cancellation.</summary>
    public static async Task ResizeAsync(string filePath, string outFilePath, int percentage, CancellationToken cancellationToken = default) {
        cancellationToken.ThrowIfCancellationRequested(); using var image = await Image.LoadAsync(filePath, cancellationToken).ConfigureAwait(false);
        image.Resize(percentage); await image.SaveAsync(outFilePath, cancellationToken: cancellationToken).ConfigureAwait(false);
    }
    /// <summary>Combines the first frames of two images at the requested relative placement.</summary>
    public static void Combine(string filePath, string filePath2, string outFilePath, bool resizeToFit = false, ImagePlacement imagePlacement = ImagePlacement.Bottom) {
        using var first = Image.Load(filePath); using var second = Image.Load(filePath2);
        if (!Enum.IsDefined(typeof(ImagePlacement), imagePlacement)) {
            throw new ArgumentOutOfRangeException(nameof(imagePlacement));
        }
        bool vertical = imagePlacement == ImagePlacement.Top || imagePlacement == ImagePlacement.Bottom;
        if (resizeToFit) {
            if (vertical) { int width = Math.Max(first.Width, second.Width); first.Resize(width, null); second.Resize(width, null); }
            else { int height = Math.Max(first.Height, second.Height); first.Resize(null, height); second.Resize(null, height); }
        }
        int outputWidth = vertical ? Math.Max(first.Width, second.Width) : checked(first.Width + second.Width);
        int outputHeight = vertical ? checked(first.Height + second.Height) : Math.Max(first.Height, second.Height);
        var composition = ImageComposition.CreateTransparent(outputWidth, outputHeight);
        bool secondFirst = imagePlacement == ImagePlacement.Top || imagePlacement == ImagePlacement.Left;
        Image leading = secondFirst ? second : first, trailing = secondFirst ? first : second;
        composition.DrawImage(ToChartImage(leading.Raster), 0, 0, leading.Width, leading.Height);
        composition.DrawImage(ToChartImage(trailing.Raster), vertical ? 0 : leading.Width, vertical ? leading.Height : 0, trailing.Width, trailing.Height);
        using var output = Image.FromRaster(ToOfficeImage(composition.ToImage())); output.Save(outFilePath);
    }
    /// <summary>Creates a background with a grid of randomly colored squares.</summary>
    public static void Create(string filePath, int width, int height, OfficeColor color, bool open = false) {
        if (width < 20) {
            throw new ArgumentOutOfRangeException(nameof(width), "Width must be at least twenty pixels.");
        }
        if (height < 20) {
            throw new ArgumentOutOfRangeException(nameof(height), "Height must be at least twenty pixels.");
        }
        var composition = ImageComposition.Create(width, height, ChartColor.FromRgba(color.R, color.G, color.B, color.A));
        var random = new Random();
        for (int y = 20; y < height / 20 * 20; y += 20) {
            for (int x = 20; x < width / 20 * 20; x += 20) composition.FillRectangle(x-14, y-14, 28, 28, ChartColor.FromRgba((byte)random.Next(255), (byte)random.Next(255), (byte)random.Next(255), 255));
        }
        using var image = Image.FromRaster(ToOfficeImage(composition.ToImage())); image.Save(filePath, open);
    }
    internal static RgbaImage ToChartImage(OfficeRasterImage image) => new RgbaImage(image.Width, image.Height, image.GetPixels());
    internal static OfficeRasterImage ToOfficeImage(RgbaImage image) => OfficeRasterImage.FromRgba32(image.Width, image.Height, image.Pixels);
}
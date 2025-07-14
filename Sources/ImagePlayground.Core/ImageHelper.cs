using System;
using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;
using Color = SixLabors.ImageSharp.Color;

namespace ImagePlayground;
/// <summary>
/// Provides helper methods for image manipulation.
/// </summary>
public partial class ImageHelper {
    /// <summary>
    /// Converts an image from one format to another.
    /// Following image formats are supported: bmp, gif, jpeg, pbm, png, tga, tiff, webp
    /// </summary>
    /// <param name="filePath">Source image path.</param>
    /// <param name="outFilePath">Destination image path.</param>
    /// <param name="quality">Optional quality value for lossy formats.</param>
    /// <param name="compressionLevel">Optional compression level for PNG/WebP.</param>
    /// <exception cref="UnknownImageFormatException"></exception>
    public static void ConvertTo(string filePath, string outFilePath, int? quality = null, int? compressionLevel = null) {
        string fullPath = Helpers.ResolvePath(filePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);
        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outFullPath)!);
        using (var inStream = System.IO.File.OpenRead(fullPath))
        using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(inStream)) {
            FileInfo fileInfo = new FileInfo(outFilePath);
            if (fileInfo.Extension == ".ico") {
                if (System.IO.Path.GetExtension(fullPath).Equals(".ico", StringComparison.OrdinalIgnoreCase)) {
                    System.IO.File.Copy(fullPath, outFullPath, true);
                } else {
                    throw new NotSupportedException("Conversion to .ico is only supported from .ico files.");
                }
            } else {
                var encoder = Helpers.GetEncoder(fileInfo.Extension, quality, compressionLevel);
                image.Save(outFullPath, encoder);
            }
        }
    }

    /// <summary>
    /// Resizes an image to the specified width and height.
    /// Following image formats are supported: GIF, JPEG, PNG, JFIF, GIF
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="outFilePath"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="keepAspectRatio"></param>
    /// <param name="sampler"></param>
    public static void Resize(string filePath, string outFilePath, int? width, int? height, bool keepAspectRatio = true, Sampler? sampler = null) {
        string fullPath = Helpers.ResolvePath(filePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);
        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outFullPath)!);

        using (var inStream = System.IO.File.OpenRead(fullPath))
        using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(inStream)) {
            Resize(image, width, height, keepAspectRatio, sampler);
            image.Save(outFullPath);
        }
    }

    /// <summary>
    /// Asynchronously resizes an image to the specified width and height.
    /// </summary>
    public static async Task ResizeAsync(string filePath, string outFilePath, int? width, int? height, bool keepAspectRatio = true, Sampler? sampler = null) {
        string fullPath = Helpers.ResolvePath(filePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);
        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outFullPath)!);

        await Task.Run(() => {
            using var inStream = System.IO.File.OpenRead(fullPath);
            using SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(inStream);
            Resize(image, width, height, keepAspectRatio, sampler);
            image.Save(outFullPath);
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Resizes the provided <paramref name="image"/> to the specified dimensions.
    /// </summary>
    /// <param name="image">Image instance to resize.</param>
    /// <param name="width">Desired width or <c>null</c> to auto calculate.</param>
    /// <param name="height">Desired height or <c>null</c> to auto calculate.</param>
    /// <param name="keepAspectRatio">Preserve the original aspect ratio.</param>
    /// <param name="sampler">Optional resampler algorithm.</param>
    /// <returns>Resized image instance.</returns>
    public static SixLabors.ImageSharp.Image Resize(SixLabors.ImageSharp.Image image, int? width, int? height, bool keepAspectRatio = true, Sampler? sampler = null) {
        if (width == null && height == null) {
            return image;
        }

        if (width != null && height != null && image.Width == width && image.Height == height) {
            return image;
        } else if (width != null && height == null && image.Width == width) {
            return image;
        } else if (height != null && width == null && image.Height == height) {
            return image;
        }

        var options = new ResizeOptions();
        if (keepAspectRatio && (width == null || height == null)) {
            options.Mode = ResizeMode.Max;
            options.Size = new Size(width ?? 0, height ?? 0);
        } else {
            options.Mode = ResizeMode.Stretch;
            options.Size = new Size(width ?? image.Width, height ?? image.Height);
        }

        if (sampler != null) {
            options.Sampler = Helpers.GetResampler(sampler.Value);
        }

        image.Mutate(x => x.Resize(options));
        return image;
    }

    /// <summary>
    /// Resizes an image to the specified percentage.
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="outFilePath"></param>
    /// <param name="percentage"></param>
    public static void Resize(string filePath, string outFilePath, int percentage) {
        if (percentage <= 0) {
            throw new ArgumentOutOfRangeException(nameof(percentage));
        }

        string fullPath = Helpers.ResolvePath(filePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);
        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outFullPath)!);

        using (var inStream = System.IO.File.OpenRead(fullPath))
        using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(inStream)) {
            int width = image.Width * percentage / 100;
            int height = image.Height * percentage / 100;
            Resize(image, width, height, false);
            image.Save(outFullPath);
        }
    }

    /// <summary>
    /// Asynchronously resizes an image to the specified percentage.
    /// </summary>
    public static async Task ResizeAsync(string filePath, string outFilePath, int percentage) {
        if (percentage <= 0) {
            throw new ArgumentOutOfRangeException(nameof(percentage));
        }

        string fullPath = Helpers.ResolvePath(filePath);
        string outFullPath = Helpers.ResolvePath(outFilePath);
        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outFullPath)!);

        await Task.Run(() => {
            using var inStream = System.IO.File.OpenRead(fullPath);
            using SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(inStream);
            int width = image.Width * percentage / 100;
            int height = image.Height * percentage / 100;
            Resize(image, width, height, false);
            image.Save(outFullPath);
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Combines two images either horizontally or vertically.
    /// </summary>
    /// <param name="filePath">Path to the first image.</param>
    /// <param name="filePath2">Path to the second image.</param>
    /// <param name="outFilePath">Destination path of the combined image.</param>
    /// <param name="resizeToFit">Resize images so they fit next to each other.</param>
    /// <param name="imagePlacement">Determines placement of <paramref name="filePath2"/>.</param>
    public static void Combine(string filePath, string filePath2, string outFilePath, bool resizeToFit = false, ImagePlacement imagePlacement = ImagePlacement.Bottom) {
        string fullPath = Helpers.ResolvePath(filePath);
        string fullPath2 = Helpers.ResolvePath(filePath2);
        string outFullPath = Helpers.ResolvePath(outFilePath);

        using (var inStream = System.IO.File.OpenRead(fullPath))
        using (var inStream2 = System.IO.File.OpenRead(fullPath2))
        using (SixLabors.ImageSharp.Image imageIn1 = SixLabors.ImageSharp.Image.Load(inStream)) {
            using (SixLabors.ImageSharp.Image imageIn2 = SixLabors.ImageSharp.Image.Load(inStream2)) {
                var image = imageIn1;
                var image2 = imageIn2;

                int outputWidth = 0;
                int outputHeight = 0;
                if (imagePlacement == ImagePlacement.Bottom) {
                    outputWidth = image.Width > image2.Width ? image.Width : image2.Width;
                    outputHeight = image.Height + image2.Height;
                    if (resizeToFit) {
                        image = Resize(image, outputWidth, null);
                        image2 = Resize(image2, outputWidth, null);
                    }
                } else if (imagePlacement == ImagePlacement.Top) {
                    outputWidth = image.Width > image2.Width ? image.Width : image2.Width;
                    outputHeight = image.Height + image2.Height;
                    if (resizeToFit) {
                        image = Resize(image, outputWidth, null);
                        image2 = Resize(image2, outputWidth, null);
                    }
                } else if (imagePlacement == ImagePlacement.Left) {
                    outputWidth = image.Width + image2.Width;
                    outputHeight = image.Height > image2.Height ? image.Height : image2.Height;
                    if (resizeToFit) {
                        image = Resize(image, null, outputHeight);
                        image2 = Resize(image2, null, outputHeight);
                    }
                } else if (imagePlacement == ImagePlacement.Right) {
                    outputWidth = image.Width + image2.Width;
                    outputHeight = image.Height > image2.Height ? image.Height : image2.Height;
                    if (resizeToFit) {
                        image = Resize(image, null, outputHeight);
                        image2 = Resize(image2, null, outputHeight);
                    }
                } else {
                    // this is not going to happen
                    throw new ArgumentException("Invalid ImagePlacement");
                }

                using (Image<Rgba32> outputImage = new Image<Rgba32>(outputWidth, outputHeight)) {
                    if (imagePlacement == ImagePlacement.Bottom) {
                        outputImage.Mutate(x => x
                            .DrawImage(image, new Point(0, 0), 1f)
                            .DrawImage(image2, new Point(0, image.Height), 1f)
                        );
                    } else if (imagePlacement == ImagePlacement.Top) {
                        outputImage.Mutate(x => x
                            .DrawImage(image2, new Point(0, 0), 1f)
                            .DrawImage(image, new Point(0, image2.Height), 1f)
                        );
                    } else if (imagePlacement == ImagePlacement.Left) {
                        outputImage.Mutate(x => x
                            .DrawImage(image2, new Point(0, 0), 1f)
                            .DrawImage(image, new Point(image2.Width, 0), 1f)
                        );
                    } else if (imagePlacement == ImagePlacement.Right) {
                        outputImage.Mutate(x => x
                            .DrawImage(image, new Point(0, 0), 1f)
                            .DrawImage(image2, new Point(image.Width, 0), 1f)
                        );
                    }

                    outputImage.Save(outFullPath);
                }
            }
        }

    }

    /// <summary>
    /// Generates a patterned image and saves it to disk.
    /// </summary>
    /// <param name="filePath">Destination image path.</param>
    /// <param name="width">Image width.</param>
    /// <param name="height">Image height.</param>
    /// <param name="color">Background color.</param>
    /// <param name="open">Open the image after saving.</param>
    public static void Create(string filePath, int width, int height, Color color, bool open = false) {
        string fullPath = Helpers.ResolvePath(filePath);
        if (width < 20) {
            throw new ArgumentOutOfRangeException(nameof(width), "Width must be at least 20.");
        }
        if (height < 20) {
            throw new ArgumentOutOfRangeException(nameof(height), "Height must be at least 20.");
        }
        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(fullPath)!);

        using (Image<Rgba32> outputImage = new Image<Rgba32>(width, height)) {
            //outputImage.Mutate(x => x.Fill(color));
            //outputImage.Mutate(x => x.BackgroundColor(color));

            int sizeRow = 20;
            int sizeColumn = 20;
            int rowCount = height / sizeRow;
            int columnCount = width / sizeColumn;
            int imageRadius = 20;

            outputImage.Mutate(ic => {
                ic.Fill(color);

                var rotation = GeometryUtilities.DegreeToRadian(45);
                var rand = new Random();

                for (var row = 1; row < rowCount; row++) {
                    for (var col = 1; col < columnCount; col++) {
                        var r = (byte)rand.Next(0, 255);
                        var g = (byte)rand.Next(0, 255);
                        var b = (byte)rand.Next(0, 255);
                        var squareColor = new Color(new Rgba32(r, g, b, 255));

                        var polygon = new RegularPolygon(sizeColumn * col, sizeRow * row, 4, imageRadius, rotation);
                        ic.Fill(squareColor, polygon);
                    }
                }
            });
            outputImage.Save(fullPath);
        }

        Helpers.Open(fullPath, open);
    }

}

using System;
using System.IO;
using ScottPlot;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;
using Color = SixLabors.ImageSharp.Color;
using Path = System.IO.Path;
using SLImage = SixLabors.ImageSharp.Image;

namespace ImagePlayground {
    public partial class ImageHelper {
        /// <summary>
        /// Converts an image from one format to another.
        /// Following image formats are supported: bmp, gif, jpeg, pbm, png, tga, tiff, webp
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="outFilePath"></param>
        /// <exception cref="UnknownImageFormatException"></exception>
        public static void ConvertTo(string filePath, string outFilePath) {
            string fullPath = Path.GetFullPath(filePath);
            string outFullPath = Path.GetFullPath(outFilePath);
            using var inStream = File.OpenRead(fullPath);
            using SLImage image = SLImage.Load(inStream);
            FileInfo fileInfo = new FileInfo(outFilePath);
            switch (fileInfo.Extension)
            {
                case ".png":
                    image.SaveAsPng(outFullPath);
                    break;
                case ".jpg":
                case ".jpeg":
                    image.SaveAsJpeg(outFullPath);
                    break;
                case ".bmp":
                    image.SaveAsBmp(outFullPath);
                    break;
                case ".gif":
                    image.SaveAsGif(outFullPath);
                    break;
                case ".pbm":
                    image.SaveAsPbm(outFullPath);
                    break;
                case ".tga":
                    image.SaveAsTga(outFullPath);
                    break;
                case ".tiff":
                    image.SaveAsTiff(outFullPath);
                    break;
                case ".webp":
                    image.SaveAsWebp(outFullPath);
                    break;
                case ".ico":
                    // maybe it will work?
                    File.Copy(fullPath, outFullPath, true);
                    break;
                default:
                    throw new UnknownImageFormatException("Image format not supported. Feel free to open an issue/fix it.");
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
        public static void Resize(string filePath, string outFilePath, int? width, int? height, bool keepAspectRatio = true, Image.Sampler? sampler = null) {
            string fullPath = System.IO.Path.GetFullPath(filePath);
            string outFullPath = System.IO.Path.GetFullPath(outFilePath);

            using var inStream = System.IO.File.OpenRead(fullPath);
            using SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(inStream);
            if (sampler == null)
            {
                if (keepAspectRatio == true)
                {
                    if (width != null && height != null)
                    {
                        image.Mutate(x => x.Resize(width.Value, height.Value));
                    }
                    else if (width != null)
                    {
                        var newWidth = width.Value;
                        var newHeight = 0;
                        image.Mutate(x => x.Resize(newWidth, newHeight));
                    }
                    else if (height != null)
                    {
                        var newHeight = height.Value;
                        var newWidth = 0;
                        image.Mutate(x => x.Resize(newWidth, newHeight));
                    }
                }
                else
                {
                    if (width != null && height != null)
                    {
                        image.Mutate(x => x.Resize(width.Value, height.Value));
                    }
                    else if (width != null)
                    {
                        image.Mutate(x => x.Resize(width.Value, image.Height));
                    }
                    else if (height != null)
                    {
                        image.Mutate(x => x.Resize(image.Width, height.Value));
                    }
                }
            }
            else
            {
                IResampler mySampler = null;
                mySampler = sampler switch {
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
                    _ => throw new ArgumentException("Invalid sampler type"),
                };
                if (keepAspectRatio == true)
                {
                    if (width != null && height != null)
                    {
                        image.Mutate(x => x.Resize(width.Value, height.Value, mySampler));
                    }
                    else if (width != null)
                    {
                        var newWidth = width.Value;
                        var newHeight = 0;
                        image.Mutate(x => x.Resize(newWidth, newHeight, mySampler));
                    }
                    else if (height != null)
                    {
                        var newHeight = height.Value;
                        var newWidth = 0;
                        image.Mutate(x => x.Resize(newWidth, newHeight, mySampler));
                    }
                }
                else
                {
                    if (width != null && height != null)
                    {
                        image.Mutate(x => x.Resize(width.Value, height.Value, mySampler));
                    }
                    else if (width != null)
                    {
                        image.Mutate(x => x.Resize(width.Value, image.Height, mySampler));
                    }
                    else if (height != null)
                    {
                        image.Mutate(x => x.Resize(image.Width, height.Value, mySampler));
                    }
                }
            }

            image.Save(outFullPath);
        }

        public static SixLabors.ImageSharp.Image Resize(SixLabors.ImageSharp.Image image, int? width, int? height, bool keepAspectRatio = true, Image.Sampler? sampler = null) {
            // lets try to keep the original image if possible
            if (width != null && height != null && image.Width == width && image.Height == height) {
                return image;
            } else if (width != null && image.Width == width) {
                return image;
            } else if (height != null && image.Height == height) {
                return image;
            }

            if (sampler == null) {
                if (keepAspectRatio == true) {
                    if (width != null && height != null) {
                        image.Mutate(x => x.Resize(width.Value, height.Value));
                    } else if (width != null) {
                        var newWidth = width.Value;
                        var newHeight = 0;
                        image.Mutate(x => x.Resize(newWidth, newHeight));
                    } else if (height != null) {
                        var newHeight = height.Value;
                        var newWidth = 0;
                        image.Mutate(x => x.Resize(newWidth, newHeight));
                    }
                } else {
                    if (width != null && height != null) {
                        image.Mutate(x => x.Resize(width.Value, height.Value));
                    } else if (width != null) {
                        image.Mutate(x => x.Resize(width.Value, image.Height));
                    } else if (height != null) {
                        image.Mutate(x => x.Resize(image.Width, height.Value));
                    }
                }
            } else {
                IResampler mySampler = null;
                mySampler = sampler switch {
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
                    _ => throw new ArgumentException("Invalid sampler type"),
                };
                image.Mutate(x => x.Resize(width.Value, height.Value));
                if (width != null) {
                        var newWidth = width.Value;
                        var newHeight = 0;
                        image.Mutate(x => x.Resize(newWidth, newHeight));
                    } else if (height != null) {
                        var newHeight = height.Value;
                        var newWidth = 0;
                        image.Mutate(x => x.Resize(newWidth, newHeight));
                    }
                else {
                    if (width != null && height != null) {
                        image.Mutate(x => x.Resize(width.Value, height.Value));
                    } else if (width != null) {
                        image.Mutate(x => x.Resize(width.Value, image.Height));
                    } else if (height != null) {
                        image.Mutate(x => x.Resize(image.Width, height.Value));
                    }
                }
            }

            return image;
        }

        /// <summary>
        /// Resizes an image to the specified percentage.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="outFilePath"></param>
        /// <param name="percentage"></param>
        public static void Resize(string filePath, string outFilePath, int percentage) {
            string fullPath = Path.GetFullPath(filePath);
            string outFullPath = Path.GetFullPath(outFilePath);

            using var inStream = File.OpenRead(fullPath);
            using SLImage image = SLImage.Load(inStream);
            int width = image.Width * percentage / 100;
            int height = image.Height * percentage / 100;
            if (image.Width == width && image.Height == height)
            {
                System.IO.File.Copy(fullPath, outFullPath, true);
            }
            else
            {
                image.Mutate(x => x.Resize(width, height));
                image.Save(outFullPath);
            }
        }

        public static void Combine(string filePath, string filePath2, string outFilePath, bool resizeToFit = false, ImagePlacement imagePlacement = ImagePlacement.Bottom) {
            string fullPath = Path.GetFullPath(filePath);
            string fullPath2 = Path.GetFullPath(filePath2);
            string outFullPath = Path.GetFullPath(outFilePath);

            using var inStream = File.OpenRead(fullPath);
            using var inStream2 = File.OpenRead(fullPath2);
            using SLImage imageIn1 = SLImage.Load(inStream);
            using SLImage imageIn2 = SLImage.Load(inStream2);
            var image = imageIn1;
            var image2 = imageIn2;

            int outputWidth = 0;
            int outputHeight = 0;

            switch (imagePlacement)
            {
                case ImagePlacement.Bottom:
                    outputWidth = image.Width > image2.Width ? image.Width : image2.Width;
                    outputHeight = image.Height + image2.Height;
                    break;
                case ImagePlacement.Top:
                    outputWidth = image.Width > image2.Width ? image.Width : image2.Width;
                    outputHeight = image.Height + image2.Height;
                    break;
                case ImagePlacement.Left:
                    outputWidth = image.Width + image2.Width;
                    outputHeight = image.Height > image2.Height ? image.Height : image2.Height;
                    break;
                case ImagePlacement.Right:
                    outputWidth = image.Width + image2.Width;
                    outputHeight = image.Height > image2.Height ? image.Height : image2.Height;
                    break;
            }
            if (resizeToFit)
            {
                if (imagePlacement == ImagePlacement.Bottom || imagePlacement == ImagePlacement.Top)
                {
                    image = Resize(image, outputWidth, null);
                    image2 = Resize(image2, outputWidth, null);
                }
                else
                {
                    image = Resize(image, null, outputHeight);
                    image2 = Resize(image2, null, outputHeight);
                }
            }
            using Image<Rgba32> outputImage = new Image<Rgba32>(outputWidth, outputHeight);
            switch (imagePlacement)
            {
                case ImagePlacement.Bottom:
                    outputImage.Mutate(x => x
                        .DrawImage(image, new Point(0, 0), 1f)
                        .DrawImage(image2, new Point(0, image.Height), 1f)
                    );
                    break;
                case ImagePlacement.Top:
                    outputImage.Mutate(x => x
                        .DrawImage(image2, new Point(0, 0), 1f)
                        .DrawImage(image, new Point(0, image2.Height), 1f)
                    );
                    break;
                case ImagePlacement.Left:
                    outputImage.Mutate(x => x
                        .DrawImage(image2, new Point(0, 0), 1f)
                        .DrawImage(image, new Point(image2.Width, 0), 1f)
                    );
                    break;
                case ImagePlacement.Right:
                    outputImage.Mutate(x => x
                        .DrawImage(image, new Point(0, 0), 1f)
                        .DrawImage(image2, new Point(image.Width, 0), 1f)
                    );
                    break;
            }
            outputImage.Save(outFullPath);
        }

        public static void Create(string filePath, int width, int height, Color color, bool open = false) {
            string fullPath = System.IO.Path.GetFullPath(filePath);

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


                    for (var row = 1; row < rowCount; row++) {
                        for (var col = 1; col < columnCount; col++) {
                            var rand = new Random();
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
}

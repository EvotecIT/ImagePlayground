﻿using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ImagePlayground {
    public class ImageHelper {
        /// <summary>
        /// Converts an image from one format to another.
        /// Following image formats are supported: bmp, gif, jpeg, pbm, png, tga, tiff, webp
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="outFilePath"></param>
        /// <exception cref="UnknownImageFormatException"></exception>
        public static void ConvertTo(string filePath, string outFilePath) {
            using (var inStream = System.IO.File.OpenRead(filePath)) {
                using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(inStream)) {
                    FileInfo fileInfo = new FileInfo(outFilePath);
                    if (fileInfo.Extension == ".png") {
                        image.SaveAsPng(outFilePath);
                    } else if (fileInfo.Extension == ".jpg" || fileInfo.Extension == ".jpeg") {
                        image.SaveAsJpeg(outFilePath);
                    } else if (fileInfo.Extension == ".bmp") {
                        image.SaveAsBmp(outFilePath);
                    } else if (fileInfo.Extension == ".gif") {
                        image.SaveAsGif(outFilePath);
                    } else if (fileInfo.Extension == ".pbm") {
                        image.SaveAsPbm(outFilePath);
                    } else if (fileInfo.Extension == ".tga") {
                        image.SaveAsTga(outFilePath);
                    } else if (fileInfo.Extension == ".tiff") {
                        image.SaveAsTiff(outFilePath);
                    } else if (fileInfo.Extension == ".webp") {
                        image.SaveAsWebp(outFilePath);
                    } else if (fileInfo.Extension == ".ico") {
                        // maybe it will work?
                        System.IO.File.Copy(filePath, outFilePath, true);
                    } else {
                        throw new UnknownImageFormatException("Image format not supported. Feel free to open an issue/fix it.");
                    }
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
        public static void Resize(string filePath, string outFilePath, int? width, int? height, bool keepAspectRatio = true) {
            using (var inStream = System.IO.File.OpenRead(filePath)) {
                using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(inStream)) {

                    if (keepAspectRatio == true) {
                        if (width != null && height != null) {
                            image.Mutate(x => x.Resize(width.Value, height.Value));
                        } else if (width != null) {
                            var newWidth = width.Value;
                            var newHeight = (image.Height / image.Width) * newWidth;
                            image.Mutate(x => x.Resize(newWidth, newHeight));
                        } else if (height != null) {
                            var newHeight = height.Value;
                            var newWidth = (image.Width / image.Height) * newHeight;
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
                    image.Save(outFilePath);
                }
            }
        }

        public static SixLabors.ImageSharp.Image Resize(SixLabors.ImageSharp.Image image, int? width, int? height, bool keepAspectRatio = true) {
            // lets try to keep the original image if possible
            if (width != null && height != null && image.Width == width && image.Height == height) {
                return image;
            } else if (width != null && image.Width == width) {
                return image;
            } else if (height != null && image.Height == height) {
                return image;
            }

            if (keepAspectRatio == true) {
                if (width != null && height != null) {
                    image.Mutate(x => x.Resize(width.Value, height.Value));
                } else if (width != null) {
                    var newWidth = width.Value;
                    var newHeight = (image.Height / image.Width) * newWidth;
                    image.Mutate(x => x.Resize(newWidth, newHeight));
                } else if (height != null) {
                    var newHeight = height.Value;
                    var newWidth = (image.Width / image.Height) * newHeight;
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
            return image;
        }

        /// <summary>
        /// Resizes an image to the specified percentage.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="outFilePath"></param>
        /// <param name="percentage"></param>
        public static void Resize(string filePath, string outFilePath, int percentage) {
            using (var inStream = System.IO.File.OpenRead(filePath)) {
                using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(inStream)) {
                    int width = image.Width * percentage / 100;
                    int height = image.Height * percentage / 100;
                    if (image.Width == width && image.Height == height) {
                        System.IO.File.Copy(filePath, outFilePath, true);
                    } else {
                        image.Mutate(x => x.Resize(width, height));
                        image.Save(outFilePath);
                    }
                }
            }
        }

        public static void Combine(string filePath, string filePath2, string outFilePath, bool resizeToFit = false, ImagePlacement imagePlacement = ImagePlacement.Bottom) {
            using (var inStream = System.IO.File.OpenRead(filePath))
            using (var inStream2 = System.IO.File.OpenRead(filePath2))
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
                        outputImage.Save(outFilePath);
                    }
                }
            }

        }
    }
}

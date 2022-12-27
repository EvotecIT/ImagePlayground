﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SixLabors.ImageSharp;
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
            using (var inStream = System.IO.File.OpenRead(filePath))
            {
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
        public static void Resize(string filePath, string outFilePath, int width, int height) {
            using (var inStream = System.IO.File.OpenRead(filePath))
            {
                using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(inStream)) {
                    image.Mutate(x => x.Resize(width, height));
                    image.Save(outFilePath);
                }
            }
        }

        /// <summary>
        /// Resizes an image to the specified percentage.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="outFilePath"></param>
        /// <param name="percentage"></param>
        public static void Resize(string filePath, string outFilePath, int percentage) {
            using (var inStream = System.IO.File.OpenRead(filePath))
            {
                using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(inStream)) {
                    int width = image.Width * percentage / 100;
                    int height = image.Height * percentage / 100;
                    image.Mutate(x => x.Resize(width, height));
                    image.Save(outFilePath);
                }
            }
        }

    }
}

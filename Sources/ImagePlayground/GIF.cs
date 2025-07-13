using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SixLabors.ImageSharp;
using ImageSharpImage = SixLabors.ImageSharp.Image;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.PixelFormats;

namespace ImagePlayground;
/// <summary>Helper methods for creating animated GIFs.</summary>
/// <remarks>
/// Each frame is loaded using ImageSharp and combined into a single GIF image.
/// </remarks>
public static class Gif {
    /// <summary>
    /// Generates an animated GIF from a set of images.
    /// </summary>
    /// <param name="sourceImages">Paths to images used as frames.</param>
    /// <param name="filePath">Destination GIF file path.</param>
    /// <param name="frameDelay">Delay between frames in milliseconds.</param>
    public static void Generate(IEnumerable<string> sourceImages, string filePath, int frameDelay = 100) {
        if (sourceImages is null) {
            throw new ArgumentNullException(nameof(sourceImages));
        }

        var frames = sourceImages.Select(Helpers.ResolvePath).ToList();
        if (frames.Count == 0) {
            throw new ArgumentException("No frames specified", nameof(sourceImages));
        }

        foreach (var frame in frames) {
            if (!File.Exists(frame)) {
                throw new FileNotFoundException($"Frame not found: {frame}", frame);
            }
        }

        string output = Helpers.ResolvePath(filePath);

        using var gif = ImageSharpImage.Load(frames[0]);
        gif.Metadata.GetGifMetadata().RepeatCount = 0;
        gif.Frames.RootFrame.Metadata.GetGifMetadata().FrameDelay = Math.Max(1, frameDelay / 10);

        for (int i = 1; i < frames.Count; i++) {
            using var image = ImageSharpImage.Load(frames[i]);
            image.Frames.RootFrame.Metadata.GetGifMetadata().FrameDelay = Math.Max(1, frameDelay / 10);
            gif.Frames.AddFrame(image.Frames.RootFrame);
        }

        SixLabors.ImageSharp.ImageExtensions.SaveAsGif(gif, output, new GifEncoder());
    }
}

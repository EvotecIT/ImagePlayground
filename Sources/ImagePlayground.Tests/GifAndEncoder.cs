using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for GifAndEncoder.
/// </summary>
public partial class ImagePlayground {
    [Fact]
    public void Test_GifGenerate_Success() {
        string dest = Path.Combine(_directoryWithTests, "anim.gif");
        if (File.Exists(dest)) File.Delete(dest);
        var frames = new List<string> {
                Path.Combine(_directoryWithImages, "QRCode1.png")
            };
        Gif.Generate(frames, dest, 50);
        Assert.True(File.Exists(dest));
        using var img = Image.Load(dest);
        Assert.True(img.Frames.Count >= 1);
    }

    [Fact]
    public void Test_GifGenerate_NullSourceThrows() {
        Assert.Throws<ArgumentNullException>(() => Gif.Generate(null!, Path.Combine(_directoryWithTests, "x.gif")));
    }

    [Fact]
    public void Test_GifGenerate_EmptySourceThrows() {
        Assert.Throws<ArgumentException>(() => Gif.Generate(new List<string>(), Path.Combine(_directoryWithTests, "x.gif")));
    }

    [Fact]
    public void Test_GifGenerate_MissingFrameThrows() {
        var frames = new List<string> { Path.Combine(_directoryWithImages, "missing.png") };
        Assert.Throws<FileNotFoundException>(() => Gif.Generate(frames, Path.Combine(_directoryWithTests, "x.gif")));
    }

    [Fact]
    public void Test_GetEncoder_PngCompressionClamped() {
        var enc = Helpers.GetEncoder(".png", null, 15) as PngEncoder;
        Assert.NotNull(enc);
        Assert.Equal(PngCompressionLevel.BestCompression, enc.CompressionLevel);
    }

    [Fact]
    public void Test_GetEncoder_JpegQualityClamped() {
        var enc = Helpers.GetEncoder(".jpg", 200, null) as JpegEncoder;
        Assert.NotNull(enc);
        Assert.Equal(100, enc.Quality);
    }

    [Fact]
    public void Test_GetEncoder_UnknownExtensionThrows() {
        var ex = Assert.Throws<SixLabors.ImageSharp.UnknownImageFormatException>(() => Helpers.GetEncoder(".xyz", null, null));
        Assert.Contains(".png", ex.Message);
    }
}

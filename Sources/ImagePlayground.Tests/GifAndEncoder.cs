using OfficeIMO.Drawing;
using Color = OfficeIMO.Drawing.OfficeColor;
using ExifTag = OfficeIMO.Drawing.OfficeExifTag;
using Rgba32 = OfficeIMO.Drawing.OfficeColor;
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
    public void Test_Save_PngCompressionClamped() {
        using var image = Image.Load(Path.Combine(_directoryWithImages, "QRCode1.png"));
        string clamped = Path.Combine(_directoryWithTests, "compression_clamped.png");
        string maximum = Path.Combine(_directoryWithTests, "compression_maximum.png");
        image.Save(clamped, compressionLevel: 15);
        image.Save(maximum, compressionLevel: 9);
        Assert.Equal(File.ReadAllBytes(maximum), File.ReadAllBytes(clamped));
    }

    [Fact]
    public void Test_Save_JpegQualityClamped() {
        using var image = Image.Load(Path.Combine(_directoryWithImages, "QRCode1.png"));
        string clamped = Path.Combine(_directoryWithTests, "quality_clamped.jpg");
        string maximum = Path.Combine(_directoryWithTests, "quality_maximum.jpg");
        image.Save(clamped, quality: 200);
        image.Save(maximum, quality: 100);
        Assert.Equal(File.ReadAllBytes(maximum), File.ReadAllBytes(clamped));
    }

    [Fact]
    public void Test_Save_UnknownExtensionThrows() {
        using var image = Image.Load(Path.Combine(_directoryWithImages, "QRCode1.png"));
        string destination = Path.Combine(_directoryWithTests, "unsupported.xyz");
        Assert.Throws<NotSupportedException>(() => image.Save(destination));
        Assert.False(File.Exists(destination));
    }
}

using System;
using System.IO;
using OfficeIMO.Drawing;
using Xunit;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for ConvertToIcon.
/// </summary>
public partial class ImagePlayground {
    [Fact]
    public void Test_ConvertToIcon_FromIcon_CopiesFile() {
        string src = Path.Combine(_directoryWithTests, "actual-icon-source.ico");
        using (var source = global::ImagePlayground.Image.FromRaster(new OfficeRasterImage(16, 16, OfficeColor.Blue))) {
            source.Save(src);
        }
        string dest = Path.Combine(_directoryWithTests, "copy.ico");
        if (File.Exists(dest)) File.Delete(dest);

        ImageHelper.ConvertTo(src, dest);
        Assert.True(File.Exists(dest));
        Assert.Equal(File.ReadAllBytes(src), File.ReadAllBytes(dest));
    }

    [Fact]
    public void Test_ConvertToIcon_OversizedPng_RejectsInvalidResolution() {
        string src = Path.Combine(_directoryWithImages, "QRCode1.png");
        string dest = Path.Combine(_directoryWithTests, "invalid.ico");
        if (File.Exists(dest)) File.Delete(dest);
        Assert.Throws<ArgumentOutOfRangeException>(() => ImageHelper.ConvertTo(src, dest));
        Assert.False(File.Exists(dest));
    }

    [Fact]
    public void Test_ConvertToIcon_FromPng_PreservesValidPixelsAndDimensions() {
        string source = Path.Combine(_directoryWithTests, "convert-icon-source.png");
        string output = Path.Combine(_directoryWithTests, "convert-icon-output.ico");
        using (var image = global::ImagePlayground.Image.FromRaster(new OfficeRasterImage(16, 9, OfficeColor.Red))) {
            image.Save(source);
        }

        ImageHelper.ConvertTo(source, output);

        using var converted = global::ImagePlayground.Image.Load(output);
        Assert.Single(converted.Frames);
        Assert.Equal(16, converted.Width);
        Assert.Equal(9, converted.Height);
        Assert.Equal(OfficeColor.Red, converted.Raster.GetPixel(0, 0));
        Assert.Equal(OfficeColor.Red, converted.Raster.GetPixel(15, 8));
    }
}

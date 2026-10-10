using OfficeIMO.Drawing;
using Color = OfficeIMO.Drawing.OfficeColor;
using ExifTag = OfficeIMO.Drawing.OfficeExifTag;
using Rgba32 = OfficeIMO.Drawing.OfficeColor;
using System.IO;
using Xunit;
using PlaygroundImage = global::ImagePlayground.Image;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for aspect ratio preservation and explicit stretching.
/// </summary>
public partial class ImagePlayground {
    [Theory]
    [InlineData(true, 32, 13, 20, 32)]
    [InlineData(false, 32, 32, 32, 32)]
    public void ResizeBoundsApplyToRawPixelsFilesAndEveryRetainedFrame(bool keepAspectRatio, int firstWidth, int firstHeight, int secondWidth, int secondHeight) {
        var original = new OfficeRasterImage(100, 40, Color.Red);
        OfficeRasterImage resized = global::ImagePlayground.ImageHelper.Resize(original, 32, 32, keepAspectRatio);
        Assert.Equal(firstWidth, resized.Width);
        Assert.Equal(firstHeight, resized.Height);
        Assert.Equal(Color.Red, resized.GetPixel(firstWidth - 1, firstHeight - 1));
        Assert.Equal(100, original.Width);
        Assert.Equal(40, original.Height);

        var frames = new OfficeRasterFrames(new[] {
            new OfficeRasterFrame(original, System.TimeSpan.FromMilliseconds(100)),
            new OfficeRasterFrame(new OfficeRasterImage(50, 80, Color.Blue), System.TimeSpan.FromMilliseconds(200))
        }, playCount: 3);
        using var image = PlaygroundImage.FromFrames(frames);
        image.Resize(32, 32, keepAspectRatio);
        Assert.Equal(2, image.Frames.Count);
        Assert.Equal(3, image.Frames.PlayCount);
        Assert.Equal(firstWidth, image.Frames[0].Image.Width);
        Assert.Equal(firstHeight, image.Frames[0].Image.Height);
        Assert.Equal(secondWidth, image.Frames[1].Image.Width);
        Assert.Equal(secondHeight, image.Frames[1].Image.Height);
        Assert.Equal(System.TimeSpan.FromMilliseconds(100), image.Frames[0].Duration);
        Assert.Equal(System.TimeSpan.FromMilliseconds(200), image.Frames[1].Duration);
        Assert.Equal(Color.Blue, image.Frames[1].Image.GetPixel(secondWidth - 1, secondHeight - 1));

        string source = Path.Combine(_directoryWithTests, "resize-bounds-source-" + keepAspectRatio + ".png");
        string output = Path.Combine(_directoryWithTests, "resize-bounds-output-" + keepAspectRatio + ".png");
        using (var fileImage = PlaygroundImage.FromRaster(original)) { fileImage.Save(source); }
        global::ImagePlayground.ImageHelper.Resize(source, output, 32, 32, keepAspectRatio);
        using var loaded = PlaygroundImage.Load(output);
        Assert.Equal(firstWidth, loaded.Width);
        Assert.Equal(firstHeight, loaded.Height);
        Assert.Equal(resized.GetPixels(), loaded.Raster.GetPixels());
    }

    [Fact]
    public void Test_Resize_KeepAspectRatio_WidthOnly() {
        string src = Path.Combine(_directoryWithImages, "PrzemyslawKlysAndKulkozaurr.jpg");
        using var img = PlaygroundImage.Load(src);
        int originalWidth = img.Width;
        int originalHeight = img.Height;
        int newWidth = 200;
        int expectedHeight = (int)System.Math.Round(newWidth * originalHeight / (double)originalWidth);
        img.Resize(newWidth, null);
        Assert.Equal(newWidth, img.Width);
        Assert.Equal(expectedHeight, img.Height);
    }

    [Fact]
    public void Test_Resize_KeepAspectRatio_HeightOnly() {
        string src = Path.Combine(_directoryWithImages, "PrzemyslawKlysAndKulkozaurr.jpg");
        using var img = PlaygroundImage.Load(src);
        int originalWidth = img.Width;
        int originalHeight = img.Height;
        int newHeight = 150;
        int expectedWidth = (int)System.Math.Round(newHeight * originalWidth / (double)originalHeight);
        img.Resize(null, newHeight);
        Assert.Equal(expectedWidth, img.Width);
        Assert.Equal(newHeight, img.Height);
    }
}

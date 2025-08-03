using SixLabors.ImageSharp;
using System.IO;
using Xunit;
using ImageSharpImage = SixLabors.ImageSharp.Image;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for aspect ratio preservation when one dimension is missing.
/// </summary>
public partial class ImagePlayground {
    [Fact]
    public void Test_Resize_KeepAspectRatio_WidthOnly() {
        string src = Path.Combine(_directoryWithImages, "PrzemyslawKlysAndKulkozaurr.jpg");
        using var img = ImageSharpImage.Load(src);
        int originalWidth = img.Width;
        int originalHeight = img.Height;
        int newWidth = 200;
        int expectedHeight = (int)System.Math.Round(newWidth * originalHeight / (double)originalWidth);
        ImageHelper.Resize(img, newWidth, null);
        Assert.Equal(newWidth, img.Width);
        Assert.Equal(expectedHeight, img.Height);
    }

    [Fact]
    public void Test_Resize_KeepAspectRatio_HeightOnly() {
        string src = Path.Combine(_directoryWithImages, "PrzemyslawKlysAndKulkozaurr.jpg");
        using var img = ImageSharpImage.Load(src);
        int originalWidth = img.Width;
        int originalHeight = img.Height;
        int newHeight = 150;
        int expectedWidth = (int)System.Math.Round(newHeight * originalWidth / (double)originalHeight);
        ImageHelper.Resize(img, null, newHeight);
        Assert.Equal(expectedWidth, img.Width);
        Assert.Equal(newHeight, img.Height);
    }
}

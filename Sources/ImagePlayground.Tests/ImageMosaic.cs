using ImagePlayground;
using System.IO;
using Xunit;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for ImageHelper.Mosaic.
/// </summary>
public partial class ImagePlayground {
    [Fact]
    public void Test_CreateMosaic() {
        string[] files = {
            Path.Combine(_directoryWithImages, "LogoEvotec.png"),
            Path.Combine(_directoryWithImages, "QRCode1.png"),
            Path.Combine(_directoryWithImages, "BarcodeEAN7.png"),
            Path.Combine(_directoryWithImages, "BarcodeEAN13.png")
        };
        string dest = Path.Combine(_directoryWithTests, "mosaic.png");
        if (File.Exists(dest)) File.Delete(dest);
        ImageHelper.Mosaic(files, dest, 2, 100, 100);
        Assert.True(File.Exists(dest));
        using var img = SixLabors.ImageSharp.Image.Load(dest);
        Assert.Equal(200, img.Width);
        Assert.Equal(200, img.Height);
    }
}

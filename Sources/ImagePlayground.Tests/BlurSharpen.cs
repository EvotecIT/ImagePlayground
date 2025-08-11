using System.IO;
using Xunit;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for blur and sharpen helpers.
/// </summary>
public partial class ImagePlayground {
    [Fact]
    public void Test_ImageHelper_Blur() {
        string source = Path.Combine(_directoryWithImages, "QRCode1.png");
        string output = Path.Combine(_directoryWithImages, "QRCode1_blur.png");
        if (File.Exists(output)) File.Delete(output);
        ImageHelper.Blur(source, output, 5);
        Assert.True(File.Exists(output));
        using var img = Image.GetImage(output);
        Assert.Equal(660, img.Width);
        Assert.Equal(660, img.Height);
        img.Dispose();
    }

    [Fact]
    public void Test_ImageHelper_Sharpen() {
        string source = Path.Combine(_directoryWithImages, "QRCode1.png");
        string output = Path.Combine(_directoryWithImages, "QRCode1_sharpen.png");
        if (File.Exists(output)) File.Delete(output);
        ImageHelper.Sharpen(source, output, 5);
        Assert.True(File.Exists(output));
        using var img = Image.GetImage(output);
        Assert.Equal(660, img.Width);
        Assert.Equal(660, img.Height);
        img.Dispose();
    }
}
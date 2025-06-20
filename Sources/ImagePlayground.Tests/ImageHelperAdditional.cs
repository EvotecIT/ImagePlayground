using ImagePlayground;
using System;
using System.IO;
using Xunit;

namespace ImagePlayground.Tests;
public partial class ImagePlayground {
    [Fact]
    public void Test_ConvertImageFormat() {
        string source = Path.Combine(_directoryWithImages, "QRCode1.png");
        string output = Path.Combine(_directoryWithImages, "QRCode1_converted.jpg");
        if (File.Exists(output)) File.Delete(output);

        ImageHelper.ConvertTo(source, output);
        Assert.True(File.Exists(output));

        var img = Image.GetImage(output);
        Assert.Equal(660, img.Width);
        Assert.Equal(660, img.Height);
        img.Dispose();
    }

    [Fact]
    public void Test_CombineImages() {
        string file1 = Path.Combine(_directoryWithImages, "LogoEvotec.png");
        string file2 = Path.Combine(_directoryWithImages, "QRCode1.png");
        string combined = Path.Combine(_directoryWithImages, "combined.png");
        if (File.Exists(combined)) File.Delete(combined);

        ImageHelper.Combine(file1, file2, combined);
        Assert.True(File.Exists(combined));

        var img = Image.GetImage(combined);
        Assert.Equal(1675, img.Width);
        Assert.Equal(533 + 660, img.Height);
        img.Dispose();
    }
}

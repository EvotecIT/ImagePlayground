using SixLabors.ImageSharp;
using System.IO;
using Xunit;

namespace ImagePlayground.Tests;
public partial class ImagePlayground {
    [Fact]
    public void Test_Combine_Bottom() {
        string file1 = Path.Combine(_directoryWithImages, "LogoEvotec.png");
        string file2 = Path.Combine(_directoryWithImages, "QRCode1.png");
        string dest = Path.Combine(_directoryWithTests, "combine_bottom.png");
        if (File.Exists(dest)) File.Delete(dest);
        ImageHelper.Combine(file1, file2, dest, false, ImagePlacement.Bottom);
        Assert.True(File.Exists(dest));
        using var img = Image.Load(dest);
        Assert.Equal(1675, img.Width);
        Assert.Equal(1193, img.Height);
    }

    [Fact]
    public void Test_Combine_Top() {
        string file1 = Path.Combine(_directoryWithImages, "LogoEvotec.png");
        string file2 = Path.Combine(_directoryWithImages, "QRCode1.png");
        string dest = Path.Combine(_directoryWithTests, "combine_top.png");
        if (File.Exists(dest)) File.Delete(dest);
        ImageHelper.Combine(file1, file2, dest, false, ImagePlacement.Top);
        Assert.True(File.Exists(dest));
        using var img = Image.Load(dest);
        Assert.Equal(1675, img.Width);
        Assert.Equal(1193, img.Height);
    }

    [Fact]
    public void Test_Combine_Right() {
        string file1 = Path.Combine(_directoryWithImages, "LogoEvotec.png");
        string file2 = Path.Combine(_directoryWithImages, "QRCode1.png");
        string dest = Path.Combine(_directoryWithTests, "combine_right.png");
        if (File.Exists(dest)) File.Delete(dest);
        ImageHelper.Combine(file1, file2, dest, false, ImagePlacement.Right);
        Assert.True(File.Exists(dest));
        using var img = Image.Load(dest);
        Assert.Equal(2335, img.Width);
        Assert.Equal(660, img.Height);
    }
}

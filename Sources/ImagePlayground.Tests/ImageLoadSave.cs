using System.IO;
using Xunit;

namespace ImagePlayground.Tests;
public partial class ImagePlayground {
    [Fact]
    public void Test_LoadImage() {
        string filePath = Path.Combine(_directoryWithImages, "QRCode1.png");
        using var img = Image.Load(filePath);
        Assert.Equal(660, img.Width);
        Assert.Equal(660, img.Height);
    }

    [Fact]
    public void Test_SaveImage() {
        string src = Path.Combine(_directoryWithImages, "QRCode1.png");
        string dest = Path.Combine(_directoryWithTests, "saved.png");
        if (File.Exists(dest)) File.Delete(dest);
        using var img = Image.Load(src);
        img.Save(dest);
        Assert.True(File.Exists(dest));
        using var img2 = Image.Load(dest);
        Assert.Equal(img.Width, img2.Width);
        Assert.Equal(img.Height, img2.Height);
    }
}

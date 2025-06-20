using ImagePlayground;
using System.IO;
using Xunit;

namespace ImagePlayground.Tests;
public partial class ImagePlayground {
    [Fact]
    public void Test_ImageManipulation() {
        string src = Path.Combine(_directoryWithImages, "QRCode1.png");
        string dest = Path.Combine(_directoryWithImages, "QRCode1_pixelate.png");
        if (File.Exists(dest)) File.Delete(dest);

        using (var img = Image.Load(src)) {
            img.Pixelate(10);
            img.Save(dest);
            Assert.Equal(660, img.Width);
            Assert.Equal(660, img.Height);
        }
        Assert.True(File.Exists(dest));
    }
}

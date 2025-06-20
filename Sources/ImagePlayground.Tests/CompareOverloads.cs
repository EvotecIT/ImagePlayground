using System.IO;
using Xunit;

namespace ImagePlayground.Tests;
public partial class ImagePlayground {
    [Fact]
    public void Test_ImageCompare_WithImageOverload() {
        string baseFile = Path.Combine(_directoryWithImages, "QRCode1.png");
        string diffFile = Path.Combine(_directoryWithTests, "compare_overload_image.png");
        if (File.Exists(diffFile)) File.Delete(diffFile);
        ImageHelper.AddText(baseFile, diffFile, 1, 1, "Diff", SixLabors.ImageSharp.Color.Blue);

        using var baseImg = Image.Load(baseFile);
        using var modified = Image.Load(diffFile);
        var result = baseImg.Compare(modified);
        Assert.True(result.PixelErrorCount > 0);
    }

    [Fact]
    public void Test_ImageCompare_WithPathOverload() {
        string baseFile = Path.Combine(_directoryWithImages, "QRCode1.png");
        string diffFile = Path.Combine(_directoryWithTests, "compare_overload_path.png");
        if (File.Exists(diffFile)) File.Delete(diffFile);
        ImageHelper.AddText(baseFile, diffFile, 1, 1, "Diff", SixLabors.ImageSharp.Color.Blue);

        using var baseImg = Image.Load(baseFile);
        var result = baseImg.Compare(diffFile);
        Assert.True(result.PixelErrorCount > 0);
    }
}

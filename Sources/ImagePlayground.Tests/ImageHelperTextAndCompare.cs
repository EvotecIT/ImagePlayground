using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace ImagePlayground.Tests;
public partial class ImagePlayground {
    [Fact]
    public void Test_CompareImages() {
        string img1 = Path.Combine(_directoryWithImages, "QRCode1.png");
        string modified = Path.Combine(_directoryWithTests, "qr_modified.png");
        if (File.Exists(modified)) File.Delete(modified);
        ImageHelper.AddText(img1, modified, 1, 1, "Diff", SixLabors.ImageSharp.Color.Red);

        var result = ImageHelper.Compare(img1, modified);
        Assert.True(result.PixelErrorCount > 0);
    }

    [Fact]
    public void Test_AddTextToImage() {
        string src = Path.Combine(_directoryWithImages, "QRCode1.png");
        string dest = Path.Combine(_directoryWithTests, "text.png");
        if (File.Exists(dest)) File.Delete(dest);
        ImageHelper.AddText(src, dest, 1, 1, "Test", SixLabors.ImageSharp.Color.Red);
        Assert.True(File.Exists(dest));
        using var img = SixLabors.ImageSharp.Image.Load<Rgba32>(dest);
        Assert.Equal(660, img.Width);
        Assert.Equal(660, img.Height);
    }

    [Fact]
    public void Test_AddTextWithShadowAndOutline() {
        string src = Path.Combine(_directoryWithImages, "QRCode1.png");
        string dest = Path.Combine(_directoryWithTests, "text_shadow_outline.png");
        if (File.Exists(dest)) File.Delete(dest);
        ImageHelper.AddText(
            src,
            dest,
            1,
            1,
            "Test",
            SixLabors.ImageSharp.Color.Red,
            16f,
            "Arial",
            SixLabors.ImageSharp.Color.Black,
            1,
            1,
            SixLabors.ImageSharp.Color.Yellow,
            1);
        Assert.True(File.Exists(dest));
        using var img = SixLabors.ImageSharp.Image.Load<Rgba32>(dest);
        Assert.Equal(660, img.Width);
        Assert.Equal(660, img.Height);
    }

    [Fact]
    public void Test_AddTextBox() {
        string src = Path.Combine(_directoryWithImages, "QRCode1.png");
        string dest = Path.Combine(_directoryWithTests, "textbox.png");
        if (File.Exists(dest)) File.Delete(dest);
        ImageHelper.AddTextBox(src, dest, 1, 1, "Wrapped Text", 100, SixLabors.ImageSharp.Color.Red);
        Assert.True(File.Exists(dest));
        using var img = SixLabors.ImageSharp.Image.Load<Rgba32>(dest);
        Assert.Equal(660, img.Width);
        Assert.Equal(660, img.Height);
    }

    [Fact]
    public void Test_GridImageContainsMultipleColors() {
        string dest = Path.Combine(_directoryWithTests, "gridcolors.png");
        if (File.Exists(dest)) File.Delete(dest);
        ImageHelper.Create(dest, 100, 100, SixLabors.ImageSharp.Color.White);
        Assert.True(File.Exists(dest));
        using var img = SixLabors.ImageSharp.Image.Load<Rgba32>(dest);
        var colors = new HashSet<Rgba32>();
        for (var x = 0; x < img.Width; x++) {
            for (var y = 0; y < img.Height; y++) {
                colors.Add(img[x, y]);
                if (colors.Count > 1) {
                    break;
                }
            }
            if (colors.Count > 1) {
                break;
            }
        }

        Assert.True(colors.Count > 1);
    }

    [Fact]
    public void Test_CreateGridImage() {
        string dest = Path.Combine(_directoryWithTests, "grid.png");
        if (File.Exists(dest)) File.Delete(dest);
        ImageHelper.Create(dest, 50, 50, SixLabors.ImageSharp.Color.White);
        Assert.True(File.Exists(dest));
        using var img = Image.Load(dest);
        Assert.Equal(50, img.Width);
        Assert.Equal(50, img.Height);
    }

    [Fact]
    public void Test_CreateGridImageRandomColors() {
        string dest = Path.Combine(_directoryWithTests, "grid_random.png");
        if (File.Exists(dest)) File.Delete(dest);
        ImageHelper.Create(dest, 80, 80, SixLabors.ImageSharp.Color.White);
        Assert.True(File.Exists(dest));

        using Image<Rgba32> img = SixLabors.ImageSharp.Image.Load<Rgba32>(dest);
        Rgba32 first = img[20, 20];
        Rgba32 second = img[60, 60];
        Assert.NotEqual(first, second);
    }
}

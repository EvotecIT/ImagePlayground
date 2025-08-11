using SixLabors.ImageSharp;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for AsyncMethods.
/// </summary>
public partial class ImagePlayground {
    [Fact]
    public async Task Test_ResizeAsync() {
        string src = Path.Combine(_directoryWithImages, "LogoEvotec.png");
        string dest = Path.Combine(_directoryWithTests, "logo_async.png");
        if (File.Exists(dest)) File.Delete(dest);

        await ImageHelper.ResizeAsync(src, dest, 40, 40);
        using var img = Image.Load(dest);
        Assert.Equal(40, img.Width);
        Assert.Equal(40, img.Height);
    }

    [Fact]
    public async Task Test_WatermarkImageAsync() {
        string src = Path.Combine(_directoryWithImages, "QRCode1.png");
        string wmk = Path.Combine(_directoryWithImages, "LogoEvotec.png");
        string dest = Path.Combine(_directoryWithTests, "wm_async.png");
        if (File.Exists(dest)) File.Delete(dest);

        await ImageHelper.WatermarkImageAsync(src, dest, wmk, WatermarkPlacement.Middle, watermarkPercentage: 100);
        Assert.True(File.Exists(dest));
        using var orig = Image.Load(src);
        using var res = Image.Load(dest);
        Assert.Equal(orig.Width, res.Width);
        Assert.Equal(orig.Height, res.Height);
    }

    [Fact]
    public async Task Test_RotateAsync() {
        string src = Path.Combine(_directoryWithImages, "PrzemyslawKlysAndKulkozaurr.jpg");
        string dest = Path.Combine(_directoryWithTests, "rotate_async.jpg");
        if (File.Exists(dest)) File.Delete(dest);
        using var orig = Image.Load(src);
        int w = orig.Width;
        int h = orig.Height;

        await ImageHelper.RotateAsync(src, dest, 90);
        using var img = Image.Load(dest);
        Assert.Equal(w, img.Height);
        Assert.Equal(h, img.Width);
    }

    [Fact]
    public async Task Test_BlurAsync() {
        string src = Path.Combine(_directoryWithImages, "LogoEvotec.png");
        string dest = Path.Combine(_directoryWithTests, "logo_blur_async.png");
        if (File.Exists(dest)) File.Delete(dest);
        await ImageHelper.BlurAsync(src, dest, 5);
        Assert.True(File.Exists(dest));
    }

    [Fact]
    public async Task Test_SharpenAsync() {
        string src = Path.Combine(_directoryWithImages, "LogoEvotec.png");
        string dest = Path.Combine(_directoryWithTests, "logo_sharp_async.png");
        if (File.Exists(dest)) File.Delete(dest);
        await ImageHelper.SharpenAsync(src, dest, 2);
        Assert.True(File.Exists(dest));
    }
}

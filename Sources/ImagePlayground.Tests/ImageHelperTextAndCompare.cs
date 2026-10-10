using OfficeIMO.Drawing;
using Color = OfficeIMO.Drawing.OfficeColor;
using ExifTag = OfficeIMO.Drawing.OfficeExifTag;
using Rgba32 = OfficeIMO.Drawing.OfficeColor;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for ImageHelperTextAndCompare.
/// </summary>
public partial class ImagePlayground {
    [Fact]
    public void Test_CompareImages() {
        string img1 = Path.Combine(_directoryWithImages, "QRCode1.png");
        string modified = Path.Combine(_directoryWithTests, "qr_modified.png");
        if (File.Exists(modified)) File.Delete(modified);
        ImageHelper.AddText(img1, modified, 1, 1, "Diff", OfficeColor.Red);

        var result = ImageHelper.Compare(img1, modified);
        Assert.True(result.ChangedPixels > 0);
    }

    [Fact]
    public void Test_AddTextToImage() {
        string src = Path.Combine(_directoryWithImages, "QRCode1.png");
        string dest = Path.Combine(_directoryWithTests, "text.png");
        if (File.Exists(dest)) File.Delete(dest);
        ImageHelper.AddText(src, dest, 1, 1, "Test", OfficeColor.Red);
        Assert.True(File.Exists(dest));
        using var img = global::ImagePlayground.Image.Load(dest);
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
            OfficeColor.Red,
            16f,
            "Arial",
            OfficeColor.Black,
            1,
            1,
            OfficeColor.Yellow,
            1);
        Assert.True(File.Exists(dest));
        using var img = global::ImagePlayground.Image.Load(dest);
        Assert.Equal(660, img.Width);
        Assert.Equal(660, img.Height);
    }

    [Fact]
    public void Test_AddTextBox() {
        string src = Path.Combine(_directoryWithImages, "QRCode1.png");
        string dest = Path.Combine(_directoryWithTests, "textbox.png");
        if (File.Exists(dest)) File.Delete(dest);
        ImageHelper.AddTextBox(src, dest, 1, 1, "Wrapped Text", 100, OfficeColor.Red);
        Assert.True(File.Exists(dest));
        using var img = global::ImagePlayground.Image.Load(dest);
        Assert.Equal(660, img.Width);
        Assert.Equal(660, img.Height);
    }

    [Fact]
    public void Test_AddTextBoxAutoHeightEqualsExplicitHeight() {
        string src = Path.Combine(_directoryWithImages, "QRCode1.png");
        string autoDest = Path.Combine(_directoryWithTests, "textbox_autoheight.png");
        string explicitDest = Path.Combine(_directoryWithTests, "textbox_explicitheight.png");
        if (File.Exists(autoDest)) File.Delete(autoDest);
        if (File.Exists(explicitDest)) File.Delete(explicitDest);

        ImageHelper.AddTextBox(src, autoDest, 1, 1, "Wrapped Text", 100, OfficeColor.Red);

        using var img = global::ImagePlayground.Image.Load(src);
        float height = (float)OfficeRasterText.Measure("Wrapped Text", 16f, "Arial", 100).Height;

        ImageHelper.AddTextBox(src, explicitDest, 1, 1, "Wrapped Text", 100, height, OfficeColor.Red);

        byte[] autoBytes = File.ReadAllBytes(autoDest);
        byte[] explicitBytes = File.ReadAllBytes(explicitDest);
        Assert.Equal(explicitBytes, autoBytes);
    }

    [Fact]
    public void Test_GridImageContainsMultipleColors() {
        string dest = Path.Combine(_directoryWithTests, "gridcolors.png");
        if (File.Exists(dest)) File.Delete(dest);
        ImageHelper.Create(dest, 100, 100, OfficeColor.White);
        Assert.True(File.Exists(dest));
        using var img = global::ImagePlayground.Image.Load(dest);
        var colors = new HashSet<Rgba32>();
        for (var x = 0; x < img.Width; x++) {
            for (var y = 0; y < img.Height; y++) {
                colors.Add(img.Raster.GetPixel(x, y));
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
        ImageHelper.Create(dest, 50, 50, OfficeColor.White);
        Assert.True(File.Exists(dest));
        using var img = Image.Load(dest);
        Assert.Equal(50, img.Width);
        Assert.Equal(50, img.Height);
    }

    [Fact]
    public void Test_CreateGridImageRandomColors() {
        string dest = Path.Combine(_directoryWithTests, "grid_random.png");
        if (File.Exists(dest)) File.Delete(dest);
        ImageHelper.Create(dest, 80, 80, OfficeColor.White);
        Assert.True(File.Exists(dest));

        using global::ImagePlayground.Image img = global::ImagePlayground.Image.Load(dest);
        Rgba32 first = img.Raster.GetPixel(20, 20);
        Rgba32 second = img.Raster.GetPixel(60, 60);
        Assert.NotEqual(first, second);
    }
}

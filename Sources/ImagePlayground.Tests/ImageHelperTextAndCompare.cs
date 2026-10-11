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
    public void Test_AddTextBoxAutoHeightMatchesMeasuredUnclippedText() {
        string src = Path.Combine(_directoryWithImages, "QRCode1.png");
        string autoDest = Path.Combine(_directoryWithTests, "textbox_autoheight.png");
        string expectedDest = Path.Combine(_directoryWithTests, "textbox_measured_unclipped.png");
        if (File.Exists(autoDest)) { File.Delete(autoDest); }
        if (File.Exists(expectedDest)) { File.Delete(expectedDest); }

        ImageHelper.AddTextBox(src, autoDest, 1, 1, "Wrapped Text", 100, OfficeColor.Red);

        var options = new OfficeRasterTextOptions {
            FontSize = 16,
            FontFamily = "Arial",
            Wrap = true,
            Clip = false
        };
        var measured = OfficeRasterText.Measure("Wrapped Text", options, 100);
        using var expected = global::ImagePlayground.Image.Load(src);
        OfficeRasterText.Draw(expected.Raster, "Wrapped Text", 1, 1, 100, measured.Height, OfficeColor.Red, options);
        expected.Save(expectedDest);

        using var actual = global::ImagePlayground.Image.Load(autoDest);
        Assert.Equal(expected.Raster.GetPixels(), actual.Raster.GetPixels());
        Assert.True(ImageHelper.Compare(src, autoDest).ChangedPixels > 0);
    }

    [Fact]
    public void Test_AddTextBoxExplicitHeightClipsTextAndShadow() {
        string src = Path.Combine(_directoryWithTests, "textbox_clip_source.png");
        string dest = Path.Combine(_directoryWithTests, "textbox_clipped.png");
        using (var source = global::ImagePlayground.Image.FromRaster(new OfficeRasterImage(160, 120, OfficeColor.White))) {
            source.Save(src);
        }
        ImageHelper.AddTextBox(src, dest, 20, 20, "Clipped text", 80, 20, OfficeColor.Red,
            shadowColor: OfficeColor.Black, shadowOffsetY: 30);

        using var actual = global::ImagePlayground.Image.Load(dest);
        using var unclipped = global::ImagePlayground.Image.Load(src);
        OfficeRasterText.Draw(unclipped.Raster, "Clipped text", 20, 20, 80, 20, OfficeColor.Red,
            new OfficeRasterTextOptions {
                FontSize = 16,
                FontFamily = "Arial",
                Wrap = true,
                Clip = false,
                ShadowColor = OfficeColor.Black,
                ShadowOffsetY = 30
            });
        unclipped.Save(Path.Combine(_directoryWithTests, "textbox_unclipped_control.png"));

        int paintedInside = 0;
        int paintedOutside = 0;
        int unclippedPaintedOutside = 0;
        for (int y = 0; y < actual.Height; y++) {
            for (int x = 0; x < actual.Width; x++) {
                bool inside = x >= 20 && x < 100 && y >= 20 && y < 40;
                bool painted = actual.Raster.GetPixel(x, y) != OfficeColor.White;
                if (inside && painted) { paintedInside++; }
                if (!inside && painted) { paintedOutside++; }
                if (!inside && unclipped.Raster.GetPixel(x, y) != OfficeColor.White) { unclippedPaintedOutside++; }
            }
        }
        Assert.True(paintedInside > 0);
        Assert.Equal(0, paintedOutside);
        Assert.True(unclippedPaintedOutside > 0);
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

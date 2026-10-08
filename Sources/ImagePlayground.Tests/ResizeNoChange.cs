using OfficeIMO.Drawing;
using Color = OfficeIMO.Drawing.OfficeColor;
using ExifTag = OfficeIMO.Drawing.OfficeExifTag;
using Rgba32 = OfficeIMO.Drawing.OfficeColor;
using System.IO;
using Xunit;
using PlaygroundImage = global::ImagePlayground.Image;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for ResizeNoChange.
/// </summary>
public partial class ImagePlayground {
    [Fact]
    public void Test_Resize_NoChange_PreservesPixelsAndDimensions() {
        string src = Path.Combine(_directoryWithImages, "QRCode1.png");
        using var img = global::ImagePlayground.Image.Load(src);
        var result = ImageHelper.Resize(img.Raster, img.Width, img.Height);
        Assert.Equal(img.Raster.GetPixels(), result.GetPixels());
        Assert.Equal(img.Width, result.Width);
        Assert.Equal(img.Height, result.Height);
        Assert.Equal(660, img.Width);
        Assert.Equal(660, img.Height);
    }
}

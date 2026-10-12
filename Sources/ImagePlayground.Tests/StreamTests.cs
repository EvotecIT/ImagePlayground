using OfficeIMO.Drawing;
using Color = OfficeIMO.Drawing.OfficeColor;
using ExifTag = OfficeIMO.Drawing.OfficeExifTag;
using Rgba32 = OfficeIMO.Drawing.OfficeColor;
using System.IO;
using Xunit;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for StreamTests.
/// </summary>
public partial class ImagePlayground {
    [Fact]
    public void Test_Image_ToStream_ReturnsResetStream() {
        string src = Path.Combine(_directoryWithImages, "QRCode1.png");
        var img = Image.Load(src);
        using var ms = img.ToStream();
        img.Dispose();
        Assert.Equal(0, ms.Position);
        Assert.True(ms.Length > 0);
        using var reloaded = Image.Load(ms.ToArray());
        Assert.Equal(660, reloaded.Width);
        Assert.Equal(660, reloaded.Height);
        Assert.True(ms.CanWrite);
        long encodedLength = ms.Length;
        ms.Position = encodedLength;
        ms.WriteByte(0);
        Assert.Equal(encodedLength + 1, ms.Length);
    }

    [Fact]
    public void Test_Image_SaveStream_ResetsPosition() {
        string src = Path.Combine(_directoryWithImages, "QRCode1.png");
        using var img = Image.Load(src);
        using var ms = new MemoryStream();
        ms.Write(new byte[512 * 1024], 0, 512 * 1024);
        ms.Position = 128;
        img.Save(ms, quality: 50, compressionLevel: 5);
        Assert.Equal(0, ms.Position);
        Assert.InRange(ms.Length, 1, 512 * 1024 - 1);
        using var reloaded = Image.Load(ms.ToArray());
        Assert.Equal(img.Raster.GetPixels(), reloaded.Raster.GetPixels());
        img.Dispose();
        Assert.True(ms.CanRead);
        Assert.True(ms.CanWrite);
    }
}

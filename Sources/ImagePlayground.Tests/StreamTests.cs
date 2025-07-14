using SixLabors.ImageSharp;
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
        using var img = Image.Load(src);
        using var ms = img.ToStream();
        Assert.Equal(0, ms.Position);
        Assert.True(ms.Length > 0);
    }

    [Fact]
    public void Test_Image_SaveStream_ResetsPosition() {
        string src = Path.Combine(_directoryWithImages, "QRCode1.png");
        using var img = Image.Load(src);
        using var ms = new MemoryStream();
        img.Save(ms, quality: 50, compressionLevel: 5);
        Assert.Equal(0, ms.Position);
        Assert.True(ms.Length > 0);
    }
}

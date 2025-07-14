using System;
using System.IO;
using Xunit;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for ConvertToIcon.
/// </summary>
public partial class ImagePlayground {
    [Fact]
    public void Test_ConvertToIcon_FromIcon_CopiesFile() {
        string src = Path.Combine(_directoryWithImages, "QRCode1.ico");
        string dest = Path.Combine(_directoryWithTests, "copy.ico");
        if (File.Exists(dest)) File.Delete(dest);

        ImageHelper.ConvertTo(src, dest);
        Assert.True(File.Exists(dest));
        Assert.Equal(new FileInfo(src).Length, new FileInfo(dest).Length);
    }

    [Fact]
    public void Test_ConvertToIcon_FromPng_Throws() {
        string src = Path.Combine(_directoryWithImages, "QRCode1.png");
        string dest = Path.Combine(_directoryWithTests, "invalid.ico");
        Assert.Throws<NotSupportedException>(() => ImageHelper.ConvertTo(src, dest));
    }
}

using System;
using System.IO;
using Xunit;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for CodeGlyphX-based memory stability checks.
/// </summary>
public partial class ImagePlayground {
    [Fact]
    public void Test_QrCodeRead_MemoryUsageStable() {
        string filePath = Path.Combine(_directoryWithImages, "QRCode1.png");
        var warmup = QrCode.Read(filePath);
#if NET8_0_OR_GREATER
        Assert.Equal(Status.Found, warmup.Status);
        long before = GC.GetTotalMemory(true);
        for (int i = 0; i < 50; i++) {
            var result = QrCode.Read(filePath);
            Assert.Equal(Status.Found, result.Status);
        }
        GC.Collect();
        GC.WaitForPendingFinalizers();
        long after = GC.GetTotalMemory(true);
        Assert.True(Math.Abs(after - before) < 5 * 1024 * 1024); // less than 5MB diff
#else
        Assert.Equal(Status.Error, warmup.Status);
#endif
    }

    [Fact]
    public void Test_BarCodeRead_MemoryUsageStable() {
        string filePath = Path.Combine(_directoryWithImages, "BarcodeEAN13.png");
        var warmup = BarCode.Read(filePath);
        Assert.Equal(Status.Found, warmup.Status);
        long before = GC.GetTotalMemory(true);
        for (int i = 0; i < 50; i++) {
            var result = BarCode.Read(filePath);
            Assert.Equal(Status.Found, result.Status);
        }
        GC.Collect();
        GC.WaitForPendingFinalizers();
        long after = GC.GetTotalMemory(true);
        Assert.True(Math.Abs(after - before) < 5 * 1024 * 1024); // less than 5MB diff
    }
}

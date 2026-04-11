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
        Assert.Equal(Status.Found, warmup.Status);
        AssertReadMemoryStable(() => {
            var result = QrCode.Read(filePath);
            Assert.Equal(Status.Found, result.Status);
        }, 5 * 1024 * 1024);
    }

    [Fact]
    public void Test_BarCodeRead_MemoryUsageStable() {
        string filePath = Path.Combine(_directoryWithImages, "BarcodeEAN13.png");
        var warmup = BarCode.Read(filePath);
        Assert.Equal(Status.Found, warmup.Status);
        AssertReadMemoryStable(() => {
            var result = BarCode.Read(filePath);
            Assert.Equal(Status.Found, result.Status);
        }, 10 * 1024 * 1024);
    }

    private static void AssertReadMemoryStable(Action readAction, long allowedGrowthBytes) {
        for (int i = 0; i < 10; i++) {
            readAction();
        }

        long before = CollectMemory();
        for (int i = 0; i < 25; i++) {
            readAction();
        }

        long middle = CollectMemory();
        for (int i = 0; i < 25; i++) {
            readAction();
        }

        long after = CollectMemory();
        long firstGrowth = Math.Abs(middle - before);
        long secondGrowth = Math.Abs(after - middle);
        Assert.True(secondGrowth < allowedGrowthBytes, $"Expected steady-state memory growth below {allowedGrowthBytes} bytes, but got {secondGrowth} bytes after an initial growth of {firstGrowth} bytes.");
    }

    private static long CollectMemory() {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        return GC.GetTotalMemory(true);
    }
}

using System;
using System.IO;
using BarcodeReader.ImageSharp;
using Xunit;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for ZXingMemory.
/// </summary>
public partial class ImagePlayground {
    [Fact]
    public void Test_QrCodeRead_MemoryUsageStable() {
        string filePath = Path.Combine(_directoryWithImages, "QRCode1.png");
        long before = GC.GetTotalMemory(true);
        for (int i = 0; i < 50; i++) {
            var result = QrCode.Read(filePath);
            Assert.Equal(Status.Found, result.Status);
        }
        GC.Collect();
        GC.WaitForPendingFinalizers();
        long after = GC.GetTotalMemory(true);
        Assert.True(Math.Abs(after - before) < 1024 * 1024); // less than 1MB diff
    }

    [Fact]
    public void Test_BarCodeRead_MemoryUsageStable() {
        string filePath = Path.Combine(_directoryWithImages, "BarcodeEAN13.png");
        long before = GC.GetTotalMemory(true);
        for (int i = 0; i < 50; i++) {
            var result = BarCode.Read(filePath);
            Assert.Equal(Status.Found, result.Status);
        }
        GC.Collect();
        GC.WaitForPendingFinalizers();
        long after = GC.GetTotalMemory(true);
        Assert.True(Math.Abs(after - before) < 1024 * 1024); // less than 1MB diff
    }
}

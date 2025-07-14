using System;
using System.IO;
using Xunit;

namespace ImagePlayground.Tests;
public partial class ImagePlayground {
    [Fact]
    public void Test_QrCodeGenerate_MemoryUsageStable() {
        string filePath = Path.Combine(_directoryWithTests, "qr_mem.png");
        long before = GC.GetTotalMemory(true);
        for (int i = 0; i < 50; i++) {
            QrCode.Generate("https://example.com", filePath);
            if (File.Exists(filePath)) {
                File.Delete(filePath);
            }
        }
        GC.Collect();
        GC.WaitForPendingFinalizers();
        long after = GC.GetTotalMemory(true);
        Assert.True(Math.Abs(after - before) < 1024 * 1024);
    }
}

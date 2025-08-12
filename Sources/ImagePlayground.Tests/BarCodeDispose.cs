using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for BarCodeDispose.
/// </summary>
public partial class ImagePlayground {
    [Fact]
    public void Test_BarCodeRead_DoesNotLockFile() {
        string filePath = Path.Combine(_directoryWithImages, "BarcodeEAN13.png");
        var result = BarCode.Read(filePath);
        Assert.True(result.Message == "9012341234571");
        using var stream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
    }

    [Fact]
    public async Task Test_BarCodeReadAsync_DoesNotLockFile() {
        string filePath = Path.Combine(_directoryWithImages, "BarcodeEAN13.png");
        var result = await BarCode.ReadAsync(filePath);
        Assert.True(result.Message == "9012341234571");
        using var stream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
    }

    [Fact]
    public void Test_QrCodeRead_DoesNotLockFile() {
        string filePath = Path.Combine(_directoryWithImages, "QRCode1.png");
        var result = QrCode.Read(filePath);
        Assert.True(!string.IsNullOrEmpty(result.Message));
        Assert.Null(result.Image);
        using var stream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
    }
}

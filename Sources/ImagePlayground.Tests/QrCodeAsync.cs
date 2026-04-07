using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for async QR code operations.
/// </summary>
public partial class ImagePlayground {
    [Fact]
    public async Task Test_QrCodeGenerateAsync() {
        string filePath = Path.Combine(_directoryWithTests, "QrAsync.png");
        File.Delete(filePath);

        await QrCode.GenerateAsync("https://evotec.xyz", filePath);

        Assert.True(File.Exists(filePath));
        var result = await QrCode.ReadAsync(filePath);
        Assert.Equal(Status.Found, result.Status);
        Assert.Equal("https://evotec.xyz", result.Message);
    }

    [Fact]
    public async Task Test_QrCodeGenerateAsync_Cancelled() {
        string filePath = Path.Combine(_directoryWithTests, "QrAsyncCancelled.png");
        File.Delete(filePath);

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => QrCode.GenerateAsync("https://evotec.xyz", filePath, cancellationToken: cts.Token));
        Assert.False(File.Exists(filePath));
    }

    [Fact]
    public async Task Test_QrCodeReadAsync_Cancelled() {
        string filePath = Path.Combine(_directoryWithImages, "QRCode1.png");
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => QrCode.ReadAsync(filePath, cts.Token));
    }

    [Fact]
    public async Task Test_QrCodeReadAsync_DoesNotLockFile() {
        string filePath = Path.Combine(_directoryWithImages, "QRCode1.png");
        var result = await QrCode.ReadAsync(filePath);
        Assert.Equal(Status.Found, result.Status);
        Assert.True(!string.IsNullOrEmpty(result.Message));
        using var stream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
    }
}

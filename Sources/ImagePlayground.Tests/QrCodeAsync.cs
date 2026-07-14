using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CodeGlyphX;
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
        Assert.NotNull(result);
        Assert.Equal("https://evotec.xyz", result.Text);
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
    public async Task Test_QrCodeReadAsync_CancelsActiveRecognition() {
        string filePath = Path.Combine(_directoryWithImages, "KulekWSluchawkach.jpg");
        QrPixelDecodeOptions options = QrPixelDecodeOptions.Robust();
        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
        var stopwatch = Stopwatch.StartNew();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => QrCode.ReadAsync(filePath, cts.Token, options));

        Assert.True(stopwatch.Elapsed < TimeSpan.FromSeconds(5), $"Cancellation took {stopwatch.Elapsed}.");
    }

    [Fact]
    public async Task Test_QrCodeReadAsync_NonQrImageReturnsNullWithinBudget() {
        string filePath = Path.Combine(_directoryWithImages, "KulekWSluchawkach.jpg");
        QrPixelDecodeOptions options = QrPixelDecodeOptions.Screen(250, 800);
        var stopwatch = Stopwatch.StartNew();

        QrDecoded? result = await QrCode.ReadAsync(filePath, CancellationToken.None, options);

        Assert.Null(result);
        Assert.True(stopwatch.Elapsed < TimeSpan.FromSeconds(5), $"Budgeted recognition took {stopwatch.Elapsed}.");
    }

    [Fact]
    public async Task Test_QrCodeReadAsync_DoesNotLockFile() {
        string filePath = Path.Combine(_directoryWithImages, "QRCode1.png");
        var result = await QrCode.ReadAsync(filePath);
        Assert.NotNull(result);
        Assert.False(string.IsNullOrEmpty(result.Text));
        using var stream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
    }
}

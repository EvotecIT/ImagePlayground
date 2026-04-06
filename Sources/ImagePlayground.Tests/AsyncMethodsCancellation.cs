using System;
using System.IO;
using SixLabors.ImageSharp.Processing;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for cancellation in async image operations.
/// </summary>
public partial class ImagePlayground {
    [Fact]
    public async Task Test_ResizeAsync_Cancelled() {
        string src = Path.Combine(_directoryWithImages, "LogoEvotec.png");
        string dest = Path.Combine(_directoryWithTests, "logo_async_cancel.png");
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => ImageHelper.ResizeAsync(src, dest, 40, 40, cancellationToken: cts.Token));
    }

    [Fact]
    public async Task Test_RotateAsync_Cancelled() {
        string src = Path.Combine(_directoryWithImages, "PrzemyslawKlysAndKulkozaurr.jpg");
        string dest = Path.Combine(_directoryWithTests, "rotate_async_cancel.jpg");
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => ImageHelper.RotateAsync(src, dest, 90, cts.Token));
    }

    [Fact]
    public async Task Test_RotateAsync_Mode_Cancelled() {
        string src = Path.Combine(_directoryWithImages, "PrzemyslawKlysAndKulkozaurr.jpg");
        string dest = Path.Combine(_directoryWithTests, "rotate_mode_async_cancel.jpg");
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => ImageHelper.RotateAsync(src, dest, RotateMode.Rotate90, cts.Token));
    }

    [Fact]
    public async Task Test_ResizeAsync_Percentage_Cancelled() {
        string src = Path.Combine(_directoryWithImages, "LogoEvotec.png");
        string dest = Path.Combine(_directoryWithTests, "logo_async_percentage_cancel.png");
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => ImageHelper.ResizeAsync(src, dest, 50, cts.Token));
    }

    [Fact]
    public async Task Test_BlurAsync_Cancelled() {
        string src = Path.Combine(_directoryWithImages, "LogoEvotec.png");
        string dest = Path.Combine(_directoryWithTests, "logo_blur_async_cancel.png");
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => ImageHelper.BlurAsync(src, dest, 5, cts.Token));
    }

    [Fact]
    public async Task Test_SharpenAsync_Cancelled() {
        string src = Path.Combine(_directoryWithImages, "LogoEvotec.png");
        string dest = Path.Combine(_directoryWithTests, "logo_sharpen_async_cancel.png");
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => ImageHelper.SharpenAsync(src, dest, 2, cts.Token));
    }

    [Fact]
    public async Task Test_AdjustAsync_Cancelled() {
        string src = Path.Combine(_directoryWithImages, "LogoEvotec.png");
        string dest = Path.Combine(_directoryWithTests, "logo_adjust_async_cancel.png");
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => ImageHelper.AdjustAsync(src, dest, brightness: 1.1f, cancellationToken: cts.Token));
    }

    [Fact]
    public async Task Test_WatermarkAsync_Cancelled() {
        string src = Path.Combine(_directoryWithImages, "QRCode1.png");
        string dest = Path.Combine(_directoryWithTests, "watermark_async_cancel.png");
        string watermark = Path.Combine(_directoryWithImages, "LogoEvotec.png");
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => ImageHelper.WatermarkImageAsync(src, dest, watermark, WatermarkPlacement.Middle, cancellationToken: cts.Token));
    }

    [Fact]
    public async Task Test_WatermarkTiledAsync_Cancelled() {
        string src = Path.Combine(_directoryWithImages, "QRCode1.png");
        string dest = Path.Combine(_directoryWithTests, "watermark_tiled_async_cancel.png");
        string watermark = Path.Combine(_directoryWithImages, "LogoEvotec.png");
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => ImageHelper.WatermarkImageTiledAsync(src, dest, watermark, 20, cancellationToken: cts.Token));
    }
}


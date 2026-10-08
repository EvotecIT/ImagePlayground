using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for cancellation in barcode reading.
/// </summary>
public partial class ImagePlayground {
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Test_BarCodeRead_CancelledBeforeResolvingInput(bool useOptionsToken) {
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        var token = useOptionsToken ? default : cts.Token;
        var options = new ScanOptions { CancellationToken = useOptionsToken ? cts.Token : default };

        // An empty path would fail resolution, so cancellation must take precedence.
        if (!useOptionsToken) {
            await Assert.ThrowsAnyAsync<OperationCanceledException>(() => BarCode.ReadAsync(string.Empty, token));
        }
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => BarCode.ReadAsync(string.Empty, token, options));
        Assert.ThrowsAny<OperationCanceledException>(() => BarCode.Read(string.Empty, options, token));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Test_BarCodeScan_CancelledBeforeResolvingInput(bool useOptionsToken) {
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        var token = useOptionsToken ? default : cts.Token;
        var options = new ScanOptions { CancellationToken = useOptionsToken ? cts.Token : default };

        var scan = await BarCode.ScanAsync(string.Empty, options, token);
        Assert.Equal(ScanStatus.Cancelled, scan.Status);
        Assert.Equal(ScanCompletionReason.Cancelled, scan.CompletionReason);
        Assert.Empty(scan.Symbols);
        var synchronousScan = BarCode.Scan(string.Empty, options, token);
        Assert.Equal(ScanStatus.Cancelled, synchronousScan.Status);
        Assert.Equal(ScanCompletionReason.Cancelled, synchronousScan.CompletionReason);
    }

    [Fact]
    public async Task Test_BarCodeReadAsync_Cancelled() {
        string filePath = Path.Combine(_directoryWithImages, "BarcodeEAN13.png");
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => BarCode.ReadAsync(filePath, cts.Token));
    }

    [Fact]
    public async Task Test_BarCodeGenerateAsync_Cancelled() {
        string filePath = Path.Combine(_directoryWithTests, "BarcodeCancelled.png");
        File.Delete(filePath);

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => BarCode.GenerateAsync(SymbolFormat.Ean, "9012341234571", filePath, cts.Token));
        Assert.False(File.Exists(filePath));
    }
}


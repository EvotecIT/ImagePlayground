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
    [Fact]
    public async Task Test_BarCodeReadAsync_Cancelled() {
        string filePath = Path.Combine(_directoryWithImages, "BarcodeEAN13.png");
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => BarCode.ReadAsync(filePath, cts.Token));
    }
}


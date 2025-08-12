using System;
using System.IO;
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
}


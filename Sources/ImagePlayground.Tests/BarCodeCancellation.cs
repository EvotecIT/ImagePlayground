using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
    public async Task Test_BarCodeRead_CancelledDuringInputResolutionPreservesToken(bool useOptionsToken) {
        using var cancellation = new CancellationTokenSource();
        using var unrelated = new CancellationTokenSource();
        using var watchdog = new CancellationTokenSource(TimeSpan.FromSeconds(15));
        var methodToken = useOptionsToken ? unrelated.Token : cancellation.Token;
        var options = new ScanOptions { CancellationToken = useOptionsToken ? cancellation.Token : unrelated.Token };
        byte[] image = File.ReadAllBytes(Path.Combine(_directoryWithImages, "BarcodeEAN13.png"));
        var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        using var stopListener = watchdog.Token.Register(listener.Stop);
        int port = ((IPEndPoint)listener.LocalEndpoint).Port;
        var response = Task.Run(async () => {
            using var client = await listener.AcceptTcpClientAsync();
            using var stopClient = watchdog.Token.Register(client.Close);
            using var stream = client.GetStream();
            using var reader = new StreamReader(stream, Encoding.ASCII, false, 1024, true);
            while (!string.IsNullOrEmpty(await reader.ReadLineAsync())) { }
            // The public read has passed its initial checks and is resolving this input.
            cancellation.Cancel();
            byte[] header = Encoding.ASCII.GetBytes($"HTTP/1.1 200 OK\r\nContent-Type: image/png\r\nContent-Length: {image.Length}\r\nConnection: close\r\n\r\n");
            await stream.WriteAsync(header, 0, header.Length);
            await stream.WriteAsync(image, 0, image.Length);
        });
        try {
            var error = await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
                BarCode.ReadAsync($"http://127.0.0.1:{port}/barcode.png", methodToken, options));
            await response;
            Assert.Equal(cancellation.Token, error.CancellationToken);
            Assert.True(error.CancellationToken.IsCancellationRequested);
            Assert.False(unrelated.IsCancellationRequested);
        } finally {
            listener.Stop();
            Helpers.CleanupTempFiles();
        }
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


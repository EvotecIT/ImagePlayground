using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodeGlyphX;

namespace ImagePlayground;

public partial class BarCode {
    /// <summary>Reads the first barcode, returning null only when a completed scan finds none.</summary>
    /// <remarks>Default scans allow five seconds and exclude QR, Pharmacode, and Patch Code. Cancellation throws; a deadline throws TimeoutException. Use Scan to retain partial results and completion details.</remarks>
    public static DetectedSymbol? Read(string filePath) => ReadAsync(filePath).GetAwaiter().GetResult();

    /// <summary>Reads the first barcode with explicit scanning options.</summary>
    public static DetectedSymbol? Read(string filePath, ScanOptions? options, CancellationToken cancellationToken = default)
        => ReadAsync(filePath, cancellationToken, options).GetAwaiter().GetResult();

    /// <summary>Reads the first barcode asynchronously; cancellation throws rather than returning null.</summary>
    public static Task<DetectedSymbol?> ReadAsync(string filePath, CancellationToken cancellationToken = default)
        => ReadAsync(filePath, cancellationToken, options: null);

    /// <summary>Reads the first barcode asynchronously with explicit scanning options.</summary>
    public static async Task<DetectedSymbol?> ReadAsync(string filePath, CancellationToken cancellationToken, ScanOptions? options) {
        var scan = await SymbolScanner.ScanFileAsync(Helpers.ResolvePath(filePath), CreateScanOptions(options, firstMatch: true), cancellationToken).ConfigureAwait(false);
        if (scan.CompletionReason == ScanCompletionReason.Cancelled) {
            throw new OperationCanceledException(scan.Failure, cancellationToken);
        }
        if (scan.CompletionReason == ScanCompletionReason.DeadlineExceeded) {
            throw new TimeoutException(scan.Failure ?? "Barcode recognition exceeded its deadline.");
        }
        return scan.Status switch {
            ScanStatus.Success => scan.Symbols.FirstOrDefault(),
            ScanStatus.NoSymbolFound => null,
            ScanStatus.InvalidImage => throw new InvalidDataException(scan.Failure),
            ScanStatus.UnsupportedFormats => throw new NotSupportedException(scan.Failure),
            _ => throw new InvalidOperationException(scan.Failure ?? $"Barcode scan failed: {scan.Status}.")
        };
    }

    /// <summary>Scans barcodes and returns symbols with deadline, cancellation, and unsupported-format details.</summary>
    /// <remarks>Partial results remain available. Explicit formats may opt into Pharmacode or Patch Code; QR formats use the CodeGlyphX scanner directly. Supplied options are not modified.</remarks>
    public static ScanResult Scan(string filePath, ScanOptions? options = null, CancellationToken cancellationToken = default)
        => ScanAsync(filePath, options, cancellationToken).GetAwaiter().GetResult();

    /// <summary>Scans barcodes asynchronously, retaining partial results and structured cancellation.</summary>
    public static Task<ScanResult> ScanAsync(string filePath, ScanOptions? options = null, CancellationToken cancellationToken = default)
        => SymbolScanner.ScanFileAsync(Helpers.ResolvePath(filePath), CreateScanOptions(options, firstMatch: false), cancellationToken);

    private static ScanOptions CreateScanOptions(ScanOptions? options, bool firstMatch) {
        var source = options ?? new ScanOptions { TimeoutMilliseconds = 5000, MaxSymbols = firstMatch ? 1 : 32 };
        var formats = source.Formats;
        if (formats is null || formats.Length == 0) {
            formats = SymbolCapabilities.All.Where(capability => capability.IsDefaultScanFormat && !IsQrFormat(capability.Format)).Select(capability => capability.Format).ToArray();
        } else if (formats.Any(IsQrFormat)) {
            throw new ArgumentException("Barcode scan formats must exclude QR formats. Use QrCode.Read or SymbolScanner for QR recognition.", nameof(options));
        }

        // SymbolScanner owns the deep snapshot and validation of recognition options.
        return new ScanOptions {
            Formats = formats,
            Region = source.Region,
            TimeoutMilliseconds = source.TimeoutMilliseconds,
            MaxSymbols = source.MaxSymbols,
            Deduplicate = source.Deduplicate,
            EnableTileScan = source.EnableTileScan,
            TileGrid = source.TileGrid,
            Profile = source.Profile,
            Qr = source.Qr,
            Barcode = source.Barcode,
            DirectPartMarking = source.DirectPartMarking,
            Image = source.Image,
            CancellationToken = source.CancellationToken
        };
    }
}

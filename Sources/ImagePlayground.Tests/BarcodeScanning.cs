using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OfficeIMO.Drawing;
using Xunit;

namespace ImagePlayground.Tests;

public partial class ImagePlayground {
    [Theory]
    [InlineData(SymbolFormat.MaxiCode)]
    [InlineData(SymbolFormat.Gs1Composite)]
    public void BarcodeUnsupportedGeometryDoesNotOverwriteOutput(SymbolFormat format) {
        string path = Path.Combine(_directoryWithTests, "unsupported-" + format + ".png");
        File.WriteAllText(path, "existing output");
        Assert.Throws<NotSupportedException>(() => BarCode.Generate(format, "1234567890123", path));
        Assert.Equal("existing output", File.ReadAllText(path));
    }

    [Fact]
    public async Task BarcodeScanRetainsStructuredCancellationWhileReadThrows() {
        string path = Path.Combine(_directoryWithImages, "BarcodeEAN13.png");
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        var scan = await BarCode.ScanAsync(path, cancellationToken: cancellation.Token);
        Assert.Equal(ScanStatus.Cancelled, scan.Status);
        Assert.Equal(ScanCompletionReason.Cancelled, scan.CompletionReason);
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => BarCode.ReadAsync(path, cancellation.Token));
    }

    [Fact]
    public void BarcodeScanReturnsOwnerResultWithoutMutatingCallerOptions() {
        string path = Path.Combine(_directoryWithTests, "scan-ean.png");
        BarCode.GenerateEan("9012341234571", path);
        var formats = new[] { SymbolFormat.Ean };
        var options = new ScanOptions { Formats = formats, MaxSymbols = 1, TimeoutMilliseconds = 5000 };
        var scan = BarCode.Scan(path, options);
        Assert.True(scan.IsSuccess, scan.Failure);
        Assert.Equal("9012341234571", Assert.Single(scan.Symbols).Text);
        Assert.Equal(SymbolFormat.Ean, scan.Symbols[0].Format);
        Assert.Same(formats, options.Formats);
        Assert.Equal(5000, options.TimeoutMilliseconds);
    }

    [Fact]
    public void BarcodeReadDistinguishesBlankAndInvalidImages() {
        string blankPath = Path.Combine(_directoryWithTests, "blank-barcode.png");
        using (var blank = global::ImagePlayground.Image.FromRaster(new OfficeRasterImage(48, 48, OfficeColor.White))) {
            blank.Save(blankPath);
        }
        var options = new ScanOptions { Formats = new[] { SymbolFormat.Ean }, TimeoutMilliseconds = 5000 };
        Assert.Null(BarCode.Read(blankPath, options));
        string invalidPath = Path.Combine(_directoryWithTests, "invalid-barcode.bin");
        File.WriteAllText(invalidPath, "not an image");
        Assert.Throws<InvalidDataException>(() => BarCode.Read(invalidPath, options));
    }

    [Fact]
    public async Task QrLogoSvgEmbedsImageAndInvalidLogoPreservesOutput() {
        string path = Path.Combine(_directoryWithTests, "qr-logo.svg");
        string logo = Path.Combine(_directoryWithImages, "LogoEvotec.png");
        await QrCode.GenerateAsync("https://example.com/logo", path, logo);
        var document = System.Xml.Linq.XDocument.Load(path);
        Assert.Contains(document.Descendants(), element => element.Name.LocalName == "image" && element.Attributes().Any(attribute => attribute.Value.StartsWith("data:image/png;base64,")));
        string previous = File.ReadAllText(path);
        await Assert.ThrowsAsync<FileNotFoundException>(() => QrCode.GenerateAsync("changed", path, path + ".missing"));
        Assert.Equal(previous, File.ReadAllText(path));
    }

    [Theory]
    [InlineData(QrErrorCorrectionLevel.Q)]
    [InlineData(QrErrorCorrectionLevel.H)]
    public void QrGenerationPreservesExplicitErrorCorrection(QrErrorCorrectionLevel level) {
        string path = Path.Combine(_directoryWithTests, "qr-ecc-" + level + ".png");
        QrCode.Generate("encoding contract", path, eccLevel: level, pixelSize: 8);
        var decoded = QrCode.Read(path);
        Assert.NotNull(decoded);
        Assert.Equal(level, decoded.ErrorCorrectionLevel);
    }
}

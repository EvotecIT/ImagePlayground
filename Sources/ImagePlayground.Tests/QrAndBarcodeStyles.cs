using System.IO;
using CodeGlyphX;
using CodeGlyphX.Rendering;
using Xunit;

namespace ImagePlayground.Tests;

public partial class ImagePlayground {
    [Fact]
    public void Test_QrPngOutputIsCompressed() {
        var file = Path.Combine(_directoryWithTests, "qr-compressed.png");
        QrCode.Generate("https://codeglyphx.com", file);

        Assert.True(new FileInfo(file).Length < 2048);
        AssertQrDecoded(file, "https://codeglyphx.com");
    }

    [Fact]
    public void Test_QrStyleAppliesSizeShapesAndErrorCorrection() {
        var file = Path.Combine(_directoryWithTests, "qr-styled.png");
        var style = new QrImageStyle {
            Size = 300,
            QuietZone = 2,
            ModuleShape = QrModuleShape.Rounded,
            EyeShape = QrModuleShape.Circle,
            EyeColor = OfficeIMO.Drawing.OfficeColor.Parse("#4c1d95")
        };
        QrCode.Generate("https://codeglyphx.com", file, style, QrErrorCorrectionLevel.H);

        using (var image = Image.Load(file)) {
            Assert.True(image.Width <= 300);
            Assert.True(image.Width > 250);
        }
        AssertQrDecoded(file, "https://codeglyphx.com");
    }

    [Fact]
    public void Test_QrStyleRejectsInvalidLogoScale() {
        var style = new QrImageStyle { LogoScale = 0.9 };
        Assert.Throws<System.ArgumentOutOfRangeException>(() => QrCode.Generate("x", Path.Combine(_directoryWithTests, "qr-bad-logo.png"), style));
    }

    [Fact]
    public void Test_BarcodeStyleAppliesTextAndCompression() {
        var file = Path.Combine(_directoryWithTests, "barcode-styled.png");
        BarCode.Generate(SymbolFormat.Code128, "EVO-LAP-0142", file, new BarCodeStyle { ModuleSize = 3, Height = 30, ShowText = true });

        Assert.True(new FileInfo(file).Length < 2048);
        var result = BarCode.Read(file, new ScanOptions { Formats = new[] { SymbolFormat.Code128 }, TimeoutMilliseconds = 5000, MaxSymbols = 1 });
        Assert.NotNull(result);
        Assert.Equal("EVO-LAP-0142", result!.Text);
    }
}

using System.IO;
using Xunit;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for BarCodes.
/// </summary>

public partial class ImagePlayground {
    [Theory]
    [InlineData(SymbolFormat.Code128, "1234567890", "barcode_code128.png", "1234567890", true)]
    [InlineData(SymbolFormat.Code93, "HELLOCODE93", "barcode_code93.png", "HELLOCODE93", true)]
    [InlineData(SymbolFormat.Code39, "HELLO39", "barcode_code39.png", "HELLO39N", true)]
    [InlineData(SymbolFormat.KixCode, "1234567890AB", "barcode_kix.png", "", false)]
    [InlineData(SymbolFormat.UpcE, "123456", "barcode_upce.png", "01234565", true)]
    [InlineData(SymbolFormat.UpcA, "123456789012", "barcode_upca.png", "123456789012", true)]
    [InlineData(SymbolFormat.Ean, "9012341234571", "barcode_ean.png", "9012341234571", true)]
    [InlineData(SymbolFormat.DataMatrix, "MatrixTest", "barcode_datamatrix.png", "MatrixTest", true)]
    [InlineData(SymbolFormat.Pdf417, "Pdf417Example", "barcode_pdf417.png", "Pdf417Example", true)]
    [InlineData(SymbolFormat.DotCode, "1234567890123", "barcode_dotcode.png", "", false)]
    [InlineData(SymbolFormat.HanXin, "1234567890123", "barcode_hanxin.png", "", false)]
    [InlineData(SymbolFormat.Gs1DataBarStackedOmnidirectional, "1234567890123", "barcode_stacked_omni.png", "", false)]
    public void Test_AllBarCodes(SymbolFormat type, string value, string fileName, string expected, bool shouldDecode) {
        string filePath = Path.Combine(_directoryWithTests, fileName);
        if (File.Exists(filePath)) File.Delete(filePath);

        BarCode.Generate(type, value, filePath);

        Assert.True(File.Exists(filePath));
        if (shouldDecode) {
            var result = BarCode.Read(filePath, new ScanOptions { Formats = new[] { type }, TimeoutMilliseconds = 5000, MaxSymbols = 1 });
            Assert.NotNull(result);
            Assert.Equal(expected, result.Text);
            Assert.Equal(type, result.Format);
        } else {
            using var image = SixLabors.ImageSharp.Image.Load(filePath);
            Assert.True(image.Width > 0 && image.Height > 0);
        }
    }
}

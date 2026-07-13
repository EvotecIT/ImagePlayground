using System.IO;
using Xunit;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for BarCodes.
/// </summary>

public partial class ImagePlayground {
    [Theory]
    [InlineData(BarcodeType.Code128, "1234567890", "barcode_code128.png", "1234567890", true)]
    [InlineData(BarcodeType.Code93, "HELLOCODE93", "barcode_code93.png", "HELLOCODE93", true)]
    [InlineData(BarcodeType.Code39, "HELLO39", "barcode_code39.png", "HELLO39N", true)]
    [InlineData(BarcodeType.KixCode, "1234567890AB", "barcode_kix.png", "", false)]
    [InlineData(BarcodeType.UPCE, "123456", "barcode_upce.png", "01234565", true)]
    [InlineData(BarcodeType.UPCA, "123456789012", "barcode_upca.png", "123456789012", true)]
    [InlineData(BarcodeType.EAN, "9012341234571", "barcode_ean.png", "9012341234571", true)]
    [InlineData(BarcodeType.DataMatrix, "MatrixTest", "barcode_datamatrix.png", "MatrixTest", true)]
    [InlineData(BarcodeType.PDF417, "Pdf417Example", "barcode_pdf417.png", "Pdf417Example", true)]
    public void Test_AllBarCodes(BarcodeType type, string value, string fileName, string expected, bool shouldDecode) {
        string filePath = Path.Combine(_directoryWithTests, fileName);
        if (File.Exists(filePath)) File.Delete(filePath);

        BarCode.Generate(type, value, filePath);

        Assert.True(File.Exists(filePath));
        var result = BarCode.Read(filePath);
        Assert.Equal(shouldDecode, result is not null);
        if (shouldDecode) {
            Assert.NotNull(result);
            Assert.Equal(expected, result.Text);
        }
    }
}

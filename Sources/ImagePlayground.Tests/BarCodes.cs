using System.IO;
using BarcodeReader.ImageSharp;
using Xunit;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for BarCodes.
/// </summary>

public partial class ImagePlayground {
    [Theory]
    [InlineData(BarcodeType.Code128, "1234567890", "barcode_code128.png", "1234567890", Status.Found)]
    [InlineData(BarcodeType.Code93, "HELLOCODE93", "barcode_code93.png", "HELLOCODE93", Status.Found)]
    [InlineData(BarcodeType.Code39, "HELLO39", "barcode_code39.png", "HELLO39N", Status.Found)]
    [InlineData(BarcodeType.KixCode, "1234567890AB", "barcode_kix.png", "", Status.NotFound)]
    [InlineData(BarcodeType.UPCE, "123456", "barcode_upce.png", "01234565", Status.Found)]
    [InlineData(BarcodeType.UPCA, "123456789012", "barcode_upca.png", "123456789012", Status.Found)]
    [InlineData(BarcodeType.EAN, "9012341234571", "barcode_ean.png", "9012341234571", Status.Found)]
    [InlineData(BarcodeType.DataMatrix, "MatrixTest", "barcode_datamatrix.png", "MatrixTest", Status.Found)]
    [InlineData(BarcodeType.PDF417, "Pdf417Example", "barcode_pdf417.png", "Pdf417Example", Status.Found)]
    public void Test_AllBarCodes(BarcodeType type, string value, string fileName, string expected, Status status) {
        string filePath = Path.Combine(_directoryWithTests, fileName);
        if (File.Exists(filePath)) File.Delete(filePath);

        BarCode.Generate(type, value, filePath);

        Assert.True(File.Exists(filePath));
        var result = BarCode.Read(filePath);
        Assert.Equal(status, result.Status);
        if (status == Status.Found) {
            Assert.Equal(expected, result.Message);
        }
    }
}
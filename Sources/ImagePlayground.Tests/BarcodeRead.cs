using System.IO;
using BarcodeReader.ImageSharp;
using Xunit;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for reading barcodes.
/// </summary>
public partial class ImagePlayground {
    [Fact]
    public void Test_BarCodeRead_ReturnsError_ForInvalidImage() {
        string filePath = Path.Combine(_directoryWithImages, "LogoEvotec.png");
        var result = BarCode.Read(filePath);
        Assert.Equal(Status.NotFound, result.Status);
    }
}


using System;
using System.IO;
using Xunit;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for invalid BarCode generation.
/// </summary>
public partial class ImagePlayground {
    [Fact]
    public void Test_BarCode_Generate_InvalidType_Throws() {
        string filePath = Path.Combine(_directoryWithTests, "barcode_invalid.png");
        Assert.Throws<ArgumentOutOfRangeException>(() => BarCode.Generate((BarcodeType)999, "invalid", filePath));
    }
}


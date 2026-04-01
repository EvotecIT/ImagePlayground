using System;
using System.IO;
using Xunit;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for BarCode output directory creation.
/// </summary>
public partial class ImagePlayground {
    [Fact]
    public void Test_BarCode_CreatesOutputDirectory_ForCode128() {
        string destDir = Path.Combine(_directoryWithTests, "barcode_created");
        string dest = Path.Combine(destDir, "barcode.png");
        if (Directory.Exists(destDir)) {
            Directory.Delete(destDir, true);
        }

        BarCode.GenerateCode128("123456", dest);

        Assert.True(File.Exists(dest));
    }

    [Fact]
    public void Test_BarCode_Code128_RejectsDisabledChecksum() {
        string dest = Path.Combine(_directoryWithTests, "barcode_no_checksum.png");

        Assert.Throws<NotSupportedException>(() => BarCode.GenerateCode128("123456", dest, includeChecksum: false));
    }

    [Fact]
    public void Test_BarCode_CreatesOutputDirectory_ForPdf417() {
        string destDir = Path.Combine(_directoryWithTests, "barcode_pdf417_created");
        string dest = Path.Combine(destDir, "barcode.png");
        if (Directory.Exists(destDir)) {
            Directory.Delete(destDir, true);
        }

        BarCode.GeneratePdf417("Pdf417Example", dest);

        Assert.True(File.Exists(dest));
    }
}


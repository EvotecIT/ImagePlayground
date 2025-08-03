using System.IO;
using Xunit;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for directory creation when saving barcodes.
/// </summary>
public partial class ImagePlayground {
    [Fact]
    public void Test_BarCodeSave_CreatesParentDirectory() {
        string dir = Path.Combine(_directoryWithTests, "BarcodeOutput");
        string file = Path.Combine(dir, "code128.png");
        if (Directory.Exists(dir)) {
            Directory.Delete(dir, true);
        }

        BarCode.GenerateCode128("123456", file);

        Assert.True(Directory.Exists(dir));
        Assert.True(File.Exists(file));
    }
}

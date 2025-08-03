using System.IO;
using Xunit;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for output directory creation in Combine.
/// </summary>
public partial class ImagePlayground {
    [Fact]
    public void Test_Combine_CreatesOutputDirectory() {
        string file1 = Path.Combine(_directoryWithImages, "LogoEvotec.png");
        string file2 = Path.Combine(_directoryWithImages, "QRCode1.png");
        string destDir = Path.Combine(_directoryWithTests, "combine_created");
        string dest = Path.Combine(destDir, "combine.png");
        if (Directory.Exists(destDir)) {
            Directory.Delete(destDir, true);
        }

        ImageHelper.Combine(file1, file2, dest);

        Assert.True(File.Exists(dest));
    }
}

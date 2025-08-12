using System.IO;
using Xunit;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for relative output paths in ConvertTo.
/// </summary>
public partial class ImagePlayground {
    [Fact]
    public void Test_ConvertTo_RelativeOutputPath_CreatesFile() {
        string src = Path.Combine(_directoryWithImages, "QRCode1.png");
        string relativeDir = Path.Combine("Tests", "relative");
        string relativePath = Path.Combine(relativeDir, "converted.jpg");
        if (File.Exists(relativePath)) File.Delete(relativePath);
        if (Directory.Exists(relativeDir)) Directory.Delete(relativeDir, true);

        ImageHelper.ConvertTo(src, relativePath);
        string expectedFullPath = Path.GetFullPath(relativePath);
        Assert.True(File.Exists(expectedFullPath));
    }
}

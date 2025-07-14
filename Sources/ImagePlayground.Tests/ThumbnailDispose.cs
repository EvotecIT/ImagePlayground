using System.IO;
using Xunit;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for ThumbnailDispose.
/// </summary>

public partial class ImagePlayground {
    [Fact]
    public void Test_GetImageThumbnail_DoesNotLockSourceFile() {
        string src = Path.Combine(_directoryWithImages, "LogoEvotec.png");
        using var thumb = ImageHelper.GetImageThumbnail(src, 32, 32);
        thumb.Dispose();
        using var stream = File.Open(src, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
    }
}

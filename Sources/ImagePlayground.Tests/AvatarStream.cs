using System.IO;
using Xunit;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for AvatarStream.
/// </summary>
public partial class ImagePlayground {
    [Fact]
    public void Test_SaveAsAvatar_StreamReset() {
        string filePath = Path.Combine(_directoryWithImages, "LogoEvotec.png");
        using var img = Image.Load(filePath);
        using var ms = new MemoryStream();
        img.SaveAsAvatar(ms, 64, 64, 10f);
        Assert.Equal(0, ms.Position);
        Assert.True(ms.Length > 0);
    }
}

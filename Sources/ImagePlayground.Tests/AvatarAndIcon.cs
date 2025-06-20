using SixLabors.ImageSharp;
using System.IO;
using Xunit;

namespace ImagePlayground.Tests;
public partial class ImagePlayground {
    [Fact]
    public void Test_SaveAsAvatar_File() {
        string src = Path.Combine(_directoryWithImages, "LogoEvotec.png");
        string dest = Path.Combine(_directoryWithTests, "avatar.png");
        if (File.Exists(dest)) File.Delete(dest);
        using var img = Image.Load(src);
        img.SaveAsAvatar(dest, 64, 64, 10f);
        Assert.True(File.Exists(dest));
        using var avatar = Image.Load(dest);
        Assert.Equal(64, avatar.Width);
        Assert.Equal(64, avatar.Height);
    }

    [Fact]
    public void Test_SaveAsCircularAvatar_File() {
        string src = Path.Combine(_directoryWithImages, "LogoEvotec.png");
        string dest = Path.Combine(_directoryWithTests, "avatar_circular.png");
        if (File.Exists(dest)) File.Delete(dest);
        using var img = Image.Load(src);
        img.SaveAsCircularAvatar(dest, 50);
        Assert.True(File.Exists(dest));
        using var avatar = Image.Load(dest);
        Assert.Equal(50, avatar.Width);
        Assert.Equal(50, avatar.Height);
    }

    [Fact]
    public void Test_SaveAsCircularAvatar_Stream() {
        string src = Path.Combine(_directoryWithImages, "LogoEvotec.png");
        using var img = Image.Load(src);
        using var ms = new MemoryStream();
        img.SaveAsCircularAvatar(ms, 32);
        Assert.Equal(0, ms.Position);
        Assert.True(ms.Length > 0);
        ms.Position = 0;
        using var avatar = SixLabors.ImageSharp.Image.Load(ms);
        Assert.Equal(32, avatar.Width);
        Assert.Equal(32, avatar.Height);
    }

    [Fact]
    public void Test_SaveAsIcon_CreatesFile() {
        string src = Path.Combine(_directoryWithImages, "LogoEvotec.png");
        string dest = Path.Combine(_directoryWithTests, "icon.ico");
        if (File.Exists(dest)) File.Delete(dest);
        using var img = Image.Load(src);
        img.SaveAsIcon(dest);
        Assert.True(File.Exists(dest));
        Assert.True(new FileInfo(dest).Length > 0);
    }

    [Fact]
    public void Test_SaveAsIcon_MultipleSizes() {
        string src = Path.Combine(_directoryWithImages, "LogoEvotec.png");
        string dest = Path.Combine(_directoryWithTests, "icon_multi.ico");
        if (File.Exists(dest)) File.Delete(dest);
        using var img = Image.Load(src);
        img.SaveAsIcon(dest, 16, 32, 48);
        Assert.True(File.Exists(dest));
        Assert.True(new FileInfo(dest).Length > 0);
    }
}

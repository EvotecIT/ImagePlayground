using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SixLabors.ImageSharp.Metadata.Profiles.Iptc;
using SixLabors.ImageSharp.Metadata.Profiles.Xmp;
using System.IO;
using Xunit;
using PlaygroundImage = ImagePlayground.Image;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for removing metadata profiles.
/// </summary>
public partial class ImagePlayground {
    [Fact]
    public void Test_Remove_Metadata() {
        string imgPath = Path.Combine(_directoryWithTests, "metadata_remove.jpg");
        string outPath = Path.Combine(_directoryWithTests, "metadata_removed.jpg");
        if (File.Exists(imgPath)) File.Delete(imgPath);
        if (File.Exists(outPath)) File.Delete(outPath);

        using (var img = new PlaygroundImage()) {
            img.Create(imgPath, 20, 20);
            img.SetExifValue(ExifTag.Software, "ImagePlayground");
            img.Metadata.XmpProfile = new XmpProfile();
            img.Metadata.IptcProfile = new IptcProfile();
            img.Save();
        }

        ImageHelper.RemoveMetadata(imgPath, outPath);

        using var check = PlaygroundImage.Load(outPath);
        Assert.Null(check.Metadata.ExifProfile);
        Assert.Null(check.Metadata.XmpProfile);
        Assert.Null(check.Metadata.IptcProfile);
    }
}

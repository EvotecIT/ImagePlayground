using OfficeIMO.Drawing;
using Color = OfficeIMO.Drawing.OfficeColor;
using ExifTag = OfficeIMO.Drawing.OfficeExifTag;
using Rgba32 = OfficeIMO.Drawing.OfficeColor;
using System.IO;
using Xunit;
using PlaygroundImage = global::ImagePlayground.Image;

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
            img.Metadata.XmpProfile = System.Text.Encoding.UTF8.GetBytes("<x:xmpmeta xmlns:x=\"adobe:ns:meta/\" />");
            img.Metadata.IptcProfile = new byte[] { 0x1c, 2, 5, 0, 4, 84, 101, 115, 116 };
            img.Save();
        }

        ImageHelper.RemoveMetadata(imgPath, outPath);

        using var check = PlaygroundImage.Load(outPath);
        Assert.Null(check.Metadata.ExifProfile);
        Assert.Null(check.Metadata.XmpProfile);
        Assert.Null(check.Metadata.IptcProfile);
    }
}

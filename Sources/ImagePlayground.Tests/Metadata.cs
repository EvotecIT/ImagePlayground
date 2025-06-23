using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using System.IO;
using Xunit;
using PlaygroundImage = ImagePlayground.Image;

namespace ImagePlayground.Tests;
public partial class ImagePlayground {
    [Fact]
    public void Test_Metadata_RoundTrip() {
        string imgPath = Path.Combine(_directoryWithTests, "metadata.jpg");
        string metaPath = Path.Combine(_directoryWithTests, "metadata.json");
        if (File.Exists(imgPath)) File.Delete(imgPath);
        if (File.Exists(metaPath)) File.Delete(metaPath);

        using (var img = new PlaygroundImage()) {
            img.Create(imgPath, 10, 10);
            img.Metadata.HorizontalResolution = 300;
            img.Metadata.VerticalResolution = 300;
            img.SetExifValue(ExifTag.Software, "ImagePlayground");
            img.Save();
        }

        ImageHelper.ExportMetadata(imgPath, metaPath);

        using (var img = PlaygroundImage.Load(imgPath)) {
            img.Metadata.HorizontalResolution = 72;
            img.Metadata.VerticalResolution = 72;
            img.ClearExifValues();
            img.Save();
        }

        ImageHelper.ImportMetadata(imgPath, metaPath, imgPath);

        using var check = PlaygroundImage.Load(imgPath);
        Assert.Equal(300, check.Metadata.HorizontalResolution);
        Assert.Equal(300, check.Metadata.VerticalResolution);
        Assert.Contains(check.GetExifValues(), v => v.Tag == ExifTag.Software && v.GetValue()?.ToString() == "ImagePlayground");
    }
}

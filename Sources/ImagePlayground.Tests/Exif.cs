using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using System;
using System.IO;
using Xunit;
using PlaygroundImage = ImagePlayground.Image;

namespace ImagePlayground.Tests;
public partial class ImagePlayground {
    [Fact]
    public void Test_ReadExif() {
        string filePath = Path.Combine(_directoryWithImages, "exif-read.jpg");
        using (var img = new PlaygroundImage()) {
            img.Create(filePath, 10, 10);
            img.SetExifValue(ExifTag.Software, "ImagePlayground");
            img.Save();
        }

        using var loaded = Image.Load(filePath);
        var values = loaded.GetExifValues();
        Assert.Contains(values, v => v.Tag == ExifTag.Software && v.GetValue()?.ToString() == "ImagePlayground");
    }

    [Fact]
    public void Test_SetAndRemoveExif() {
        string dest = Path.Combine(_directoryWithTests, "exif-edit.jpg");
        if (File.Exists(dest)) File.Delete(dest);

        using (var img = new PlaygroundImage()) {
            img.Create(dest, 10, 10);
            img.SetExifValue(ExifTag.Software, "ImagePlayground");
            img.Save();
        }

        using (var img = Image.Load(dest)) {
            img.SetExifValue(ExifTag.Software, "Modified");
            img.Save(dest);
        }

        using (var img = Image.Load(dest)) {
            Assert.Equal("Modified", img.GetExifValues()[0].GetValue());
            img.RemoveExifValues(ExifTag.Software);
            img.Save();
        }

        using var check = Image.Load(dest);
        Assert.Empty(check.GetExifValues());
    }
}

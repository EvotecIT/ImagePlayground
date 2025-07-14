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
            img.Create(imgPath, 20, 20);
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

        var options1 = new ImageHelper.ImportMetadataOptions(imgPath, metaPath, imgPath);
        ImageHelper.ImportMetadata(options1);

        using var check = PlaygroundImage.Load(imgPath);
        Assert.Equal(300, check.Metadata.HorizontalResolution);
        Assert.Equal(300, check.Metadata.VerticalResolution);
        Assert.Contains(check.GetExifValues(), v => v.Tag == ExifTag.Software && v.GetValue()?.ToString() == "ImagePlayground");
    }

    [Fact]
    public void Test_Metadata_RoundTrip_Edits() {
        string imgPath = Path.Combine(_directoryWithTests, "metadata_edit.jpg");
        string metaPath = Path.Combine(_directoryWithTests, "metadata_edit.json");
        if (File.Exists(imgPath)) File.Delete(imgPath);
        if (File.Exists(metaPath)) File.Delete(metaPath);

        using (var img = new PlaygroundImage()) {
            img.Create(imgPath, 20, 20);
            img.Metadata.HorizontalResolution = 72;
            img.Metadata.VerticalResolution = 72;
            img.Save();
        }

        ImageHelper.ExportMetadata(imgPath, metaPath);

        var node = System.Text.Json.Nodes.JsonNode.Parse(File.ReadAllText(metaPath))!;
        node["HorizontalResolution"] = 150;
        node["VerticalResolution"] = 150;
        File.WriteAllText(metaPath, node.ToJsonString(new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));

        var options2 = new ImageHelper.ImportMetadataOptions(imgPath, metaPath, imgPath);
        ImageHelper.ImportMetadata(options2);

        using var check = PlaygroundImage.Load(imgPath);
        Assert.Equal(150, check.Metadata.HorizontalResolution);
        Assert.Equal(150, check.Metadata.VerticalResolution);
    }

    [Fact]
    public void Test_Metadata_InvalidJson_Throws() {
        string imgPath = Path.Combine(_directoryWithTests, "metadata_invalid.jpg");
        string metaPath = Path.Combine(_directoryWithTests, "metadata_invalid.json");
        if (File.Exists(imgPath)) File.Delete(imgPath);
        if (File.Exists(metaPath)) File.Delete(metaPath);

        using (var img = new PlaygroundImage()) {
            img.Create(imgPath, 20, 20);
            img.Save();
        }

        File.WriteAllText(metaPath, "{ invalid json");

        var options = new ImageHelper.ImportMetadataOptions(imgPath, metaPath, imgPath);
        Assert.Throws<InvalidDataException>(() => ImageHelper.ImportMetadata(options));
    }

    [Fact]
    public void Test_Metadata_MissingProperties_Throws() {
        string imgPath = Path.Combine(_directoryWithTests, "metadata_missing.jpg");
        string metaPath = Path.Combine(_directoryWithTests, "metadata_missing.json");
        if (File.Exists(imgPath)) File.Delete(imgPath);
        if (File.Exists(metaPath)) File.Delete(metaPath);

        using (var img = new PlaygroundImage()) {
            img.Create(imgPath, 20, 20);
            img.Metadata.HorizontalResolution = 72;
            img.Metadata.VerticalResolution = 72;
            img.Save();
        }

        ImageHelper.ExportMetadata(imgPath, metaPath);

        var node = System.Text.Json.Nodes.JsonNode.Parse(File.ReadAllText(metaPath))!;
        node.AsObject().Remove("HorizontalResolution");
        File.WriteAllText(metaPath, node.ToJsonString(new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));

        var options = new ImageHelper.ImportMetadataOptions(imgPath, metaPath, imgPath);
        Assert.Throws<InvalidDataException>(() => ImageHelper.ImportMetadata(options));
    }
}

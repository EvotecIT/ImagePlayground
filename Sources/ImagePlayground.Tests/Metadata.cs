using OfficeIMO.Drawing;
using Color = OfficeIMO.Drawing.OfficeColor;
using ExifTag = OfficeIMO.Drawing.OfficeExifTag;
using Rgba32 = OfficeIMO.Drawing.OfficeColor;
using System.IO;
using Xunit;
using PlaygroundImage = global::ImagePlayground.Image;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for Metadata.
/// </summary>
public partial class ImagePlayground {
    [Theory]
    [InlineData("utf8-bom")]
    [InlineData("utf16-le-bom")]
    public void MetadataImportDetectsPowerShellCompatibleByteOrderMarks(string encodingName) {
        string source = Path.Combine(_directoryWithTests, "metadata-bom-source-" + encodingName + ".png");
        string metadataPath = Path.Combine(_directoryWithTests, "metadata-bom-" + encodingName + ".json");
        string output = Path.Combine(_directoryWithTests, "metadata-bom-output-" + encodingName + ".png");
        byte[] xmp = System.Text.Encoding.UTF8.GetBytes("<x:xmpmeta xmlns:x=\"adobe:ns:meta/\">Preserved packet</x:xmpmeta>");
        using (var image = PlaygroundImage.FromRaster(new OfficeRasterImage(7, 4, Color.Red))) {
            image.SetExifValue(ExifTag.Software, "Preserved software");
            image.Metadata.XmpProfile = xmp;
            image.Save(source);
        }
        byte[] original = File.ReadAllBytes(source);
        var node = System.Text.Json.Nodes.JsonNode.Parse(ImageHelper.ExportMetadata(source))!;
        node["HorizontalResolution"] = 150;
        node["VerticalResolution"] = 120;
        node["ResolutionUnits"] = "PixelsPerInch";
        System.Text.Encoding encoding = encodingName == "utf8-bom" ? new System.Text.UTF8Encoding(true) : System.Text.Encoding.Unicode;
        File.WriteAllText(metadataPath, node.ToJsonString(), encoding);
        byte[] preamble = encoding.GetPreamble();
        byte[] actualPreamble = new byte[preamble.Length];
        System.Buffer.BlockCopy(File.ReadAllBytes(metadataPath), 0, actualPreamble, 0, actualPreamble.Length);
        Assert.Equal(preamble, actualPreamble);

        ImageHelper.ImportMetadata(source, metadataPath, output);

        Assert.Equal(original, File.ReadAllBytes(source));
        using var loaded = PlaygroundImage.Load(source);
        using var imported = PlaygroundImage.Load(output);
        Assert.Equal(loaded.Raster.GetPixels(), imported.Raster.GetPixels());
        // PNG stores integer pixels per meter, so roundtrip DPI has a 0.0254 increment.
        const double halfPngDensityIncrement = 0.0127;
        Assert.InRange(imported.Metadata.PhysicalDpiX!.Value, 150D - halfPngDensityIncrement, 150D + halfPngDensityIncrement);
        Assert.InRange(imported.Metadata.PhysicalDpiY!.Value, 120D - halfPngDensityIncrement, 120D + halfPngDensityIncrement);
        Assert.Equal("Preserved software", imported.Metadata.GetExifValue(ExifTag.Software)!.Value);
        Assert.Equal(xmp, imported.Metadata.XmpProfile);
    }

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
        Assert.Contains(check.GetExifValues(), v => v.Tag.Equals(ExifTag.Software) && v.Value?.ToString() == "ImagePlayground");
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

using System;
using System.IO;
using System.Text.Json.Nodes;
using OfficeIMO.Drawing;
using Xunit;
using PlaygroundImage = global::ImagePlayground.Image;

namespace ImagePlayground.Tests;

/// <summary>Flat metadata JSON resolution edits survive profile import and subsequent facade encoding.</summary>
public sealed class ImageConsumerMetadataDensityContracts {
    [Theory]
    [InlineData("png", OfficeImageResolutionUnit.PixelsPerInch)]
    [InlineData("png", OfficeImageResolutionUnit.PixelsPerCentimeter)]
    [InlineData("png", OfficeImageResolutionUnit.PixelsPerMeter)]
    [InlineData("jpg", OfficeImageResolutionUnit.PixelsPerInch)]
    [InlineData("jpg", OfficeImageResolutionUnit.PixelsPerCentimeter)]
    [InlineData("jpg", OfficeImageResolutionUnit.PixelsPerMeter)]
    [InlineData("webp", OfficeImageResolutionUnit.PixelsPerInch)]
    [InlineData("webp", OfficeImageResolutionUnit.PixelsPerCentimeter)]
    [InlineData("webp", OfficeImageResolutionUnit.PixelsPerMeter)]
    [InlineData("tiff", OfficeImageResolutionUnit.PixelsPerInch)]
    [InlineData("tiff", OfficeImageResolutionUnit.PixelsPerCentimeter)]
    [InlineData("tiff", OfficeImageResolutionUnit.PixelsPerMeter)]
    [InlineData("bmp", OfficeImageResolutionUnit.PixelsPerInch)]
    [InlineData("bmp", OfficeImageResolutionUnit.PixelsPerCentimeter)]
    [InlineData("bmp", OfficeImageResolutionUnit.PixelsPerMeter)]
    public void ImportedDensityOverridesIncludedExifAndSurvivesSaveAndStream(string extension, OfficeImageResolutionUnit unit) {
        string directory = Path.Combine(Path.GetTempPath(), "ImageConsumerDensity-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        try {
            string sourcePath = Path.Combine(directory, "source." + extension);
            string metadataPath = Path.Combine(directory, "metadata.json");
            string importedPath = Path.Combine(directory, "imported." + extension);
            string savedPath = Path.Combine(directory, "saved." + extension);
            using (var original = PlaygroundImage.FromRaster(new OfficeRasterImage(8, 4, OfficeColor.Red))) {
                original.Metadata.Resolution = new OfficeImageResolution(72, 72);
                if (extension != "bmp") { original.SetExifValue(OfficeExifTag.Software, "density-import-contract"); }
                original.Save(sourcePath);
            }
            byte[] sourceBytes = File.ReadAllBytes(sourcePath);
            using var source = PlaygroundImage.Load(sourceBytes);
            var node = JsonNode.Parse(ImageHelper.ExportMetadata(sourcePath))!;
            // Meter values also map to integral pixels/cm in JPEG's JFIF carrier.
            double horizontal = unit == OfficeImageResolutionUnit.PixelsPerMeter ? 15000 : 150;
            double vertical = unit == OfficeImageResolutionUnit.PixelsPerMeter ? 12000 : 120;
            node["HorizontalResolution"] = horizontal;
            node["VerticalResolution"] = vertical;
            node["ResolutionUnits"] = unit.ToString();
            File.WriteAllText(metadataPath, node.ToJsonString());

            ImageHelper.ImportMetadata(sourcePath, metadataPath, importedPath);

            var expected = new OfficeImageResolution(horizontal, vertical, unit);
            using var imported = PlaygroundImage.Load(importedPath);
            AssertDensity(imported.Metadata, expected);
            Assert.Equal(source.Raster.GetPixels(), imported.Raster.GetPixels());
            Assert.Equal(sourceBytes, File.ReadAllBytes(sourcePath));
            if (extension != "bmp") { Assert.Equal("density-import-contract", imported.Metadata.GetExifValue(OfficeExifTag.Software)!.Value); }
            imported.Save(savedPath);
            using var saved = PlaygroundImage.Load(savedPath);
            AssertDensity(saved.Metadata, expected);
            using var stream = imported.ToStream(Helpers.GetImageType("." + extension));
            using var streamed = PlaygroundImage.Load(stream);
            AssertDensity(streamed.Metadata, expected);
        } finally { Directory.Delete(directory, true); }
    }

    private static void AssertDensity(OfficeImageMetadata metadata, OfficeImageResolution expected) {
        // PNG/BMP native integer pixels-per-meter density rounds to increments of 0.0254 DPI.
        const double tolerance = 0.013;
        Assert.InRange(metadata.PhysicalDpiX!.Value, expected.PhysicalDpiX!.Value - tolerance, expected.PhysicalDpiX.Value + tolerance);
        Assert.InRange(metadata.PhysicalDpiY!.Value, expected.PhysicalDpiY!.Value - tolerance, expected.PhysicalDpiY.Value + tolerance);
    }
}

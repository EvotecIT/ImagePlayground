using System.IO;
using System;
using OfficeIMO.Drawing;
using Xunit;

namespace ImagePlayground.Tests;

public partial class ImagePlayground {
    [Fact]
    public void PublicIconSaveRetainsResolutionEntriesAndThumbnailsResizeTheirPixels() {
        string input = Path.Combine(_directoryWithTests, "icon-thumbnail-input");
        string output = Path.Combine(_directoryWithTests, "icon-thumbnail-output");
        Directory.CreateDirectory(input);
        string source = Path.Combine(input, "resolutions.ico");
        using (var image = global::ImagePlayground.Image.FromFrames(new OfficeRasterFrames(new[] {
            new OfficeRasterFrame(new OfficeRasterImage(64, 64, OfficeColor.Red)),
            new OfficeRasterFrame(new OfficeRasterImage(32, 32, OfficeColor.Blue))
        }))) {
            image.Save(source);
        }
        using (var saved = global::ImagePlayground.Image.Load(source)) {
            Assert.Equal(2, saved.Frames.Count);
            Assert.Equal(64, saved.Frames[0].Image.Width);
            Assert.Equal(32, saved.Frames[1].Image.Width);
            Assert.Equal(OfficeColor.Red, saved.Frames[0].Image.GetPixel(0, 0));
            Assert.Equal(OfficeColor.Blue, saved.Frames[1].Image.GetPixel(0, 0));
        }

        ImageHelper.GenerateThumbnails(input, output, 16, 16);
        using var thumbnail = global::ImagePlayground.Image.Load(Path.Combine(output, "resolutions.ico"));
        Assert.Equal(2, thumbnail.Frames.Count);
        Assert.All(thumbnail.Frames, frame => {
            Assert.Equal(16, frame.Image.Width);
            Assert.Equal(16, frame.Image.Height);
        });
        Assert.Equal(OfficeColor.Red, thumbnail.Frames[0].Image.GetPixel(15, 15));
        Assert.Equal(OfficeColor.Blue, thumbnail.Frames[1].Image.GetPixel(15, 15));
    }

    [Fact]
    public void TiffThumbnailMetadataFailurePropagatesWithoutCopyingOriginal() {
        string input = Path.Combine(_directoryWithTests, "opaque-thumbnail-input");
        string output = Path.Combine(_directoryWithTests, "opaque-thumbnail-output");
        Directory.CreateDirectory(input);
        string source = Path.Combine(input, "opaque.tiff");
        string destination = Path.Combine(output, "opaque.tiff");
        if (File.Exists(destination)) {
            File.Delete(destination);
        }
        byte[] tiff = OfficeTiffCodec.Encode(new OfficeRasterImage(40, 20, OfficeColor.Red));
        OfficeImageMetadata metadata = OfficeImageMetadata.Read(tiff);
        metadata.SetExifValue(new OfficeExifTag(37500, OfficeExifDataType.Undefined, OfficeExifDirectory.Exif),
            System.Text.Encoding.ASCII.GetBytes("opaque maker-note fixture"));
        File.WriteAllBytes(source, OfficeImageMetadata.Apply(tiff, metadata));
        using (var original = global::ImagePlayground.Image.Load(source)) {
            Assert.True(original.Metadata.RequiresOriginalTiffContainer);
        }

        Assert.Throws<NotSupportedException>(() => ImageHelper.GenerateThumbnails(input, output, 8, 4));
        Assert.False(File.Exists(destination));
    }

    [Theory]
    [InlineData(1, OfficeImageResolutionUnit.AspectRatio, 4D, 3D)]
    [InlineData(2, OfficeImageResolutionUnit.AspectRatio, 4D, 3D)]
    [InlineData(1, OfficeImageResolutionUnit.PixelsPerCentimeter, 60D, 50D)]
    [InlineData(2, OfficeImageResolutionUnit.PixelsPerCentimeter, 60D, 50D)]
    public void TiffSavingPreservesNativeResolutionForSingleImageAndEveryPage(int count, OfficeImageResolutionUnit unit, double x, double y) {
        string output = Path.Combine(_directoryWithTests, $"native-resolution-{count}-{unit}.tiff");
        var frames = new OfficeRasterFrame[count];
        for (int index = 0; index < count; index++) {
            frames[index] = new OfficeRasterFrame(new OfficeRasterImage(3, 2, index == 0 ? OfficeColor.Red : OfficeColor.Blue));
        }
        using var image = global::ImagePlayground.Image.FromFrames(new OfficeRasterFrames(frames));
        image.Metadata.ResolutionUnits = unit;
        image.Metadata.HorizontalResolution = x;
        image.Metadata.VerticalResolution = y;
        image.Save(output);

        using var loaded = global::ImagePlayground.Image.Load(output);
        Assert.Equal(count, loaded.Frames.Count);
        Assert.Equal(unit, loaded.Metadata.ResolutionUnits);
        Assert.Equal(x, loaded.Metadata.HorizontalResolution);
        Assert.Equal(y, loaded.Metadata.VerticalResolution);
        Assert.True(OfficeRasterContainerInspector.TryInspect(File.ReadAllBytes(output), out var container));
        Assert.Equal(count, container!.Count);
        for (int index = 0; index < count; index++) {
            Assert.Equal(frames[index].Image.GetPixels(), loaded.Frames[index].Image.GetPixels());
            Assert.Equal(image.Metadata.PhysicalDpiX, container.Frames[index].DpiX);
            Assert.Equal(image.Metadata.PhysicalDpiY, container.Frames[index].DpiY);
        }
        Assert.Equal(unit, image.Metadata.ResolutionUnits);
    }

    [Fact]
    public void JpegToPngReportsUnsupportedProfileAndKeepsSupportedMetadata() {
        string source = Path.Combine(_directoryWithImages, "PrzemyslawKlysAndKulkozaurr.jpg");
        string output = Path.Combine(_directoryWithTests, "metadata-projected.png");
        using var image = global::ImagePlayground.Image.Load(source);
        byte[] iptc = image.Metadata.IptcProfile!;
        Assert.NotNull(iptc);
        Assert.NotEmpty(iptc);
        byte[] xmp = System.Text.Encoding.UTF8.GetBytes("<x:xmpmeta xmlns:x=\"adobe:ns:meta/\" />");
        image.Metadata.XmpProfile = xmp;
        image.SetExifValue(OfficeExifTag.Software, "Metadata projection contract");
        byte[] pixels = image.Raster.GetPixels();

        Assert.Equal(OfficeImageMetadataProfileKinds.Iptc, image.GetEncodingMetadataOmissions(ImageType.Png));
        image.Save(output);

        using var reloaded = global::ImagePlayground.Image.Load(output);
        Assert.Equal(pixels, reloaded.Raster.GetPixels());
        Assert.Equal("Metadata projection contract", reloaded.Metadata.GetExifValue(OfficeExifTag.Software)!.Value);
        Assert.Equal(xmp, reloaded.Metadata.XmpProfile);
        Assert.Null(reloaded.Metadata.IptcProfile);
        Assert.Equal(iptc, image.Metadata.IptcProfile);
        Assert.Equal(xmp, image.Metadata.XmpProfile);
    }

    [Fact]
    public void CentimeterJpegDensityRetainsPhysicalSizeWhenSavedAsPng() {
        string source = Path.Combine(_directoryWithTests, "density-centimeters.jpg");
        string output = Path.Combine(_directoryWithTests, "density-meters.png");
        using (var image = global::ImagePlayground.Image.FromRaster(new OfficeRasterImage(4, 3, OfficeColor.Red))) {
            image.Metadata.HorizontalResolution = 100;
            image.Metadata.VerticalResolution = 50;
            image.Metadata.ResolutionUnits = OfficeImageResolutionUnit.PixelsPerCentimeter;
            image.Save(source);
        }
        using var loaded = global::ImagePlayground.Image.Load(source);
        Assert.Equal(OfficeImageResolutionUnit.PixelsPerCentimeter, loaded.Metadata.ResolutionUnits);
        Assert.Equal(254D, loaded.Metadata.PhysicalDpiX!.Value, 2);
        Assert.Equal(127D, loaded.Metadata.PhysicalDpiY!.Value, 2);
        loaded.Save(output);
        using var reloaded = global::ImagePlayground.Image.Load(output);
        Assert.Equal(OfficeImageResolutionUnit.PixelsPerMeter, reloaded.Metadata.ResolutionUnits);
        Assert.Equal(loaded.Metadata.PhysicalDpiX.Value, reloaded.Metadata.PhysicalDpiX!.Value, 2);
        Assert.Equal(loaded.Metadata.PhysicalDpiY.Value, reloaded.Metadata.PhysicalDpiY!.Value, 2);
        Assert.Equal(loaded.Raster.GetPixels(), reloaded.Raster.GetPixels());
    }

    [Theory]
    [InlineData("gif")]
    [InlineData("png")]
    [InlineData("tiff")]
    public void SeveralFramesSurviveSharedAnimationAndMultipageEncoding(string extension) {
        string output = Path.Combine(_directoryWithTests, "managed-frames." + extension);
        var frames = new OfficeRasterFrames(new[] {
            new OfficeRasterFrame(new OfficeRasterImage(5, 3, OfficeColor.Red), TimeSpan.FromMilliseconds(100)),
            new OfficeRasterFrame(new OfficeRasterImage(5, 3, OfficeColor.Blue), TimeSpan.FromMilliseconds(200))
        }, playCount: 2);
        using (var image = global::ImagePlayground.Image.FromFrames(frames)) {
            image.Save(output);
        }
        using var loaded = global::ImagePlayground.Image.Load(output);
        Assert.Equal(2, loaded.Frames.Count);
        Assert.Equal(OfficeColor.Red, loaded.Frames[0].Image.GetPixel(4, 2));
        Assert.Equal(OfficeColor.Blue, loaded.Frames[1].Image.GetPixel(4, 2));
        if (extension != "tiff") {
            Assert.Equal(2, loaded.Frames.PlayCount);
            Assert.Equal(TimeSpan.FromMilliseconds(100), loaded.Frames[0].Duration);
            Assert.Equal(TimeSpan.FromMilliseconds(200), loaded.Frames[1].Duration);
        }
    }

    [Theory]
    [InlineData("png")]
    [InlineData("tiff")]
    [InlineData("jpg")]
    public void StoredOrientationRemainsExplicitAndAutoOrientRunsOnce(string extension) {
        string source = Path.Combine(_directoryWithTests, "orientation-source." + extension);
        string output = Path.Combine(_directoryWithTests, "orientation-normal." + extension);
        using (var image = global::ImagePlayground.Image.FromRaster(new OfficeRasterImage(3, 2, OfficeColor.White))) {
            image.Raster.SetPixel(0, 0, OfficeColor.Red);
            image.Raster.SetPixel(2, 1, OfficeColor.Blue);
            image.SetExifValue(OfficeExifTag.Orientation, (ushort)6);
            image.Save(source);
        }

        using var loaded = global::ImagePlayground.Image.Load(source);
        Assert.Equal(3, loaded.Width);
        Assert.Equal(2, loaded.Height);
        if (extension != "jpg") {
            Assert.Equal(OfficeColor.Red, loaded.Raster.GetPixel(0, 0));
            Assert.Equal(OfficeColor.Blue, loaded.Raster.GetPixel(2, 1));
        }
        loaded.AutoOrient();
        Assert.Equal(2, loaded.Width);
        Assert.Equal(3, loaded.Height);
        Assert.Equal((ushort)1, loaded.Metadata.GetExifValue(OfficeExifTag.Orientation)!.Value);
        if (extension != "jpg") {
            Assert.Equal(OfficeColor.Red, loaded.Raster.GetPixel(1, 0));
            Assert.Equal(OfficeColor.Blue, loaded.Raster.GetPixel(0, 2));
        }
        byte[] oriented = loaded.Raster.GetPixels();
        loaded.AutoOrient();
        Assert.Equal(oriented, loaded.Raster.GetPixels());
        loaded.Save(output);
        using var reloaded = global::ImagePlayground.Image.Load(output);
        Assert.Equal(2, reloaded.Width);
        Assert.Equal(3, reloaded.Height);
        Assert.Equal((ushort)1, reloaded.Metadata.GetExifValue(OfficeExifTag.Orientation)!.Value);
    }

    [Fact]
    public void FittedHorizontalCombinationRetainsBothOuterEdges() {
        string first = Path.Combine(_directoryWithTests, "combine-red.png");
        string second = Path.Combine(_directoryWithTests, "combine-blue.png");
        string output = Path.Combine(_directoryWithTests, "combine-fit-edges.png");
        using (var image = global::ImagePlayground.Image.FromRaster(new OfficeRasterImage(8, 4, OfficeColor.Red))) {
            image.Save(first);
        }
        using (var image = global::ImagePlayground.Image.FromRaster(new OfficeRasterImage(4, 8, OfficeColor.Blue))) {
            image.Save(second);
        }
        global::ImagePlayground.ImageHelper.Combine(first, second, output, true, ImagePlacement.Left);

        using var combined = global::ImagePlayground.Image.Load(output);
        Assert.Equal(20, combined.Width);
        Assert.Equal(8, combined.Height);
        Assert.Equal(OfficeColor.Blue, combined.Raster.GetPixel(0, 7));
        Assert.Equal(OfficeColor.Blue, combined.Raster.GetPixel(3, 7));
        Assert.Equal(OfficeColor.Red, combined.Raster.GetPixel(4, 7));
        Assert.Equal(OfficeColor.Red, combined.Raster.GetPixel(19, 7));
    }
}

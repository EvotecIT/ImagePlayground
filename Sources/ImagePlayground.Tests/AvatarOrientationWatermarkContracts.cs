using System;
using System.IO;
using System.Linq;
using System.Threading;
using OfficeIMO.Drawing;
using Xunit;

namespace ImagePlayground.Tests;

public partial class ImagePlayground {
    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void AvatarExportsRetainSourcePixelsAndMetadataAndStreamsUsePng(bool streamOutput, bool circular) {
        using var image = global::ImagePlayground.Image.FromRaster(new OfficeRasterImage(40, 30, OfficeColor.Red), ImageType.Jpeg);
        image.Raster.SetPixel(0, 0, OfficeColor.Blue);
        image.Metadata.SetExifValue(OfficeExifTag.Software, "Unchanged source metadata");
        byte[] sourcePixels = image.Raster.GetPixels();
        byte[] sourceExif = image.Metadata.ExifProfile!;
        byte[] exported;
        using var stream = new MemoryStream();
        if (streamOutput) {
            stream.Write(new byte[4096], 0, 4096);
            stream.Position = 123;
            if (circular) {
                image.SaveAsCircularAvatar(stream, 16);
            } else {
                image.SaveAsAvatar(stream, 16, 16, 5);
            }
            Assert.Equal(0, stream.Position);
            Assert.True(stream.CanWrite);
            exported = stream.ToArray();
            Assert.Equal(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 }, (System.Collections.Generic.IEnumerable<byte>)new ArraySegment<byte>(exported, 0, 8));
        } else {
            string destination = Path.Combine(_directoryWithTests, "avatar-source-contract-" + circular + ".png");
            if (circular) {
                image.SaveAsCircularAvatar(destination, 16);
            } else {
                image.SaveAsAvatar(destination, 16, 16, 5);
            }
            exported = File.ReadAllBytes(destination);
        }

        Assert.Equal(40, image.Width);
        Assert.Equal(30, image.Height);
        Assert.Equal(sourcePixels, image.Raster.GetPixels());
        Assert.Equal(sourceExif, image.Metadata.ExifProfile);
        using var avatar = global::ImagePlayground.Image.Load(exported);
        Assert.Equal(16, avatar.Width);
        Assert.Equal(16, avatar.Height);
        Assert.Equal(0, avatar.Raster.GetPixel(0, 0).A);
        Assert.Equal(OfficeColor.Red, avatar.Raster.GetPixel(8, 8));
    }

    [Fact]
    public void FailedAvatarExportLeavesTheSourceUsableAndUnchanged() {
        using var image = global::ImagePlayground.Image.FromRaster(new OfficeRasterImage(40, 30, OfficeColor.Blue));
        byte[] pixels = image.Raster.GetPixels();
        using var readOnlyStream = new MemoryStream(new byte[8], writable: false);

        Assert.Throws<NotSupportedException>(() => image.SaveAsCircularAvatar(readOnlyStream, 16));

        Assert.Equal(40, image.Width);
        Assert.Equal(30, image.Height);
        Assert.Equal(pixels, image.Raster.GetPixels());
    }

    [Fact]
    public void AvatarPlansItsIntermediateBuffersBeforeTransformingAnyFrame() {
        // The public frame budget counts the sequence's pixel buffers; aliases keep this boundary fixture small.
        var raster = new OfficeRasterImage(100, 100, OfficeColor.Red);
        var frames = new OfficeRasterFrames(Enumerable.Range(0, 3346).Select(_ => new OfficeRasterFrame(raster)));
        using var image = global::ImagePlayground.Image.FromFrames(frames);

        Assert.Throws<ArgumentException>(() => image.Avatar(100, 100, 10));

        Assert.Same(frames, image.Frames);
        Assert.Equal(OfficeColor.Red, raster.GetPixel(0, 0));
    }

    [Fact]
    public void AvatarPngStreamPreservesAllFrameTimingWithoutChangingSourceGeometry() {
        var frames = new OfficeRasterFrames(new[] {
            new OfficeRasterFrame(new OfficeRasterImage(32, 24, OfficeColor.Red), TimeSpan.FromMilliseconds(100)),
            new OfficeRasterFrame(new OfficeRasterImage(24, 32, OfficeColor.Blue), TimeSpan.FromMilliseconds(200))
        }, playCount: 2);
        using var image = global::ImagePlayground.Image.FromFrames(frames, ImageType.Jpeg);
        using var stream = new MemoryStream();

        image.SaveAsCircularAvatar(stream, 16);

        using var exported = global::ImagePlayground.Image.Load(stream.ToArray());
        Assert.Equal(2, exported.Frames.Count);
        Assert.Equal(2, exported.Frames.PlayCount);
        Assert.Same(frames, image.Frames);
        for (int index = 0; index < frames.Count; index++) {
            Assert.Equal(frames[index].Duration, exported.Frames[index].Duration);
            Assert.Equal(16, exported.Frames[index].Image.Width);
            Assert.Equal(16, exported.Frames[index].Image.Height);
            Assert.Equal(0, exported.Frames[index].Image.GetPixel(0, 0).A);
            Assert.Equal(frames[index].Image.GetPixel(0, 0), exported.Frames[index].Image.GetPixel(8, 8));
        }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void AutoOrientWithoutStoredOrientationLeavesPixelsAndExifUnchanged(bool withOtherExif) {
        using var image = global::ImagePlayground.Image.FromRaster(new OfficeRasterImage(3, 2, OfficeColor.Red));
        image.Raster.SetPixel(2, 1, OfficeColor.Blue);
        if (withOtherExif) {
            image.Metadata.SetExifValue(OfficeExifTag.Software, "Existing unrelated EXIF");
        }
        byte[] pixels = image.Raster.GetPixels();
        byte[]? exif = image.Metadata.ExifProfile;

        image.AutoOrient();

        Assert.Equal(3, image.Width);
        Assert.Equal(2, image.Height);
        Assert.Equal(pixels, image.Raster.GetPixels());
        Assert.Null(image.Metadata.GetExifValue(OfficeExifTag.Orientation));
        Assert.Equal(exif, image.Metadata.ExifProfile);
        using var output = image.ToStream();
        Assert.Null(OfficeImageMetadata.Read(output.ToArray()).GetExifValue(OfficeExifTag.Orientation));
    }

    [Theory]
    [InlineData(0f)]
    [InlineData(0.5f)]
    [InlineData(1f)]
    public void TiledWatermarkUsesEveryFramesGeometryAndRetainsTimingAndClippedPixels(float opacity) {
        string watermark = Path.Combine(_directoryWithTests, "tiled-contract-watermark.png");
        using (var tile = global::ImagePlayground.Image.FromRaster(new OfficeRasterImage(2, 2, OfficeColor.Red))) {
            tile.Save(watermark);
        }
        var source = new OfficeRasterFrames(new[] {
            new OfficeRasterFrame(new OfficeRasterImage(5, 4, OfficeColor.Black), TimeSpan.FromMilliseconds(100)),
            new OfficeRasterFrame(new OfficeRasterImage(7, 6, OfficeColor.Blue), TimeSpan.FromMilliseconds(250))
        }, playCount: 3);
        using var image = global::ImagePlayground.Image.FromFrames(source);

        image.WatermarkImageTiled(watermark, spacing: 1, opacity: opacity, watermarkPercentage: 100);

        Assert.Equal(2, image.Frames.Count);
        Assert.Equal(3, image.Frames.PlayCount);
        for (int index = 0; index < image.Frames.Count; index++) {
            var frame = image.Frames[index];
            OfficeColor background = index == 0 ? OfficeColor.Black : OfficeColor.Blue;
            byte alpha = (byte)Math.Round(opacity * 255);
            OfficeColor painted = OfficeColor.FromRgba(alpha, 0, index == 0 ? (byte)0 : (byte)(255 - alpha), 255);
            Assert.Equal(source[index].Duration, frame.Duration);
            Assert.Equal(source[index].Image.Width, frame.Image.Width);
            Assert.Equal(source[index].Image.Height, frame.Image.Height);
            for (int y = 0; y < frame.Image.Height; y++) {
                for (int x = 0; x < frame.Image.Width; x++) {
                    bool tilePixel = x >= 1 && y >= 1 && (x - 1) % 3 < 2 && (y - 1) % 3 < 2;
                    Assert.Equal(tilePixel ? painted : background, frame.Image.GetPixel(x, y));
                    Assert.Equal(background, source[index].Image.GetPixel(x, y));
                }
            }
        }
    }

    [Theory]
    [InlineData(-0.1f)]
    [InlineData(1.1f)]
    [InlineData(float.NaN)]
    public void TiledWatermarkRejectsInvalidOpacityEvenWhenSpacingPlacesEveryTileOutsideTheImage(float opacity) {
        using var image = global::ImagePlayground.Image.FromRaster(new OfficeRasterImage(3, 2, OfficeColor.Blue));
        var original = image.Frames;
        string watermark = Path.Combine(_directoryWithImages, "LogoEvotec.png");

        Assert.Throws<ArgumentOutOfRangeException>(() => image.WatermarkImageTiled(watermark, int.MaxValue, opacity));

        Assert.Same(original, image.Frames);
        Assert.Equal(OfficeColor.Blue, image.Raster.GetPixel(2, 1));
    }

    [Fact]
    public void TiledWatermarkPreCancellationLeavesFramesUnchangedBeforeReadingTheWatermark() {
        using var image = global::ImagePlayground.Image.FromRaster(new OfficeRasterImage(3, 2, OfficeColor.Blue));
        var original = image.Frames;
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        Assert.Throws<OperationCanceledException>(() => image.WatermarkImageTiled("not-read.png", 0, cancellationToken: cancellation.Token));

        Assert.Same(original, image.Frames);
        Assert.Equal(OfficeColor.Blue, image.Raster.GetPixel(2, 1));
    }

    [Fact]
    public void TiledWatermarkCancellationDoesNotPublishPartialFrames() {
        string watermark = Path.Combine(_directoryWithTests, "canceled-tile.png");
        using (var tile = global::ImagePlayground.Image.FromRaster(new OfficeRasterImage(1, 1, OfficeColor.Red))) {
            tile.Save(watermark);
        }
        var frames = new OfficeRasterFrames(new[] {
            new OfficeRasterFrame(new OfficeRasterImage(1536, 1024, OfficeColor.Black), TimeSpan.FromMilliseconds(100)),
            new OfficeRasterFrame(new OfficeRasterImage(1536, 1024, OfficeColor.Blue), TimeSpan.FromMilliseconds(200))
        }, playCount: 2);
        using var image = global::ImagePlayground.Image.FromFrames(frames);
        byte[][] originalPixels = frames.Select(frame => frame.Image.GetPixels()).ToArray();
        using var cancellation = new CancellationTokenSource();
        cancellation.CancelAfter(10);

        Assert.Throws<OperationCanceledException>(() => image.WatermarkImageTiled(watermark, 0, watermarkPercentage: 100, cancellationToken: cancellation.Token));

        Assert.Same(frames, image.Frames);
        for (int index = 0; index < frames.Count; index++) {
            Assert.Equal(originalPixels[index], image.Frames[index].Image.GetPixels());
        }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ImageOverlayIncludesItsRetainedPixelsInTheSharedFrameBudgetBeforePublishing(bool tiled) {
        string watermark = Path.Combine(_directoryWithTests, "budgeted-tile.png");
        using (var tile = global::ImagePlayground.Image.FromRaster(new OfficeRasterImage(640, 640, OfficeColor.Red))) {
            tile.Save(watermark);
        }
        // Repeated frames keep this boundary fixture small while exercising the sequence's aggregate budget.
        var source = new OfficeRasterImage(100, 100, OfficeColor.Blue);
        var frames = new OfficeRasterFrames(Enumerable.Range(0, 3330).Select(_ => new OfficeRasterFrame(source)));
        using var image = global::ImagePlayground.Image.FromFrames(frames);

        if (tiled) {
            Assert.Throws<ArgumentException>(() => image.WatermarkImageTiled(watermark, 0, watermarkPercentage: 100));
        } else {
            using var overlay = global::ImagePlayground.Image.Load(watermark);
            Assert.Throws<ArgumentException>(() => image.AddImage(overlay.Raster, 0, 0, 1));
        }

        Assert.Same(frames, image.Frames);
        Assert.Equal(OfficeColor.Blue, source.GetPixel(99, 99));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void FileAndWatermarkOverlaysDrawTheFirstAnimatedSourceFrameOnEveryTargetFrame(int operation) {
        string watermark = Path.Combine(_directoryWithTests, "animated-overlay-source.gif");
        using (var tile = global::ImagePlayground.Image.FromFrames(new OfficeRasterFrames(new[] {
            new OfficeRasterFrame(new OfficeRasterImage(3, 3, OfficeColor.Red), TimeSpan.FromMilliseconds(100)),
            new OfficeRasterFrame(new OfficeRasterImage(3, 3, OfficeColor.Blue), TimeSpan.FromMilliseconds(200))
        }))) {
            tile.Save(watermark);
        }
        var frames = new OfficeRasterFrames(new[] {
            new OfficeRasterFrame(new OfficeRasterImage(3, 3, OfficeColor.Black), TimeSpan.FromMilliseconds(100)),
            new OfficeRasterFrame(new OfficeRasterImage(3, 3, OfficeColor.White), TimeSpan.FromMilliseconds(200))
        }, playCount: 2);
        using var image = global::ImagePlayground.Image.FromFrames(frames);

        switch (operation) {
            case 0:
                image.AddImage(watermark, 0, 0, 1);
                break;
            case 1:
                image.WatermarkImage(watermark, WatermarkPlacement.TopLeft, padding: 0, watermarkPercentage: 100);
                break;
            case 2:
                image.WatermarkImage(watermark, 0, 0, watermarkPercentage: 100);
                break;
            default:
                image.WatermarkImageTiled(watermark, 0, watermarkPercentage: 100);
                break;
        }

        Assert.Equal(2, image.Frames.Count);
        Assert.Equal(2, image.Frames.PlayCount);
        for (int index = 0; index < image.Frames.Count; index++) {
            Assert.Equal(frames[index].Duration, image.Frames[index].Duration);
            for (int y = 0; y < 3; y++) {
                for (int x = 0; x < 3; x++) {
                    Assert.Equal(OfficeColor.Red, image.Frames[index].Image.GetPixel(x, y));
                }
            }
        }
    }
}

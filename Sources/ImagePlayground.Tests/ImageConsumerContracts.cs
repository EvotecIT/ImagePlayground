using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using ChartForgeX.Composition;
using ChartForgeX.Raster;
using OfficeIMO.Drawing;
using Xunit;
using PlaygroundImage = global::ImagePlayground.Image;

namespace ImagePlayground.Tests;

/// <summary>Observable load, edit, compose and export contracts of the consumer facade.</summary>
public sealed class ImageConsumerContracts {
    [Fact]
    public void DetectedFormatControlsByteAndStreamDefaultsAndFilenameCannotOverrideIt() {
        using var source = PlaygroundImage.FromRaster(new OfficeRasterImage(7, 3, OfficeColor.Red));
        byte[] jpeg = source.Encode(ImageType.Jpeg).EncodedBytes;
        using var bytes = PlaygroundImage.Load(jpeg);
        Assert.Equal(OfficeImageFormat.Jpeg, bytes.SourceFormat);
        Assert.Equal(ImageType.Jpeg, bytes.DefaultOutputFormat);
        using var encoded = bytes.ToStream();
        Assert.Equal(0, encoded.Position);
        using var streamImage = PlaygroundImage.Load(encoded);
        Assert.Equal(OfficeImageFormat.Jpeg, streamImage.SourceFormat);
        Assert.True(encoded.CanRead);
        Assert.True(encoded.CanWrite);

        string directory = Path.Combine(Path.GetTempPath(), "ImageConsumerContracts-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        try {
            string mislabeled = Path.Combine(directory, "actual-jpeg.png");
            File.WriteAllBytes(mislabeled, jpeg);
            using var file = PlaygroundImage.Load(mislabeled);
            Assert.Equal(OfficeImageFormat.Jpeg, file.SourceFormat);
            Assert.Equal(ImageType.Jpeg, file.DefaultOutputFormat);
        } finally { Directory.Delete(directory, true); }
    }

    [Fact]
    public void ExplicitStreamFormatAndOwnedResolutionProduceInspectableMetadataEvidence() {
        using var image = PlaygroundImage.FromRaster(new OfficeRasterImage(8, 4, OfficeColor.Blue));
        image.Metadata.SetExifValue(OfficeExifTag.Software, "consumer-encode-contract");
        image.Metadata.XmpProfile = Encoding.UTF8.GetBytes("<x:xmpmeta xmlns:x=\"adobe:ns:meta/\" />");
        image.Metadata.IptcProfile = new byte[] { 0x1c, 2, 5, 0, 4, 116, 101, 115, 116 };
        image.Metadata.Resolution = new OfficeImageResolution(72, 72);
        var options = new OfficeRasterEncodingOptions { Resolution = new OfficeImageResolution(300, 150) };
        var prepared = image.Encode(ImageType.Png, options);
        Assert.Equal(OfficeImageMetadataProfileKinds.Iptc, prepared.OmittedProfiles);
        Assert.Throws<InvalidOperationException>(() => prepared.RequireMetadataPreservation());
        using var output = new MemoryStream();
        output.Write(new byte[10000], 0, 10000);
        output.Position = 19;
        image.Save(output, ImageType.Png, options);
        Assert.Equal(0, output.Position);
        Assert.True(output.Length < 10000);
        using var loaded = PlaygroundImage.Load(output);
        Assert.Equal(OfficeImageFormat.Png, loaded.SourceFormat);
        Assert.Equal(image.Raster.GetPixels(), loaded.Raster.GetPixels());
        Assert.Equal("consumer-encode-contract", loaded.Metadata.GetExifValue(OfficeExifTag.Software)!.Value);
        Assert.Equal(image.Metadata.XmpProfile, loaded.Metadata.XmpProfile);
        Assert.Null(loaded.Metadata.IptcProfile);
        Assert.Equal(300, loaded.Metadata.PhysicalDpiX!.Value, 1);
        Assert.Equal(150, loaded.Metadata.PhysicalDpiY!.Value, 1);
        Assert.Equal(72, image.Metadata.Resolution.Horizontal);
        Assert.NotNull(image.Metadata.IptcProfile);
    }

    [Fact]
    public void IndependentRgbaBridgeSupportsEditComposeAndSaveWithoutSharingConsumerPixels() {
        var pixels = new byte[] { 255, 0, 0, 255, 0, 0, 255, 255 };
        var carrier = new RgbaImage(2, 1, pixels);
        using var image = PlaygroundImage.FromRgbaImage(carrier);
        pixels[0] = 0;
        Assert.Equal(OfficeColor.Red, image.Raster.GetPixel(0, 0));
        image.Resize(new OfficeRasterResizeOptions { Width = 4, Height = 4, Fit = OfficeImageFit.Cover, ResamplingMode = OfficeRasterResamplingMode.NearestNeighbor });
        var snapshot = image.ToRgbaImage();
        image.Fill(OfficeColor.White);
        var composition = ImageComposition.CreateTransparent(6, 6).DrawImage(snapshot, 1, 1, 4, 4);
        using var composed = PlaygroundImage.FromRgbaImage(composition.ToImage());
        using var output = composed.ToStream(ImageType.Png);
        using var reloaded = PlaygroundImage.Load(output);
        Assert.Equal(6, reloaded.Width);
        Assert.Equal(6, reloaded.Height);
        Assert.Equal((byte)0, reloaded.Raster.GetPixel(0, 0).A);
        Assert.NotEqual(OfficeColor.White, reloaded.Raster.GetPixel(1, 1));
        snapshot.Pixels[0] = 0;
        Assert.Equal(OfficeColor.White, image.Raster.GetPixel(0, 0));
    }

    [Theory]
    [InlineData(ImageType.Gif)]
    [InlineData(ImageType.Png)]
    public void ZeroAndPositiveFrameDelaysSurviveLoadEditSaveReload(ImageType format) {
        var frames = new OfficeRasterFrames(new[] {
            new OfficeRasterFrame(new OfficeRasterImage(4, 2, OfficeColor.Red), TimeSpan.Zero),
            new OfficeRasterFrame(new OfficeRasterImage(4, 2, OfficeColor.Blue), TimeSpan.FromMilliseconds(70)),
            new OfficeRasterFrame(new OfficeRasterImage(4, 2, OfficeColor.White), TimeSpan.Zero)
        }, playCount: 3);
        using var source = PlaygroundImage.FromFrames(frames);
        byte[] input = source.Encode(format).EncodedBytes;
        using var loaded = PlaygroundImage.Load(input);
        Assert.Equal(new[] { TimeSpan.Zero, TimeSpan.FromMilliseconds(70), TimeSpan.Zero }, loaded.Frames.Select(frame => frame.Duration));
        loaded.Resize(new OfficeRasterResizeOptions { Width = 2, Height = 1, Fit = OfficeImageFit.Stretch });
        using var output = loaded.ToStream(format);
        using var roundTrip = PlaygroundImage.Load(output);
        Assert.Equal(3, roundTrip.Frames.PlayCount);
        Assert.Equal(3, roundTrip.Frames.Count);
        Assert.Equal(new[] { TimeSpan.Zero, TimeSpan.FromMilliseconds(70), TimeSpan.Zero }, roundTrip.Frames.Select(frame => frame.Duration));
        Assert.All(roundTrip.Frames, frame => Assert.Equal(2, frame.Image.Width));
        Assert.Equal(OfficeColor.Blue, roundTrip.Frames[1].Image.GetPixel(1, 0));
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(70, 3)]
    public void SingleFrameApngKeepsSourceAnimationSemanticsThroughCloneResizeAndAvatar(int milliseconds, int playCount) {
        byte[] input = RasterAnimationEncoder.Encode(new[] {
            new RasterAnimationFrame(new RgbaImage(4, 2, new OfficeRasterImage(4, 2, OfficeColor.Red).GetPixels()), TimeSpan.FromMilliseconds(milliseconds))
        }, RasterAnimationFormat.Apng, new RasterAnimationOptions { PlayCount = playCount });
        using var original = PlaygroundImage.Load(input);
        using var clone = original.Clone();
        clone.Resize(50);
        clone.Avatar(2, 2, 0);
        using var stream = clone.ToStream(ImageType.Png);
        byte[] output = stream.ToArray();
        Assert.True(OfficeRasterContainerInspector.TryInspect(output, out var container));
        Assert.True(container!.IsAnimated);
        using var loaded = PlaygroundImage.Load(output);
        Assert.Single(loaded.Frames);
        Assert.Equal(TimeSpan.FromMilliseconds(milliseconds), loaded.Frames[0].Duration);
        Assert.Equal(playCount, loaded.Frames.PlayCount);
        Assert.Equal(2, loaded.Width);
        Assert.Equal(2, loaded.Height);
        Assert.Equal(4, original.Width);
        Assert.Equal(2, original.Height);
        using var still = PlaygroundImage.FromRaster(clone.Raster);
        byte[] stillOutput = still.Encode(ImageType.Png).EncodedBytes;
        Assert.True(OfficeRasterContainerInspector.TryInspect(stillOutput, out var stillContainer));
        Assert.False(stillContainer!.IsAnimated);
        using var jpeg = PlaygroundImage.Load(clone.Encode(ImageType.Jpeg).EncodedBytes);
        Assert.Equal(OfficeImageFormat.Jpeg, jpeg.SourceFormat);
        using var authored = PlaygroundImage.FromFrames(new OfficeRasterFrames(new[] {
            new OfficeRasterFrame(new OfficeRasterImage(2, 1, OfficeColor.Blue), TimeSpan.FromMilliseconds(70))
        }, playCount: 3));
        Assert.True(OfficeRasterContainerInspector.TryInspect(authored.Encode(ImageType.Png).EncodedBytes, out var authoredContainer));
        Assert.True(authoredContainer!.IsAnimated);
        Assert.Equal(3, authoredContainer.PlayCount);
    }

    [Theory]
    [InlineData(OfficePngCompression.Stored, (byte)0x01)]
    [InlineData(OfficePngCompression.Fastest, (byte)0x5e)]
    [InlineData(OfficePngCompression.Optimal, (byte)0x9c)]
    public void AnimatedPngHonorsOwnedCompressionAndResolutionSuppression(OfficePngCompression compression, byte zlibFlags) {
        using var image = PlaygroundImage.FromFrames(new OfficeRasterFrames(new[] {
            new OfficeRasterFrame(new OfficeRasterImage(8, 4, OfficeColor.Red), TimeSpan.Zero),
            new OfficeRasterFrame(new OfficeRasterImage(8, 4, OfficeColor.Blue), TimeSpan.FromMilliseconds(70))
        }));
        image.Metadata.SetExifValue(OfficeExifTag.Software, "animation-options-contract");
        image.Metadata.Resolution = new OfficeImageResolution(300, 150);
        var options = new OfficeRasterEncodingOptions { WriteResolutionMetadata = false };
        options.Png.Compression = compression;
        var prepared = image.Encode(ImageType.Png, options);
        byte[] idat = FindPngChunk(prepared.EncodedBytes, "IDAT")!;
        Assert.Equal(new byte[] { 0x78, zlibFlags }, idat.Take(2));
        Assert.Null(FindPngChunk(prepared.EncodedBytes, "pHYs"));
        using var loaded = PlaygroundImage.Load(prepared.EncodedBytes);
        Assert.Equal("animation-options-contract", loaded.Metadata.GetExifValue(OfficeExifTag.Software)!.Value);
        Assert.Equal(TimeSpan.Zero, loaded.Frames[0].Duration);
        Assert.Equal(TimeSpan.FromMilliseconds(70), loaded.Frames[1].Duration);
        Assert.Equal(OfficeColor.Blue, loaded.Frames[1].Image.GetPixel(7, 3));
        Assert.Equal(300, image.Metadata.Resolution.Horizontal);
    }

    private static byte[]? FindPngChunk(byte[] encoded, string type) {
        for (int offset = 8; offset + 12 <= encoded.Length;) {
            int size = checked((int)(((uint)encoded[offset] << 24) | ((uint)encoded[offset + 1] << 16) | ((uint)encoded[offset + 2] << 8) | encoded[offset + 3]));
            if (Encoding.ASCII.GetString(encoded, offset + 4, 4) == type) {
                var chunk = new byte[size];
                Buffer.BlockCopy(encoded, offset + 8, chunk, 0, size);
                return chunk;
            }
            offset += checked(size + 12);
        }
        return null;
    }

    [Fact]
    public void ExplicitOwnerOrientationNormalizesMetadataAndAutoOrientCannotRepeatIt() {
        using var original = PlaygroundImage.FromRaster(new OfficeRasterImage(3, 2, OfficeColor.Red));
        original.Metadata.SetExifValue(OfficeExifTag.Orientation, (ushort)6);
        byte[] encoded = original.Encode(ImageType.Jpeg).EncodedBytes;
        using var loaded = PlaygroundImage.Load(encoded, new OfficeRasterDecodeOptions { ApplyExifOrientation = true });
        Assert.Equal(2, loaded.Width);
        Assert.Equal(3, loaded.Height);
        Assert.Equal(1, Convert.ToInt32(loaded.Metadata.GetExifValue(OfficeExifTag.Orientation)!.Value));
        byte[] pixels = loaded.Raster.GetPixels();
        loaded.AutoOrient();
        Assert.Equal(pixels, loaded.Raster.GetPixels());
        Assert.Equal(2, loaded.Width);
        Assert.Equal(3, loaded.Height);
    }

    [Fact]
    public void ConversionChecksDecodedContainerBeforeIconBytePreservation() {
        using var source = PlaygroundImage.FromRaster(new OfficeRasterImage(12, 8, OfficeColor.Red));
        byte[] jpeg = source.Encode(ImageType.Jpeg).EncodedBytes;
        string directory = Path.Combine(Path.GetTempPath(), "ImageConsumerConversion-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        try {
            string input = Path.Combine(directory, "mislabeled.ico");
            string output = Path.Combine(directory, "converted.ico");
            File.WriteAllBytes(input, jpeg);
            ImageHelper.ConvertTo(input, output);
            using var loaded = PlaygroundImage.Load(output);
            Assert.Equal(OfficeImageFormat.Icon, loaded.SourceFormat);
            Assert.Equal(12, loaded.Width);
            Assert.Equal(8, loaded.Height);
            Assert.NotEqual(jpeg, File.ReadAllBytes(output));
        } finally { Directory.Delete(directory, true); }
    }

    [Fact]
    public void OwnerLimitRejectsOversizedResizeBeforePublishingPixels() {
        using var image = PlaygroundImage.FromRaster(new OfficeRasterImage(8, 4, OfficeColor.Red));
        var frames = image.Frames;
        byte[] pixels = image.Raster.GetPixels();
        Assert.ThrowsAny<ArgumentException>(() => image.Resize(new OfficeRasterResizeOptions { Width = int.MaxValue }));
        Assert.Same(frames, image.Frames);
        Assert.Equal(pixels, image.Raster.GetPixels());
    }

    [Fact]
    public void CanceledEditsAndEncodeLeaveFramesMetadataAndSeekableOutputUntouched() {
        using var image = PlaygroundImage.FromRaster(new OfficeRasterImage(6, 4, OfficeColor.Red));
        image.Metadata.SetExifValue(OfficeExifTag.Software, "cancellation-contract");
        byte[] pixels = image.Raster.GetPixels();
        var frames = image.Frames;
        var token = new CancellationToken(true);
        Assert.Throws<OperationCanceledException>(() => image.Resize(new OfficeRasterResizeOptions { Width = 3 }, token));
        Assert.Throws<OperationCanceledException>(() => image.GaussianBlur(3, token));
        Assert.Throws<OperationCanceledException>(() => image.Crop(new Rectangle(0, 0, 2, 2), token));
        Assert.Throws<OperationCanceledException>(() => image.AddText(0, 0, "cancel", 6, 4, OfficeColor.Blue, new OfficeRasterTextOptions(), token));
        Assert.Throws<OperationCanceledException>(() => image.Avatar(2, 2, 1, token));
        using var stream = new MemoryStream();
        stream.Write(new byte[] { 1, 2, 3 }, 0, 3);
        stream.Position = 2;
        Assert.Throws<OperationCanceledException>(() => image.Save(stream, ImageType.Png, cancellationToken: token));
        Assert.Equal(2, stream.Position);
        Assert.Equal(new byte[] { 1, 2, 3 }, stream.ToArray());
        Assert.Same(frames, image.Frames);
        Assert.Equal(pixels, image.Raster.GetPixels());
        Assert.Equal("cancellation-contract", image.Metadata.GetExifValue(OfficeExifTag.Software)!.Value);
    }
}

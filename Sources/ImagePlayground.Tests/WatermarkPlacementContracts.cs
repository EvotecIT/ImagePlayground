using System;
using System.IO;
using OfficeIMO.Drawing;
using Xunit;

namespace ImagePlayground.Tests;

public partial class ImagePlayground {
    [Theory]
    [InlineData(WatermarkPlacement.TopLeft, 2, 2, 2, 2)]
    [InlineData(WatermarkPlacement.TopRight, 88, 2, 40, 2)]
    [InlineData(WatermarkPlacement.BottomLeft, 2, 58, 2, 26)]
    [InlineData(WatermarkPlacement.BottomRight, 88, 58, 40, 26)]
    [InlineData(WatermarkPlacement.Middle, 45, 30, 21, 14)]
    public void PredefinedImageWatermarksUseEachFramesCanvas(WatermarkPlacement placement, int firstX, int firstY, int secondX, int secondY) {
        string watermark = Path.Combine(_directoryWithTests, "placement-tile-" + placement + ".png");
        using (var tile = global::ImagePlayground.Image.FromRaster(new OfficeRasterImage(6, 4, OfficeColor.Red))) {
            tile.Save(watermark);
        }
        var original = new OfficeRasterFrames(new[] {
            new OfficeRasterFrame(new OfficeRasterImage(96, 64, OfficeColor.Blue), TimeSpan.FromMilliseconds(100)),
            new OfficeRasterFrame(new OfficeRasterImage(48, 32, OfficeColor.Blue), TimeSpan.FromMilliseconds(200))
        }, playCount: 2);
        using var image = global::ImagePlayground.Image.FromFrames(original);

        image.WatermarkImage(watermark, placement, padding: 2, watermarkPercentage: 100);

        Assert.Equal(2, image.Frames.PlayCount);
        for (int index = 0; index < original.Count; index++) {
            var frame = image.Frames[index];
            using var rendered = global::ImagePlayground.Image.FromRaster(frame.Image);
            rendered.Save(Path.Combine(_directoryWithTests, "placement-image-" + placement + "-" + index + ".png"));
            int left = index == 0 ? firstX : secondX;
            int top = index == 0 ? firstY : secondY;
            Assert.Equal(original[index].Duration, frame.Duration);
            Assert.Equal(original[index].Image.Width, frame.Image.Width);
            Assert.Equal(original[index].Image.Height, frame.Image.Height);
            for (int y = 0; y < frame.Image.Height; y++) {
                for (int x = 0; x < frame.Image.Width; x++) {
                    Assert.Equal(x >= left && x < left + 6 && y >= top && y < top + 4 ? OfficeColor.Red : OfficeColor.Blue,
                        frame.Image.GetPixel(x, y));
                }
            }
        }
    }

    [Theory]
    [InlineData(WatermarkPlacement.BottomRight)]
    [InlineData(WatermarkPlacement.Middle)]
    public void PredefinedTextWatermarksUseEachFramesCanvas(WatermarkPlacement placement) {
        var original = new OfficeRasterFrames(new[] {
            new OfficeRasterFrame(new OfficeRasterImage(96, 64, OfficeColor.Blue), TimeSpan.FromMilliseconds(100)),
            new OfficeRasterFrame(new OfficeRasterImage(48, 32, OfficeColor.Blue), TimeSpan.FromMilliseconds(200))
        }, playCount: 2);
        using var image = global::ImagePlayground.Image.FromFrames(original);
        var size = image.GetTextSize("W", 9, "Arial");

        image.Watermark("W", placement, OfficeColor.White, fontSize: 9, padding: 2);

        Assert.Equal(2, image.Frames.PlayCount);
        for (int index = 0; index < original.Count; index++) {
            var frame = image.Frames[index];
            using var rendered = global::ImagePlayground.Image.FromRaster(frame.Image);
            rendered.Save(Path.Combine(_directoryWithTests, "placement-text-" + placement + "-" + index + ".png"));
            using var expected = global::ImagePlayground.Image.FromRaster(new OfficeRasterImage(frame.Image.Width, frame.Image.Height, OfficeColor.Blue));
            double x = placement == WatermarkPlacement.Middle ? (frame.Image.Width - size.Width) / 2 : frame.Image.Width - size.Width - 2;
            double y = placement == WatermarkPlacement.Middle ? (frame.Image.Height - size.Height) / 2 : frame.Image.Height - size.Height - 2;
            expected.AddText((float)x, (float)y, "W", OfficeColor.White, fontSize: 9);
            Assert.Equal(original[index].Duration, frame.Duration);
            Assert.Equal(expected.Raster.GetPixels(), frame.Image.GetPixels());
        }
    }
}
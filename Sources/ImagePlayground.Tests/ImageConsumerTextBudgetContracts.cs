using System;
using OfficeIMO.Drawing;
using Xunit;
using PlaygroundImage = global::ImagePlayground.Image;

namespace ImagePlayground.Tests;

/// <summary>Aggregate text scratch is planned before the consumer copies retained frames.</summary>
public sealed class ImageConsumerTextBudgetContracts {
    [Theory]
    [InlineData("point")]
    [InlineData("rectangle")]
    [InlineData("wrapped")]
    [InlineData("clipped")]
    [InlineData("watermark-point")]
    [InlineData("watermark-placement")]
    public void TextEntryPointsRejectAggregateWorkingSetBeforePublishingPixels(string entryPoint) {
        using var image = PlaygroundImage.FromRaster(new OfficeRasterImage(10000, 3000, OfficeColor.White));
        var frames = image.Frames;
        Action draw = entryPoint switch {
            "point" => () => image.AddText(10, 10, "budget", OfficeColor.Black),
            "rectangle" => () => image.AddText(10, 10, "budget", 200, 40, OfficeColor.Black, new OfficeRasterTextOptions()),
            "wrapped" => () => image.AddTextBox(10, 10, "budget", 200, OfficeColor.Black),
            "clipped" => () => image.AddTextBox(10, 10, "budget", 200, 40, OfficeColor.Black),
            "watermark-point" => () => image.Watermark("budget", 10, 10, OfficeColor.Black),
            "watermark-placement" => () => image.Watermark("budget", WatermarkPlacement.Middle, OfficeColor.Black),
            _ => throw new ArgumentOutOfRangeException(nameof(entryPoint))
        };
        Assert.ThrowsAny<ArgumentException>(draw);
        Assert.Same(frames, image.Frames);
        Assert.Equal(OfficeColor.White, image.Raster.GetPixel(10, 10));
        Assert.Equal(OfficeColor.White, image.Raster.GetPixel(9999, 2999));
    }

    [Fact]
    public void TextBudgetIncludesOutlineAndLargestFrameRatherThanOnlyTheFirstFrame() {
        var frames = new OfficeRasterFrames(new[] {
            new OfficeRasterFrame(new OfficeRasterImage(2, 1, OfficeColor.Blue)),
            new OfficeRasterFrame(new OfficeRasterImage(8000, 2500, OfficeColor.White))
        });
        using var image = PlaygroundImage.FromFrames(frames);
        var options = new OfficeRasterTextOptions { OutlineColor = OfficeColor.Red, OutlineWidth = 1 };
        Assert.ThrowsAny<ArgumentException>(() => image.AddText(10, 10, "budget", 200, 40, OfficeColor.Black, options));
        Assert.Same(frames, image.Frames);
        Assert.Equal(OfficeColor.Blue, image.Frames[0].Image.GetPixel(0, 0));
        Assert.Equal(OfficeColor.White, image.Frames[1].Image.GetPixel(10, 10));
    }
}

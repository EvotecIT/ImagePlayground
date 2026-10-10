using System;
using OfficeIMO.Drawing;
using Xunit;
using PlaygroundImage = global::ImagePlayground.Image;

namespace ImagePlayground.Tests;

/// <summary>Scratch-bearing filters include all retained frames in their working-set budget.</summary>
public sealed class ImageConsumerFilterBudgetContracts {
    [Theory]
    [InlineData("gaussian")]
    [InlineData("box")]
    [InlineData("sharpen")]
    [InlineData("adaptive")]
    public void MultiframeFilterRejectsCombinedScratchBeforePublishingAnyFrame(string filter) {
        var frames = new OfficeRasterFrames(new[] {
            new OfficeRasterFrame(new OfficeRasterImage(7000, 1000, OfficeColor.White)),
            new OfficeRasterFrame(new OfficeRasterImage(7000, 1000, OfficeColor.Blue)),
            new OfficeRasterFrame(new OfficeRasterImage(7000, 1000, OfficeColor.Red))
        });
        using var image = PlaygroundImage.FromFrames(frames);
        Action apply = filter switch {
            "gaussian" => () => image.GaussianBlur(0.02f),
            "box" => () => image.BoxBlur(),
            "sharpen" => () => image.GaussianSharpen(0.02f),
            "adaptive" => () => image.AdaptiveThreshold(),
            _ => throw new ArgumentOutOfRangeException(nameof(filter))
        };
        Assert.ThrowsAny<ArgumentException>(apply);
        Assert.Same(frames, image.Frames);
        Assert.Equal(OfficeColor.White, image.Frames[0].Image.GetPixel(6999, 999));
        Assert.Equal(OfficeColor.Blue, image.Frames[1].Image.GetPixel(6999, 999));
        Assert.Equal(OfficeColor.Red, image.Frames[2].Image.GetPixel(6999, 999));
    }
}

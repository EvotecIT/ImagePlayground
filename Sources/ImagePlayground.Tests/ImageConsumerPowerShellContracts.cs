#if NET8_0
using System;
using System.IO;
using System.Linq;
using OfficeIMO.Drawing;
using Xunit;
using PlaygroundImage = global::ImagePlayground.Image;

namespace ImagePlayground.Tests;

public partial class ImagePlayground {
    [Fact]
    public void ConsumerObjectPipelineRetainsIdentityAndAcceptsSizesAboveOneThousand() {
        using var fixture = new PowerShellPathFixture();
        var result = fixture.Invoke("Invoke-Expression", ("Command", "$image = Get-Image ./source.png; $resized = @($image | Resize-Image -Width 1200 -Height 1 -DontRespectAspectRatio); [object]::ReferenceEquals($image, $resized[0]); $resized | Save-Image -FilePath ./object-pipeline.png; $image.Dispose()"));
        Assert.True((bool)Assert.Single(result).BaseObject);
        fixture.AssertImageOutput("object-pipeline.png", 1200, 1);
        Assert.Empty(fixture.Invoke("Resize-Image", ("FilePath", "./source.png"), ("OutputPath", "./path-output.png"), ("Width", 1200), ("Height", 1), ("DontRespectAspectRatio", true)));
        fixture.AssertImageOutput("path-output.png", 1200, 1);
        var percentage = fixture.Invoke("Invoke-Expression", ("Command", "Get-Image ./source.png | Resize-Image -Percentage 50"));
        using var image = Assert.IsType<PlaygroundImage>(Assert.Single(percentage).BaseObject);
        Assert.Equal(32, image.Width);
        Assert.Equal(24, image.Height);
    }

    [Fact]
    public void ConsumerSavePipelineHonorsExplicitStreamFormatAndOwnedSettings() {
        using var fixture = new PowerShellPathFixture();
        var result = fixture.Invoke("Invoke-Expression", ("Command", "$image = Get-Image ./source.png; try { $options = [OfficeIMO.Drawing.OfficeRasterEncodingOptions]::new(); $options.Resolution = [OfficeIMO.Drawing.OfficeImageResolution]::new(300, 150); $image | Save-Image -AsStream -Format Jpeg -EncodingOptions $options -Quality 80 } finally { $image.Dispose() }"));
        using var stream = Assert.IsType<MemoryStream>(Assert.Single(result).BaseObject);
        Assert.Equal(0, stream.Position);
        using var loaded = PlaygroundImage.Load(stream);
        Assert.Equal(OfficeImageFormat.Jpeg, loaded.SourceFormat);
        Assert.Equal(300, loaded.Metadata.PhysicalDpiX!.Value, 1);
        Assert.Equal(150, loaded.Metadata.PhysicalDpiY!.Value, 1);
    }
}
#endif

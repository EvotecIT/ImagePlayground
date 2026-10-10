namespace ImagePlayground;

/// <summary>Text and raster watermark workflows.</summary>
public partial class Image {
    /// <summary>Draws a text watermark at explicit pixel coordinates.</summary>
    public void Watermark(string text, float x, float y, OfficeColor color, float fontSize = 16f, string fontFamilyName = "Arial", float padding = 18f, CancellationToken cancellationToken = default) => AddText(x, y, text, color, fontSize, fontFamilyName, cancellationToken: cancellationToken);

    /// <summary>Draws a text watermark at a predefined placement.</summary>
    public void Watermark(string text, WatermarkPlacement placement, OfficeColor color, float fontSize = 16f, string fontFamilyName = "Arial", float padding = 18f, CancellationToken cancellationToken = default) {
        var size = GetTextSize(text, fontSize, fontFamilyName, cancellationToken);
        var point = GetWatermarkLocation(placement, size.Width, size.Height, padding);
        Watermark(text, (float)point.X, (float)point.Y, color, fontSize, fontFamilyName, padding, cancellationToken);
    }

    /// <summary>Draws an image watermark at a predefined placement.</summary>
    public void WatermarkImage(string filePath, WatermarkPlacement placement, float opacity = 1f, float padding = 18f, int rotate = 0, FlipMode flipMode = FlipMode.None, int watermarkPercentage = 20, CancellationToken cancellationToken = default) {
        using var watermark = PrepareWatermark(filePath, watermarkPercentage, rotate, flipMode, cancellationToken);
        AddImage(watermark.Raster, GetWatermarkLocation(placement, watermark.Width, watermark.Height, padding), opacity, cancellationToken);
    }

    /// <summary>Draws an image watermark at explicit pixel coordinates.</summary>
    public void WatermarkImage(string filePath, int x, int y, float opacity = 1f, int rotate = 0, FlipMode flipMode = FlipMode.None, int watermarkPercentage = 20, CancellationToken cancellationToken = default) {
        using var watermark = PrepareWatermark(filePath, watermarkPercentage, rotate, flipMode, cancellationToken);
        AddImage(watermark.Raster, x, y, opacity, cancellationToken);
    }

    /// <summary>Draws repeated image watermarks within each frame's canvas, observing cancellation during tiling.</summary>
    /// <remarks>Each target frame is copied once. Timing and playback count remain unchanged, and a failed or canceled operation leaves the original frame sequence intact.</remarks>
    public void WatermarkImageTiled(string filePath, int spacing, float opacity = 1f, int rotate = 0, FlipMode flipMode = FlipMode.None, int watermarkPercentage = 20, CancellationToken cancellationToken = default) {
        EnsureUsable();
        cancellationToken.ThrowIfCancellationRequested();
        if (spacing < 0) { throw new ArgumentOutOfRangeException(nameof(spacing)); }
        if (opacity < 0 || opacity > 1 || float.IsNaN(opacity)) {
            throw new ArgumentOutOfRangeException(nameof(opacity));
        }
        using var watermark = PrepareWatermark(filePath, watermarkPercentage, rotate, flipMode, cancellationToken);
        if (opacity == 0) {
            return;
        }
        long stepX = (long)watermark.Width + spacing;
        long stepY = (long)watermark.Height + spacing;
        long watermarkBytes = watermark.Frames.Sum(frame => (long)frame.Image.Width * frame.Image.Height * 4);
        _frames = _frames.Transform(source => {
            var result = source.Clone();
            var canvas = new OfficeRasterCanvas(result, font: null, fonts: null, cancellationToken: cancellationToken);
            for (long y = spacing; y < source.Height; y += stepY) {
                cancellationToken.ThrowIfCancellationRequested();
                for (long x = spacing; x < source.Width; x += stepX) {
                    cancellationToken.ThrowIfCancellationRequested();
                    canvas.DrawAffineImage(watermark.Raster, OfficeTransform.Translate(x, y), opacity);
                }
            }
            return result;
        }, cancellationToken: cancellationToken, additionalRetainedBytes: watermarkBytes);
    }

    private static Image PrepareWatermark(string filePath, int percentage, int rotate, FlipMode flipMode, CancellationToken cancellationToken = default) {
        cancellationToken.ThrowIfCancellationRequested();
        if (percentage < 1 || percentage > 100) { throw new ArgumentOutOfRangeException(nameof(percentage), "Watermark percentage must be between 1 and 100."); }
        var image = Load(filePath, new OfficeRasterDecodeOptions { ApplyExifOrientation = false, CancellationToken = cancellationToken });
        try {
            cancellationToken.ThrowIfCancellationRequested();
            if (image.Frames.Count > 1) {
                var firstFrame = FromRaster(image.Raster);
                image.Dispose();
                image = firstFrame;
            }
            if (percentage != 100) { image.Resize(Math.Max(1, checked((int)((long)image.Width * percentage / 100))), Math.Max(1, checked((int)((long)image.Height * percentage / 100))), cancellationToken: cancellationToken); }
            cancellationToken.ThrowIfCancellationRequested();
            if (flipMode != FlipMode.None) { image.Flip(flipMode, cancellationToken); }
            cancellationToken.ThrowIfCancellationRequested();
            if (rotate != 0) { image.Rotate(rotate, cancellationToken); }
            cancellationToken.ThrowIfCancellationRequested();
            return image;
        } catch { image.Dispose(); throw; }
    }

    private OfficePoint GetWatermarkLocation(WatermarkPlacement placement, double width, double height, double padding) => placement switch {
        WatermarkPlacement.TopLeft => new OfficePoint(padding, padding),
        WatermarkPlacement.TopRight => new OfficePoint(Width - width - padding, padding),
        WatermarkPlacement.BottomLeft => new OfficePoint(padding, Height - height - padding),
        WatermarkPlacement.BottomRight => new OfficePoint(Width - width - padding, Height - height - padding),
        WatermarkPlacement.Middle => new OfficePoint((Width - width) / 2, (Height - height) / 2),
        _ => throw new ArgumentOutOfRangeException(nameof(placement))
    };
}

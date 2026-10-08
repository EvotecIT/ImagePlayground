namespace ImagePlayground;

/// <summary>Text and raster watermark workflows.</summary>
public partial class Image {
    /// <summary>Draws a text watermark at explicit pixel coordinates.</summary>
    public void Watermark(string text, float x, float y, OfficeColor color, float fontSize = 16f, string fontFamilyName = "Arial", float padding = 18f) => AddText(x, y, text, color, fontSize, fontFamilyName);

    /// <summary>Draws a text watermark at a predefined placement.</summary>
    public void Watermark(string text, WatermarkPlacement placement, OfficeColor color, float fontSize = 16f, string fontFamilyName = "Arial", float padding = 18f) {
        var size = GetTextSize(text, fontSize, fontFamilyName);
        var point = GetWatermarkLocation(placement, size.Width, size.Height, padding);
        Watermark(text, (float)point.X, (float)point.Y, color, fontSize, fontFamilyName, padding);
    }

    /// <summary>Draws an image watermark at a predefined placement.</summary>
    public void WatermarkImage(string filePath, WatermarkPlacement placement, float opacity = 1f, float padding = 18f, int rotate = 0, FlipMode flipMode = FlipMode.None, int watermarkPercentage = 20) {
        using var watermark = PrepareWatermark(filePath, watermarkPercentage, rotate, flipMode);
        AddImage(watermark.Raster, GetWatermarkLocation(placement, watermark.Width, watermark.Height, padding), opacity);
    }

    /// <summary>Draws an image watermark at explicit pixel coordinates.</summary>
    public void WatermarkImage(string filePath, int x, int y, float opacity = 1f, int rotate = 0, FlipMode flipMode = FlipMode.None, int watermarkPercentage = 20) {
        using var watermark = PrepareWatermark(filePath, watermarkPercentage, rotate, flipMode);
        AddImage(watermark.Raster, x, y, opacity);
    }

    /// <summary>Draws repeated image watermarks with nonnegative spacing between tiles.</summary>
    public void WatermarkImageTiled(string filePath, int spacing, float opacity = 1f, int rotate = 0, FlipMode flipMode = FlipMode.None, int watermarkPercentage = 20) {
        if (spacing < 0) { throw new ArgumentOutOfRangeException(nameof(spacing)); }
        using var watermark = PrepareWatermark(filePath, watermarkPercentage, rotate, flipMode);
        long stepX = (long)watermark.Width + spacing;
        long stepY = (long)watermark.Height + spacing;
        for (long y = spacing; y < Height; y += stepY) {
            for (long x = spacing; x < Width; x += stepX) { AddImage(watermark.Raster, (int)x, (int)y, opacity); }
        }
    }

    private static Image PrepareWatermark(string filePath, int percentage, int rotate, FlipMode flipMode) {
        if (percentage < 1 || percentage > 100) { throw new ArgumentOutOfRangeException(nameof(percentage), "Watermark percentage must be between 1 and 100."); }
        var image = Load(filePath);
        try {
            if (percentage != 100) { image.Resize(Math.Max(1, checked((int)((long)image.Width * percentage / 100))), Math.Max(1, checked((int)((long)image.Height * percentage / 100)))); }
            if (flipMode != FlipMode.None) { image.Flip(flipMode); }
            if (rotate != 0) { image.Rotate(rotate); }
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
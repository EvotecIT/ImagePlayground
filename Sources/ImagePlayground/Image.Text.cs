namespace ImagePlayground;

/// <summary>Font-aware text workflows delegated to the shared raster text engine.</summary>
public partial class Image {
    /// <summary>Measures authored text lines with the requested managed font family.</summary>
    public OfficeTextBlockLayout GetTextSize(string text, float fontSize, string fontFamilyName, CancellationToken cancellationToken = default) =>
        OfficeRasterText.Measure(text, fontSize, fontFamilyName, cancellationToken: cancellationToken);

    /// <summary>Measures text using the same owned layout and font options as drawing.</summary>
    public OfficeTextBlockLayout GetTextSize(string text, OfficeRasterTextOptions options, double? wrapWidth = null, CancellationToken cancellationToken = default) =>
        OfficeRasterText.Measure(text, options, wrapWidth, cancellationToken);

    /// <summary>Draws text at a pixel location, with optional shadow and outline.</summary>
    public void AddText(float x, float y, string text, OfficeColor color, float fontSize = 16f, string fontFamilyName = "Arial", OfficeColor? shadowColor = null, float shadowOffsetX = 0f, float shadowOffsetY = 0f, OfficeColor? outlineColor = null, float outlineWidth = 0f, CancellationToken cancellationToken = default) {
        var options = CreateTextOptions(fontSize, fontFamilyName, shadowColor, shadowOffsetX, shadowOffsetY, outlineColor, outlineWidth);
        var size = GetTextSize(text, options, cancellationToken: cancellationToken);
        AddText(x, y, text, Math.Max(1, size.Width), Math.Max(1, size.Height), color, options, cancellationToken);
    }

    /// <summary>Draws text inside a rectangle with shared font, layout, clipping and effect options.</summary>
    /// <remarks>Every frame is updated atomically; cancellation leaves the original sequence intact.</remarks>
    public void AddText(double x, double y, string text, double width, double height, OfficeColor color, OfficeRasterTextOptions options, CancellationToken cancellationToken = default) {
        EnsureUsable();
        cancellationToken.ThrowIfCancellationRequested();
        if (options == null) { throw new ArgumentNullException(nameof(options)); }
        var settings = options.Clone();
        long additionalWorkingBytes = 0;
        foreach (var frame in _frames) {
            cancellationToken.ThrowIfCancellationRequested();
            additionalWorkingBytes = Math.Max(additionalWorkingBytes,
                OfficeRasterText.EstimateAdditionalWorkingBytes(frame.Image.Width, frame.Image.Height, settings));
        }
        Apply(source => {
            var result = source.Clone();
            OfficeRasterText.Draw(result, text, x, y, width, height, color, settings, cancellationToken);
            return result;
        }, additionalRetainedBytes: additionalWorkingBytes, cancellationToken: cancellationToken);
    }

    /// <summary>Wraps text to the supplied width without clipping its measured height.</summary>
    public void AddTextBox(float x, float y, string text, float boxWidth, OfficeColor color, float fontSize = 16f, string fontFamilyName = "Arial", OfficeTextAlignment horizontalAlignment = OfficeTextAlignment.Left, OfficeTextVerticalAlignment verticalAlignment = OfficeTextVerticalAlignment.Top, OfficeColor? shadowColor = null, float shadowOffsetX = 0f, float shadowOffsetY = 0f, OfficeColor? outlineColor = null, float outlineWidth = 0f, CancellationToken cancellationToken = default) =>
        AddTextBox(x, y, text, boxWidth, 0, color, fontSize, fontFamilyName, horizontalAlignment, verticalAlignment, shadowColor, shadowOffsetX, shadowOffsetY, outlineColor, outlineWidth, cancellationToken);

    /// <summary>Draws wrapped text within a box, clipping when an explicit positive height is supplied.</summary>
    public void AddTextBox(float x, float y, string text, float boxWidth, float boxHeight, OfficeColor color, float fontSize = 16f, string fontFamilyName = "Arial", OfficeTextAlignment horizontalAlignment = OfficeTextAlignment.Left, OfficeTextVerticalAlignment verticalAlignment = OfficeTextVerticalAlignment.Top, OfficeColor? shadowColor = null, float shadowOffsetX = 0f, float shadowOffsetY = 0f, OfficeColor? outlineColor = null, float outlineWidth = 0f, CancellationToken cancellationToken = default) {
        if (boxWidth <= 0 || boxHeight < 0) { throw new ArgumentOutOfRangeException(nameof(boxWidth)); }
        var options = CreateTextOptions(fontSize, fontFamilyName, shadowColor, shadowOffsetX, shadowOffsetY, outlineColor, outlineWidth);
        options.HorizontalAlignment = horizontalAlignment;
        options.VerticalAlignment = verticalAlignment;
        options.Wrap = true;
        options.Clip = boxHeight > 0;
        var measured = GetTextSize(text, options, boxWidth, cancellationToken);
        AddText(x, y, text, boxWidth, boxHeight > 0 ? boxHeight : Math.Max(1, measured.Height), color, options, cancellationToken);
    }

    private static OfficeRasterTextOptions CreateTextOptions(float fontSize, string fontFamily, OfficeColor? shadowColor, float shadowOffsetX, float shadowOffsetY, OfficeColor? outlineColor, float outlineWidth) => new OfficeRasterTextOptions {
        FontSize = fontSize, FontFamily = fontFamily, ShadowColor = shadowColor,
        ShadowOffsetX = shadowOffsetX, ShadowOffsetY = shadowOffsetY, OutlineColor = outlineColor, OutlineWidth = outlineWidth
    };
}

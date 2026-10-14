using System;
using CodeGlyphX;
using CodeGlyphX.Rendering;
using OfficeIMO.Drawing;

namespace ImagePlayground;

/// <summary>Visual settings for a generated barcode image.</summary>
/// <para>Unset values keep the CodeGlyphX defaults: 2 px bars and a 10-module quiet zone for linear symbols, 6 px modules and a 4-module quiet zone for matrix symbols, black on white, no text.</para>
public sealed class BarCodeStyle {
    /// <summary>Pixel width of one module (the narrowest bar or one matrix cell).</summary>
    public int? ModuleSize { get; set; }

    /// <summary>Quiet zone (blank border) in modules.</summary>
    public int? QuietZone { get; set; }

    /// <summary>Bar height in modules. Linear symbols only.</summary>
    public int? Height { get; set; }

    /// <summary>Human-readable text printed under linear symbols.</summary>
    public string? Text { get; set; }

    /// <summary>Print the encoded value under linear symbols when <see cref="Text"/> is not set.</summary>
    public bool ShowText { get; set; }

    /// <summary>Bar colour. Defaults to black.</summary>
    public OfficeColor? ForegroundColor { get; set; }

    /// <summary>Background colour. Defaults to white.</summary>
    public OfficeColor? BackgroundColor { get; set; }

    internal BarcodeOptions ToBarcodeOptions(string content) {
        Validate();
        var options = new BarcodeOptions();
        if (ModuleSize.HasValue) options.ModuleSize = ModuleSize.Value;
        if (QuietZone.HasValue) options.QuietZone = QuietZone.Value;
        if (Height.HasValue) options.HeightModules = Height.Value;
        if (ForegroundColor.HasValue) {
            options.Foreground = ToColor(ForegroundColor.Value);
            options.LabelColor = ToColor(ForegroundColor.Value);
        }
        if (BackgroundColor.HasValue) options.Background = ToColor(BackgroundColor.Value);
        if (!string.IsNullOrEmpty(Text)) options.LabelText = Text;
        else if (ShowText) options.LabelText = content;
        return options;
    }

    internal MatrixOptions ToMatrixOptions() {
        Validate();
        var options = new MatrixOptions();
        if (ModuleSize.HasValue) options.ModuleSize = ModuleSize.Value;
        if (QuietZone.HasValue) options.QuietZone = QuietZone.Value;
        if (ForegroundColor.HasValue) options.Foreground = ToColor(ForegroundColor.Value);
        if (BackgroundColor.HasValue) options.Background = ToColor(BackgroundColor.Value);
        return options;
    }

    private void Validate() {
        if (ModuleSize.HasValue && ModuleSize.Value <= 0) throw new ArgumentOutOfRangeException(nameof(ModuleSize), ModuleSize.Value, "Module size must be greater than zero.");
        if (QuietZone.HasValue && QuietZone.Value < 0) throw new ArgumentOutOfRangeException(nameof(QuietZone), QuietZone.Value, "Quiet zone cannot be negative.");
        if (Height.HasValue && Height.Value <= 0) throw new ArgumentOutOfRangeException(nameof(Height), Height.Value, "Height must be greater than zero.");
    }

    private static Rgba32 ToColor(OfficeColor color) => new(color.R, color.G, color.B, color.A);
}

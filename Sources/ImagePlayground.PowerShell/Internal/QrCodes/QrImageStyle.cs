using System;
using CodeGlyphX;
using CodeGlyphX.Rendering;
using OfficeIMO.Drawing;

namespace ImagePlayground;

/// <summary>Visual settings for a generated QR code image.</summary>
/// <para>Every property is optional; unset values keep the CodeGlyphX defaults used by ImagePlayground (black on white, 20 px modules, a four-module quiet zone, square modules and eyes).</para>
public sealed class QrImageStyle {
    /// <summary>Render the background transparent.</summary>
    public bool Transparent { get; set; }

    /// <summary>Colour of dark modules. Defaults to black.</summary>
    public OfficeColor? ForegroundColor { get; set; }

    /// <summary>Background colour. Defaults to white. Ignored when <see cref="Transparent"/> is set.</summary>
    public OfficeColor? BackgroundColor { get; set; }

    /// <summary>Pixel size of one module. Defaults to 20. Ignored when <see cref="Size"/> is set.</summary>
    public int PixelSize { get; set; } = 20;

    /// <summary>Target image width and height in pixels, including the quiet zone. The module size is the largest whole number that fits, so the image can be slightly smaller.</summary>
    public int? Size { get; set; }

    /// <summary>Quiet zone (white border) in modules. Scanners expect at least four.</summary>
    public int? QuietZone { get; set; }

    /// <summary>Shape of data modules.</summary>
    public QrModuleShape? ModuleShape { get; set; }

    /// <summary>Shape of the three finder patterns (eyes).</summary>
    public QrModuleShape? EyeShape { get; set; }

    /// <summary>Colour of the finder patterns (eyes).</summary>
    public OfficeColor? EyeColor { get; set; }

    /// <summary>Logo width as a fraction of the symbol width (excluding the quiet zone). Defaults to one eighth of the image.</summary>
    public double? LogoScale { get; set; }

    internal QrRenderOptions ToRenderOptions() {
        if (PixelSize <= 0) throw new ArgumentOutOfRangeException(nameof(PixelSize), PixelSize, "Pixel size must be greater than zero.");
        if (Size.HasValue && Size.Value <= 0) throw new ArgumentOutOfRangeException(nameof(Size), Size.Value, "Size must be greater than zero.");
        if (QuietZone.HasValue && QuietZone.Value < 0) throw new ArgumentOutOfRangeException(nameof(QuietZone), QuietZone.Value, "Quiet zone cannot be negative.");
        if (LogoScale.HasValue && (LogoScale.Value <= 0 || LogoScale.Value > 0.4)) throw new ArgumentOutOfRangeException(nameof(LogoScale), LogoScale.Value, "Logo scale must be greater than 0 and at most 0.4 of the symbol width.");

        var options = new QrRenderOptions {
            ModuleSize = PixelSize,
            Foreground = ToColor(ForegroundColor ?? OfficeColor.Black),
            Background = ToColor(Transparent ? OfficeColor.Transparent : BackgroundColor ?? OfficeColor.White)
        };
        if (Size.HasValue) {
            options.TargetSizePx = Size.Value;
            options.TargetSizeIncludesQuietZone = true;
        }
        if (QuietZone.HasValue) options.QuietZone = QuietZone.Value;
        if (ModuleShape.HasValue) options.ModuleShape = ModuleShape.Value;
        if (EyeShape.HasValue || EyeColor.HasValue) {
            var eyes = new QrEyeOptions();
            if (EyeShape.HasValue) {
                eyes.UseFrame = true;
                eyes.OuterShape = EyeShape.Value;
                eyes.InnerShape = EyeShape.Value;
            }
            if (EyeColor.HasValue) {
                eyes.OuterColor = ToColor(EyeColor.Value);
                eyes.InnerColor = ToColor(EyeColor.Value);
            }
            options.Eyes = eyes;
        }
        return options;
    }

    private static Rgba32 ToColor(OfficeColor color) => new(color.R, color.G, color.B, color.A);
}

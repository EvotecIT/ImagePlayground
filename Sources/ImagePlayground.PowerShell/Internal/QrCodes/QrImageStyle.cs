using System;
using CodeGlyphX;
using CodeGlyphX.Rendering;
using OfficeIMO.Drawing;

namespace ImagePlayground;

/// <summary>Visual settings for a generated QR code image.</summary>
/// <para>Every property is optional; unset values keep the CodeGlyphX defaults used by ImagePlayground (black on white, 20 px modules, a four-module quiet zone, square modules and eyes).</para>
public sealed class QrImageStyle {
    /// <summary>Coordinated appearance returned by CodeGlyphX; individual settings can override it.</summary>
    public QrHeroPreset? Preset { get; set; }

    /// <summary>Shape of the solid finder centre.</summary>
    public QrModuleShape? EyeBall { get; set; }

    /// <summary>Two foreground colours, from left to right.</summary>
    public OfficeColor[]? Gradient { get; set; }

    /// <summary>Call-to-action frame outside the complete quiet zone.</summary>
    public QrCanvasTemplate? Frame { get; set; }

    /// <summary>Text below the code, using the portable caption font.</summary>
    public string? Caption { get; set; }
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

        var options = Preset.HasValue ? QrArt.Hero(Preset.Value) : new QrRenderOptions();
        options.ModuleSize = PixelSize;
        if (ForegroundColor.HasValue) {
            options.Foreground = ToColor(ForegroundColor.Value);
            options.ForegroundGradient = null;
        }
        if (Transparent) { options.Background = ToColor(OfficeColor.Transparent); }
        else if (BackgroundColor.HasValue) { options.Background = ToColor(BackgroundColor.Value); }
        if (Size.HasValue) {
            options.TargetSizePx = Size.Value;
            options.TargetSizeIncludesQuietZone = true;
        }
        if (QuietZone.HasValue) options.QuietZone = QuietZone.Value;
        if (ModuleShape.HasValue) options.ModuleShape = ModuleShape.Value;
        if (EyeShape.HasValue || EyeBall.HasValue || EyeColor.HasValue) {
            var eyes = options.Eyes ?? new QrEyeOptions();
            if (EyeShape.HasValue) {
                eyes.UseFrame = true;
                eyes.OuterShape = EyeShape.Value;
                eyes.InnerShape = EyeShape.Value;
            }
            if (EyeBall.HasValue) {
                eyes.UseFrame = true;
                eyes.InnerShape = EyeBall.Value;
            }
            if (EyeColor.HasValue) {
                eyes.OuterColor = ToColor(EyeColor.Value);
                eyes.InnerColor = ToColor(EyeColor.Value);
            }
            options.Eyes = eyes;
        }
        if (Gradient is not null) {
            if (Gradient.Length != 2) {
                throw new ArgumentException("Supply exactly two gradient colours.", nameof(Gradient));
            }
            options.ForegroundGradient = new QrGradientOptions { StartColor = ToColor(Gradient[0]), EndColor = ToColor(Gradient[1]) };
        }
        if (Frame.HasValue && options.Canvas is not null) { options.Canvas.Template = Frame.Value; }
        if ((Frame.HasValue && Frame.Value != QrCanvasTemplate.None) || Caption is not null) {
            options.Canvas ??= new QrCanvasOptions { PaddingPx = 32 };
            if (Frame.HasValue) { options.Canvas.Template = Frame.Value; }
            options.Canvas.Caption = new QrCanvasCaptionOptions { Text = Caption ?? "SCAN ME" };
        }
        return options;
    }

    private static Rgba32 ToColor(OfficeColor color) => new(color.R, color.G, color.B, color.A);
}

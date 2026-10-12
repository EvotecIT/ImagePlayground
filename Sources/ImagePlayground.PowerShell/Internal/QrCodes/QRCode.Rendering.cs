using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CodeGlyphX;
using CodeGlyphX.Payloads;
using CodeGlyphX.Rendering;
using OfficeIMO.Drawing;


using CodeGlyphXRgba32 = CodeGlyphX.Rendering.Rgba32;

namespace ImagePlayground;

public partial class QrCode {
    private static QrRenderOptions BuildOptions(bool transparent, OfficeColor? foregroundColor, OfficeColor? backgroundColor, int pixelSize) {
        if (pixelSize <= 0) {
            throw new ArgumentOutOfRangeException(nameof(pixelSize));
        }
        return new QrRenderOptions {
            ModuleSize = pixelSize,
            Foreground = ToCodeGlyphXColor(foregroundColor ?? OfficeColor.Black),
            Background = ToCodeGlyphXColor(transparent ? OfficeColor.Transparent : backgroundColor ?? OfficeColor.White)
        };
    }

    private static CodeGlyphXRgba32 ToCodeGlyphXColor(OfficeColor color) {
        return new CodeGlyphXRgba32(color.R, color.G, color.B, color.A);
    }

    private static string ResolveValidatedOutputPath(string filePath) {
        string fullPath = Helpers.ResolvePath(filePath);
        if (string.IsNullOrWhiteSpace(Path.GetExtension(fullPath))) {
            throw new NotSupportedException("Specify an output image extension, for example .png or .svg.");
        }
        Helpers.CreateParentDirectory(fullPath);
        return fullPath;
    }

    private static CodeGlyphX.QrCode EncodeQr(QrPayloadData payload, QrRenderOptions options, QrErrorCorrectionLevel? errorCorrectionLevel) {
        // Resolve payload recommendations in the owner before applying the caller's ECC override.
        var builder = QR.Create(payload);
        if (errorCorrectionLevel.HasValue) {
            builder.WithErrorCorrection(errorCorrectionLevel.Value);
        }
        var symbol = builder.Encode();
        if (options.LogoPng is not null) {
            // Preserve the one-eighth-of-canvas logo bound while the owner uses symbol-relative scale.
            int symbolPixels = symbol.Size * options.ModuleSize;
            int canvasPixels = (symbol.Size + 2 * options.QuietZone) * options.ModuleSize;
            int logoPixels = Math.Max(1, canvasPixels / 8);
            // Normalize with the same resampler as the image API; the owner then composites at 1:1.
            using var logo = Image.Load(options.LogoPng);
            double scale = Math.Min(logoPixels / (double)logo.Width, logoPixels / (double)logo.Height);
            var normalized = OfficeRasterResampler.Resize(logo.Raster,
                Math.Max(1, (int)Math.Round(logo.Width * scale)),
                Math.Max(1, (int)Math.Round(logo.Height * scale)),
                OfficeRasterResamplingMode.Bicubic);
            options.LogoPng = OfficeRasterImageEncoder.Encode(normalized, OfficeImageExportFormat.Png);
            options.LogoScale = logoPixels / (double)symbolPixels;
            options.LogoPaddingPx = 0;
            options.LogoCornerRadiusPx = 0;
            options.LogoDrawBackground = false;
        }
        return symbol;
    }

    private static void RenderToFile(QrPayloadData payload, string filePath, QrRenderOptions options, QrErrorCorrectionLevel? errorCorrectionLevel = null) {
        var output = EncodeQr(payload, options, errorCorrectionLevel);
        output.Save(ResolveValidatedOutputPath(filePath), options);
    }

    private static async Task RenderToFileAsync(QrPayloadData payload, string filePath, QrRenderOptions options, CancellationToken cancellationToken, QrErrorCorrectionLevel? errorCorrectionLevel = null) {
        cancellationToken.ThrowIfCancellationRequested();
        string fullPath = ResolveValidatedOutputPath(filePath);
        var output = EncodeQr(payload, options, errorCorrectionLevel).Render(OutputFormatInfo.Resolve(fullPath, OutputFormat.Png), options);
        cancellationToken.ThrowIfCancellationRequested();
        await RenderIO.WriteBinaryAsync(fullPath, output.ToArray(), cancellationToken).ConfigureAwait(false);
    }

    private static void RenderToFileWithCenteredLogo(QrPayloadData payload, string filePath, string logoPath, QrRenderOptions options, QrErrorCorrectionLevel? errorCorrectionLevel = null) {
        options.LogoPng = LoadLogoPng(logoPath);
        RenderToFile(payload, filePath, options, errorCorrectionLevel);
    }

    private static async Task RenderToFileWithCenteredLogoAsync(QrPayloadData payload, string filePath, string logoPath, QrRenderOptions options, CancellationToken cancellationToken, QrErrorCorrectionLevel? errorCorrectionLevel = null) {
        cancellationToken.ThrowIfCancellationRequested();
        options.LogoPng = await LoadLogoPngAsync(logoPath, cancellationToken).ConfigureAwait(false);
        await RenderToFileAsync(payload, filePath, options, cancellationToken, errorCorrectionLevel).ConfigureAwait(false);
    }

    private static byte[] LoadLogoPng(string logoPath) {
        using var logo = Image.Load(Helpers.ResolvePath(logoPath));
        return OfficeRasterImageEncoder.Encode(logo.Raster, OfficeImageExportFormat.Png);
    }

    private static async Task<byte[]> LoadLogoPngAsync(string logoPath, CancellationToken cancellationToken) {
        using var logo = await Image.LoadAsync(Helpers.ResolvePath(logoPath), cancellationToken).ConfigureAwait(false);
        return OfficeRasterImageEncoder.Encode(logo.Raster, OfficeImageExportFormat.Png, options: null,
            maximumEncodedBytes: 128L * 1024L * 1024L, cancellationToken: cancellationToken);
    }
}

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CodeGlyphX;
using CodeGlyphX.Payloads;
using CodeGlyphX.Rendering;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Rgba32 = SixLabors.ImageSharp.PixelFormats.Rgba32;
using CodeGlyphXRgba32 = CodeGlyphX.Rendering.Rgba32;

namespace ImagePlayground;

public partial class QrCode {
    private static QrRenderOptions BuildOptions(bool transparent, Color? foregroundColor, Color? backgroundColor, int pixelSize) {
        if (pixelSize <= 0) {
            throw new ArgumentOutOfRangeException(nameof(pixelSize));
        }
        return new QrRenderOptions {
            ModuleSize = pixelSize,
            Foreground = ToCodeGlyphXColor(foregroundColor ?? Color.Black),
            Background = ToCodeGlyphXColor(transparent ? Color.Transparent : backgroundColor ?? Color.White)
        };
    }

    private static CodeGlyphXRgba32 ToCodeGlyphXColor(Color color) {
        var pixel = color.ToPixel<Rgba32>();
        return new CodeGlyphXRgba32(pixel.R, pixel.G, pixel.B, pixel.A);
    }

    private static string ResolveValidatedOutputPath(string filePath) {
        string fullPath = Helpers.ResolvePath(filePath);
        if (string.IsNullOrWhiteSpace(Path.GetExtension(fullPath))) {
            throw new UnknownImageFormatException("Specify an output image extension, for example .png or .svg.");
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
            using var logo = SixLabors.ImageSharp.Image.Load<Rgba32>(options.LogoPng);
            logo.Mutate(context => context.Resize(new ResizeOptions {
                Mode = ResizeMode.Max,
                Size = new Size(logoPixels, logoPixels)
            }));
            using var stream = new MemoryStream();
            logo.SaveAsPng(stream);
            options.LogoPng = stream.ToArray();
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
        using var logo = SixLabors.ImageSharp.Image.Load<Rgba32>(Helpers.ResolvePath(logoPath));
        using var stream = new MemoryStream();
        logo.SaveAsPng(stream);
        return stream.ToArray();
    }

    private static async Task<byte[]> LoadLogoPngAsync(string logoPath, CancellationToken cancellationToken) {
        using var logo = await SixLabors.ImageSharp.Image.LoadAsync<Rgba32>(Helpers.ResolvePath(logoPath), cancellationToken).ConfigureAwait(false);
        using var stream = new MemoryStream();
        await logo.SaveAsPngAsync(stream, cancellationToken).ConfigureAwait(false);
        return stream.ToArray();
    }
}

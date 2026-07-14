using System.Threading;
using System.Threading.Tasks;
using CodeGlyphX;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using CodeGlyphXPixelFormat = CodeGlyphX.PixelFormat;

namespace ImagePlayground;

public partial class QrCode {
    private const int DefaultDecodePassBudgetMilliseconds = 5000;
    private const int DefaultDecodeMaxDimension = 1600;

    /// <summary>
    /// Reads a QR code image and returns the decoded payload.
    /// </summary>
    /// <para>
    /// Uses a bounded two-stage CodeGlyphX decode: a fast upright pass followed by a robust transform pass.
    /// Pass explicit <see cref="QrPixelDecodeOptions"/> to select a different speed, accuracy, or stylized-code profile.
    /// </para>
    /// <param name="filePath">Path to the QR code image.</param>
    /// <returns>The CodeGlyphX decoded QR payload, or <see langword="null"/> when no QR code is found.</returns>
    /// <example>
    ///   <code>var result = QrCode.Read("code.png");</code>
    /// </example>
    public static QrDecoded? Read(string filePath) {
        return Read(filePath, decodeOptions: null);
    }

    /// <summary>
    /// Reads a QR code image with caller-selected CodeGlyphX decode options.
    /// </summary>
    /// <param name="filePath">Path to the QR code image.</param>
    /// <param name="decodeOptions">
    /// CodeGlyphX QR decode options. Pass <see langword="null"/> to use the bounded two-stage default.
    /// </param>
    /// <param name="cancellationToken">Cancellation token used to abort image loading and recognition.</param>
    /// <returns>The CodeGlyphX decoded QR payload, or <see langword="null"/> when no QR code is found.</returns>
    public static QrDecoded? Read(string filePath, QrPixelDecodeOptions? decodeOptions, CancellationToken cancellationToken = default) {
        return ReadAsync(filePath, cancellationToken, decodeOptions).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Reads a QR code image and returns the decoded payload asynchronously.
    /// </summary>
    /// <param name="filePath">Path to the QR code image.</param>
    /// <param name="cancellationToken">Cancellation token used to abort image loading and recognition.</param>
    /// <returns>The CodeGlyphX decoded QR payload, or <see langword="null"/> when no QR code is found.</returns>
    public static Task<QrDecoded?> ReadAsync(string filePath, CancellationToken cancellationToken = default) {
        return ReadAsync(filePath, cancellationToken, decodeOptions: null);
    }

    /// <summary>
    /// Reads a QR code image asynchronously with caller-selected CodeGlyphX decode options.
    /// </summary>
    /// <param name="filePath">Path to the QR code image.</param>
    /// <param name="cancellationToken">Cancellation token used to abort image loading and recognition.</param>
    /// <param name="decodeOptions">
    /// CodeGlyphX QR decode options. Pass <see langword="null"/> to use the bounded two-stage default.
    /// </param>
    /// <returns>The CodeGlyphX decoded QR payload, or <see langword="null"/> when no QR code is found.</returns>
    public static async Task<QrDecoded?> ReadAsync(string filePath, CancellationToken cancellationToken, QrPixelDecodeOptions? decodeOptions) {
        cancellationToken.ThrowIfCancellationRequested();
        string fullPath = Helpers.ResolvePath(filePath);
        using Image<Rgba32> image = await SixLabors.ImageSharp.Image.LoadAsync<Rgba32>(fullPath, cancellationToken).ConfigureAwait(false);
        byte[] pixels = new byte[image.Width * image.Height * 4];
        image.CopyPixelDataTo(pixels);

        cancellationToken.ThrowIfCancellationRequested();
        if (TryDecodePixels(pixels, image.Width, image.Height, decodeOptions, cancellationToken, out var decoded)) {
            return decoded;
        }

        cancellationToken.ThrowIfCancellationRequested();
        return null;
    }

    private static bool TryDecodePixels(
        byte[] pixels,
        int width,
        int height,
        QrPixelDecodeOptions? options,
        CancellationToken cancellationToken,
        out QrDecoded decoded) {
        if (options is not null) {
            return TryDecodePixelsCore(pixels, width, height, options, cancellationToken, out decoded);
        }

        QrPixelDecodeOptions upright = QrPixelDecodeOptions.Fast();
        upright.BudgetMilliseconds = DefaultDecodePassBudgetMilliseconds;
        upright.MaxDimension = DefaultDecodeMaxDimension;
        upright.DisableTransforms = true;
        if (TryDecodePixelsCore(pixels, width, height, upright, cancellationToken, out decoded)) {
            return true;
        }

        cancellationToken.ThrowIfCancellationRequested();
        QrPixelDecodeOptions robust = QrPixelDecodeOptions.Robust();
        robust.BudgetMilliseconds = DefaultDecodePassBudgetMilliseconds;
        robust.MaxDimension = DefaultDecodeMaxDimension;
        return TryDecodePixelsCore(pixels, width, height, robust, cancellationToken, out decoded);
    }

    private static bool TryDecodePixelsCore(
        byte[] pixels,
        int width,
        int height,
        QrPixelDecodeOptions options,
        CancellationToken cancellationToken,
        out QrDecoded decoded) {
        return QrImageDecoder.TryDecode(
            pixels,
            width,
            height,
            width * 4,
            CodeGlyphXPixelFormat.Rgba32,
            options,
            cancellationToken,
            out decoded);
    }
}

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CodeGlyphX;
using CodeGlyphX.DataMatrix;
using CodeGlyphX.Pdf417;
using CodeGlyphX.Code39;
using CodeGlyphX.Code93;
using CodeGlyphX.Rendering.Png;
using CodeGlyphX.UpcE;
using SixLabors.ImageSharp;
using ImageSharpRgba32 = SixLabors.ImageSharp.PixelFormats.Rgba32;

namespace ImagePlayground;
/// <summary>
/// Helper methods for generating and reading barcodes.
/// <para>
/// This class exposes convenience wrappers around CodeGlyphX for generating 1D and 2D barcodes and for
/// decoding barcode images back into text.
/// </para>
/// <para>
/// Output format is inferred from the destination file extension, while decode methods return
/// <see cref="BarcodeResult{TPixel}"/> instances that clearly distinguish found, not found, and error states.
/// </para>
/// </summary>
public class BarCode {
    /// <summary>
    /// Saves a PNG buffer to disk, converting to the requested file extension.
    /// </summary>
    /// <param name="pngBytes">PNG bytes.</param>
    /// <param name="filePath">Destination file path.</param>
    private static void SaveToFile(byte[] pngBytes, string filePath) {
        if (pngBytes is null) {
            throw new ArgumentNullException(nameof(pngBytes));
        }

        string fullPath = Helpers.ResolvePath(filePath);
        Helpers.CreateParentDirectory(fullPath);
        string extension = Path.GetExtension(fullPath).ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(extension)) {
            throw new UnknownImageFormatException(
                $"Image format not supported. Supported extensions: {string.Join(", ", Helpers.SupportedExtensions)}");
        }

        if (extension == ".png") {
            File.WriteAllBytes(fullPath, pngBytes);
            return;
        }

        using Image<ImageSharpRgba32> image = SixLabors.ImageSharp.Image.Load<ImageSharpRgba32>(pngBytes);
        image.Save(fullPath, Helpers.GetEncoder(extension, null, null));
    }

    /// <summary>
    /// Saves a PNG buffer to disk asynchronously, converting to the requested file extension.
    /// </summary>
    /// <param name="pngBytes">PNG bytes.</param>
    /// <param name="filePath">Destination file path.</param>
    /// <param name="cancellationToken">Cancellation token used to abort save operations.</param>
    private static async Task SaveToFileAsync(byte[] pngBytes, string filePath, CancellationToken cancellationToken = default) {
        if (pngBytes is null) {
            throw new ArgumentNullException(nameof(pngBytes));
        }

        cancellationToken.ThrowIfCancellationRequested();
        string fullPath = Helpers.ResolvePath(filePath);
        Helpers.CreateParentDirectory(fullPath);
        string extension = Path.GetExtension(fullPath).ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(extension)) {
            throw new UnknownImageFormatException(
                $"Image format not supported. Supported extensions: {string.Join(", ", Helpers.SupportedExtensions)}");
        }

        if (extension == ".png") {
            await WriteAllBytesAsync(fullPath, pngBytes, cancellationToken).ConfigureAwait(false);
            return;
        }

        if (extension == ".ico") {
            string tempPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.png");
            try {
                await WriteAllBytesAsync(tempPath, pngBytes, cancellationToken).ConfigureAwait(false);
                using var wrappedImage = Image.Load(tempPath);
                wrappedImage.SaveAsIcon(fullPath);
            } finally {
                if (File.Exists(tempPath)) {
                    File.Delete(tempPath);
                }
            }
            return;
        }

        using MemoryStream input = new(pngBytes, writable: false);
        using Image<ImageSharpRgba32> image = await SixLabors.ImageSharp.Image.LoadAsync<ImageSharpRgba32>(input, cancellationToken).ConfigureAwait(false);
        using FileStream output = File.Create(fullPath);
        await image.SaveAsync(output, Helpers.GetEncoder(extension, null, null), cancellationToken).ConfigureAwait(false);
    }

    private static async Task WriteAllBytesAsync(string filePath, byte[] bytes, CancellationToken cancellationToken) {
        using FileStream stream = new(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 81920, useAsync: true);
        await stream.WriteAsync(bytes, 0, bytes.Length, cancellationToken).ConfigureAwait(false);
    }

    private static CodeGlyphX.BarcodeType MapType(BarcodeType type) {
        return type switch {
            BarcodeType.Code128 => CodeGlyphX.BarcodeType.Code128,
            BarcodeType.Code93 => CodeGlyphX.BarcodeType.Code93,
            BarcodeType.Code39 => CodeGlyphX.BarcodeType.Code39,
            BarcodeType.KixCode => CodeGlyphX.BarcodeType.KixCode,
            BarcodeType.UPCA => CodeGlyphX.BarcodeType.UPCA,
            BarcodeType.UPCE => CodeGlyphX.BarcodeType.UPCE,
            BarcodeType.EAN => CodeGlyphX.BarcodeType.EAN,
            BarcodeType.DataMatrix => CodeGlyphX.BarcodeType.DataMatrix,
            BarcodeType.PDF417 => CodeGlyphX.BarcodeType.PDF417,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unsupported barcode type.")
        };
    }

    private static byte[] Render1DPng(Barcode1D barcode) {
        var options = new BarcodePngRenderOptions();
        return BarcodePngRenderer.Render(barcode, options);
    }

    private static byte[] Render1DPng(BarcodeType type, string content) {
        CodeGlyphX.BarcodeType mapped = MapType(type);
        return Render1DPng(BarcodeEncoder.Encode(mapped, content));
    }

    private static byte[] RenderMatrixPng(BarcodeType type, string content) {
        CodeGlyphX.BarcodeType mapped = MapType(type);
        var modules = MatrixBarcodeEncoder.Encode(mapped, content);
        var options = new MatrixPngRenderOptions();
        return MatrixPngRenderer.Render(modules, options);
    }

    /// <summary>Generates a QR code.</summary>
    /// <para>Use this helper when QR code generation is needed from the barcode-focused API surface.</para>
    /// <param name="content">Value encoded in the QR code.</param>
    /// <param name="filePath">Destination file path. The image format is inferred from the extension.</param>
    /// <param name="errorCorrectionLevel">Error correction level used during QR encoding.</param>
    /// <param name="encoding">Optional text encoding override for payload serialization.</param>
    /// <example>
    ///   <code>BarCode.GenerateQr("https://evotec.xyz", "barcode-qr.png");</code>
    /// </example>
    public static void GenerateQr(string content, string filePath, QrErrorCorrectionLevel errorCorrectionLevel = QrErrorCorrectionLevel.H, QrTextEncoding? encoding = null) {
        var options = new QrEasyOptions {
            ErrorCorrectionLevel = errorCorrectionLevel
        };
        if (encoding.HasValue) {
            options.TextEncoding = encoding;
        }

        string fullPath = Helpers.ResolvePath(filePath);
        Helpers.CreateParentDirectory(fullPath);
        CodeGlyphX.QrCode.Save(content, fullPath, options);
    }

    /// <summary>Generates a QR code asynchronously.</summary>
    /// <para>Uses asynchronous file I/O while keeping the encode step cancellation-aware.</para>
    /// <param name="content">Value encoded in the QR code.</param>
    /// <param name="filePath">Destination file path. The image format is inferred from the extension.</param>
    /// <param name="errorCorrectionLevel">Error correction level used during QR encoding.</param>
    /// <param name="encoding">Optional text encoding override for payload serialization.</param>
    /// <param name="cancellationToken">Cancellation token used to abort save operations.</param>
    public static async Task GenerateQrAsync(string content, string filePath, QrErrorCorrectionLevel errorCorrectionLevel = QrErrorCorrectionLevel.H, QrTextEncoding? encoding = null, CancellationToken cancellationToken = default) {
        cancellationToken.ThrowIfCancellationRequested();
        var options = new QrEasyOptions {
            ErrorCorrectionLevel = errorCorrectionLevel
        };
        if (encoding.HasValue) {
            options.TextEncoding = encoding;
        }

        byte[] pngBytes = CodeGlyphX.QrCode.Render(content, CodeGlyphX.Rendering.OutputFormat.Png, options).Data;
        cancellationToken.ThrowIfCancellationRequested();
        await SaveToFileAsync(pngBytes, filePath, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Generates an EAN barcode.</summary>
    /// <param name="content">Value encoded in the barcode.</param>
    /// <param name="filePath">Destination file path.</param>
    public static void GenerateEan(string content, string filePath) {
        SaveToFile(Render1DPng(BarcodeType.EAN, content), filePath);
    }

    /// <summary>Generates a Code 128 barcode.</summary>
    /// <param name="content">Value encoded in the barcode.</param>
    /// <param name="filePath">Destination file path. The image format is inferred from the extension.</param>
    /// <param name="includeChecksum">Reserved compatibility flag. CodeGlyphX always includes the required checksum.</param>
    public static void GenerateCode128(string content, string filePath, bool includeChecksum = true) {
        if (!includeChecksum) {
            throw new NotSupportedException("Code128 generation via CodeGlyphX always includes the required checksum.");
        }

        SaveToFile(Render1DPng(BarcodeType.Code128, content), filePath);
    }

    /// <summary>Generates a Code 93 barcode.</summary>
    public static void GenerateCode93(string content, string filePath, bool includeChecksum = true, bool fullAsciiMode = false) {
        var barcode = Code93Encoder.Encode(content, includeChecksum, fullAsciiMode);
        SaveToFile(Render1DPng(barcode), filePath);
    }

    /// <summary>Generates a Code 39 barcode.</summary>
    public static void GenerateCode39(string content, string filePath, bool includeChecksum = true, bool fullAsciiMode = false) {
        var barcode = Code39Encoder.Encode(content, includeChecksum, fullAsciiMode);
        SaveToFile(Render1DPng(barcode), filePath);
    }

    /// <summary>Generates a UPC-E barcode.</summary>
    public static void GenerateUpcE(string content, string filePath, UpcENumberSystem upcNumberSystem = UpcENumberSystem.Zero) {
        var barcode = UpcEEncoder.Encode(content, upcNumberSystem);
        SaveToFile(Render1DPng(barcode), filePath);
    }

    /// <summary>Generates a UPC-A barcode.</summary>
    public static void GenerateUpcA(string content, string filePath) {
        SaveToFile(Render1DPng(BarcodeType.UPCA, content), filePath);
    }

    /// <summary>Generates a KIX barcode.</summary>
    public static void GenerateKix(string content, string filePath) {
        SaveToFile(RenderMatrixPng(BarcodeType.KixCode, content), filePath);
    }

    /// <summary>Generates a Data Matrix barcode.</summary>
    public static void GenerateDataMatrix(string content, string filePath) {
        SaveToFile(RenderMatrixPng(BarcodeType.DataMatrix, content), filePath);
    }

    /// <summary>Generates a PDF417 barcode.</summary>
    public static void GeneratePdf417(string content, string filePath) {
        SaveToFile(RenderMatrixPng(BarcodeType.PDF417, content), filePath);
    }

    /// <summary>
     /// Dispatches barcode generation based on <paramref name="barcodeType"/>.
    /// </summary>
    /// <param name="barcodeType">Barcode symbology to generate.</param>
    /// <param name="content">Value encoded in the barcode.</param>
    /// <param name="filePath">Destination file path. The image format is inferred from the extension.</param>
    public static void Generate(BarcodeType barcodeType, string content, string filePath) {
        Action? generator = barcodeType switch {
            BarcodeType.Code128 => () => GenerateCode128(content, filePath),
            BarcodeType.Code93 => () => GenerateCode93(content, filePath),
            BarcodeType.Code39 => () => GenerateCode39(content, filePath),
            BarcodeType.KixCode => () => GenerateKix(content, filePath),
            BarcodeType.UPCA => () => GenerateUpcA(content, filePath),
            BarcodeType.UPCE => () => GenerateUpcE(content, filePath),
            BarcodeType.EAN => () => GenerateEan(content, filePath),
            BarcodeType.DataMatrix => () => GenerateDataMatrix(content, filePath),
            BarcodeType.PDF417 => () => GeneratePdf417(content, filePath),
            _ => null
        };
        if (generator is null) {
            throw new ArgumentOutOfRangeException(nameof(barcodeType), barcodeType, "Unsupported barcode type.");
        }
        generator();
    }

    /// <summary>
    /// Dispatches barcode generation asynchronously based on <paramref name="barcodeType"/>.
    /// </summary>
    /// <param name="barcodeType">Barcode symbology to generate.</param>
    /// <param name="content">Value encoded in the barcode.</param>
    /// <param name="filePath">Destination file path. The image format is inferred from the extension.</param>
    /// <param name="cancellationToken">Cancellation token used to abort save operations.</param>
    public static async Task GenerateAsync(BarcodeType barcodeType, string content, string filePath, CancellationToken cancellationToken = default) {
        cancellationToken.ThrowIfCancellationRequested();
        byte[] pngBytes = barcodeType switch {
            BarcodeType.Code128 => Render1DPng(BarcodeType.Code128, content),
            BarcodeType.Code93 => Render1DPng(Code93Encoder.Encode(content, includeChecksum: true, fullAsciiMode: false)),
            BarcodeType.Code39 => Render1DPng(Code39Encoder.Encode(content, includeChecksum: true, fullAsciiMode: false)),
            BarcodeType.KixCode => RenderMatrixPng(BarcodeType.KixCode, content),
            BarcodeType.UPCA => Render1DPng(BarcodeType.UPCA, content),
            BarcodeType.UPCE => Render1DPng(UpcEEncoder.Encode(content, UpcENumberSystem.Zero)),
            BarcodeType.EAN => Render1DPng(BarcodeType.EAN, content),
            BarcodeType.DataMatrix => RenderMatrixPng(BarcodeType.DataMatrix, content),
            BarcodeType.PDF417 => RenderMatrixPng(BarcodeType.PDF417, content),
            _ => throw new ArgumentOutOfRangeException(nameof(barcodeType), barcodeType, "Unsupported barcode type.")
        };

        cancellationToken.ThrowIfCancellationRequested();
        await SaveToFileAsync(pngBytes, filePath, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Reads and decodes a barcode from an image asynchronously.
    /// </summary>
    /// <para>The decoder attempts Data Matrix, PDF417, and standard 1D barcode recognition in sequence.</para>
    /// <param name="filePath">Path to the barcode image.</param>
    /// <param name="cancellationToken">Cancellation token used to abort image loading or decode work.</param>
    /// <returns>A task containing the decoded barcode result.</returns>
    /// <example>
    ///   <code>var result = await BarCode.ReadAsync("code128.png");</code>
    /// </example>
    public static async Task<BarcodeResult<ImageSharpRgba32>> ReadAsync(string filePath, CancellationToken cancellationToken = default) {
        cancellationToken.ThrowIfCancellationRequested();
        string fullPath = Helpers.ResolvePath(filePath);

        using Image<ImageSharpRgba32> barcodeImage = await SixLabors.ImageSharp.Image.LoadAsync<ImageSharpRgba32>(fullPath, cancellationToken).ConfigureAwait(false);
        byte[] pixels = new byte[barcodeImage.Width * barcodeImage.Height * 4];
        barcodeImage.CopyPixelDataTo(pixels);

        cancellationToken.ThrowIfCancellationRequested();
        if (DataMatrixDecoder.TryDecode(pixels, barcodeImage.Width, barcodeImage.Height, barcodeImage.Width * 4, PixelFormat.Rgba32, out var dataMatrix)) {
            return new BarcodeResult<ImageSharpRgba32> {
                Value = dataMatrix,
                Status = Status.Found,
                Message = dataMatrix
            };
        }

        cancellationToken.ThrowIfCancellationRequested();
        string pdf417;
        if (Pdf417Decoder.TryDecode(pixels, barcodeImage.Width, barcodeImage.Height, barcodeImage.Width * 4, PixelFormat.Rgba32, out pdf417)) {
            return new BarcodeResult<ImageSharpRgba32> {
                Value = pdf417,
                Status = Status.Found,
                Message = pdf417
            };
        }

        cancellationToken.ThrowIfCancellationRequested();
        if (BarcodeDecoder.TryDecode(pixels, barcodeImage.Width, barcodeImage.Height, barcodeImage.Width * 4, PixelFormat.Rgba32, out var decoded)) {
            return new BarcodeResult<ImageSharpRgba32> {
                Value = decoded.Text,
                Status = Status.Found,
                Message = decoded.Text
            };
        }

        return new BarcodeResult<ImageSharpRgba32> {
            Status = Status.NotFound
        };
    }

    /// <summary>
    /// Reads and decodes a barcode from an image.
    /// </summary>
    /// <para>This is the synchronous wrapper over <see cref="ReadAsync(string, CancellationToken)"/>.</para>
    /// <param name="filePath">Path to the barcode image.</param>
    /// <returns>Decoded barcode result.</returns>
    /// <example>
    ///   <code>var result = BarCode.Read("code128.png");</code>
    /// </example>
    public static BarcodeResult<ImageSharpRgba32> Read(string filePath) {
        return ReadAsync(filePath).GetAwaiter().GetResult();
    }
}

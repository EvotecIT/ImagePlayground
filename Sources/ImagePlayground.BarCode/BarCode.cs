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
    /// <param name="content">Value encoded in the QR code.</param>
    /// <param name="filePath">Destination file path.</param>
    /// <param name="errorCorrectionLevel">Error correction level.</param>
    /// <param name="encoding">Optional text encoding override.</param>
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

    /// <summary>Generates an EAN barcode.</summary>
    /// <param name="content">Value encoded in the barcode.</param>
    /// <param name="filePath">Destination file path.</param>
    public static void GenerateEan(string content, string filePath) {
        SaveToFile(Render1DPng(BarcodeType.EAN, content), filePath);
    }

    /// <summary>Generates a Code 128 barcode.</summary>
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
    /// Reads and decodes a barcode from an image asynchronously.
    /// </summary>
    /// <param name="filePath">Path to the barcode image.</param>
    /// <returns>A task containing the decoded barcode result.</returns>
    public static async Task<BarcodeResult<ImageSharpRgba32>> ReadAsync(string filePath, CancellationToken cancellationToken = default) {
        cancellationToken.ThrowIfCancellationRequested();
        string fullPath = Helpers.ResolvePath(filePath);

        using Image<ImageSharpRgba32> barcodeImage = await SixLabors.ImageSharp.Image.LoadAsync<ImageSharpRgba32>(fullPath, cancellationToken).ConfigureAwait(false);
        byte[] pixels = new byte[barcodeImage.Width * barcodeImage.Height * 4];
        barcodeImage.CopyPixelDataTo(pixels);

        if (DataMatrixDecoder.TryDecode(pixels, barcodeImage.Width, barcodeImage.Height, barcodeImage.Width * 4, PixelFormat.Rgba32, out var dataMatrix)) {
            return new BarcodeResult<ImageSharpRgba32> {
                Value = dataMatrix,
                Status = Status.Found,
                Message = dataMatrix
            };
        }

        string pdf417;
        if (Pdf417Decoder.TryDecode(pixels, barcodeImage.Width, barcodeImage.Height, barcodeImage.Width * 4, PixelFormat.Rgba32, out pdf417)) {
            return new BarcodeResult<ImageSharpRgba32> {
                Value = pdf417,
                Status = Status.Found,
                Message = pdf417
            };
        }

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
    /// <param name="filePath">Path to the barcode image.</param>
    /// <returns>Decoded barcode result.</returns>
    public static BarcodeResult<ImageSharpRgba32> Read(string filePath) {
        return ReadAsync(filePath).GetAwaiter().GetResult();
    }
}

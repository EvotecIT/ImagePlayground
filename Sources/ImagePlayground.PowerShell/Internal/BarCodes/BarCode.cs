using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CodeGlyphX;
using CodeGlyphX.Rendering;
using CodeGlyphX.UpcE;
using SixLabors.ImageSharp;
using ImageSharpRgba32 = SixLabors.ImageSharp.PixelFormats.Rgba32;

namespace ImagePlayground;

/// <summary>
/// PowerShell-facing barcode helpers backed by CodeGlyphX.
/// </summary>
public class BarCode {
    /// <summary>Generates a QR code.</summary>
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
    public static Task GenerateQrAsync(string content, string filePath, QrErrorCorrectionLevel errorCorrectionLevel = QrErrorCorrectionLevel.H, QrTextEncoding? encoding = null, CancellationToken cancellationToken = default) {
        cancellationToken.ThrowIfCancellationRequested();
        GenerateQr(content, filePath, errorCorrectionLevel, encoding);
        return Task.CompletedTask;
    }

    /// <summary>Generates an EAN barcode.</summary>
    public static void GenerateEan(string content, string filePath) {
        Generate(BarcodeType.EAN, content, filePath);
    }

    /// <summary>Generates a Code 128 barcode.</summary>
    public static void GenerateCode128(string content, string filePath, bool includeChecksum = true) {
        if (!includeChecksum) {
            throw new NotSupportedException("Code128 generation via CodeGlyphX always includes the required checksum.");
        }

        Generate(BarcodeType.Code128, content, filePath);
    }

    /// <summary>Generates a Code 93 barcode.</summary>
    public static void GenerateCode93(string content, string filePath, bool includeChecksum = true, bool fullAsciiMode = false) {
        var barcode = BarcodeEncoder.EncodeCode93(content, includeChecksum, fullAsciiMode);
        SaveBarcode(barcode, filePath);
    }

    /// <summary>Generates a Code 39 barcode.</summary>
    public static void GenerateCode39(string content, string filePath, bool includeChecksum = true, bool fullAsciiMode = false) {
        var barcode = BarcodeEncoder.EncodeCode39(content, includeChecksum, fullAsciiMode);
        SaveBarcode(barcode, filePath);
    }

    /// <summary>Generates a UPC-E barcode.</summary>
    public static void GenerateUpcE(string content, string filePath, UpcENumberSystem upcNumberSystem = UpcENumberSystem.Zero) {
        var barcode = BarcodeEncoder.EncodeUpcE(content, upcNumberSystem);
        SaveBarcode(barcode, filePath);
    }

    /// <summary>Generates a UPC-A barcode.</summary>
    public static void GenerateUpcA(string content, string filePath) {
        Generate(BarcodeType.UPCA, content, filePath);
    }

    /// <summary>Generates a KIX barcode.</summary>
    public static void GenerateKix(string content, string filePath) {
        Generate(BarcodeType.KixCode, content, filePath);
    }

    /// <summary>Generates a Data Matrix barcode.</summary>
    public static void GenerateDataMatrix(string content, string filePath) {
        Generate(BarcodeType.DataMatrix, content, filePath);
    }

    /// <summary>Generates a PDF417 barcode.</summary>
    public static void GeneratePdf417(string content, string filePath) {
        Generate(BarcodeType.PDF417, content, filePath);
    }

    /// <summary>
    /// Dispatches barcode generation based on <paramref name="barcodeType"/>.
    /// </summary>
    public static void Generate(BarcodeType barcodeType, string content, string filePath) {
        if (!Enum.IsDefined(typeof(BarcodeType), barcodeType)) {
            throw new ArgumentOutOfRangeException(nameof(barcodeType), barcodeType, "Unsupported barcode type.");
        }

        string fullPath = Helpers.ResolvePath(filePath);
        Helpers.CreateParentDirectory(fullPath);
        if (IsMatrixBarcode(barcodeType)) {
            MatrixBarcode.Save(barcodeType, content, fullPath);
            return;
        }

        Barcode.Save(barcodeType, content, fullPath);
    }

    /// <summary>
    /// Dispatches barcode generation asynchronously based on <paramref name="barcodeType"/>.
    /// </summary>
    public static Task GenerateAsync(BarcodeType barcodeType, string content, string filePath, CancellationToken cancellationToken = default) {
        cancellationToken.ThrowIfCancellationRequested();
        Generate(barcodeType, content, filePath);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Reads and decodes a barcode from an image asynchronously.
    /// </summary>
    public static async Task<BarcodeResult<ImageSharpRgba32>> ReadAsync(string filePath, CancellationToken cancellationToken = default) {
        cancellationToken.ThrowIfCancellationRequested();
        string fullPath = Helpers.ResolvePath(filePath);
        byte[] imageBytes = File.ReadAllBytes(fullPath);

        cancellationToken.ThrowIfCancellationRequested();
        if (DataMatrixCode.TryDecodeImage(imageBytes, cancellationToken, out var dataMatrix)) {
            return new BarcodeResult<ImageSharpRgba32> {
                Value = dataMatrix,
                Status = Status.Found,
                Message = dataMatrix
            };
        }

        cancellationToken.ThrowIfCancellationRequested();
        if (Pdf417Code.TryDecodeImage(imageBytes, cancellationToken, out string pdf417)) {
            return new BarcodeResult<ImageSharpRgba32> {
                Value = pdf417,
                Status = Status.Found,
                Message = pdf417
            };
        }

        using Image<ImageSharpRgba32> barcodeImage = await SixLabors.ImageSharp.Image.LoadAsync<ImageSharpRgba32>(fullPath, cancellationToken).ConfigureAwait(false);
        byte[] pixels = new byte[barcodeImage.Width * barcodeImage.Height * 4];
        barcodeImage.CopyPixelDataTo(pixels);

        cancellationToken.ThrowIfCancellationRequested();
        if (BarcodeDecoder.TryDecode(pixels, barcodeImage.Width, barcodeImage.Height, barcodeImage.Width * 4, CodeGlyphX.PixelFormat.Rgba32, null, null, cancellationToken, out var decoded)) {
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
    public static BarcodeResult<ImageSharpRgba32> Read(string filePath) {
        return ReadAsync(filePath).GetAwaiter().GetResult();
    }

    private static void SaveBarcode(Barcode1D barcode, string filePath) {
        if (barcode is null) throw new ArgumentNullException(nameof(barcode));
        string fullPath = Helpers.ResolvePath(filePath);
        Helpers.CreateParentDirectory(fullPath);
        var format = OutputFormatInfo.Resolve(fullPath, OutputFormat.Png);
        var output = Barcode.Render(barcode, format);
        OutputWriter.Write(fullPath, output);
    }

    private static bool IsMatrixBarcode(BarcodeType barcodeType) {
        return barcodeType switch {
            BarcodeType.KixCode => true,
            BarcodeType.PharmacodeTwoTrack => true,
            BarcodeType.Postnet => true,
            BarcodeType.Planet => true,
            BarcodeType.RoyalMail4State => true,
            BarcodeType.AustraliaPost => true,
            BarcodeType.JapanPost => true,
            BarcodeType.UspsImb => true,
            BarcodeType.GS1DataBarOmni => true,
            BarcodeType.GS1DataBarStacked => true,
            BarcodeType.GS1DataBarExpandedStacked => true,
            BarcodeType.DataMatrix => true,
            BarcodeType.PDF417 => true,
            BarcodeType.MicroPDF417 => true,
            _ => false
        };
    }
}

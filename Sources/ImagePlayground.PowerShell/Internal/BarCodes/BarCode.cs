using System;
using System.Threading;
using System.Threading.Tasks;
using CodeGlyphX;
using CodeGlyphX.Rendering;
using CodeGlyphX.UpcE;

namespace ImagePlayground;

/// <summary>PowerShell-facing barcode helpers backed by CodeGlyphX.</summary>
public partial class BarCode {
    /// <summary>Generates a QR code with the selected encoding.</summary>
    public static void GenerateQr(string content, string filePath, QrErrorCorrectionLevel errorCorrectionLevel = QrErrorCorrectionLevel.H, QrTextEncoding? encoding = null) {
        string fullPath = Helpers.ResolvePath(filePath);
        Helpers.CreateParentDirectory(fullPath);
        QR.Save(content, fullPath, encodingOptions: new QrEncodingOptions {
            ErrorCorrectionLevel = errorCorrectionLevel,
            TextEncoding = encoding ?? QrTextEncoding.Utf8
        });
    }

    /// <summary>Generates a QR code asynchronously.</summary>
    public static Task GenerateQrAsync(string content, string filePath, QrErrorCorrectionLevel errorCorrectionLevel = QrErrorCorrectionLevel.H, QrTextEncoding? encoding = null, CancellationToken cancellationToken = default)
        => Task.Run(() => { cancellationToken.ThrowIfCancellationRequested(); GenerateQr(content, filePath, errorCorrectionLevel, encoding); }, cancellationToken);

    /// <summary>Generates an EAN barcode.</summary>
    public static void GenerateEan(string content, string filePath) => Generate(SymbolFormat.Ean, content, filePath);

    /// <summary>Generates a Code 128 barcode, including its required checksum.</summary>
    public static void GenerateCode128(string content, string filePath, bool includeChecksum = true) {
        if (!includeChecksum) {
            throw new NotSupportedException("Code128 generation always includes the required checksum.");
        }
        Generate(SymbolFormat.Code128, content, filePath);
    }

    /// <summary>Generates a Code 93 barcode.</summary>
    public static void GenerateCode93(string content, string filePath, bool includeChecksum = true, bool fullAsciiMode = false)
        => SaveBarcode(BarcodeEncoder.EncodeCode93(content, includeChecksum, fullAsciiMode), filePath);

    /// <summary>Generates a Code 39 barcode.</summary>
    public static void GenerateCode39(string content, string filePath, bool includeChecksum = true, bool fullAsciiMode = false)
        => SaveBarcode(BarcodeEncoder.EncodeCode39(content, includeChecksum, fullAsciiMode), filePath);

    /// <summary>Generates a UPC-E barcode.</summary>
    public static void GenerateUpcE(string content, string filePath, UpcENumberSystem upcNumberSystem = UpcENumberSystem.Zero)
        => SaveBarcode(BarcodeEncoder.EncodeUpcE(content, upcNumberSystem), filePath);

    /// <summary>Generates a UPC-A barcode.</summary>
    public static void GenerateUpcA(string content, string filePath) => Generate(SymbolFormat.UpcA, content, filePath);

    /// <summary>Generates a KIX barcode.</summary>
    public static void GenerateKix(string content, string filePath) => Generate(SymbolFormat.KixCode, content, filePath);

    /// <summary>Generates a Data Matrix barcode.</summary>
    public static void GenerateDataMatrix(string content, string filePath) => Generate(SymbolFormat.DataMatrix, content, filePath);

    /// <summary>Generates a PDF417 barcode.</summary>
    public static void GeneratePdf417(string content, string filePath) => Generate(SymbolFormat.Pdf417, content, filePath);

    /// <summary>Generates a linear, matrix, stacked, or postal barcode using the owner's format catalogue.</summary>
    /// <remarks>QR formats use QrCode.Generate. MaxiCode needs specialized geometry; GS1 Composite needs separate payloads and is not supported by this single-value helper.</remarks>
    public static void Generate(SymbolFormat format, string content, string filePath) {
        if (!SymbolCapabilities.TryGet(format, out var capability)) {
            throw new ArgumentOutOfRangeException(nameof(format), format, "Unsupported symbol format.");
        }
        if (IsQrFormat(format)) {
            throw new NotSupportedException("Use QrCode.Generate for QR symbols.");
        }
        if (format == SymbolFormat.MaxiCode) {
            throw new NotSupportedException("MaxiCode requires a hexagonal geometry renderer and is not supported by this image helper.");
        }
        if (format == SymbolFormat.Gs1Composite) {
            throw new NotSupportedException("GS1 Composite requires separate linear and composite payloads; use the CodeGlyphX composite encoder.");
        }
        if (!capability.CanEncode) {
            throw new NotSupportedException($"{format} cannot be encoded.");
        }

        string fullPath = Helpers.ResolvePath(filePath);
        Helpers.CreateParentDirectory(fullPath);
        if (capability.Family == SymbolFamily.Linear) {
            Barcode.Save(format, content, fullPath);
        } else {
            MatrixBarcode.Save(format, content, fullPath);
        }
    }

    /// <summary>Generates a barcode asynchronously.</summary>
    public static Task GenerateAsync(SymbolFormat format, string content, string filePath, CancellationToken cancellationToken = default)
        => Task.Run(() => { cancellationToken.ThrowIfCancellationRequested(); Generate(format, content, filePath); }, cancellationToken);

    private static void SaveBarcode(Barcode1D barcode, string filePath) {
        string fullPath = Helpers.ResolvePath(filePath);
        Helpers.CreateParentDirectory(fullPath);
        OutputWriter.Write(fullPath, Barcode.Render(barcode, OutputFormatInfo.Resolve(fullPath, OutputFormat.Png)));
    }

    private static bool IsQrFormat(SymbolFormat format) => format is SymbolFormat.QrCode or SymbolFormat.MicroQrCode or SymbolFormat.RmQrCode;
}

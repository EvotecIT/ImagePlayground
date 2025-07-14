using System.IO;
using Barcoder;
using Barcoder.Code128;
using Barcoder.Code39;
using Barcoder.Code93;
using Barcoder.Ean;
using Barcoder.Kix;
using Barcoder.Qr;
using Barcoder.Renderer.Image;
using Barcoder.DataMatrix;
using Barcoder.UpcA;
using Barcoder.UpcE;
using BarcodeReader.ImageSharp;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using ZXing;
using ZXing.Common;
using Encoding = Barcoder.Qr.Encoding;

namespace ImagePlayground;
/// <summary>
/// Helper methods for generating and reading barcodes.
/// </summary>
public class BarCode {
    /// <summary>
    /// Saves the generated barcode image to disk.
    /// </summary>
    /// <param name="barcode">Barcode to render.</param>
    /// <param name="filePath">Destination file path.</param>
    private static void SaveToFile(IBarcode barcode, string filePath) {
        string fullPath = Helpers.ResolvePath(filePath);

        ImageFormat imageFormatDetected;
        FileInfo fileInfo = new FileInfo(fullPath);

        if (fileInfo.Extension == ".png") {
            imageFormatDetected = ImageFormat.Png;
        } else if (fileInfo.Extension == ".jpg" || fileInfo.Extension == ".jpeg") {
            imageFormatDetected = ImageFormat.Jpeg;
        } else if (fileInfo.Extension == ".bmp") {
            imageFormatDetected = ImageFormat.Bmp;
        } else {
            throw new UnknownImageFormatException("Image format not supported. Feel free to open an issue/fix it.");
        }

        var options = new ImageRendererOptions();
        options.ImageFormat = imageFormatDetected;
        var renderer = new ImageRenderer(options);
        //var renderer = new ImageRenderer(imageFormat: imageFormatDetected);

        using (var stream = new FileStream(fullPath, FileMode.Create)) {
            renderer.Render(barcode, stream);
        }
    }

    private static void SaveToFile(Image<Rgba32> image, string filePath) {
        string fullPath = Helpers.ResolvePath(filePath);
        FileInfo fileInfo = new FileInfo(fullPath);

        if (fileInfo.Extension == ".png") {
            image.SaveAsPng(fullPath);
        } else if (fileInfo.Extension == ".jpg" || fileInfo.Extension == ".jpeg") {
            image.SaveAsJpeg(fullPath);
        } else if (fileInfo.Extension == ".bmp") {
            image.SaveAsBmp(fullPath);
        } else {
            throw new UnknownImageFormatException("Image format not supported. Feel free to open an issue/fix it.");
        }
    }

    /// <summary>Generates a QR code.</summary>
    /// <param name="content">Value encoded in the QR code.</param>
    /// <param name="filePath">Destination file path.</param>
    /// <param name="errorCorrectionLevel">Error correction level.</param>
    /// <param name="encoding">Text encoding.</param>
    public static void GenerateQr(string content, string filePath, ErrorCorrectionLevel errorCorrectionLevel = ErrorCorrectionLevel.H, Encoding encoding = Encoding.Auto) {
        var barcode = QrEncoder.Encode(content, errorCorrectionLevel, encoding);
        SaveToFile(barcode, filePath);
    }

    /// <summary>Generates an EAN barcode.</summary>
    /// <param name="content">Value encoded in the barcode.</param>
    /// <param name="filePath">Destination file path.</param>
    public static void GenerateEan(string content, string filePath) {
        var barcode = EanEncoder.Encode(content);
        SaveToFile(barcode, filePath);
    }

    /// <summary>Generates a Code 128 barcode.</summary>
    public static void GenerateCode128(string content, string filePath, bool includeChecksum = true) {
        var barcode = Code128Encoder.Encode(content, includeChecksum);
        SaveToFile(barcode, filePath);
    }

    /// <summary>Generates a Code 93 barcode.</summary>
    public static void GenerateCode93(string content, string filePath, bool includeChecksum = true, bool fullAsciiMode = false) {
        var barcode = Code93Encoder.Encode(content, includeChecksum, fullAsciiMode);
        SaveToFile(barcode, filePath);
    }

    /// <summary>Generates a Code 39 barcode.</summary>
    public static void GenerateCode39(string content, string filePath, bool includeChecksum = true, bool fullAsciiMode = false) {
        var barcode = Code39Encoder.Encode(content, includeChecksum, fullAsciiMode);
        SaveToFile(barcode, filePath);
    }

    /// <summary>Generates a UPC-E barcode.</summary>
    public static void GenerateUpcE(string content, string filePath, UpcENumberSystem upcNumberSystem = UpcENumberSystem.Zero) {
        var barcode = UpcEEncoder.Encode(content, upcNumberSystem);
        SaveToFile(barcode, filePath);
    }

    /// <summary>Generates a UPC-A barcode.</summary>
    public static void GenerateUpcA(string content, string filePath) {
        var barcode = UpcAEncoder.Encode(content);
        SaveToFile(barcode, filePath);
    }

    /// <summary>Generates a KIX barcode.</summary>
    public static void GenerateKix(string content, string filePath) {
        var barcode = KixEncoder.Encode(content);
        SaveToFile(barcode, filePath);
    }

    /// <summary>Generates a Data Matrix barcode.</summary>
    public static void GenerateDataMatrix(string content, string filePath) {
        var barcode = DataMatrixEncoder.Encode(content);
        SaveToFile(barcode, filePath);
    }

    /// <summary>Generates a PDF417 barcode.</summary>
    public static void GeneratePdf417(string content, string filePath) {
        var writer = new ZXing.ImageSharp.BarcodeWriter<Rgba32> {
            Format = BarcodeFormat.PDF_417,
            Options = new EncodingOptions {
                Width = 300,
                Height = 150,
                Margin = 1
            }
        };
        using Image<Rgba32> image = writer.Write(content);
        SaveToFile(image, filePath);
    }

    /// <summary>
    /// Dispatches barcode generation based on <paramref name="barcodeType"/>.
    /// </summary>
    public static void Generate(BarcodeType barcodeType, string content, string filePath) {
        if (barcodeType == BarcodeType.Code128) {
            GenerateCode128(content, filePath);
        } else if (barcodeType == BarcodeType.Code93) {
            GenerateCode93(content, filePath);
        } else if (barcodeType == BarcodeType.Code39) {
            GenerateCode39(content, filePath);
        } else if (barcodeType == BarcodeType.KixCode) {
            GenerateKix(content, filePath);
        } else if (barcodeType == BarcodeType.UPCA) {
            GenerateUpcA(content, filePath);
        } else if (barcodeType == BarcodeType.UPCE) {
            GenerateUpcE(content, filePath);
        } else if (barcodeType == BarcodeType.EAN) {
            GenerateEan(content, filePath);
        } else if (barcodeType == BarcodeType.DataMatrix) {
            GenerateDataMatrix(content, filePath);
        } else if (barcodeType == BarcodeType.PDF417) {
            GeneratePdf417(content, filePath);
        }
    }

    /// <summary>
    /// Reads and decodes a barcode from an image.
    /// </summary>
    /// <param name="filePath">Path to the barcode image.</param>
    /// <returns>Decoded barcode result.</returns>
    public static BarcodeResult<Rgba32> Read(string filePath) {
        string fullPath = Helpers.ResolvePath(filePath);

        using Image<Rgba32> barcodeImage = SixLabors.ImageSharp.Image.Load<Rgba32>(fullPath);
        BarcodeReader.ImageSharp.BarcodeReader<Rgba32> reader = new(types: new[] { ZXing.BarcodeFormat.All_1D, ZXing.BarcodeFormat.DATA_MATRIX, ZXing.BarcodeFormat.PDF_417 });
        BarcodeResult<Rgba32> response = reader.Decode(barcodeImage);
        BarcodeResult<Rgba32> result = new() {
            Value = response.Value,
            Status = response.Status,
            Message = response.Message
        };
        return result;
    }
}

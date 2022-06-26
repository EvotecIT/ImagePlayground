using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Barcoder;
using Barcoder.Code128;
using Barcoder.Code39;
using Barcoder.Code93;
using Barcoder.Ean;
using Barcoder.Kix;
using Barcoder.Qr;
using Barcoder.Renderer.Image;
using Barcoder.UpcA;
using Barcoder.UpcE;
using BarcodeReader.ImageSharp;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Formats.Png;
using Encoding = Barcoder.Qr.Encoding;

namespace ImagePlayground {
    public class BarCode {
        public enum BarcodeTypes {
            Code128,
            Code93,
            Code39,
            KixCode,
            UPCE,
            UPCA,
            EAN
        }
        private static void SaveToFile(IBarcode barcode, string filePath) {
            ImageFormat imageFormatDetected;
            FileInfo fileInfo = new FileInfo(filePath);

            if (fileInfo.Extension == ".png") {
                imageFormatDetected = ImageFormat.Png;
            } else if (fileInfo.Extension == ".jpg" || fileInfo.Extension == ".jpeg") {
                imageFormatDetected = ImageFormat.Jpeg;
            } else if (fileInfo.Extension == ".bmp") {
                imageFormatDetected = ImageFormat.Bmp;
            } else {
                throw new UnknownImageFormatException("Image format not supported. Feel free to open an issue/fix it.");
            }

            var renderer = new ImageRenderer(imageFormat: imageFormatDetected);

            using (var stream = new FileStream(filePath, FileMode.Create)) {
                renderer.Render(barcode, stream);
            }
        }

        public static void GenerateQr(string content, string filePath, ErrorCorrectionLevel errorCorrectionLevel = ErrorCorrectionLevel.H, Encoding encoding = Encoding.Auto) {
            var barcode = QrEncoder.Encode(content, errorCorrectionLevel, encoding);
            SaveToFile(barcode, filePath);
        }

        public static void GenerateEan(string content, string filePath) {
            var barcode = EanEncoder.Encode(content);
            SaveToFile(barcode, filePath);
        }

        public static void GenerateCode128(string content, string filePath, bool includeChecksum = true) {
            var barcode = Code128Encoder.Encode(content, includeChecksum);
            SaveToFile(barcode, filePath);
        }

        public static void GenerateCode93(string content, string filePath, bool includeChecksum = true, bool fullAsciiMode = false) {
            var barcode = Code93Encoder.Encode(content, includeChecksum, fullAsciiMode);
            SaveToFile(barcode, filePath);
        }

        public static void GenerateCode39(string content, string filePath, bool includeChecksum = true, bool fullAsciiMode = false) {
            var barcode = Code39Encoder.Encode(content, includeChecksum, fullAsciiMode);
            SaveToFile(barcode, filePath);
        }

        public static void GenerateUpcE(string content, string filePath, UpcENumberSystem upcNumberSystem = UpcENumberSystem.Zero) {
            var barcode = UpcEEncoder.Encode(content, upcNumberSystem);
            SaveToFile(barcode, filePath);
        }

        public static void GenerateUpcA(string content, string filePath) {
            var barcode = UpcAEncoder.Encode(content);
            SaveToFile(barcode, filePath);
        }

        public static void GenerateKix(string content, string filePath) {
            var barcode = KixEncoder.Encode(content);
            SaveToFile(barcode, filePath);
        }

        public static void Generate(BarcodeTypes barcodeType, string content, string filePath) {
            if (barcodeType == BarcodeTypes.Code128) {
                GenerateCode128(content, filePath);
            } else if (barcodeType == BarcodeTypes.Code93) {
                GenerateCode93(content, filePath);
            } else if (barcodeType == BarcodeTypes.Code39) {
                GenerateCode39(content, filePath);
            } else if (barcodeType == BarcodeTypes.KixCode) {
                GenerateKix(content, filePath);
            } else if (barcodeType == BarcodeTypes.UPCA) {
                GenerateUpcA(content, filePath);
            } else if (barcodeType == BarcodeTypes.UPCE) {
                GenerateUpcE(content, filePath);
            } else if (barcodeType == BarcodeTypes.EAN) {
                GenerateEan(content, filePath);
            }
        }
        
        public static BarcodeResult<Rgba32> Read(string filePath) {
            Image<Rgba32> barcodeImage = Image.Load<Rgba32>(filePath);
            BarcodeReader<Rgba32> reader = new BarcodeReader<Rgba32>();
            var response = reader.Decode(barcodeImage);
            return response;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Barcoder.Code128;
using Barcoder.Qr;
using Barcoder.Renderer.Image;
using Encoding = Barcoder.Qr.Encoding;

namespace ImagePlayground {
    public class BarCode {

        public static void GenerateQR(string content, string filePathOutput, ImageFormat imageFormat = ImageFormat.Png) {
            var barcode = QrEncoder.Encode(content, errorCorrectionLevel: ErrorCorrectionLevel.H, Encoding.Auto);
            var renderer = new ImageRenderer(imageFormat: ImageFormat.Png);
            using (var stream = new FileStream(filePathOutput, FileMode.Create)) {
                renderer.Render(barcode, stream);
            }
        }

        public static void Generate(string content, string filePathOutput, ImageFormat imageFormat = ImageFormat.Png) {
            var barcode = Code128Encoder.Encode(content);
            var renderer = new ImageRenderer(imageFormat: ImageFormat.Png);

            using (var stream = new FileStream(filePathOutput, FileMode.Create)) {
                renderer.Render(barcode, stream);
            }
        }
    }
}

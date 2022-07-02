using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Barcoder;
using ImageFormat = System.Drawing.Imaging.ImageFormat;
using Xunit;


namespace ImagePlayground.Tests {
    public class Tests {
        private readonly string _directoryWithImages;
        private readonly string _directoryWithTests;

        public Tests() {
            _directoryWithImages = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Images");
            Setup(_directoryWithImages);
            _directoryWithTests = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Tests");
            Setup(_directoryWithTests);
        }

        public static void Setup(string path) {
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
        }

        [Fact]
        public void Test_QRCodeUrl() {

            string filePath = System.IO.Path.Combine(_directoryWithImages, "QRCodeUrl.jpg");
            File.Delete(filePath);
            Assert.True(File.Exists(filePath) == false);

            QrCode.Generate("https://evotec.xyz", filePath);

            Assert.True(File.Exists(filePath) == true);

            var read = QrCode.Read(filePath);
            Assert.True(read.Message == "https://evotec.xyz");
        }

        [Fact]
        public void Test_QRCodeWiFi() {
            string filePath = System.IO.Path.Combine(_directoryWithImages, "QRCodeWiFi.png");
            File.Delete(filePath);
            Assert.True(File.Exists(filePath) == false);

            QrCode.GenerateWiFi("Evotec", "superHardPassword123!", filePath, true);

            Assert.True(File.Exists(filePath) == true);

            var read = QrCode.Read(filePath);
            Assert.True(read.Message == "WIFI:T:WPA;S:Evotec;P:superHardPassword123!;;");
        }

        [Fact]
        public void Test_BarCode() {
            string filePath = System.IO.Path.Combine(_directoryWithTests, "BarcodeEAN13.png");
            BarCode.Generate(BarCode.BarcodeTypes.EAN, "9012341234571", filePath);

            var read1 = BarCode.Read(filePath);
            Assert.True(read1.Message == "9012341234571");
            Assert.True(File.Exists(filePath) == true);

            filePath = System.IO.Path.Combine(_directoryWithTests, "BarcodeEAN7.png");
            BarCode.Generate(BarCode.BarcodeTypes.EAN, "96385074", filePath);
            Assert.True(File.Exists(filePath) == true);

            var read2 = BarCode.Read(filePath);
            Assert.True(read2.Message == "96385074");
        }
    }
}
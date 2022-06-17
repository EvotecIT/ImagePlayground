using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageFormat = System.Drawing.Imaging.ImageFormat;
using Xunit;


namespace ImagePlayground.Tests {
    public class Tests {
        private readonly string _directoryWithImages;

        public Tests() {
            _directoryWithImages = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Images");
            Setup(_directoryWithImages);
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
        }

        [Fact]
        public void Test_QRCodeWiFi() {
            string filePath = System.IO.Path.Combine(_directoryWithImages, "QRCodeWiFi.png");
            File.Delete(filePath);
            Assert.True(File.Exists(filePath) == false);

            QrCode.GenerateWiFi("Evotec", "superHardPassword123!", filePath, true);

            Assert.True(File.Exists(filePath) == true);
        }
    }
}
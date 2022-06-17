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
        public void Test1() {

            string filePath = System.IO.Path.Combine(_directoryWithImages, "QRCode1.jpg");
            File.Delete(filePath);
            Assert.True(File.Exists(filePath) == false);

            QrCode.Generate("https://evotec.xyz", filePath, imageFormat: ImageFormat.Jpeg);

            Assert.True(File.Exists(filePath) == true);
        }
    }
}
using System;
using System.IO;
using SixLabors.ImageSharp;
using Xunit;

namespace ImagePlayground.Tests {
    public partial class ImagePlayground {
        [Fact]
        public void Test_WatermarkImage_Coordinates() {
            string src = Path.Combine(_directoryWithImages, "QRCode1.png");
            string watermark = Path.Combine(_directoryWithImages, "LogoEvotec.png");
            string dest = Path.Combine(_directoryWithTests, "watermark_xy.png");
            if (File.Exists(dest)) File.Delete(dest);

            ImageHelper.WatermarkImage(src, dest, watermark, 5, 5);
            Assert.True(File.Exists(dest));

            using var orig = Image.Load(src);
            using var result = Image.Load(dest);
            Assert.True(orig.Compare(result).PixelErrorCount > 0);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(101)]
        public void Test_WatermarkImage_Coordinates_InvalidPercentage(int perc) {
            string src = Path.Combine(_directoryWithImages, "QRCode1.png");
            string watermark = Path.Combine(_directoryWithImages, "LogoEvotec.png");
            string dest = Path.Combine(_directoryWithTests, $"watermark_invalid_{perc}.png");
            Assert.Throws<ArgumentOutOfRangeException>(() => ImageHelper.WatermarkImage(src, dest, watermark, 1, 1, watermarkPercentage: perc));
        }
    }
}

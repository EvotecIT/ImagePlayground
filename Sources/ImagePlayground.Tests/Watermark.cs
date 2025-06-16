using System.IO;
using ImagePlayground;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Xunit;

namespace ImagePlayground.Tests {
    public partial class ImagePlayground {
        [Fact]
        public void Test_WatermarkImageOriginalSize() {
            string src = Path.Combine(_directoryWithImages, "QRCode1.png");
            string watermark = Path.Combine(_directoryWithImages, "LogoEvotec.png");
            string dest = Path.Combine(_directoryWithTests, "watermarked.png");
            if (File.Exists(dest)) File.Delete(dest);

            ImageHelper.WatermarkImage(src, dest, watermark, Image.WatermarkPlacement.Middle);
            Assert.True(File.Exists(dest));

            using var result = Image.Load(dest);
            using var original = Image.Load(src);
            Assert.Equal(original.Width, result.Width);
            Assert.Equal(original.Height, result.Height);
        }
    }
}

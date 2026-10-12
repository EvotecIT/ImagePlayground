using OfficeIMO.Drawing;
using Color = OfficeIMO.Drawing.OfficeColor;
using ExifTag = OfficeIMO.Drawing.OfficeExifTag;
using Rgba32 = OfficeIMO.Drawing.OfficeColor;
using ImagePlayground;
using System.IO;
using Xunit;

namespace ImagePlayground.Tests {
    using Img = global::ImagePlayground.Image;
    public partial class ImagePlayground {
        [Fact]
        public void Test_Blur() {
            string src = Path.Combine(_directoryWithImages, "LogoEvotec.png");
            string dest = Path.Combine(_directoryWithTests, "logo_blur.png");
            if (File.Exists(dest)) File.Delete(dest);
            using var orig = Img.Load(src);
            ImageHelper.Blur(src, dest, 5);
            using var img = Img.Load(dest);
            Assert.Equal(orig.Width, img.Width);
            Assert.Equal(orig.Height, img.Height);
            Assert.NotEqual(File.ReadAllBytes(src), File.ReadAllBytes(dest));
        }

        [Fact]
        public void Test_Sharpen() {
            string src = Path.Combine(_directoryWithImages, "LogoEvotec.png");
            string dest = Path.Combine(_directoryWithTests, "logo_sharp.png");
            if (File.Exists(dest)) File.Delete(dest);
            using var orig = Img.Load(src);
            ImageHelper.Sharpen(src, dest, 2);
            using var img = Img.Load(dest);
            Assert.Equal(orig.Width, img.Width);
            Assert.Equal(orig.Height, img.Height);
            Assert.NotEqual(File.ReadAllBytes(src), File.ReadAllBytes(dest));
        }
    }
}

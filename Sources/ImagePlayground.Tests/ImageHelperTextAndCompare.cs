using System.IO;
using Xunit;

namespace ImagePlayground.Tests {
    public partial class ImagePlayground {
        [Fact]
        public void Test_CompareImages() {
            string img1 = Path.Combine(_directoryWithImages, "QRCode1.png");
            string modified = Path.Combine(_directoryWithTests, "qr_modified.png");
            if (File.Exists(modified)) File.Delete(modified);
            ImageHelper.AddText(img1, modified, 1, 1, "Diff", SixLabors.ImageSharp.Color.Red);

            var result = ImageHelper.Compare(img1, modified);
            Assert.True(result.PixelErrorCount > 0);
        }

        [Fact]
        public void Test_AddTextToImage() {
            string src = Path.Combine(_directoryWithImages, "QRCode1.png");
            string dest = Path.Combine(_directoryWithTests, "text.png");
            if (File.Exists(dest)) File.Delete(dest);
            ImageHelper.AddText(src, dest, 1, 1, "Test", SixLabors.ImageSharp.Color.Red);
            Assert.True(File.Exists(dest));
            using var img = Image.Load(dest);
            Assert.Equal(660, img.Width);
            Assert.Equal(660, img.Height);
        }

        [Fact]
        public void Test_CreateGridImage() {
            string dest = Path.Combine(_directoryWithTests, "grid.png");
            if (File.Exists(dest)) File.Delete(dest);
            ImageHelper.Create(dest, 50, 50, SixLabors.ImageSharp.Color.White);
            Assert.True(File.Exists(dest));
            using var img = Image.Load(dest);
            Assert.Equal(50, img.Width);
            Assert.Equal(50, img.Height);
        }
    }
}

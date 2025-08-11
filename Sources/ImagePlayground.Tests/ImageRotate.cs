using ImagePlayground;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.IO;
using Xunit;

namespace ImagePlayground.Tests {
    public partial class ImagePlayground {
        [Fact]
        public void Test_Rotate_Degrees() {
            string src = Path.Combine(_directoryWithImages, "PrzemyslawKlysAndKulkozaurr.jpg");
            string dest = Path.Combine(_directoryWithTests, "rotate_90.jpg");
            if (File.Exists(dest)) File.Delete(dest);
            using var orig = Image.Load(src);
            int w = orig.Width;
            int h = orig.Height;
            ImageHelper.Rotate(src, dest, 90);
            using var img = Image.Load(dest);
            Assert.Equal(w, img.Height);
            Assert.Equal(h, img.Width);
        }

        [Fact]
        public void Test_Rotate_Mode() {
            string src = Path.Combine(_directoryWithImages, "PrzemyslawKlysAndKulkozaurr.jpg");
            string dest = Path.Combine(_directoryWithTests, "rotate_mode.jpg");
            if (File.Exists(dest)) File.Delete(dest);
            using var orig = Image.Load(src);
            int w = orig.Width;
            int h = orig.Height;
            ImageHelper.Rotate(src, dest, RotateMode.Rotate270);
            using var img = Image.Load(dest);
            Assert.Equal(w, img.Height);
            Assert.Equal(h, img.Width);
        }
    }
}

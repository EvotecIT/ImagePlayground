using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using PointF = SixLabors.ImageSharp.PointF;
using ImagePlayground;
using Xunit;

namespace ImagePlayground.Tests {
    public partial class ImagePlayground {
        [Fact]
        public void Test_CropCircle_MakesCornerTransparent() {
            string src = Path.Combine(_directoryWithImages, "QRCode1.png");
            string dest = Path.Combine(_directoryWithTests, "crop_circle.png");
            if (File.Exists(dest)) File.Delete(dest);
            using var img = global::ImagePlayground.Image.Load(src);
            img.CropCircle(img.Width / 2f, img.Height / 2f, img.Width / 4f);
            img.Save(dest);
            using var result = SixLabors.ImageSharp.Image.Load<Rgba32>(dest);
            Assert.Equal(0, result[0, 0].A);
        }

        [Fact]
        public void Test_CropPolygon_MakesCornerTransparent() {
            string src = Path.Combine(_directoryWithImages, "QRCode1.png");
            string dest = Path.Combine(_directoryWithTests, "crop_polygon.png");
            if (File.Exists(dest)) File.Delete(dest);
            using var img = global::ImagePlayground.Image.Load(src);
            var points = new[] { new PointF(0, 0), new PointF(img.Width, 0), new PointF(img.Width / 2f, img.Height / 2f) };
            img.CropPolygon(points);
            img.Save(dest);
            using var result = SixLabors.ImageSharp.Image.Load<Rgba32>(dest);
            Assert.Equal(0, result[result.Width - 1, result.Height - 1].A);
        }
    }
}

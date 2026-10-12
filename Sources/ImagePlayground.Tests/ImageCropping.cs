using OfficeIMO.Drawing;
using Color = OfficeIMO.Drawing.OfficeColor;
using ExifTag = OfficeIMO.Drawing.OfficeExifTag;
using Rgba32 = OfficeIMO.Drawing.OfficeColor;
using ImagePlayground;
using System.IO;
using Xunit;
using PointF = OfficeIMO.Drawing.OfficePoint;

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
            using var result = global::ImagePlayground.Image.Load(dest);
            Assert.Equal(0, result.Raster.GetPixel(0, 0).A);
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
            using var result = global::ImagePlayground.Image.Load(dest);
            Assert.Equal(0, result.Raster.GetPixel(result.Width - 1, result.Height - 1).A);
        }
    }
}

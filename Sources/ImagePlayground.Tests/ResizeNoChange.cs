using System.IO;
using SixLabors.ImageSharp;
using Xunit;
using ImageSharpImage = SixLabors.ImageSharp.Image;

namespace ImagePlayground.Tests {
    public partial class ImagePlayground {
        [Fact]
        public void Test_Resize_NoChange_ReturnsSame() {
            string src = Path.Combine(_directoryWithImages, "QRCode1.png");
            using var img = SixLabors.ImageSharp.Image.Load(src);
            var result = ImageHelper.Resize(img, img.Width, img.Height);
            Assert.Same(img, result);
            Assert.Equal(660, img.Width);
            Assert.Equal(660, img.Height);
        }
    }
}

using System.IO;
using Xunit;

namespace ImagePlayground.Tests {
    public partial class ImagePlayground {
        [Fact]
        public void Test_ConvertToAndFromBase64() {
            string src = Path.Combine(_directoryWithImages, "QRCode1.png");
            string dest = Path.Combine(_directoryWithTests, "base64.png");
            if (File.Exists(dest)) File.Delete(dest);

            string b64 = ImageHelper.ConvertToBase64(src);
            ImageHelper.ConvertFromBase64(b64, dest);

            Assert.True(File.Exists(dest));
            byte[] a = File.ReadAllBytes(src);
            byte[] b = File.ReadAllBytes(dest);
            Assert.Equal(a.Length, b.Length);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ImagePlayground.Tests {
    public partial class ImagePlayground {
        [Fact]
        public void Test_ImageManipulation() {
            string filePath = System.IO.Path.Combine(_directoryWithImages, "QRCodeUrlBefore.jpg");
            File.Delete(filePath);
            Assert.True(File.Exists(filePath) == false);
        }
    }
}

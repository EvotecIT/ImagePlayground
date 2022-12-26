using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScottPlot.Plottable;
using Xunit;

namespace ImagePlayground.Tests {
    public partial class ImagePlayground {


        [Fact]
        public void Test_ResizeImage() {
            string filePath = System.IO.Path.Combine(_directoryWithImages, "QRCodeUrl.jpg");
            File.Delete(filePath);
            Assert.True(File.Exists(filePath) == false);

            QrCode.Generate("https://evotec.xyz", filePath);

            Assert.True(File.Exists(filePath) == true);

            var image = Images.GetImage(filePath);
            Assert.True(image.Width == 660);
            Assert.True(image.Height == 660);

            var newImage = System.IO.Path.Combine(_directoryWithImages, "QRCodeUrlResized.jpg");
            Images.Resize(filePath, newImage, 100, 100);

            var imageResized = Images.GetImage(newImage);
            Assert.True(imageResized.Width == 100);
            Assert.True(imageResized.Height == 100);
        }

    }
}

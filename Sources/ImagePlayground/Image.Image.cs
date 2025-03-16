using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Path = System.IO.Path;
using SLImage = SixLabors.ImageSharp.Image;

namespace ImagePlayground {
    public partial class Image : IDisposable {
        public void AddImage(string filePath, int x, int y, float opacity) {
            string fullPath = Path.GetFullPath(filePath);

            var location = new Point(x, y);
            using var image = SLImage.Load(fullPath);
            _image.Mutate(mx => mx.DrawImage(image, location, opacity));
        }
        public void AddImage(SLImage image, int x, int y, float opacity) {
            var location = new Point(x, y);
            _image.Mutate(mx => mx.DrawImage(image, location, opacity));
        }

        public void AddImage(SLImage image, Point location, float opacity) {
            _image.Mutate(mx => mx.DrawImage(image, location, opacity));
        }
    }
}

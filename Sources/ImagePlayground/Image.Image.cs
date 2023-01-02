using System;
using System.Collections.Generic;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;


namespace ImagePlayground {
    public partial class Image : IDisposable {
        public void AddImage(string filePath, int x, int y, float opacity) {
            string fullPath = System.IO.Path.GetFullPath(filePath);

            var location = new Point(x, y);
            using (var image = SixLabors.ImageSharp.Image.Load(fullPath)) {
                _image.Mutate(mx => mx.DrawImage(image, location, opacity));
            }
        }
        public void AddImage(SixLabors.ImageSharp.Image image, int x, int y, float opacity) {
            var location = new Point(x, y);
            _image.Mutate(mx => mx.DrawImage(image, location, opacity));
        }

        public void AddImage(SixLabors.ImageSharp.Image image, Point location, float opacity) {
            _image.Mutate(mx => mx.DrawImage(image, location, opacity));
        }
    }
}

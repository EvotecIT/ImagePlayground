using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ImagePlayground;
public partial class Image : IDisposable {
    public void Avatar(int width, int height, float cornerRadius) {
        _image.Mutate(x => ConvertToAvatar(x, new Size(width, height), cornerRadius));
    }

    public void SaveAsAvatar(string filePath, int width, int height, float cornerRadius) {
        string fullPath = Helpers.ResolvePath(filePath);
        using var clone = _image.Clone(x => ConvertToAvatar(x, new Size(width, height), cornerRadius));
        clone.Save(fullPath);
    }

    public void SaveAsAvatar(Stream stream, int width, int height, float cornerRadius) {
        using var clone = _image.Clone(x => ConvertToAvatar(x, new Size(width, height), cornerRadius));
        clone.SaveAsPng(stream);
        stream.Seek(0, SeekOrigin.Begin);
    }

    public void SaveAsCircularAvatar(string filePath, int size) {
        SaveAsAvatar(filePath, size, size, size / 2f);
    }

    public void SaveAsCircularAvatar(Stream stream, int size) {
        SaveAsAvatar(stream, size, size, size / 2f);
    }

    private static IImageProcessingContext ConvertToAvatar(IImageProcessingContext context, Size size, float cornerRadius) {
        return ApplyRoundedCorners(context.Resize(new ResizeOptions {
            Size = size,
            Mode = ResizeMode.Crop
        }), cornerRadius);
    }

    private static IImageProcessingContext ApplyRoundedCorners(IImageProcessingContext context, float cornerRadius) {
        Size size = context.GetCurrentSize();
        IPathCollection corners = BuildCorners(size.Width, size.Height, cornerRadius);

        context.SetGraphicsOptions(new GraphicsOptions {
            Antialias = true,
            AlphaCompositionMode = PixelAlphaCompositionMode.DestOut
        });

        foreach (IPath path in corners) {
            context = context.Fill(Color.Red, path);
        }

        return context;
    }

    private static PathCollection BuildCorners(int imageWidth, int imageHeight, float cornerRadius) {
        var rect = new RectangularPolygon(-0.5f, -0.5f, cornerRadius, cornerRadius);
        IPath cornerTopLeft = rect.Clip(new EllipsePolygon(cornerRadius - 0.5f, cornerRadius - 0.5f, cornerRadius));

        float rightPos = imageWidth - cornerTopLeft.Bounds.Width + 1;
        float bottomPos = imageHeight - cornerTopLeft.Bounds.Height + 1;

        IPath cornerTopRight = cornerTopLeft.RotateDegree(90).Translate(rightPos, 0);
        IPath cornerBottomLeft = cornerTopLeft.RotateDegree(-90).Translate(0, bottomPos);
        IPath cornerBottomRight = cornerTopLeft.RotateDegree(180).Translate(rightPos, bottomPos);

        return new PathCollection(cornerTopLeft, cornerBottomLeft, cornerTopRight, cornerBottomRight);
    }
}

using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace ImagePlayground.Gdi;

/// <summary>Represents a GDI+ backed image with simple drawing helpers.</summary>
/// <para>
/// This type provides a lightweight Windows-focused drawing surface for PowerBGInfo-style rendering,
/// chart composition, annotations, and basic image save/load operations.
/// </para>
public sealed class Image : IDisposable {
    private static readonly string[] SupportedExtensions = new[] {
        ".bmp", ".gif", ".jpg", ".jpeg", ".tif", ".tiff", ".ico", ".png"
    };

    private Bitmap _bitmap = null!;
    private string _filePath = string.Empty;

    /// <summary>Gets the image width in pixels.</summary>
    public int Width => _bitmap.Width;

    /// <summary>Gets the image height in pixels.</summary>
    public int Height => _bitmap.Height;

    /// <summary>Gets the path used to load or save the image.</summary>
    public string FilePath => _filePath;

    /// <summary>Loads an image from disk.</summary>
    /// <param name="filePath">Path to an existing image file.</param>
    /// <returns>A new <see cref="Image"/> instance backed by a cloned bitmap.</returns>
    public static Image Load(string filePath) {
        if (string.IsNullOrWhiteSpace(filePath)) {
            throw new ArgumentException("File path is required.", nameof(filePath));
        }

        var fullPath = Path.GetFullPath(filePath);
        using var stream = File.OpenRead(fullPath);
        using var loaded = new Bitmap(stream);
        var bitmap = new Bitmap(loaded);
        return new Image {
            _filePath = fullPath,
            _bitmap = bitmap
        };
    }

    /// <summary>Creates a blank image.</summary>
    /// <param name="filePath">Optional file path associated with the image for later save operations.</param>
    /// <param name="width">Image width in pixels.</param>
    /// <param name="height">Image height in pixels.</param>
    /// <param name="background">Optional background color. Transparent is used when omitted.</param>
    public void Create(string filePath, int width, int height, Color? background = null) {
        if (width <= 0) throw new ArgumentOutOfRangeException(nameof(width));
        if (height <= 0) throw new ArgumentOutOfRangeException(nameof(height));
        _filePath = string.IsNullOrWhiteSpace(filePath) ? string.Empty : Path.GetFullPath(filePath);
        _bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
        using var graphics = Graphics.FromImage(_bitmap);
        ConfigureGraphics(graphics);
        graphics.Clear(background ?? Color.Transparent);
    }

    /// <summary>Saves the image to disk.</summary>
    /// <param name="filePath">Optional destination path. When omitted, the path associated with the image is reused.</param>
    /// <param name="openImage">When set, opens the saved image using the shell after writing it to disk.</param>
    public void Save(string filePath = "", bool openImage = false) {
        var targetPath = string.IsNullOrWhiteSpace(filePath) ? _filePath : Path.GetFullPath(filePath);
        if (string.IsNullOrWhiteSpace(targetPath)) {
            throw new InvalidOperationException("No output path specified.");
        }

        var directory = Path.GetDirectoryName(targetPath);
        if (!string.IsNullOrEmpty(directory)) {
            Directory.CreateDirectory(directory);
        }

        var format = GetImageFormat(Path.GetExtension(targetPath));
        _bitmap.Save(targetPath, format);

        if (openImage) {
            Process.Start(new ProcessStartInfo(targetPath) {
                UseShellExecute = true
            });
        }
    }

    /// <summary>Saves the image to a stream.</summary>
    /// <param name="stream">Destination stream that receives the encoded image bytes.</param>
    /// <param name="format">Optional image format. PNG is used when omitted.</param>
    public void Save(Stream stream, ImageFormat? format = null) {
        if (stream is null) throw new ArgumentNullException(nameof(stream));
        _bitmap.Save(stream, format ?? ImageFormat.Png);
        if (stream.CanSeek) {
            stream.Seek(0, SeekOrigin.Begin);
        }
    }

    /// <summary>Resizes the image to the specified size.</summary>
    public void Resize(int width, int height) {
        if (width <= 0) throw new ArgumentOutOfRangeException(nameof(width));
        if (height <= 0) throw new ArgumentOutOfRangeException(nameof(height));
        var resized = new Bitmap(width, height, PixelFormat.Format32bppArgb);
        using (var graphics = Graphics.FromImage(resized)) {
            ConfigureGraphics(graphics);
            graphics.DrawImage(_bitmap, 0, 0, width, height);
        }
        _bitmap.Dispose();
        _bitmap = resized;
    }

    /// <summary>Measures the size of the text using the specified font.</summary>
    /// <param name="text">Text that should be measured.</param>
    /// <param name="fontSize">Font size in pixels.</param>
    /// <param name="fontFamilyName">Preferred font family name.</param>
    /// <returns>The measured text size for the configured font.</returns>
    public SizeF GetTextSize(string text, float fontSize, string fontFamilyName) {
        using var graphics = Graphics.FromImage(_bitmap);
        ConfigureGraphics(graphics);
        using var font = CreateFont(fontFamilyName, fontSize);
        return graphics.MeasureString(text, font);
    }

    /// <summary>Adds text to the image.</summary>
    /// <para>This helper supports optional shadow and outline rendering for labels, overlays, and dashboard text.</para>
    public void AddText(float x, float y, string text, Color color, float fontSize = 16f, string fontFamilyName = "Calibri", Color? shadowColor = null, float shadowOffsetX = 0f, float shadowOffsetY = 0f, Color? outlineColor = null, float outlineWidth = 0f) {
        using var graphics = Graphics.FromImage(_bitmap);
        ConfigureGraphics(graphics);
        using var font = CreateFont(fontFamilyName, fontSize);

        if (shadowColor.HasValue) {
            using var shadowBrush = new SolidBrush(shadowColor.Value);
            graphics.DrawString(text, font, shadowBrush, x + shadowOffsetX, y + shadowOffsetY);
        }

        if (outlineColor.HasValue && outlineWidth > 0f) {
            using var path = new GraphicsPath();
            path.AddString(text, font.FontFamily, (int)font.Style, font.Size, new PointF(x, y), StringFormat.GenericDefault);
            using var outlinePen = new Pen(outlineColor.Value, outlineWidth) {
                LineJoin = LineJoin.Round
            };
            graphics.DrawPath(outlinePen, path);
            using var fillBrush = new SolidBrush(color);
            graphics.FillPath(fillBrush, path);
            return;
        }

        using var brush = new SolidBrush(color);
        graphics.DrawString(text, font, brush, x, y);
    }

    /// <summary>Draws a line.</summary>
    public void DrawLine(Color color, float width, float x1, float y1, float x2, float y2) {
        using var graphics = Graphics.FromImage(_bitmap);
        ConfigureGraphics(graphics);
        using var pen = new Pen(color, width);
        graphics.DrawLine(pen, x1, y1, x2, y2);
    }

    /// <summary>Draws a rectangle outline.</summary>
    public void DrawRectangle(Color color, float width, RectangleF rect) {
        using var graphics = Graphics.FromImage(_bitmap);
        ConfigureGraphics(graphics);
        using var pen = new Pen(color, width);
        graphics.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
    }

    /// <summary>Fills a rectangle.</summary>
    public void FillRectangle(Color color, RectangleF rect) {
        using var graphics = Graphics.FromImage(_bitmap);
        ConfigureGraphics(graphics);
        using var brush = new SolidBrush(color);
        graphics.FillRectangle(brush, rect);
    }

    /// <summary>Fills an ellipse.</summary>
    public void FillEllipse(Color color, RectangleF rect) {
        using var graphics = Graphics.FromImage(_bitmap);
        ConfigureGraphics(graphics);
        using var brush = new SolidBrush(color);
        graphics.FillEllipse(brush, rect);
    }

    /// <summary>Draws an ellipse outline.</summary>
    public void DrawEllipse(Color color, float width, RectangleF rect) {
        using var graphics = Graphics.FromImage(_bitmap);
        ConfigureGraphics(graphics);
        using var pen = new Pen(color, width);
        graphics.DrawEllipse(pen, rect);
    }

    /// <summary>Draws another image onto this image.</summary>
    public void DrawImage(Image image, float x, float y, float? width = null, float? height = null) {
        if (image is null) throw new ArgumentNullException(nameof(image));
        using var graphics = Graphics.FromImage(_bitmap);
        ConfigureGraphics(graphics);
        if (width.HasValue && height.HasValue) {
            graphics.DrawImage(image._bitmap, x, y, width.Value, height.Value);
        } else {
            graphics.DrawImage(image._bitmap, x, y);
        }
    }

    /// <summary>Provides access to the underlying graphics for custom rendering.</summary>
    /// <param name="action">Callback that receives a configured <see cref="Graphics"/> instance.</param>
    public void WithGraphics(Action<Graphics> action) {
        if (action is null) throw new ArgumentNullException(nameof(action));
        using var graphics = Graphics.FromImage(_bitmap);
        ConfigureGraphics(graphics);
        action(graphics);
    }

    /// <summary>Disposes the underlying bitmap.</summary>
    public void Dispose() {
        _bitmap?.Dispose();
    }

    private static void ConfigureGraphics(Graphics graphics) {
        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
        graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
    }

    private static Font CreateFont(string fontFamilyName, float fontSize) {
        try {
            return new Font(fontFamilyName, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
        } catch (ArgumentException) {
            return new Font(FontFamily.GenericSansSerif, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
        }
    }

    private static ImageFormat GetImageFormat(string extension) {
        switch (extension.ToLowerInvariant()) {
            case ".bmp":
                return ImageFormat.Bmp;
            case ".gif":
                return ImageFormat.Gif;
            case ".jpg":
            case ".jpeg":
                return ImageFormat.Jpeg;
            case ".tif":
            case ".tiff":
                return ImageFormat.Tiff;
            case ".ico":
                return ImageFormat.Icon;
            case ".png":
                return ImageFormat.Png;
            default:
                throw new NotSupportedException($"Image format not supported. Supported extensions: {string.Join(", ", SupportedExtensions)}");
        }
    }
}

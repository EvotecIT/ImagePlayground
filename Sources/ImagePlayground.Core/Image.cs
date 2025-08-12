using System;
using System.IO;
using Codeuctivity.ImageSharpCompare;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Metadata;
using SixLabors.ImageSharp.PixelFormats;

namespace ImagePlayground;

/// <summary>
/// Represents an image loaded using ImageSharp and exposes basic manipulation helpers.
/// </summary>
public partial class Image : IDisposable {
    private SixLabors.ImageSharp.Image _image;
    private string _filePath;

    /// <summary>Gets the width of the image.</summary>
    public int Width => _image.Width;

    /// <summary>Gets the height of the image.</summary>
    public int Height => _image.Height;

    /// <summary>Gets the path the image was loaded from.</summary>
    public string FilePath => _filePath;

    /// <summary>Gets the metadata associated with the image.</summary>
    public ImageMetadata Metadata => _image.Metadata;

    /// <summary>Gets information about the underlying pixel type.</summary>
    public PixelTypeInfo PixelType => _image.PixelType;

    /// <summary>Gets the frame collection for multi-frame images.</summary>
    public ImageFrameCollection Frames => _image.Frames;

    /// <summary>
    /// Compares the current image with <paramref name="imageToCompare"/>.
    /// </summary>
    /// <param name="imageToCompare">Image to compare with.</param>
    /// <returns>Comparison result.</returns>
    public ICompareResult Compare(Image imageToCompare) {
        return ImageSharpCompare.CalcDiff(_image, imageToCompare._image);
    }

    /// <summary>
    /// Compares the current image with the file at <paramref name="filePathToCompare"/>.
    /// </summary>
    /// <param name="filePathToCompare">Path to the image to compare with.</param>
    /// <returns>Comparison result.</returns>
    public ICompareResult Compare(string filePathToCompare) {
        string fullPath = Helpers.ResolvePath(filePathToCompare);

        using (var imageToCompare = GetImage(fullPath)) {
            return ImageSharpCompare.CalcDiff(_image, imageToCompare);
        }
    }

    /// <summary>
    /// Creates a difference mask between this image and <paramref name="imageToCompare"/> and saves it.
    /// </summary>
    /// <param name="imageToCompare">Image to compare with.</param>
    /// <param name="filePathToSave">Output path for the mask image.</param>
    public void Compare(Image imageToCompare, string filePathToSave) {
        string outFullPath = Helpers.ResolvePath(filePathToSave);
        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outFullPath)!);
        using (var fileStreamDifferenceMask = File.Create(outFullPath)) {
            using (var maskImage = ImageSharpCompare.CalcDiffMaskImage(_image, imageToCompare._image)) {
                SixLabors.ImageSharp.ImageExtensions.SaveAsPng(maskImage, fileStreamDifferenceMask);
            }
        }
    }

    /// <summary>
    /// Creates a difference mask between this image and the file at <paramref name="filePathToCompare"/> and saves it.
    /// </summary>
    /// <param name="filePathToCompare">Path to the image to compare with.</param>
    /// <param name="filePathToSave">Output path for the mask image.</param>
    public void Compare(string filePathToCompare, string filePathToSave) {
        string fullPath = Helpers.ResolvePath(filePathToCompare);
        string outFullPath = Helpers.ResolvePath(filePathToSave);
        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outFullPath)!);

        using (var fileStreamDifferenceMask = File.Create(outFullPath)) {
            using (var imageToCompare = GetImage(fullPath)) {
                using (var maskImage = ImageSharpCompare.CalcDiffMaskImage(_image, imageToCompare)) {
                    SixLabors.ImageSharp.ImageExtensions.SaveAsPng(maskImage, fileStreamDifferenceMask);
                }
            }
        }
    }

    /// <summary>
    /// Loads an <see cref="SixLabors.ImageSharp.Image"/> from disk.
    /// </summary>
    /// <param name="filePath">Path to the image file.</param>
    /// <returns>The loaded image.</returns>
    public static SixLabors.ImageSharp.Image GetImage(string filePath) {
        string fullPath = Helpers.ResolvePath(filePath);
        using (var inStream = System.IO.File.OpenRead(fullPath)) {
            return SixLabors.ImageSharp.Image.Load(inStream);
        }
    }

    /// <summary>
    /// Creates a new blank image and assigns the provided <paramref name="filePath"/>.
    /// </summary>
    /// <param name="filePath">Path where the image will be saved.</param>
    /// <param name="width">Image width.</param>
    /// <param name="height">Image height.</param>
    public void Create(string filePath, int width, int height) {
        _filePath = filePath;
        _image = new Image<Rgba32>(width, height);
    }

    /// <summary>
    /// Loads an image from the specified path.
    /// </summary>
    /// <param name="filePath">Path to the image file.</param>
    /// <returns>Loaded <see cref="Image"/> instance.</returns>
    public static Image Load(string filePath) {
        string fullPath = Helpers.ResolvePath(filePath);

        using var stream = System.IO.File.OpenRead(fullPath);
        Image image = new Image {
            _filePath = fullPath,
            _image = SixLabors.ImageSharp.Image.Load(stream)
        };

        return image;
    }

    /// <summary>
    /// Saves the image to disk.
    /// </summary>
    /// <param name="filePath">Target path or empty to overwrite the original file.</param>
    /// <param name="openImage">Whether to open the saved file.</param>
    /// <param name="quality">Optional quality for lossy formats.</param>
    /// <param name="compressionLevel">Optional compression level for PNG/WebP.</param>
    public void Save(string filePath = "", bool openImage = false, int? quality = null, int? compressionLevel = null) {
        if (filePath == "") {
            filePath = _filePath;
        } else {
            filePath = Helpers.ResolvePath(filePath);
        }

        string? directory = System.IO.Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory) && !System.IO.Directory.Exists(directory)) {
            System.IO.Directory.CreateDirectory(directory);
        }

        var encoder = Helpers.GetEncoder(System.IO.Path.GetExtension(filePath), quality, compressionLevel);
        _image.Save(filePath, encoder);
        Helpers.Open(filePath, openImage);
    }

    /// <summary>
    /// Saves the image overwriting the original file and optionally opens it.
    /// </summary>
    /// <param name="openImage">Whether to open the saved file.</param>
    public void Save(bool openImage) {
        Save("", openImage, null, null);
    }

    /// <summary>
    /// Saves the image to the provided <paramref name="stream"/>.
    /// </summary>
    /// <param name="stream">Destination stream.</param>
    /// <param name="quality">Optional quality for lossy formats.</param>
    /// <param name="compressionLevel">Optional compression level for PNG/WebP.</param>
    public void Save(Stream stream, int? quality = null, int? compressionLevel = null) {
        string extension = System.IO.Path.GetExtension(_filePath)?.ToLowerInvariant();
        var encoder = Helpers.GetEncoder(extension, quality, compressionLevel);
        _image.Save(stream, encoder);
        stream.Seek(0, SeekOrigin.Begin);
    }

    /// <summary>
    /// Converts the image to a memory stream.
    /// </summary>
    /// <param name="quality">Optional quality for lossy formats.</param>
    /// <param name="compressionLevel">Optional compression level for PNG/WebP.</param>
    /// <returns>Stream containing the image data.</returns>
    public MemoryStream ToStream(int? quality = null, int? compressionLevel = null) {
        var ms = new MemoryStream();
        Save(ms, quality, compressionLevel);
        return ms;
    }

    /// <summary>
    /// Disposes the underlying image resources.
    /// </summary>
    public void Dispose() {
        if (_image != null) {
            _image.Dispose();
        }
    }
}

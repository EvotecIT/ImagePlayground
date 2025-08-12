using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Extensions.Transforms;

namespace ImagePlayground;

/// <summary>
/// Provides geometric transformation operations.
/// </summary>
public partial class Image : IDisposable {
    /// <summary>
    /// Automatically rotates the image according to EXIF orientation data.
    /// </summary>
    public void AutoOrient() {
        _image.Mutate(x => x.AutoOrient());
    }

    /// <summary>
    /// Flips the image using the provided <paramref name="flipMode"/>.
    /// </summary>
    /// <param name="flipMode">Flip mode.</param>
    public void Flip(FlipMode flipMode) {
        _image.Mutate(x => x.Flip(flipMode));
    }

    /// <summary>
    /// Rotates the image using the specified <paramref name="rotateMode"/>.
    /// </summary>
    /// <param name="rotateMode">Rotation mode.</param>
    public void Rotate(RotateMode rotateMode) {
        _image.Mutate(x => x.Rotate(rotateMode));
    }

    /// <summary>
    /// Rotates the image by an arbitrary number of <paramref name="degrees"/>.
    /// </summary>
    /// <param name="degrees">Angle in degrees.</param>
    public void Rotate(float degrees) {
        _image.Mutate(x => x.Rotate(degrees: degrees));
    }

    /// <summary>
    /// Rotates and flips the image in a single operation.
    /// </summary>
    /// <param name="rotateMode">Rotation mode.</param>
    /// <param name="flipMode">Flip mode.</param>
    public void RotateFlip(RotateMode rotateMode, FlipMode flipMode) {
        _image.Mutate(x => x.RotateFlip(rotateMode, flipMode));
    }

    /// <summary>
    /// Resizes the image to the specified dimensions.
    /// </summary>
    /// <param name="width">New width.</param>
    /// <param name="height">New height.</param>
    /// <param name="keepAspectRatio">Maintain aspect ratio if possible.</param>
    public void Resize(int? width, int? height, bool keepAspectRatio = true) {
        if (width == null && height == null) {
            return;
        }

        var options = new ResizeOptions();
        if (keepAspectRatio && (width == null || height == null)) {
            options.Mode = ResizeMode.Max;
            if (width == null) {
                int calculatedWidth = (int)Math.Round(height!.Value * _image.Width / (double)_image.Height);
                options.Size = new Size(calculatedWidth, height.Value);
            } else {
                int calculatedHeight = (int)Math.Round(width.Value * _image.Height / (double)_image.Width);
                options.Size = new Size(width.Value, calculatedHeight);
            }
        } else if (keepAspectRatio) {
            options.Mode = ResizeMode.Max;
            options.Size = new Size(width ?? _image.Width, height ?? _image.Height);
        } else {
            options.Mode = ResizeMode.Stretch;
            options.Size = new Size(width ?? _image.Width, height ?? _image.Height);
        }

        _image.Mutate(x => x.Resize(options));
    }

    /// <summary>
    /// Resizes the image by a <paramref name="percentage"/> of the current size.
    /// </summary>
    /// <param name="percentage">Scale percentage.</param>
    public void Resize(int percentage) {
        if (percentage <= 0) {
            throw new ArgumentOutOfRangeException(nameof(percentage));
        }

        int width = _image.Width * percentage / 100;
        int height = _image.Height * percentage / 100;
        var options = new ResizeOptions {
            Mode = ResizeMode.Stretch,
            Size = new Size(width, height)
        };
        _image.Mutate(x => x.Resize(options));
    }

    /// <summary>
    /// Skews the image by the specified angles.
    /// </summary>
    /// <param name="degreesX">Skew angle on the X axis.</param>
    /// <param name="degreesY">Skew angle on the Y axis.</param>
    public void Skew(float degreesX, float degreesY) {
        _image.Mutate(x => x.Skew(degreesX, degreesY));
    }
}


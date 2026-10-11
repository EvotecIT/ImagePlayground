namespace ImagePlayground;

/// <summary>Geometric image edits delegated to the shared managed raster engine.</summary>
public partial class Image {
    /// <summary>Applies the stored EXIF orientation and resets it to normal, leaving images without an orientation tag unchanged.</summary>
    public void AutoOrient(CancellationToken cancellationToken = default) {
        cancellationToken.ThrowIfCancellationRequested();
        object? value = Metadata.GetExifValue(OfficeExifTag.Orientation)?.Value;
        if (value == null) {
            return;
        }
        int orientation = Convert.ToInt32(value);
        if (orientation == 1) {
            return;
        }
        Apply(image => OfficeRasterTransforms.AutoOrient(image, orientation, cancellationToken), image => OfficeRasterTransforms.GetRotatedSize(image, orientation >= 5 ? 90 : 0), cancellationToken: cancellationToken);
        Metadata.SetExifValue(OfficeExifTag.Orientation, (ushort)1);
    }
    /// <summary>Mirrors each image frame along the selected axis.</summary>
    public void Flip(FlipMode flipMode, CancellationToken cancellationToken = default) {
        if (!Enum.IsDefined(typeof(FlipMode), flipMode)) {
            throw new ArgumentOutOfRangeException(nameof(flipMode));
        }
        Apply(image => OfficeRasterTransforms.Flip(image, flipMode == FlipMode.Horizontal, flipMode == FlipMode.Vertical, cancellationToken), cancellationToken: cancellationToken);
    }
    /// <summary>Rotates each frame clockwise in quarter turns.</summary>
    public void Rotate(RotateMode rotateMode, CancellationToken cancellationToken = default) {
        if (!Enum.IsDefined(typeof(RotateMode), rotateMode)) {
            throw new ArgumentOutOfRangeException(nameof(rotateMode));
        }
        Rotate((float)rotateMode, cancellationToken);
    }
    /// <summary>Rotates each frame clockwise, expanding the canvas to preserve the image.</summary>
    public void Rotate(float degrees, CancellationToken cancellationToken = default) => Apply(image => OfficeRasterTransforms.Rotate(image, degrees, cancellationToken: cancellationToken), image => OfficeRasterTransforms.GetRotatedSize(image, degrees), cancellationToken: cancellationToken);
    /// <summary>Rotates and then mirrors each image frame.</summary>
    public void RotateFlip(RotateMode rotateMode, FlipMode flipMode, CancellationToken cancellationToken = default) {
        EnsureUsable();
        if (!Enum.IsDefined(typeof(RotateMode), rotateMode)) { throw new ArgumentOutOfRangeException(nameof(rotateMode)); }
        if (!Enum.IsDefined(typeof(FlipMode), flipMode)) { throw new ArgumentOutOfRangeException(nameof(flipMode)); }
        long intermediateBytes = _frames.Max(frame => (long)frame.Image.Width * frame.Image.Height * 4);
        Apply(source => {
            var rotated = OfficeRasterTransforms.Rotate(source, (float)rotateMode, cancellationToken: cancellationToken);
            return OfficeRasterTransforms.Flip(rotated, flipMode == FlipMode.Horizontal, flipMode == FlipMode.Vertical, cancellationToken);
        }, source => OfficeRasterTransforms.GetRotatedSize(source, (float)rotateMode), intermediateBytes, cancellationToken);
    }
    /// <summary>Resizes every frame. Preserving aspect ratio fits within both supplied bounds, or infers a missing dimension.</summary>
    public void Resize(int? width, int? height, bool keepAspectRatio = true, CancellationToken cancellationToken = default) => Resize(width, height, keepAspectRatio, null, cancellationToken);
    /// <summary>Resizes every frame using an explicit shared resampling kernel.</summary>
    public void Resize(int? width, int? height, bool keepAspectRatio, Sampler? sampler, CancellationToken cancellationToken = default) =>
        Resize(new OfficeRasterResizeOptions { Width = width, Height = height, Fit = keepAspectRatio ? OfficeImageFit.Contain : OfficeImageFit.Stretch,
            ResamplingMode = sampler.HasValue ? Helpers.GetResampler(sampler.Value) : OfficeRasterResamplingMode.Bicubic }, cancellationToken);
    /// <summary>Resizes all frames with shared fit, resampling, and color-space options.</summary>
    public void Resize(OfficeRasterResizeOptions options, CancellationToken cancellationToken = default) {
        EnsureUsable();
        _frames = _frames.Resize(options, cancellationToken);
    }
    /// <summary>Scales each frame by a positive percentage.</summary>
    public void Resize(int percentage, CancellationToken cancellationToken = default) {
        EnsureUsable();
        _frames = _frames.Resize(percentage, cancellationToken);
    }
    /// <summary>Skews each image frame, expanding its canvas to retain the image.</summary>
    public void Skew(float degreesX, float degreesY, CancellationToken cancellationToken = default) => Apply(image => OfficeRasterTransforms.Skew(image, degreesX, degreesY, cancellationToken: cancellationToken), image => OfficeRasterTransforms.GetSkewedSize(image, degreesX, degreesY), cancellationToken: cancellationToken);
}

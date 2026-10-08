namespace ImagePlayground;

/// <summary>Geometric image edits delegated to the shared managed raster engine.</summary>
public partial class Image {
    /// <summary>Applies the stored EXIF orientation and resets it to the normal orientation.</summary>
    public void AutoOrient() {
        object? value = Metadata.ExifValues.FirstOrDefault(entry => entry.Tag.Equals(OfficeExifTag.Orientation))?.Value;
        int orientation = value == null ? 1 : Convert.ToInt32(value);
        Apply(image => OfficeRasterTransforms.AutoOrient(image, orientation), image => OfficeRasterTransforms.GetRotatedSize(image, orientation >= 5 ? 90 : 0));
        Metadata.SetExifValue(OfficeExifTag.Orientation, (ushort)1);
    }
    /// <summary>Mirrors each image frame along the selected axis.</summary>
    public void Flip(FlipMode flipMode) {
        if (!Enum.IsDefined(typeof(FlipMode), flipMode)) {
            throw new ArgumentOutOfRangeException(nameof(flipMode));
        }
        Apply(image => OfficeRasterTransforms.Flip(image, flipMode == FlipMode.Horizontal, flipMode == FlipMode.Vertical));
    }
    /// <summary>Rotates each frame clockwise in quarter turns.</summary>
    public void Rotate(RotateMode rotateMode) {
        if (!Enum.IsDefined(typeof(RotateMode), rotateMode)) {
            throw new ArgumentOutOfRangeException(nameof(rotateMode));
        }
        Rotate((float)rotateMode);
    }
    /// <summary>Rotates each frame clockwise, expanding the canvas to preserve the image.</summary>
    public void Rotate(float degrees) => Apply(image => OfficeRasterTransforms.Rotate(image, degrees), image => OfficeRasterTransforms.GetRotatedSize(image, degrees));
    /// <summary>Rotates and then mirrors each image frame.</summary>
    public void RotateFlip(RotateMode rotateMode, FlipMode flipMode) { Rotate(rotateMode); Flip(flipMode); }
    /// <summary>Resizes every frame. Preserving aspect ratio fits within both supplied bounds, or infers a missing dimension.</summary>
    public void Resize(int? width, int? height, bool keepAspectRatio = true) => Resize(width, height, keepAspectRatio, null);
    /// <summary>Resizes every frame using an explicit shared resampling kernel.</summary>
    public void Resize(int? width, int? height, bool keepAspectRatio, Sampler? sampler) => Apply(image => ImageHelper.Resize(image, width, height, keepAspectRatio, sampler), image => ImageHelper.GetResizeDimensions(image, width, height, keepAspectRatio));
    /// <summary>Scales each frame by a positive percentage.</summary>
    public void Resize(int percentage) {
        if (percentage <= 0) {
            throw new ArgumentOutOfRangeException(nameof(percentage));
        }
        Apply(image => OfficeRasterResampler.Resize(image, Math.Max(1, checked((int)((long)image.Width * percentage / 100))), Math.Max(1, checked((int)((long)image.Height * percentage / 100))), OfficeRasterResamplingMode.Bicubic), image => (Math.Max(1, checked((int)((long)image.Width * percentage / 100))), Math.Max(1, checked((int)((long)image.Height * percentage / 100)))));
    }
    /// <summary>Skews each image frame, expanding its canvas to retain the image.</summary>
    public void Skew(float degreesX, float degreesY) => Apply(image => OfficeRasterTransforms.Skew(image, degreesX, degreesY), image => OfficeRasterTransforms.GetSkewedSize(image, degreesX, degreesY));
}
using System.Collections.Generic;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;

namespace ImagePlayground;
/// <summary>
/// Provides image manipulation operations.
/// </summary>
public partial class Image {
    /// <summary>
    /// Gets all EXIF values from the image.
    /// </summary>
    /// <returns>List of EXIF values.</returns>
    public IReadOnlyList<IExifValue> GetExifValues() =>
        _image.Metadata.ExifProfile?.Values ?? new List<IExifValue>();

    /// <summary>
    /// Sets an EXIF value on the image.
    /// </summary>
    /// <param name="tag">Tag to set.</param>
    /// <param name="value">Value for the tag.</param>
    public void SetExifValue(ExifTag tag, object value) {
        _image.Metadata.ExifProfile ??= new ExifProfile();
        var method = typeof(ExifProfile).GetMethod("SetValue")!;
        var generic = method.MakeGenericMethod(tag.GetType().GenericTypeArguments[0]);
        generic.Invoke(_image.Metadata.ExifProfile, new[] { tag, value });
    }

    /// <summary>
    /// Removes specific EXIF values from the image.
    /// </summary>
    /// <param name="tags">Tags to remove.</param>
    public void RemoveExifValues(params ExifTag[] tags) {
        var profile = _image.Metadata.ExifProfile;
        if (profile is null) { return; }
        foreach (var t in tags) {
            profile.RemoveValue(t);
        }
    }

    /// <summary>
    /// Clears all EXIF values from the image.
    /// </summary>
    public void ClearExifValues() {
        var profile = _image.Metadata.ExifProfile;
        if (profile != null) {
            if (profile.Values is List<IExifValue> list) {
                list.Clear();
            }
        }
    }
}

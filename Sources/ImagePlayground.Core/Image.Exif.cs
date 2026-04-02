using System.Collections.Generic;
using System.Reflection;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;

namespace ImagePlayground;

/// <summary>
/// Provides image manipulation operations.
/// </summary>
public partial class Image {
    private static readonly MethodInfo SetExifValueGenericMethod = typeof(Image).GetMethod(nameof(SetExifValueGeneric), BindingFlags.Static | BindingFlags.NonPublic)!;

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
        if (value is null) {
            throw new ArgumentNullException(nameof(value));
        }

        var profile = _image.Metadata.ExifProfile;
        if (TrySetExifValue(profile, tag, value)) {
            return;
        }

        throw new ArgumentException($"Value of type '{value.GetType()}' does not match tag type '{tag.GetType()}'.", nameof(value));
    }

    private static bool TrySetExifValue(ExifProfile profile, ExifTag tag, object value) {
        if (!TryGetExifTagValueType(tag.GetType(), out Type tagValueType)) {
            return false;
        }

        if (!tagValueType.IsInstanceOfType(value)) {
            return false;
        }

        SetExifValueGenericMethod.MakeGenericMethod(tagValueType).Invoke(null, new[] { profile, tag, value });
        return true;
    }

    private static void SetExifValueGeneric<TValue>(ExifProfile profile, ExifTag<TValue> tag, TValue value) {
        profile.SetValue(tag, value);
    }

    private static bool TryGetExifTagValueType(Type tagType, out Type valueType) {
        var currentType = tagType;
        while (currentType != null) {
            if (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == typeof(ExifTag<>)) {
                valueType = currentType.GenericTypeArguments[0];
                return true;
            }

            currentType = currentType.BaseType;
        }

        valueType = null!;
        return false;
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

using System.Collections.Generic;
using SixLabors.ImageSharp;
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
        if (tag is ExifTag<byte> byteTag && value is byte byteValue) {
            profile.SetValue(byteTag, byteValue);
            return true;
        }
        if (tag is ExifTag<sbyte> sbyteTag && value is sbyte sbyteValue) {
            profile.SetValue(sbyteTag, sbyteValue);
            return true;
        }
        if (tag is ExifTag<short> shortTag && value is short shortValue) {
            profile.SetValue(shortTag, shortValue);
            return true;
        }
        if (tag is ExifTag<ushort> ushortTag && value is ushort ushortValue) {
            profile.SetValue(ushortTag, ushortValue);
            return true;
        }
        if (tag is ExifTag<int> intTag && value is int intValue) {
            profile.SetValue(intTag, intValue);
            return true;
        }
        if (tag is ExifTag<uint> uintTag && value is uint uintValue) {
            profile.SetValue(uintTag, uintValue);
            return true;
        }
        if (tag is ExifTag<long> longTag && value is long longValue) {
            profile.SetValue(longTag, longValue);
            return true;
        }
        if (tag is ExifTag<ulong> ulongTag && value is ulong ulongValue) {
            profile.SetValue(ulongTag, ulongValue);
            return true;
        }
        if (tag is ExifTag<float> floatTag && value is float floatValue) {
            profile.SetValue(floatTag, floatValue);
            return true;
        }
        if (tag is ExifTag<double> doubleTag && value is double doubleValue) {
            profile.SetValue(doubleTag, doubleValue);
            return true;
        }
        if (tag is ExifTag<string> stringTag && value is string stringValue) {
            profile.SetValue(stringTag, stringValue);
            return true;
        }
        if (tag is ExifTag<EncodedString> encodedTag && value is EncodedString encodedValue) {
            profile.SetValue(encodedTag, encodedValue);
            return true;
        }
        if (tag is ExifTag<Rational> rationalTag && value is Rational rationalValue) {
            profile.SetValue(rationalTag, rationalValue);
            return true;
        }
        if (tag is ExifTag<SignedRational> signedRationalTag && value is SignedRational signedRationalValue) {
            profile.SetValue(signedRationalTag, signedRationalValue);
            return true;
        }
        if (tag is ExifTag<byte[]> byteArrayTag && value is byte[] byteArrayValue) {
            profile.SetValue(byteArrayTag, byteArrayValue);
            return true;
        }
        if (tag is ExifTag<sbyte[]> sbyteArrayTag && value is sbyte[] sbyteArrayValue) {
            profile.SetValue(sbyteArrayTag, sbyteArrayValue);
            return true;
        }
        if (tag is ExifTag<short[]> shortArrayTag && value is short[] shortArrayValue) {
            profile.SetValue(shortArrayTag, shortArrayValue);
            return true;
        }
        if (tag is ExifTag<ushort[]> ushortArrayTag && value is ushort[] ushortArrayValue) {
            profile.SetValue(ushortArrayTag, ushortArrayValue);
            return true;
        }
        if (tag is ExifTag<int[]> intArrayTag && value is int[] intArrayValue) {
            profile.SetValue(intArrayTag, intArrayValue);
            return true;
        }
        if (tag is ExifTag<uint[]> uintArrayTag && value is uint[] uintArrayValue) {
            profile.SetValue(uintArrayTag, uintArrayValue);
            return true;
        }
        if (tag is ExifTag<long[]> longArrayTag && value is long[] longArrayValue) {
            profile.SetValue(longArrayTag, longArrayValue);
            return true;
        }
        if (tag is ExifTag<ulong[]> ulongArrayTag && value is ulong[] ulongArrayValue) {
            profile.SetValue(ulongArrayTag, ulongArrayValue);
            return true;
        }
        if (tag is ExifTag<float[]> floatArrayTag && value is float[] floatArrayValue) {
            profile.SetValue(floatArrayTag, floatArrayValue);
            return true;
        }
        if (tag is ExifTag<double[]> doubleArrayTag && value is double[] doubleArrayValue) {
            profile.SetValue(doubleArrayTag, doubleArrayValue);
            return true;
        }
        if (tag is ExifTag<string[]> stringArrayTag && value is string[] stringArrayValue) {
            profile.SetValue(stringArrayTag, stringArrayValue);
            return true;
        }
        if (tag is ExifTag<Rational[]> rationalArrayTag && value is Rational[] rationalArrayValue) {
            profile.SetValue(rationalArrayTag, rationalArrayValue);
            return true;
        }
        if (tag is ExifTag<SignedRational[]> signedRationalArrayTag && value is SignedRational[] signedRationalArrayValue) {
            profile.SetValue(signedRationalArrayTag, signedRationalArrayValue);
            return true;
        }
        if (tag is ExifTag<EncodedString[]> encodedArrayTag && value is EncodedString[] encodedArrayValue) {
            profile.SetValue(encodedArrayTag, encodedArrayValue);
            return true;
        }

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

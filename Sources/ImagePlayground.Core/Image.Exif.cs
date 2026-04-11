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
    private const string HeifExifWriteNotSupportedMessage = "Updating HEIF/HEIC EXIF requires an existing EXIF item with a single writable file extent. Creating a brand-new HEIF EXIF item is not supported yet.";
    private const string HeifExifReadNotSupportedMessage = "The HEIF/HEIC file declares an EXIF item, but the EXIF payload could not be read.";

    /// <summary>
    /// Gets all EXIF values from the image.
    /// </summary>
    /// <returns>List of EXIF values currently attached to the image, or an empty list when no EXIF profile exists.</returns>
    public IReadOnlyList<IExifValue> GetExifValues() =>
        _image.Metadata.ExifProfile?.Values ?? new List<IExifValue>();

    /// <summary>
    /// Gets all EXIF values from an image file without requiring callers to load pixels directly.
    /// </summary>
    /// <param name="filePath">Path to the image file.</param>
    /// <returns>List of EXIF values attached to the image, or an empty list when no EXIF profile exists.</returns>
    public static IReadOnlyList<IExifValue> GetExifValues(string filePath) {
        string fullPath = Helpers.ResolvePath(filePath);
        if (Helpers.IsHeifExtension(fullPath)) {
            if (HeifMetadataReader.TryReadExifProfile(fullPath, out ExifProfile? heifProfile)) {
                return heifProfile is null
                    ? new List<IExifValue>()
                    : new List<IExifValue>(heifProfile.Values);
            }

            return new List<IExifValue>();
        }

        using var image = Load(fullPath);
        return new List<IExifValue>(image.GetExifValues());
    }

    /// <summary>
    /// Sets an EXIF value on an image file.
    /// </summary>
    /// <param name="filePath">Path to the image file.</param>
    /// <param name="filePathOutput">Optional output path. When omitted, the input file is overwritten.</param>
    /// <param name="tag">Tag to set.</param>
    /// <param name="value">Value for the tag.</param>
    public static void SetExifValue(string filePath, string? filePathOutput, ExifTag tag, object value) {
        string fullPath = Helpers.ResolvePath(filePath);
        string outputPath = string.IsNullOrWhiteSpace(filePathOutput)
            ? fullPath
            : Helpers.ResolvePath(filePathOutput!);

        if (value is null) {
            throw new ArgumentNullException(nameof(value));
        }

        if (Helpers.IsHeifExtension(fullPath)) {
            ExifProfile profile;
            if (HeifMetadataReader.HasExifItem(fullPath)) {
                if (!HeifMetadataReader.TryReadExifProfile(fullPath, out ExifProfile? heifProfile) || heifProfile is null) {
                    throw new NotSupportedException(HeifExifReadNotSupportedMessage);
                }

                profile = heifProfile;
            } else {
                profile = new ExifProfile();
            }

            if (!TrySetExifValue(profile, tag, value)) {
                throw new ArgumentException($"Value of type '{value.GetType()}' does not match tag type '{tag.GetType()}'.", nameof(value));
            }

            if (!HeifMetadataReader.TryWriteExifProfile(fullPath, outputPath, profile)) {
                throw new NotSupportedException(HeifExifWriteNotSupportedMessage);
            }

            return;
        }

        using var img = Load(fullPath);
        img.SetExifValue(tag, value);
        img.Save(outputPath);
    }

    /// <summary>
    /// Removes specific EXIF values from an image file.
    /// </summary>
    /// <param name="filePath">Path to the image file.</param>
    /// <param name="filePathOutput">Optional output path. When omitted, the input file is overwritten.</param>
    /// <param name="tags">Tags to remove.</param>
    public static void RemoveExifValues(string filePath, string? filePathOutput, params ExifTag[] tags) {
        string fullPath = Helpers.ResolvePath(filePath);
        string outputPath = string.IsNullOrWhiteSpace(filePathOutput)
            ? fullPath
            : Helpers.ResolvePath(filePathOutput!);

        if (Helpers.IsHeifExtension(fullPath)) {
            if (!HeifMetadataReader.HasExifItem(fullPath)) {
                CopyIfNeeded(fullPath, outputPath);
                return;
            }

            if (!HeifMetadataReader.TryReadExifProfile(fullPath, out ExifProfile? profile) || profile is null) {
                throw new NotSupportedException(HeifExifReadNotSupportedMessage);
            }

            foreach (ExifTag tag in tags) {
                profile.RemoveValue(tag);
            }

            ExifProfile? profileToWrite = profile.Values.Count == 0
                ? null
                : profile;
            if (!HeifMetadataReader.TryWriteExifProfile(fullPath, outputPath, profileToWrite)) {
                throw new NotSupportedException(HeifExifWriteNotSupportedMessage);
            }

            return;
        }

        using var img = Load(fullPath);
        img.RemoveExifValues(tags);
        img.Save(outputPath);
    }

    /// <summary>
    /// Clears all EXIF values from an image file.
    /// </summary>
    /// <param name="filePath">Path to the image file.</param>
    /// <param name="filePathOutput">Optional output path. When omitted, the input file is overwritten.</param>
    public static void ClearExifValues(string filePath, string? filePathOutput) {
        string fullPath = Helpers.ResolvePath(filePath);
        string outputPath = string.IsNullOrWhiteSpace(filePathOutput)
            ? fullPath
            : Helpers.ResolvePath(filePathOutput!);

        if (Helpers.IsHeifExtension(fullPath)) {
            if (!HeifMetadataReader.HasExifItem(fullPath)) {
                CopyIfNeeded(fullPath, outputPath);
                return;
            }

            if (!HeifMetadataReader.TryWriteExifProfile(fullPath, outputPath, null)) {
                throw new NotSupportedException(HeifExifWriteNotSupportedMessage);
            }

            return;
        }

        using var img = Load(fullPath);
        img.ClearExifValues();
        img.Save(outputPath);
    }

    /// <summary>
    /// Sets an EXIF value on the image.
    /// </summary>
    /// <para>
    /// The supplied <paramref name="value"/> must match the value type declared by the specific
    /// <paramref name="tag"/>, including specialized ImageSharp EXIF value wrappers such as
    /// <see cref="Number"/>, <see cref="Rational"/>, or array-based tag payloads.
    /// </para>
    /// <param name="tag">Tag to set.</param>
    /// <param name="value">Value for the tag.</param>
    /// <example>
    ///   <code>image.SetExifValue(ExifTag.Software, "ImagePlayground");</code>
    /// </example>
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

    private static void CopyIfNeeded(string fullPath, string outputPath) {
        if (fullPath.Equals(outputPath, StringComparison.OrdinalIgnoreCase)) {
            return;
        }

        Helpers.CreateParentDirectory(outputPath);
        File.Copy(fullPath, outputPath, true);
    }

    /// <summary>
    /// Removes specific EXIF values from the image.
    /// </summary>
    /// <param name="tags">Tags to remove from the current EXIF profile.</param>
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
    /// <para>This removes all values from the existing EXIF profile but keeps the image object loaded and usable.</para>
    public void ClearExifValues() {
        var profile = _image.Metadata.ExifProfile;
        if (profile != null) {
            if (profile.Values is List<IExifValue> list) {
                list.Clear();
            }
        }
    }
}

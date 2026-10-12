namespace ImagePlayground;

/// <summary>EXIF metadata workflows over the shared typed profile model.</summary>
public partial class Image {
    /// <summary>Returns the decoded EXIF entries of the current image.</summary>
    public IReadOnlyList<OfficeExifValue> GetExifValues() => Metadata.ExifValues;
    /// <summary>Reads EXIF entries without decoding pixels.</summary>
    public static IReadOnlyList<OfficeExifValue> GetExifValues(string filePath) => ReadMetadataFile(Helpers.ResolvePath(filePath), OfficeImageMetadataProfileKinds.Exif).ExifValues;
    /// <summary>Sets a typed EXIF value on the current image metadata.</summary>
    public void SetExifValue(OfficeExifTag tag, object value) => Metadata.SetExifValue(tag, value);
    /// <summary>Removes selected EXIF values from the current image metadata.</summary>
    public void RemoveExifValues(params OfficeExifTag[] tags) { foreach (var tag in tags) Metadata.RemoveExifValue(tag); }
    /// <summary>Removes all EXIF entries from the current image metadata.</summary>
    public void ClearExifValues() => Metadata.ClearExif();
    /// <summary>Sets a typed EXIF value while preserving the encoded image data.</summary>
    public static void SetExifValue(string filePath, string? filePathOutput, OfficeExifTag tag, object value) {
        string fullPath = Helpers.ResolvePath(filePath);
        var metadata = ReadMetadataFile(fullPath, OfficeImageMetadataProfileKinds.Exif);
        metadata.SetExifValue(tag, value);
        WriteMetadataFile(fullPath, filePathOutput, metadata, OfficeImageMetadataProfileKinds.Exif);
    }
    /// <summary>Removes selected EXIF values while preserving the encoded image data.</summary>
    public static void RemoveExifValues(string filePath, string? filePathOutput, params OfficeExifTag[] tags) {
        string fullPath = Helpers.ResolvePath(filePath);
        var metadata = ReadMetadataFile(fullPath, OfficeImageMetadataProfileKinds.Exif);
        foreach (var tag in tags) {
            metadata.RemoveExifValue(tag);
        }
        WriteMetadataFile(fullPath, filePathOutput, metadata, OfficeImageMetadataProfileKinds.Exif);
    }
    /// <summary>Removes all EXIF values while preserving the encoded image data.</summary>
    public static void ClearExifValues(string filePath, string? filePathOutput) {
        string fullPath = Helpers.ResolvePath(filePath);
        var metadata = ReadMetadataFile(fullPath, OfficeImageMetadataProfileKinds.Exif);
        metadata.ClearExif();
        WriteMetadataFile(fullPath, filePathOutput, metadata, OfficeImageMetadataProfileKinds.Exif);
    }
    // HEIF callers select the requested profile families so an unrelated item is never parsed.
    internal static OfficeImageMetadata ReadMetadataFile(string fullPath, OfficeImageMetadataProfileKinds profiles = OfficeImageMetadataProfileKinds.All) {
        if (!Helpers.IsHeifExtension(fullPath)) {
            return OfficeImageMetadata.Read(Helpers.ReadEncodedFile(fullPath));
        }
        var metadata = new OfficeImageMetadata();
        if ((profiles & OfficeImageMetadataProfileKinds.Exif) != 0) {
            if (OfficeHeifMetadataReader.TryReadExifProfile(fullPath, out var exif)) {
                metadata = exif ?? metadata;
            } else if (OfficeHeifMetadataReader.HasExifItem(fullPath)) {
                throw new NotSupportedException("The declared HEIF EXIF item cannot be read.");
            }
        }
        if ((profiles & OfficeImageMetadataProfileKinds.Xmp) != 0) {
            if (OfficeHeifMetadataReader.TryReadXmp(fullPath, out var xmp)) {
                if (xmp != null) {
                    metadata.XmpProfile = Encoding.UTF8.GetBytes(xmp);
                }
            } else if (OfficeHeifMetadataReader.HasXmpItem(fullPath)) {
                throw new NotSupportedException("The declared HEIF XMP item cannot be read.");
            }
        }
        return metadata;
    }
    // A full import replaces all supported families; a selective edit touches only its requested item.
    internal static void WriteMetadataFile(string fullPath, string? filePathOutput, OfficeImageMetadata metadata, OfficeImageMetadataProfileKinds profiles = OfficeImageMetadataProfileKinds.All) {
        string output = string.IsNullOrEmpty(filePathOutput) ? fullPath : Helpers.ResolvePath(filePathOutput!);
        Helpers.CreateParentDirectory(output);
        if (Helpers.IsHeifExtension(fullPath)) {
            if (((profiles & OfficeImageMetadataProfileKinds.Icc) != 0 && metadata.IccProfile != null) ||
                ((profiles & OfficeImageMetadataProfileKinds.Iptc) != 0 && metadata.IptcProfile != null)) {
                throw new NotSupportedException("HEIF metadata import supports EXIF and XMP profiles.");
            }
            byte[] source = Helpers.ReadEncodedFile(fullPath);
            OfficeImageMetadataProfileKinds selected = OfficeImageMetadataProfileKinds.None;
            if ((profiles & OfficeImageMetadataProfileKinds.Exif) != 0) {
                bool hasExif = OfficeHeifMetadataReader.HasExifItem(source);
                if (!hasExif && metadata.HasExifProfile) {
                    throw new NotSupportedException("HEIF EXIF editing requires an existing writable metadata item.");
                }
                if (hasExif) { selected |= OfficeImageMetadataProfileKinds.Exif; }
            }
            if ((profiles & OfficeImageMetadataProfileKinds.Xmp) != 0) {
                bool hasXmp = OfficeHeifMetadataReader.HasXmpItem(source);
                if (!hasXmp && metadata.XmpProfile != null) {
                    throw new NotSupportedException("HEIF XMP editing requires an existing writable metadata item.");
                }
                if (hasXmp) { selected |= OfficeImageMetadataProfileKinds.Xmp; }
            }
            if (!OfficeHeifMetadataReader.TryWriteMetadata(source, metadata, selected, out byte[]? completed)) {
                throw new NotSupportedException("The selected HEIF metadata items cannot be rewritten. Requested items must use supported writable layouts, and XMP import requires valid UTF-8 metadata.");
            }
            OfficeImageFileWriter.WriteAllBytes(output, completed!);
        } else {
            OfficeImageFileWriter.WriteAllBytes(output, OfficeImageMetadata.Apply(Helpers.ReadEncodedFile(fullPath), metadata));
        }
    }
}

using System.Collections.Generic;
using System.Text;
using SixLabors.ImageSharp.Metadata;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;

namespace ImagePlayground;

/// <summary>
/// Represents a metadata snapshot read from an image.
/// </summary>
public sealed class ImageMetadataInfo {
    private readonly byte[]? _exifProfile;
    private readonly byte[]? _xmpProfile;
    private readonly byte[]? _iccProfile;
    private readonly byte[]? _iptcProfile;

    internal ImageMetadataInfo(
        string filePath,
        double? horizontalResolution,
        double? verticalResolution,
        PixelResolutionUnit? resolutionUnits,
        byte[]? exifProfile,
        byte[]? xmpProfile,
        byte[]? iccProfile,
        byte[]? iptcProfile,
        ImageProvenanceInfo provenance) {
        FilePath = filePath;
        HorizontalResolution = horizontalResolution;
        VerticalResolution = verticalResolution;
        ResolutionUnits = resolutionUnits;
        _exifProfile = Clone(exifProfile);
        _xmpProfile = Clone(xmpProfile);
        _iccProfile = Clone(iccProfile);
        _iptcProfile = Clone(iptcProfile);
        Provenance = provenance;

        ExifValues = _exifProfile is null
            ? new List<IExifValue>().AsReadOnly()
            : new List<IExifValue>(new ExifProfile(_exifProfile).Values).AsReadOnly();
    }

    /// <summary>Resolved path to the inspected image.</summary>
    public string FilePath { get; }

    /// <summary>Horizontal resolution, or <c>null</c> when the format reader does not expose it.</summary>
    public double? HorizontalResolution { get; }

    /// <summary>Vertical resolution, or <c>null</c> when the format reader does not expose it.</summary>
    public double? VerticalResolution { get; }

    /// <summary>Resolution measurement unit, or <c>null</c> when the format reader does not expose it.</summary>
    public PixelResolutionUnit? ResolutionUnits { get; }

    /// <summary>Decoded EXIF values.</summary>
    public IReadOnlyList<IExifValue> ExifValues { get; }

    /// <summary>Raw serialized EXIF profile, or <c>null</c> when absent.</summary>
    public byte[]? ExifProfile => Clone(_exifProfile);

    /// <summary>Raw serialized XMP profile, or <c>null</c> when absent.</summary>
    public byte[]? XmpProfile => Clone(_xmpProfile);

    /// <summary>XMP profile decoded as UTF-8 text, or <c>null</c> when absent.</summary>
    public string? XmpText => _xmpProfile is null ? null : Encoding.UTF8.GetString(_xmpProfile);

    /// <summary>Raw serialized ICC profile, or <c>null</c> when absent.</summary>
    public byte[]? IccProfile => Clone(_iccProfile);

    /// <summary>Raw serialized IPTC profile, or <c>null</c> when absent.</summary>
    public byte[]? IptcProfile => Clone(_iptcProfile);

    /// <summary>Detected C2PA containers and direct XMP generative-AI declarations.</summary>
    public ImageProvenanceInfo Provenance { get; }

    /// <summary>Whether the image contains an EXIF profile.</summary>
    public bool HasExif => _exifProfile is not null;

    /// <summary>Whether the image contains an XMP profile.</summary>
    public bool HasXmp => _xmpProfile is not null;

    /// <summary>Whether the image contains an ICC profile.</summary>
    public bool HasIcc => _iccProfile is not null;

    /// <summary>Whether the image contains an IPTC profile.</summary>
    public bool HasIptc => _iptcProfile is not null;

    private static byte[]? Clone(byte[]? value) => value is null ? null : (byte[])value.Clone();
}
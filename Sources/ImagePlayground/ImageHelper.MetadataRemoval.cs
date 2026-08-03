using System.IO;
using SixLabors.ImageSharp;

namespace ImagePlayground;

/// <summary>
/// Provides helper methods for image manipulation.
/// </summary>
public partial class ImageHelper {
    private static readonly byte[] ExifJpegPrefix = Encoding.ASCII.GetBytes("Exif\0\0");
    private static readonly byte[] XmpJpegPrefix = Encoding.ASCII.GetBytes("http://ns.adobe.com/xap/1.0/\0");
    private static readonly byte[] ExtendedXmpJpegPrefix = Encoding.ASCII.GetBytes("http://ns.adobe.com/xmp/extension/\0");
    private static readonly byte[] IccJpegPrefix = Encoding.ASCII.GetBytes("ICC_PROFILE\0");
    private static readonly byte[] PhotoshopJpegPrefix = Encoding.ASCII.GetBytes("Photoshop 3.0\0");

    /// <summary>
    /// Removes selected metadata while preserving encoded JPEG and PNG image data.
    /// </summary>
    /// <param name="options">Removal options.</param>
    /// <returns>Removal outcome, including the metadata families that were present.</returns>
    /// <remarks>
    /// JPEG and PNG files are rewritten at the segment or chunk level, so compressed image data is copied byte for byte.
    /// Other ImageSharp-supported formats may require re-encoding. HEIF and HEIC currently support EXIF and XMP removal.
    /// </remarks>
    public static ImageMetadataRemovalResult RemoveMetadata(ImageMetadataRemovalOptions options) {
        if (options is null) {
            throw new ArgumentNullException(nameof(options));
        }

        ImageMetadataType requested = ValidateMetadataTypes(options.MetadataTypes);
        string fullPath = Helpers.ResolvePath(options.FilePath);
        string outFullPath = Helpers.ResolvePath(options.OutputPath);
        Directory.CreateDirectory(Path.GetDirectoryName(outFullPath)!);
        long originalLength = new FileInfo(fullPath).Length;

        if (requested == ImageMetadataType.None) {
            CopyIfDifferent(fullPath, outFullPath);
            return CreateRemovalResult(
                fullPath,
                outFullPath,
                requested,
                ImageMetadataType.None,
                false,
                originalLength);
        }

        if (Helpers.IsHeifExtension(fullPath)) {
            ImageMetadataType removedHeif = RemoveHeifMetadata(fullPath, outFullPath, requested);
            return CreateRemovalResult(fullPath, outFullPath, requested, removedHeif, false, originalLength);
        }

        byte[] input = File.ReadAllBytes(fullPath);
        if (HasPrefix(input, input.Length, PngSignature)) {
            byte[] output = RemovePngMetadata(input, requested, out ImageMetadataType removedPng);
            File.WriteAllBytes(outFullPath, output);
            return CreateRemovalResult(fullPath, outFullPath, requested, removedPng, false, originalLength);
        }

        if (HasPrefix(input, input.Length, JpegSignature)) {
            byte[] output = RemoveJpegMetadata(input, requested, out ImageMetadataType removedJpeg);
            File.WriteAllBytes(outFullPath, output);
            return CreateRemovalResult(fullPath, outFullPath, requested, removedJpeg, false, originalLength);
        }

        if ((requested & ImageMetadataType.C2pa) != 0 && requested != ImageMetadataType.All) {
            throw new NotSupportedException("C2PA removal is currently supported for JPEG and PNG images.");
        }

        ImageMetadataType removed = RemoveMetadataWithImageSharp(fullPath, outFullPath, requested);
        return CreateRemovalResult(fullPath, outFullPath, requested, removed, true, originalLength);
    }

    private static ImageMetadataRemovalResult CreateRemovalResult(
        string fullPath,
        string outFullPath,
        ImageMetadataType requested,
        ImageMetadataType removed,
        bool wasReencoded,
        long originalLength) =>
        new ImageMetadataRemovalResult(
            fullPath,
            outFullPath,
            requested,
            removed,
            wasReencoded,
            originalLength,
            new FileInfo(outFullPath).Length);

    private static ImageMetadataType ValidateMetadataTypes(ImageMetadataType metadataTypes) {
        if ((metadataTypes & ~ImageMetadataType.All) != 0) {
            throw new ArgumentOutOfRangeException(nameof(metadataTypes), metadataTypes, "Unknown metadata type flag.");
        }

        return metadataTypes;
    }

    private static byte[] RemovePngMetadata(byte[] input, ImageMetadataType requested, out ImageMetadataType removed) {
        removed = ImageMetadataType.None;
        using var output = new MemoryStream(input.Length);
        output.Write(input, 0, PngSignature.Length);
        int offset = PngSignature.Length;
        bool foundEnd = false;

        while (offset < input.Length) {
            if (input.Length - offset < 12) {
                throw new InvalidDataException("PNG contains a truncated chunk.");
            }

            uint payloadLengthValue = ReadUInt32BigEndian(input, offset);
            if (payloadLengthValue > int.MaxValue) {
                throw new InvalidDataException("PNG chunk is too large.");
            }

            int payloadLength = (int)payloadLengthValue;
            long chunkLengthValue = 12L + payloadLength;
            if (chunkLengthValue > input.Length - offset) {
                throw new InvalidDataException("PNG chunk length exceeds the remaining file size.");
            }

            int chunkLength = (int)chunkLengthValue;
            string chunkType = Encoding.ASCII.GetString(input, offset + 4, 4);
            ImageMetadataType chunkMetadataType = GetPngMetadataType(input, offset + 8, payloadLength, chunkType);
            if (chunkMetadataType != ImageMetadataType.None && (requested & chunkMetadataType) != 0) {
                removed |= chunkMetadataType;
            } else {
                output.Write(input, offset, chunkLength);
            }

            offset += chunkLength;
            if (chunkType.Equals("IEND", StringComparison.Ordinal)) {
                foundEnd = true;
                break;
            }
        }

        if (!foundEnd) {
            throw new InvalidDataException("PNG does not contain an IEND chunk.");
        }

        if (offset < input.Length) {
            output.Write(input, offset, input.Length - offset);
        }

        return output.ToArray();
    }

    private static ImageMetadataType GetPngMetadataType(byte[] input, int payloadOffset, int payloadLength, string chunkType) {
        if (chunkType.Equals("caBX", StringComparison.Ordinal)) {
            return ImageMetadataType.C2pa;
        }

        if (chunkType.Equals("eXIf", StringComparison.Ordinal)) {
            return ImageMetadataType.Exif;
        }

        if (chunkType.Equals("iCCP", StringComparison.Ordinal)) {
            return ImageMetadataType.Icc;
        }

        if (!chunkType.Equals("iTXt", StringComparison.Ordinal)
            && !chunkType.Equals("tEXt", StringComparison.Ordinal)
            && !chunkType.Equals("zTXt", StringComparison.Ordinal)) {
            return ImageMetadataType.None;
        }

        string keyword = ReadPngTextKeyword(input, payloadOffset, payloadLength);
        if (keyword.Equals("XML:com.adobe.xmp", StringComparison.OrdinalIgnoreCase)) {
            return ImageMetadataType.Xmp;
        }

        if (keyword.Equals("Raw profile type exif", StringComparison.OrdinalIgnoreCase)) {
            return ImageMetadataType.Exif;
        }

        if (keyword.Equals("Raw profile type iptc", StringComparison.OrdinalIgnoreCase)
            || keyword.Equals("IPTC", StringComparison.OrdinalIgnoreCase)) {
            return ImageMetadataType.Iptc;
        }

        return ImageMetadataType.None;
    }

    private static string ReadPngTextKeyword(byte[] input, int payloadOffset, int payloadLength) {
        int end = payloadOffset + payloadLength;
        int current = payloadOffset;
        while (current < end && input[current] != 0) {
            current++;
        }

        return Encoding.ASCII.GetString(input, payloadOffset, current - payloadOffset);
    }

    private static byte[] RemoveJpegMetadata(byte[] input, ImageMetadataType requested, out ImageMetadataType removed) {
        removed = ImageMetadataType.None;
        using var output = new MemoryStream(input.Length);
        int offset = 0;
        bool foundImage = false;

        while (offset < input.Length) {
            int imageStart = FindNextJpegStart(input, offset);
            if (imageStart < 0) {
                if (!foundImage) {
                    throw new InvalidDataException("JPEG start marker was not found.");
                }

                output.Write(input, offset, input.Length - offset);
                break;
            }

            output.Write(input, offset, imageStart - offset);
            offset = RemoveJpegImageMetadata(input, imageStart, requested, output, ref removed);
            foundImage = true;
        }

        return output.ToArray();
    }

    private static int RemoveJpegImageMetadata(
        byte[] input,
        int imageStart,
        ImageMetadataType requested,
        Stream output,
        ref ImageMetadataType removed) {
        output.Write(input, imageStart, JpegSignature.Length);
        int offset = imageStart + JpegSignature.Length;

        while (offset < input.Length) {
            int segmentStart = offset;
            if (input[offset] != 0xFF) {
                throw new InvalidDataException("Invalid JPEG marker prefix.");
            }

            while (offset < input.Length && input[offset] == 0xFF) {
                offset++;
            }

            if (offset >= input.Length) {
                throw new EndOfStreamException("Unexpected end of JPEG data.");
            }

            byte marker = input[offset++];
            if (marker == 0xD9) {
                output.Write(input, segmentStart, offset - segmentStart);
                return offset;
            }

            if (marker == 0x01 || (marker >= 0xD0 && marker <= 0xD7)) {
                output.Write(input, segmentStart, offset - segmentStart);
                continue;
            }

            if (input.Length - offset < 2) {
                throw new EndOfStreamException("Unexpected end of JPEG segment length.");
            }

            int segmentLength = (input[offset] << 8) | input[offset + 1];
            if (segmentLength < 2 || segmentLength > input.Length - offset) {
                throw new InvalidDataException("JPEG segment length exceeds the remaining file size.");
            }

            int segmentEnd = offset + segmentLength;
            int payloadOffset = offset + 2;
            int payloadLength = segmentLength - 2;
            if (marker == 0xDA) {
                int imageEnd = FindJpegEndOffset(input, segmentStart);
                output.Write(input, segmentStart, imageEnd - segmentStart);
                return imageEnd;
            }

            if (marker == 0xEB
                && (requested & ImageMetadataType.C2pa) != 0
                && TryGetC2paJpegSequenceEnd(input, segmentStart, payloadOffset, payloadLength, out int c2paEnd)) {
                removed |= ImageMetadataType.C2pa;
                offset = c2paEnd;
                continue;
            }

            ImageMetadataType segmentMetadataType = GetJpegMetadataType(input, payloadOffset, payloadLength, marker);
            bool removeSegment = segmentMetadataType != ImageMetadataType.None && (requested & segmentMetadataType) != 0;

            if (removeSegment) {
                removed |= segmentMetadataType;
            } else if (marker == 0xED && (requested & ImageMetadataType.Iptc) != 0
                && StartsWith(input, payloadOffset, payloadLength, PhotoshopJpegPrefix)) {
                byte[]? rewrittenPayload = RemoveIptcPhotoshopResource(input, payloadOffset, payloadLength, out bool removedIptc);
                if (removedIptc) {
                    removed |= ImageMetadataType.Iptc;
                    if (rewrittenPayload is not null) {
                        WriteJpegSegment(output, marker, rewrittenPayload);
                    }
                } else {
                    output.Write(input, segmentStart, segmentEnd - segmentStart);
                }
            } else {
                output.Write(input, segmentStart, segmentEnd - segmentStart);
            }

            offset = segmentEnd;
        }

        throw new InvalidDataException("JPEG does not contain an end marker.");
    }

    private static ImageMetadataType GetJpegMetadataType(byte[] input, int payloadOffset, int payloadLength, byte marker) {
        if (marker == 0xE1) {
            if (StartsWith(input, payloadOffset, payloadLength, ExifJpegPrefix)) {
                return ImageMetadataType.Exif;
            }

            if (StartsWith(input, payloadOffset, payloadLength, XmpJpegPrefix)
                || StartsWith(input, payloadOffset, payloadLength, ExtendedXmpJpegPrefix)) {
                return ImageMetadataType.Xmp;
            }
        } else if (marker == 0xE2 && StartsWith(input, payloadOffset, payloadLength, IccJpegPrefix)) {
            return ImageMetadataType.Icc;
        }

        return ImageMetadataType.None;
    }

    private static bool IsC2paJpegApp11(byte[] input, int payloadOffset, int payloadLength) {
        return payloadLength >= 46
            && input[payloadOffset] == 0x4A
            && input[payloadOffset + 1] == 0x50
            && ReadUInt32BigEndian(input, payloadOffset + 4) == 1
            && ReadUInt32BigEndian(input, payloadOffset + 8) >= 38
            && Matches(input, payloadOffset + 12, "jumb")
            && ReadUInt32BigEndian(input, payloadOffset + 16) == 30
            && Matches(input, payloadOffset + 20, "jumd")
            && StartsWith(input, payloadOffset + 24, payloadLength - 24, C2paUuid)
            && StartsWith(input, payloadOffset + 41, payloadLength - 41, Encoding.ASCII.GetBytes("c2pa\0"));
    }

    private static bool IsJpegXtContinuation(
        byte[] input,
        int payloadOffset,
        int payloadLength,
        byte instanceHigh,
        byte instanceLow,
        uint expectedSequence) {
        return payloadLength >= 8
            && input[payloadOffset] == 0x4A
            && input[payloadOffset + 1] == 0x50
            && input[payloadOffset + 2] == instanceHigh
            && input[payloadOffset + 3] == instanceLow
            && ReadUInt32BigEndian(input, payloadOffset + 4) == expectedSequence;
    }

    private static bool TryGetC2paJpegSequenceEnd(
        byte[] input,
        int segmentStart,
        int payloadOffset,
        int payloadLength,
        out int sequenceEnd) {
        sequenceEnd = segmentStart;
        if (!IsC2paJpegApp11(input, payloadOffset, payloadLength)) {
            return false;
        }

        uint boxLength = ReadUInt32BigEndian(input, payloadOffset + 8);
        long collectedBoxBytes = payloadLength - 8L;
        int current = payloadOffset + payloadLength;
        if (collectedBoxBytes == boxLength) {
            sequenceEnd = current;
            return true;
        }

        if (collectedBoxBytes > boxLength) {
            return false;
        }

        byte instanceHigh = input[payloadOffset + 2];
        byte instanceLow = input[payloadOffset + 3];
        uint expectedSequence = 2;
        while (collectedBoxBytes < boxLength) {
            if (!TryReadJpegSegment(input, current, out byte marker, out int nextPayloadOffset, out int nextPayloadLength, out int segmentEnd)
                || marker != 0xEB
                || !IsJpegXtContinuation(
                    input,
                    nextPayloadOffset,
                    nextPayloadLength,
                    instanceHigh,
                    instanceLow,
                    expectedSequence)) {
                return false;
            }

            collectedBoxBytes += nextPayloadLength - 8L;
            if (collectedBoxBytes > boxLength) {
                return false;
            }

            current = segmentEnd;
            expectedSequence++;
        }

        sequenceEnd = current;
        return true;
    }

    private static bool TryReadJpegSegment(
        byte[] input,
        int segmentStart,
        out byte marker,
        out int payloadOffset,
        out int payloadLength,
        out int segmentEnd) {
        marker = 0;
        payloadOffset = 0;
        payloadLength = 0;
        segmentEnd = segmentStart;
        if (segmentStart < 0 || segmentStart >= input.Length || input[segmentStart] != 0xFF) {
            return false;
        }

        int offset = segmentStart;
        while (offset < input.Length && input[offset] == 0xFF) {
            offset++;
        }

        if (offset >= input.Length) {
            return false;
        }

        marker = input[offset++];
        if (marker == 0x00 || marker == 0x01 || marker == 0xD8 || marker == 0xD9 || (marker >= 0xD0 && marker <= 0xD7)
            || input.Length - offset < 2) {
            return false;
        }

        int segmentLength = (input[offset] << 8) | input[offset + 1];
        if (segmentLength < 2 || segmentLength > input.Length - offset) {
            return false;
        }

        payloadOffset = offset + 2;
        payloadLength = segmentLength - 2;
        segmentEnd = offset + segmentLength;
        return true;
    }

    private static int FindNextJpegStart(byte[] input, int offset) {
        for (int index = Math.Max(0, offset); index <= input.Length - 3; index++) {
            if (input[index] == 0xFF && input[index + 1] == 0xD8 && input[index + 2] == 0xFF) {
                return index;
            }
        }

        return -1;
    }

    private static int FindJpegEndOffset(byte[] input, int markerStart) {
        int offset = markerStart;
        while (offset < input.Length) {
            if (!TryReadJpegMarker(input, offset, out byte marker, out int markerEnd)) {
                throw new InvalidDataException("JPEG contains an invalid marker after scan data.");
            }

            if (marker == 0xD9) {
                return markerEnd;
            }

            offset = marker == 0xDA
                ? FindNextJpegMarkerInScan(input, markerEnd)
                : markerEnd;
        }

        throw new InvalidDataException("JPEG does not contain an end marker.");
    }

    private static bool TryReadJpegMarker(byte[] input, int markerStart, out byte marker, out int markerEnd) {
        marker = 0;
        markerEnd = markerStart;
        if (markerStart < 0 || markerStart >= input.Length || input[markerStart] != 0xFF) {
            return false;
        }

        int offset = markerStart;
        while (offset < input.Length && input[offset] == 0xFF) {
            offset++;
        }

        if (offset >= input.Length) {
            return false;
        }

        marker = input[offset++];
        if (marker == 0x00) {
            return false;
        }

        if (marker == 0x01 || marker == 0xD8 || marker == 0xD9 || (marker >= 0xD0 && marker <= 0xD7)) {
            markerEnd = offset;
            return true;
        }

        if (input.Length - offset < 2) {
            return false;
        }

        int segmentLength = (input[offset] << 8) | input[offset + 1];
        if (segmentLength < 2 || segmentLength > input.Length - offset) {
            return false;
        }

        markerEnd = offset + segmentLength;
        return true;
    }

    private static int FindNextJpegMarkerInScan(byte[] input, int offset) {
        while (offset < input.Length) {
            if (input[offset] != 0xFF) {
                offset++;
                continue;
            }

            int markerStart = offset;
            while (offset < input.Length && input[offset] == 0xFF) {
                offset++;
            }

            if (offset >= input.Length) {
                break;
            }

            byte marker = input[offset];
            if (marker == 0x00 || (marker >= 0xD0 && marker <= 0xD7)) {
                offset++;
                continue;
            }

            return markerStart;
        }

        throw new InvalidDataException("JPEG scan data does not contain an end marker.");
    }

    private static byte[]? RemoveIptcPhotoshopResource(
        byte[] input,
        int payloadOffset,
        int payloadLength,
        out bool removedIptc) {
        removedIptc = false;
        using var output = new MemoryStream(payloadLength);
        output.Write(PhotoshopJpegPrefix, 0, PhotoshopJpegPrefix.Length);
        int current = payloadOffset + PhotoshopJpegPrefix.Length;
        int end = payloadOffset + payloadLength;
        int preservedResources = 0;

        while (current < end) {
            int resourceStart = current;
            if (end - current < 7) {
                throw new InvalidDataException("JPEG Photoshop APP13 resource is truncated.");
            }

            bool validSignature = Matches(input, current, "8BIM") || Matches(input, current, "8B64");
            if (!validSignature) {
                throw new InvalidDataException("JPEG Photoshop APP13 resource signature is invalid.");
            }

            current += 4;
            int resourceId = (input[current] << 8) | input[current + 1];
            current += 2;
            int nameLength = input[current];
            int nameFieldLength = 1 + nameLength;
            current += nameFieldLength;
            if ((nameFieldLength & 1) != 0) {
                current++;
            }

            if (current > end - 4) {
                throw new InvalidDataException("JPEG Photoshop APP13 resource name is truncated.");
            }

            uint dataLengthValue = ReadUInt32BigEndian(input, current);
            current += 4;
            if (dataLengthValue > int.MaxValue) {
                throw new InvalidDataException("JPEG Photoshop APP13 resource is too large.");
            }

            int dataLength = (int)dataLengthValue;
            long resourceEndValue = (long)current + dataLength + (dataLength & 1);
            if (resourceEndValue > end) {
                throw new InvalidDataException("JPEG Photoshop APP13 resource data is truncated.");
            }

            current = (int)resourceEndValue;
            if (resourceId == 0x0404) {
                removedIptc = true;
            } else {
                output.Write(input, resourceStart, current - resourceStart);
                preservedResources++;
            }
        }

        if (!removedIptc) {
            return null;
        }

        return preservedResources == 0 ? null : output.ToArray();
    }

    private static void WriteJpegSegment(Stream output, byte marker, byte[] payload) {
        int segmentLength = payload.Length + 2;
        if (segmentLength > ushort.MaxValue) {
            throw new InvalidDataException("Rewritten JPEG segment exceeds the maximum segment size.");
        }

        output.WriteByte(0xFF);
        output.WriteByte(marker);
        output.WriteByte((byte)(segmentLength >> 8));
        output.WriteByte((byte)segmentLength);
        output.Write(payload, 0, payload.Length);
    }

    private static ImageMetadataType RemoveMetadataWithImageSharp(
        string fullPath,
        string outFullPath,
        ImageMetadataType requested) {
        using var image = Image.Load(fullPath);
        ImageMetadataType removed = ImageMetadataType.None;
        if ((requested & ImageMetadataType.Exif) != 0 && image.Metadata.ExifProfile is not null) {
            image.Metadata.ExifProfile = null;
            removed |= ImageMetadataType.Exif;
        }

        if ((requested & ImageMetadataType.Xmp) != 0 && image.Metadata.XmpProfile is not null) {
            image.Metadata.XmpProfile = null;
            removed |= ImageMetadataType.Xmp;
        }

        if ((requested & ImageMetadataType.Icc) != 0 && image.Metadata.IccProfile is not null) {
            image.Metadata.IccProfile = null;
            removed |= ImageMetadataType.Icc;
        }

        if ((requested & ImageMetadataType.Iptc) != 0 && image.Metadata.IptcProfile is not null) {
            image.Metadata.IptcProfile = null;
            removed |= ImageMetadataType.Iptc;
        }

        image.Save(outFullPath);
        return removed;
    }

    private static bool StartsWith(byte[] value, int offset, int availableLength, byte[] expected) {
        return expected.Length <= availableLength && StartsWith(value, offset, expected);
    }
}

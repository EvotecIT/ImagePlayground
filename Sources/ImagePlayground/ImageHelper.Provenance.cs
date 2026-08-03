using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace ImagePlayground;

/// <summary>
/// Provides helper methods for image manipulation.
/// </summary>
public partial class ImageHelper {
    private const string TrainedAlgorithmicMedia = "trainedAlgorithmicMedia";
    private const string CompositeWithTrainedAlgorithmicMedia = "compositeWithTrainedAlgorithmicMedia";
    private const string DigitalSourceTypeVocabulary = "cv.iptc.org/newscodes/digitalsourcetype/";
    private static readonly byte[] PngSignature = { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
    private static readonly byte[] JpegSignature = { 0xFF, 0xD8 };
    private static readonly byte[] C2paUuid = {
        0x63, 0x32, 0x70, 0x61, 0x00, 0x11, 0x00, 0x10,
        0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71
    };

    /// <summary>
    /// Inspects an image for embedded C2PA Content Credentials and standardized XMP generative-AI declarations.
    /// </summary>
    /// <param name="filePath">Path to the image to inspect.</param>
    /// <returns>Detected provenance signals and their metadata sources.</returns>
    /// <remarks>
    /// The inspection detects JPEG APP11 and PNG caBX C2PA containers but does not interpret or validate their
    /// active manifests. A conforming C2PA validator is required to determine what the active claim declares and
    /// to validate signatures, trust chains, and asset hashes. Direct XMP DigitalSourceType declarations are parsed
    /// for ImageSharp-supported formats.
    /// </remarks>
    public static ImageProvenanceInfo InspectProvenance(string filePath) {
        string fullPath = Helpers.ResolvePath(filePath);
        byte[]? xmp;

        if (Helpers.IsHeifExtension(fullPath)) {
            if (HeifMetadataReader.TryReadXmp(fullPath, out string? heifXmp)) {
                xmp = heifXmp is null ? null : Encoding.UTF8.GetBytes(heifXmp);
            } else {
                xmp = null;
            }
        } else {
            SixLabors.ImageSharp.IImageInfo? imageInfo = SixLabors.ImageSharp.Image.Identify(fullPath);
            xmp = imageInfo?.Metadata.XmpProfile?.ToByteArray();
        }

        return InspectProvenanceCore(fullPath, xmp);
    }

    private static ImageProvenanceInfo InspectProvenanceCore(string fullPath, byte[]? xmp) {
        var evidence = new List<ImageProvenanceEvidence>();

        using (FileStream stream = File.OpenRead(fullPath)) {
            byte[] signature = new byte[PngSignature.Length];
            int signatureLength = ReadUpTo(stream, signature, 0, signature.Length);
            stream.Position = 0;

            if (HasPrefix(signature, signatureLength, PngSignature)) {
                InspectPngProvenance(stream, evidence);
            } else if (HasPrefix(signature, signatureLength, JpegSignature)) {
                InspectJpegProvenance(File.ReadAllBytes(fullPath), evidence);
            }
        }

        if (xmp is not null) {
            InspectXmpProvenance(xmp, evidence);
        }

        return new ImageProvenanceInfo(fullPath, evidence.AsReadOnly());
    }

    private static void InspectPngProvenance(Stream stream, List<ImageProvenanceEvidence> evidence) {
        SkipExactly(stream, PngSignature.Length);

        while (stream.Position + 12 <= stream.Length) {
            uint chunkLength = ReadUInt32BigEndian(stream);
            byte[] chunkType = ReadExactly(stream, 4);
            long payloadLength = chunkLength;
            if (payloadLength > stream.Length - stream.Position - 4) {
                throw new InvalidDataException("PNG chunk length exceeds the remaining file size.");
            }

            if (Matches(chunkType, "caBX")) {
                AddEvidence(evidence, ImageProvenanceSource.C2pa, ImageProvenanceSignal.C2paManifest, "C2PA");
            }

            SkipExactly(stream, payloadLength + 4);
            if (Matches(chunkType, "IEND")) {
                break;
            }
        }
    }

    private static void InspectJpegProvenance(byte[] input, List<ImageProvenanceEvidence> evidence) {
        int searchOffset = 0;
        while (searchOffset < input.Length) {
            int imageStart = FindNextJpegStart(input, searchOffset);
            if (imageStart < 0) {
                return;
            }

            int offset = imageStart + JpegSignature.Length;
            while (offset < input.Length) {
                int segmentStart = offset;
                if (!TryReadJpegMarker(input, segmentStart, out byte marker, out int segmentEnd)) {
                    throw new InvalidDataException("JPEG contains an invalid marker.");
                }

                if (marker == 0xD9) {
                    searchOffset = segmentEnd;
                    break;
                }

                if (marker == 0xDA) {
                    searchOffset = FindJpegEndOffset(input, segmentStart);
                    break;
                }

                if (marker != 0x01 && marker != 0xD8 && !(marker >= 0xD0 && marker <= 0xD7)) {
                    int markerCodeOffset = segmentStart;
                    while (markerCodeOffset < input.Length && input[markerCodeOffset] == 0xFF) {
                        markerCodeOffset++;
                    }

                    int payloadOffset = markerCodeOffset + 3;
                    int payloadLength = segmentEnd - payloadOffset;
                    if (marker == 0xEB
                        && TryGetC2paJpegSequenceEnd(input, segmentStart, payloadOffset, payloadLength, out _)) {
                        AddEvidence(evidence, ImageProvenanceSource.C2pa, ImageProvenanceSignal.C2paManifest, "C2PA");
                        return;
                    }
                }

                offset = segmentEnd;
            }

            if (offset >= input.Length) {
                return;
            }
        }
    }

    private static void InspectXmpProvenance(byte[] xmp, List<ImageProvenanceEvidence> evidence) {
        XDocument document;
        try {
            using var stream = new MemoryStream(xmp, false);
            document = XDocument.Load(stream, LoadOptions.None);
        } catch (XmlException) {
            return;
        }

        foreach (XElement element in document.Descendants()) {
            foreach (XAttribute attribute in element.Attributes()) {
                if (attribute.Name.LocalName.Equals("DigitalSourceType", StringComparison.Ordinal)) {
                    AddDigitalSourceTypeEvidence(attribute.Value, evidence);
                }
            }

            if (!element.Name.LocalName.Equals("DigitalSourceType", StringComparison.Ordinal)) {
                continue;
            }

            AddDigitalSourceTypeEvidence(element.Value, evidence);
            foreach (XAttribute attribute in element.Attributes()) {
                if (attribute.Name.LocalName.Equals("resource", StringComparison.Ordinal)
                    || attribute.Name.LocalName.Equals("value", StringComparison.Ordinal)) {
                    AddDigitalSourceTypeEvidence(attribute.Value, evidence);
                }
            }

            foreach (XElement descendant in element.Descendants()) {
                AddDigitalSourceTypeEvidence(descendant.Value, evidence);
                foreach (XAttribute attribute in descendant.Attributes()) {
                    if (attribute.Name.LocalName.Equals("resource", StringComparison.Ordinal)
                        || attribute.Name.LocalName.Equals("value", StringComparison.Ordinal)) {
                        AddDigitalSourceTypeEvidence(attribute.Value, evidence);
                    }
                }
            }
        }
    }

    private static void AddDigitalSourceTypeEvidence(string value, List<ImageProvenanceEvidence> evidence) {
        string normalized = value.Trim();
        if (MatchesDigitalSourceType(normalized, TrainedAlgorithmicMedia)) {
            AddEvidence(evidence, ImageProvenanceSource.Xmp, ImageProvenanceSignal.CreatedUsingGenerativeAi, normalized);
        } else if (MatchesDigitalSourceType(normalized, CompositeWithTrainedAlgorithmicMedia)) {
            AddEvidence(evidence, ImageProvenanceSource.Xmp, ImageProvenanceSignal.EditedUsingGenerativeAi, normalized);
        }
    }

    private static bool MatchesDigitalSourceType(string value, string term) {
        return value.Equals($"http://{DigitalSourceTypeVocabulary}{term}", StringComparison.Ordinal)
            || value.Equals($"https://{DigitalSourceTypeVocabulary}{term}", StringComparison.Ordinal);
    }

    private static void AddEvidence(List<ImageProvenanceEvidence> evidence, ImageProvenanceSource source, ImageProvenanceSignal signal, string value) {
        foreach (ImageProvenanceEvidence item in evidence) {
            if (item.Source == source && item.Signal == signal) {
                return;
            }
        }

        evidence.Add(new ImageProvenanceEvidence(source, signal, value));
    }

    private static bool HasPrefix(byte[] value, int valueLength, byte[] prefix) {
        if (valueLength < prefix.Length) {
            return false;
        }

        for (int index = 0; index < prefix.Length; index++) {
            if (value[index] != prefix[index]) {
                return false;
            }
        }

        return true;
    }

    private static bool StartsWith(byte[] value, int offset, byte[] expected) {
        if (offset < 0 || expected.Length > value.Length - offset) {
            return false;
        }

        for (int index = 0; index < expected.Length; index++) {
            if (value[offset + index] != expected[index]) {
                return false;
            }
        }

        return true;
    }

    private static bool Matches(byte[] value, string expected) {
        byte[] expectedBytes = Encoding.ASCII.GetBytes(expected);
        return HasPrefix(value, value.Length, expectedBytes) && value.Length == expectedBytes.Length;
    }

    private static bool Matches(byte[] value, int offset, string expected) {
        return StartsWith(value, offset, Encoding.ASCII.GetBytes(expected));
    }

    private static int ReadUpTo(Stream stream, byte[] buffer, int offset, int count) {
        int total = 0;
        while (total < count) {
            int read = stream.Read(buffer, offset + total, count - total);
            if (read == 0) {
                break;
            }

            total += read;
        }

        return total;
    }

    private static byte[] ReadExactly(Stream stream, int count) {
        byte[] buffer = new byte[count];
        if (ReadUpTo(stream, buffer, 0, count) != count) {
            throw new EndOfStreamException("Unexpected end of image data.");
        }

        return buffer;
    }

    private static int ReadUInt16BigEndian(Stream stream) {
        int first = stream.ReadByte();
        int second = stream.ReadByte();
        if (first < 0 || second < 0) {
            throw new EndOfStreamException("Unexpected end of image data.");
        }

        return (first << 8) | second;
    }

    private static uint ReadUInt32BigEndian(Stream stream) {
        int first = stream.ReadByte();
        int second = stream.ReadByte();
        int third = stream.ReadByte();
        int fourth = stream.ReadByte();
        if (first < 0 || second < 0 || third < 0 || fourth < 0) {
            throw new EndOfStreamException("Unexpected end of image data.");
        }

        return ((uint)first << 24) | ((uint)second << 16) | ((uint)third << 8) | (uint)fourth;
    }

    private static uint ReadUInt32BigEndian(byte[] value, int offset) {
        return ((uint)value[offset] << 24)
            | ((uint)value[offset + 1] << 16)
            | ((uint)value[offset + 2] << 8)
            | value[offset + 3];
    }

    private static void SkipExactly(Stream stream, long count) {
        if (count < 0 || count > stream.Length - stream.Position) {
            throw new EndOfStreamException("Unexpected end of image data.");
        }

        stream.Seek(count, SeekOrigin.Current);
    }
}

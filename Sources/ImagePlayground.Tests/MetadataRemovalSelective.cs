using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SixLabors.ImageSharp.Metadata.Profiles.Xmp;
using System;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests selective, format-native metadata removal.
/// </summary>
public partial class ImagePlayground {
    [Fact]
    public void RemoveMetadata_PngC2paOnly_PreservesEveryOtherByte() {
        string cleanPath = Path.Combine(_directoryWithTests, "metadata-selective-clean.png");
        string sourcePath = Path.Combine(_directoryWithTests, "metadata-selective-c2pa.png");
        string outputPath = Path.Combine(_directoryWithTests, "metadata-selective-output.png");
        CreatePngWithXmp(cleanPath);
        byte[] clean = File.ReadAllBytes(cleanPath);
        File.WriteAllBytes(sourcePath, InsertPngChunk(clean, "caBX", Encoding.ASCII.GetBytes("c2pa manifest")));

        ImageMetadataRemovalResult result = ImageHelper.RemoveMetadata(new ImageMetadataRemovalOptions(sourcePath, outputPath) {
            MetadataTypes = ImageMetadataType.C2pa
        });

        Assert.Equal(clean, File.ReadAllBytes(outputPath));
        Assert.Equal(ImageMetadataType.C2pa, result.RemovedMetadataTypes);
        Assert.False(result.WasReencoded);
        Assert.True(ImageHelper.InspectMetadata(outputPath).HasXmp);
        Assert.False(ImageHelper.InspectProvenance(outputPath).HasC2paManifest);
    }

    [Fact]
    public void RemoveMetadata_JpegC2paOnly_RemovesAllFragmentsAndPreservesOtherSegments() {
        string cleanPath = Path.Combine(_directoryWithTests, "metadata-selective-clean.jpg");
        string sourcePath = Path.Combine(_directoryWithTests, "metadata-selective-c2pa.jpg");
        string expectedPath = Path.Combine(_directoryWithTests, "metadata-selective-expected.jpg");
        string outputPath = Path.Combine(_directoryWithTests, "metadata-selective-output.jpg");
        CreateJpegWithExifAndXmp(cleanPath);
        byte[] clean = File.ReadAllBytes(cleanPath);
        byte[] unrelatedApp11 = Encoding.ASCII.GetBytes("not a C2PA JPEG XT segment");
        byte[][] c2paSegments = CreateFragmentedC2paPayloads();
        File.WriteAllBytes(expectedPath, InsertJpegApp11Segments(clean, unrelatedApp11));
        File.WriteAllBytes(sourcePath, InsertJpegApp11Segments(clean, unrelatedApp11, c2paSegments[0], c2paSegments[1]));

        ImageMetadataRemovalResult result = ImageHelper.RemoveMetadata(new ImageMetadataRemovalOptions(sourcePath, outputPath) {
            MetadataTypes = ImageMetadataType.C2pa
        });

        Assert.Equal(File.ReadAllBytes(expectedPath), File.ReadAllBytes(outputPath));
        Assert.Equal(ImageMetadataType.C2pa, result.RemovedMetadataTypes);
        Assert.False(result.WasReencoded);
        ImageMetadataInfo metadata = ImageHelper.InspectMetadata(outputPath);
        Assert.True(metadata.HasExif);
        Assert.True(metadata.HasXmp);
        Assert.False(metadata.Provenance.HasC2paManifest);
    }

    [Fact]
    public void RemoveMetadata_JpegC2paOnly_PreservesInvalidOrNonContiguousApp11Sequences() {
        string cleanPath = Path.Combine(_directoryWithTests, "metadata-selective-invalid-clean.jpg");
        string malformedPath = Path.Combine(_directoryWithTests, "metadata-selective-invalid-c2pa.jpg");
        string malformedOutputPath = Path.Combine(_directoryWithTests, "metadata-selective-invalid-output.jpg");
        string interleavedPath = Path.Combine(_directoryWithTests, "metadata-selective-interleaved-c2pa.jpg");
        string interleavedOutputPath = Path.Combine(_directoryWithTests, "metadata-selective-interleaved-output.jpg");
        string overlongPath = Path.Combine(_directoryWithTests, "metadata-selective-overlong-c2pa.jpg");
        string overlongOutputPath = Path.Combine(_directoryWithTests, "metadata-selective-overlong-output.jpg");
        CreateJpegWithExifAndXmp(cleanPath);
        byte[] clean = File.ReadAllBytes(cleanPath);

        byte[][] malformed = CreateFragmentedC2paPayloads();
        malformed[0][41] = (byte)'x';
        byte[] malformedBytes = InsertJpegApp11Segments(clean, malformed[0], malformed[1]);
        File.WriteAllBytes(malformedPath, malformedBytes);

        byte[][] interleaved = CreateFragmentedC2paPayloads();
        byte[] unrelatedApp11 = Encoding.ASCII.GetBytes("breaks contiguous C2PA sequence");
        byte[] interleavedBytes = InsertJpegApp11Segments(clean, interleaved[0], unrelatedApp11, interleaved[1]);
        File.WriteAllBytes(interleavedPath, interleavedBytes);

        byte[][] overlong = CreateFragmentedC2paPayloads();
        Array.Resize(ref overlong[1], overlong[1].Length + 1);
        overlong[1][overlong[1].Length - 1] = 0x7F;
        byte[] overlongBytes = InsertJpegApp11Segments(clean, overlong[0], overlong[1]);
        File.WriteAllBytes(overlongPath, overlongBytes);

        ImageMetadataRemovalResult malformedResult = ImageHelper.RemoveMetadata(new ImageMetadataRemovalOptions(malformedPath, malformedOutputPath) {
            MetadataTypes = ImageMetadataType.C2pa
        });
        ImageMetadataRemovalResult interleavedResult = ImageHelper.RemoveMetadata(new ImageMetadataRemovalOptions(interleavedPath, interleavedOutputPath) {
            MetadataTypes = ImageMetadataType.C2pa
        });
        ImageMetadataRemovalResult overlongResult = ImageHelper.RemoveMetadata(new ImageMetadataRemovalOptions(overlongPath, overlongOutputPath) {
            MetadataTypes = ImageMetadataType.C2pa
        });

        Assert.Equal(malformedBytes, File.ReadAllBytes(malformedOutputPath));
        Assert.Equal(interleavedBytes, File.ReadAllBytes(interleavedOutputPath));
        Assert.Equal(overlongBytes, File.ReadAllBytes(overlongOutputPath));
        Assert.Equal(ImageMetadataType.None, malformedResult.RemovedMetadataTypes);
        Assert.Equal(ImageMetadataType.None, interleavedResult.RemovedMetadataTypes);
        Assert.Equal(ImageMetadataType.None, overlongResult.RemovedMetadataTypes);
    }

    [Fact]
    public void RemoveMetadata_JpegC2paOnly_RemovesManifestFromAdditionalJpegImage() {
        string firstPath = Path.Combine(_directoryWithTests, "metadata-selective-mpf-first.jpg");
        string secondPath = Path.Combine(_directoryWithTests, "metadata-selective-mpf-second.jpg");
        string sourcePath = Path.Combine(_directoryWithTests, "metadata-selective-mpf-source.jpg");
        string outputPath = Path.Combine(_directoryWithTests, "metadata-selective-mpf-output.jpg");
        CreateJpegWithExifAndXmp(firstPath);
        CreateJpegWithExifAndXmp(secondPath);
        byte[] first = File.ReadAllBytes(firstPath);
        byte[] second = File.ReadAllBytes(secondPath);
        byte[][] c2pa = CreateFragmentedC2paPayloads();
        byte[] secondWithC2pa = InsertJpegApp11Segments(second, c2pa[0], c2pa[1]);
        byte[] source = CombineMetadataBytes(first, secondWithC2pa);
        File.WriteAllBytes(sourcePath, source);

        Assert.True(ImageHelper.InspectProvenance(sourcePath).HasC2paManifest);

        ImageMetadataRemovalResult result = ImageHelper.RemoveMetadata(new ImageMetadataRemovalOptions(sourcePath, outputPath) {
            MetadataTypes = ImageMetadataType.C2pa
        });

        Assert.Equal(CombineMetadataBytes(first, second), File.ReadAllBytes(outputPath));
        Assert.Equal(ImageMetadataType.C2pa, result.RemovedMetadataTypes);
        Assert.False(ImageHelper.InspectProvenance(outputPath).HasC2paManifest);
    }

    [Fact]
    public void RemoveMetadata_JpegExifOnly_PreservesXmpAndCompressedImageData() {
        string sourcePath = Path.Combine(_directoryWithTests, "metadata-selective-exif.jpg");
        string outputPath = Path.Combine(_directoryWithTests, "metadata-selective-exif-output.jpg");
        CreateJpegWithExifAndXmp(sourcePath);
        byte[] source = File.ReadAllBytes(sourcePath);

        ImageMetadataRemovalResult result = ImageHelper.RemoveMetadata(new ImageMetadataRemovalOptions(sourcePath, outputPath) {
            MetadataTypes = ImageMetadataType.Exif
        });

        ImageMetadataInfo metadata = ImageHelper.InspectMetadata(outputPath);
        Assert.False(metadata.HasExif);
        Assert.True(metadata.HasXmp);
        Assert.Equal(GetJpegScanAndTail(source), GetJpegScanAndTail(File.ReadAllBytes(outputPath)));
        Assert.Equal(ImageMetadataType.Exif, result.RemovedMetadataTypes);
        Assert.False(result.WasReencoded);
    }

    [Fact]
    public void RemoveMetadata_JpegIptcOnly_PreservesOtherPhotoshopResources() {
        string cleanPath = Path.Combine(_directoryWithTests, "metadata-selective-iptc-clean.jpg");
        string sourcePath = Path.Combine(_directoryWithTests, "metadata-selective-iptc.jpg");
        string expectedPath = Path.Combine(_directoryWithTests, "metadata-selective-iptc-expected.jpg");
        string outputPath = Path.Combine(_directoryWithTests, "metadata-selective-iptc-output.jpg");
        CreateJpegWithExifAndXmp(cleanPath);
        byte[] clean = File.ReadAllBytes(cleanPath);
        byte[] preservedResource = CreatePhotoshopResource(0x0405, new byte[] { 0x10, 0x20 });
        byte[] iptcResource = CreatePhotoshopResource(0x0404, new byte[] { 0x01, 0x02, 0x03 });
        File.WriteAllBytes(expectedPath, InsertJpegSegment(clean, 0xED, CombineMetadataBytes(Encoding.ASCII.GetBytes("Photoshop 3.0\0"), preservedResource)));
        File.WriteAllBytes(sourcePath, InsertJpegSegment(clean, 0xED, CombineMetadataBytes(Encoding.ASCII.GetBytes("Photoshop 3.0\0"), preservedResource, iptcResource)));

        ImageMetadataRemovalResult result = ImageHelper.RemoveMetadata(new ImageMetadataRemovalOptions(sourcePath, outputPath) {
            MetadataTypes = ImageMetadataType.Iptc
        });

        Assert.Equal(File.ReadAllBytes(expectedPath), File.ReadAllBytes(outputPath));
        Assert.Equal(ImageMetadataType.Iptc, result.RemovedMetadataTypes);
        Assert.False(result.WasReencoded);
    }

    private static void CreatePngWithXmp(string path) {
        using var image = new SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32>(10, 10);
        image.Metadata.XmpProfile = new XmpProfile(Encoding.UTF8.GetBytes("<x:xmpmeta xmlns:x=\"adobe:ns:meta/\" />"));
        image.Save(path);
    }

    private static void CreateJpegWithExifAndXmp(string path) {
        using var image = new SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32>(10, 10);
        image.Metadata.ExifProfile = new ExifProfile();
        image.Metadata.ExifProfile.SetValue(ExifTag.Software, "ImagePlayground");
        image.Metadata.XmpProfile = new XmpProfile(Encoding.UTF8.GetBytes("<x:xmpmeta xmlns:x=\"adobe:ns:meta/\" />"));
        image.SaveAsJpeg(path);
    }

    private static byte[] InsertPngChunk(byte[] png, string chunkType, byte[] payload) {
        byte[] chunk = CreatePngChunk(chunkType, payload);
        int firstChunkLength = ReadUInt32BigEndian(png, 8);
        int insertOffset = 8 + 12 + firstChunkLength;
        byte[] result = new byte[png.Length + chunk.Length];
        Buffer.BlockCopy(png, 0, result, 0, insertOffset);
        Buffer.BlockCopy(chunk, 0, result, insertOffset, chunk.Length);
        Buffer.BlockCopy(png, insertOffset, result, insertOffset + chunk.Length, png.Length - insertOffset);
        return result;
    }

    private static byte[][] CreateFragmentedC2paPayloads() {
        byte[] manifestStore = new byte[50];
        WriteUInt32BigEndian(manifestStore, 0, (uint)manifestStore.Length);
        Encoding.ASCII.GetBytes("jumb").CopyTo(manifestStore, 4);
        WriteUInt32BigEndian(manifestStore, 8, 30);
        Encoding.ASCII.GetBytes("jumd").CopyTo(manifestStore, 12);
        byte[] c2paUuid = {
            0x63, 0x32, 0x70, 0x61, 0x00, 0x11, 0x00, 0x10,
            0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71
        };
        c2paUuid.CopyTo(manifestStore, 16);
        manifestStore[32] = 0x03;
        Encoding.ASCII.GetBytes("c2pa\0").CopyTo(manifestStore, 33);

        const int firstManifestLength = 38;
        byte[] first = new byte[8 + firstManifestLength];
        first[0] = 0x4A;
        first[1] = 0x50;
        first[2] = 0x02;
        first[3] = 0x11;
        WriteUInt32BigEndian(first, 4, 1);
        Buffer.BlockCopy(manifestStore, 0, first, 8, firstManifestLength);

        int remainingManifestLength = manifestStore.Length - firstManifestLength;
        byte[] continuation = new byte[8 + remainingManifestLength];
        continuation[0] = 0x4A;
        continuation[1] = 0x50;
        continuation[2] = 0x02;
        continuation[3] = 0x11;
        WriteUInt32BigEndian(continuation, 4, 2);
        Buffer.BlockCopy(manifestStore, firstManifestLength, continuation, 8, remainingManifestLength);
        return new[] { first, continuation };
    }

    private static byte[] InsertJpegApp11Segments(byte[] jpeg, params byte[][] payloads) {
        byte[] result = jpeg;
        for (int index = payloads.Length - 1; index >= 0; index--) {
            result = InsertJpegSegment(result, 0xEB, payloads[index]);
        }

        return result;
    }

    private static byte[] InsertJpegSegment(byte[] jpeg, byte marker, byte[] payload) {
        int segmentLength = payload.Length + 2;
        byte[] segment = new byte[payload.Length + 4];
        segment[0] = 0xFF;
        segment[1] = marker;
        segment[2] = (byte)(segmentLength >> 8);
        segment[3] = (byte)segmentLength;
        Buffer.BlockCopy(payload, 0, segment, 4, payload.Length);
        byte[] result = new byte[jpeg.Length + segment.Length];
        Buffer.BlockCopy(jpeg, 0, result, 0, 2);
        Buffer.BlockCopy(segment, 0, result, 2, segment.Length);
        Buffer.BlockCopy(jpeg, 2, result, 2 + segment.Length, jpeg.Length - 2);
        return result;
    }

    private static byte[] CreatePhotoshopResource(ushort resourceId, byte[] data) {
        using var output = new MemoryStream();
        byte[] signature = Encoding.ASCII.GetBytes("8BIM");
        output.Write(signature, 0, signature.Length);
        output.WriteByte((byte)(resourceId >> 8));
        output.WriteByte((byte)resourceId);
        output.WriteByte(0);
        output.WriteByte(0);
        output.WriteByte((byte)(data.Length >> 24));
        output.WriteByte((byte)(data.Length >> 16));
        output.WriteByte((byte)(data.Length >> 8));
        output.WriteByte((byte)data.Length);
        output.Write(data, 0, data.Length);
        if ((data.Length & 1) != 0) {
            output.WriteByte(0);
        }

        return output.ToArray();
    }

    private static byte[] GetJpegScanAndTail(byte[] jpeg) {
        for (int offset = 2; offset < jpeg.Length - 3;) {
            if (jpeg[offset] != 0xFF) {
                throw new InvalidDataException("Invalid JPEG marker prefix.");
            }

            byte marker = jpeg[offset + 1];
            if (marker == 0xDA) {
                return jpeg.Skip(offset).ToArray();
            }

            int segmentLength = (jpeg[offset + 2] << 8) | jpeg[offset + 3];
            offset += 2 + segmentLength;
        }

        throw new InvalidDataException("JPEG scan marker not found.");
    }

    private static byte[] CombineMetadataBytes(params byte[][] values) {
        byte[] result = new byte[values.Sum(value => value.Length)];
        int offset = 0;
        foreach (byte[] value in values) {
            Buffer.BlockCopy(value, 0, result, offset, value.Length);
            offset += value.Length;
        }

        return result;
    }
}

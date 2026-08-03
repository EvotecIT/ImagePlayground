using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Xmp;
using System;
using System.IO;
using System.Text;
using Xunit;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for embedded image provenance detection and cleanup.
/// </summary>
public partial class ImagePlayground {
    [Fact]
    public void InspectMetadata_ReturnsProfilesAndProvenanceFromOneSnapshot() {
        string path = Path.Combine(AppContext.BaseDirectory, "Images", "QRCode1.png");

        ImageMetadataInfo result = ImageHelper.InspectMetadata(path);

        Assert.Equal(Path.GetFullPath(path), result.FilePath);
        Assert.False(result.HasExif);
        Assert.False(result.HasXmp);
        Assert.False(result.Provenance.HasC2paManifest);
        Assert.False(result.Provenance.HasXmpAiDeclaration);
    }

    [Fact]
    public void InspectProvenance_DetectsC2paContainerPng() {
        string path = Path.Combine(_directoryWithTests, "provenance-c2pa.png");
        CreatePngWithC2pa(path, "trainedAlgorithmicMedia");

        ImageProvenanceInfo result = ImageHelper.InspectProvenance(path);

        Assert.True(result.HasC2paManifest);
        Assert.False(result.XmpDeclaresAiGenerated);
        Assert.False(result.XmpDeclaresAiEdited);
        Assert.False(result.HasXmpAiDeclaration);
        Assert.False(result.C2paValidationPerformed);
        Assert.Contains(result.Evidence, item =>
            item.Source == ImageProvenanceSource.C2pa &&
            item.Signal == ImageProvenanceSignal.C2paManifest);
    }

    [Fact]
    public void InspectProvenance_DetectsC2paJpegXtHeader() {
        string path = Path.Combine(_directoryWithTests, "provenance-c2pa.jpg");
        CreateJpegWithC2pa(path);

        ImageProvenanceInfo result = ImageHelper.InspectProvenance(path);

        Assert.True(result.HasC2paManifest);
        Assert.False(result.HasXmpAiDeclaration);
    }

    [Fact]
    public void InspectProvenance_IgnoresUnstructuredJpegApp11Text() {
        string path = Path.Combine(_directoryWithTests, "provenance-not-c2pa.jpg");
        CreateJpegWithApp11(path, Encoding.ASCII.GetBytes("jumb c2pa trainedAlgorithmicMedia"));

        ImageProvenanceInfo result = ImageHelper.InspectProvenance(path);

        Assert.False(result.HasC2paManifest);
        Assert.Empty(result.Evidence);
    }

    [Fact]
    public void InspectProvenance_DetectsAiEditedXmp() {
        string path = Path.Combine(_directoryWithTests, "provenance-xmp.png");
        if (File.Exists(path)) {
            File.Delete(path);
        }

        using (var image = new SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32>(10, 10)) {
            string xmp = "<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\" xmlns:Iptc4xmpExt=\"http://iptc.org/std/Iptc4xmpExt/2008-02-29/\"><rdf:Description Iptc4xmpExt:DigitalSourceType=\"http://cv.iptc.org/newscodes/digitalsourcetype/compositeWithTrainedAlgorithmicMedia\" /></rdf:RDF>";
            image.Metadata.XmpProfile = new XmpProfile(Encoding.UTF8.GetBytes(xmp));
            image.Save(path);
        }

        ImageProvenanceInfo result = ImageHelper.InspectProvenance(path);

        Assert.False(result.HasC2paManifest);
        Assert.False(result.XmpDeclaresAiGenerated);
        Assert.True(result.XmpDeclaresAiEdited);
        Assert.True(result.HasXmpAiDeclaration);
        Assert.Contains(result.Evidence, item => item.Source == ImageProvenanceSource.Xmp);
    }

    [Fact]
    public void InspectProvenance_DetectsAiGeneratedXmpResource() {
        string path = Path.Combine(_directoryWithTests, "provenance-xmp-resource.png");
        if (File.Exists(path)) {
            File.Delete(path);
        }

        using (var image = new SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32>(10, 10)) {
            string xmp = "<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\" xmlns:Iptc4xmpExt=\"http://iptc.org/std/Iptc4xmpExt/2008-02-29/\"><rdf:Description><Iptc4xmpExt:DigitalSourceType rdf:resource=\"http://cv.iptc.org/newscodes/digitalsourcetype/trainedAlgorithmicMedia\" /></rdf:Description></rdf:RDF>";
            image.Metadata.XmpProfile = new XmpProfile(Encoding.UTF8.GetBytes(xmp));
            image.Save(path);
        }

        ImageProvenanceInfo result = ImageHelper.InspectProvenance(path);

        Assert.True(result.XmpDeclaresAiGenerated);
        Assert.False(result.XmpDeclaresAiEdited);
        Assert.True(result.HasXmpAiDeclaration);
    }

    [Fact]
    public void InspectProvenance_IgnoresAiVocabularyOutsideDigitalSourceType() {
        string path = Path.Combine(_directoryWithTests, "provenance-xmp-description.png");
        if (File.Exists(path)) {
            File.Delete(path);
        }

        using (var image = new SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32>(10, 10)) {
            string xmp = "<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description><rdf:value>http://cv.iptc.org/newscodes/digitalsourcetype/trainedAlgorithmicMedia</rdf:value></rdf:Description></rdf:RDF>";
            image.Metadata.XmpProfile = new XmpProfile(Encoding.UTF8.GetBytes(xmp));
            image.Save(path);
        }

        ImageProvenanceInfo result = ImageHelper.InspectProvenance(path);

        Assert.False(result.HasXmpAiDeclaration);
        Assert.Empty(result.Evidence);
    }

    [Fact]
    public void RemoveMetadata_RemovesC2paContainer() {
        string path = Path.Combine(_directoryWithTests, "provenance-remove.png");
        string outputPath = Path.Combine(_directoryWithTests, "provenance-removed.png");
        CreatePngWithC2pa(path, "trainedAlgorithmicMedia");
        if (File.Exists(outputPath)) {
            File.Delete(outputPath);
        }

        ImageHelper.RemoveMetadata(path, outputPath);

        ImageProvenanceInfo result = ImageHelper.InspectProvenance(outputPath);
        Assert.False(result.HasC2paManifest);
        Assert.False(result.HasXmpAiDeclaration);
        using SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(outputPath);
        Assert.Equal(10, image.Width);
        Assert.Equal(10, image.Height);
    }

    [Fact]
    public void InspectProvenance_ReturnsNoSignalsForCleanPng() {
        string path = Path.Combine(_directoryWithTests, "provenance-clean.png");
        if (File.Exists(path)) {
            File.Delete(path);
        }

        using (var image = new SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32>(10, 10)) {
            image.Save(path);
        }

        ImageProvenanceInfo result = ImageHelper.InspectProvenance(path);

        Assert.False(result.HasC2paManifest);
        Assert.False(result.HasXmpAiDeclaration);
        Assert.Empty(result.Evidence);
    }

    private static void CreatePngWithC2pa(string path, string digitalSourceType) {
        if (File.Exists(path)) {
            File.Delete(path);
        }

        using (var image = new SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32>(10, 10)) {
            image.Save(path);
        }

        byte[] png = File.ReadAllBytes(path);
        byte[] payload = Encoding.UTF8.GetBytes($"jumb\0c2pa\0http://cv.iptc.org/newscodes/digitalsourcetype/{digitalSourceType}");
        byte[] chunk = CreatePngChunk("caBX", payload);
        int firstChunkLength = ReadUInt32BigEndian(png, 8);
        int insertOffset = 8 + 12 + firstChunkLength;
        byte[] result = new byte[png.Length + chunk.Length];
        System.Buffer.BlockCopy(png, 0, result, 0, insertOffset);
        System.Buffer.BlockCopy(chunk, 0, result, insertOffset, chunk.Length);
        System.Buffer.BlockCopy(png, insertOffset, result, insertOffset + chunk.Length, png.Length - insertOffset);
        File.WriteAllBytes(path, result);
    }

    private static void CreateJpegWithC2pa(string path) {
        byte[] manifestStore = new byte[38];
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

        byte[] payload = new byte[8 + manifestStore.Length];
        payload[0] = 0x4A;
        payload[1] = 0x50;
        payload[2] = 0x02;
        payload[3] = 0x11;
        WriteUInt32BigEndian(payload, 4, 1);
        System.Buffer.BlockCopy(manifestStore, 0, payload, 8, manifestStore.Length);
        CreateJpegWithApp11(path, payload);
    }

    private static void CreateJpegWithApp11(string path, byte[] payload) {
        if (File.Exists(path)) {
            File.Delete(path);
        }

        using (var image = new SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32>(10, 10)) {
            image.SaveAsJpeg(path);
        }

        byte[] jpeg = File.ReadAllBytes(path);
        byte[] segment = new byte[payload.Length + 4];
        segment[0] = 0xFF;
        segment[1] = 0xEB;
        int segmentLength = payload.Length + 2;
        segment[2] = (byte)(segmentLength >> 8);
        segment[3] = (byte)segmentLength;
        System.Buffer.BlockCopy(payload, 0, segment, 4, payload.Length);

        byte[] result = new byte[jpeg.Length + segment.Length];
        System.Buffer.BlockCopy(jpeg, 0, result, 0, 2);
        System.Buffer.BlockCopy(segment, 0, result, 2, segment.Length);
        System.Buffer.BlockCopy(jpeg, 2, result, 2 + segment.Length, jpeg.Length - 2);
        File.WriteAllBytes(path, result);
    }

    private static byte[] CreatePngChunk(string type, byte[] payload) {
        byte[] typeBytes = Encoding.ASCII.GetBytes(type);
        byte[] chunk = new byte[12 + payload.Length];
        WriteUInt32BigEndian(chunk, 0, (uint)payload.Length);
        System.Buffer.BlockCopy(typeBytes, 0, chunk, 4, typeBytes.Length);
        System.Buffer.BlockCopy(payload, 0, chunk, 8, payload.Length);

        byte[] crcInput = new byte[typeBytes.Length + payload.Length];
        System.Buffer.BlockCopy(typeBytes, 0, crcInput, 0, typeBytes.Length);
        System.Buffer.BlockCopy(payload, 0, crcInput, typeBytes.Length, payload.Length);
        WriteUInt32BigEndian(chunk, 8 + payload.Length, ComputeCrc32(crcInput));
        return chunk;
    }

    private static uint ComputeCrc32(byte[] data) {
        uint crc = 0xFFFFFFFF;
        foreach (byte value in data) {
            crc ^= value;
            for (int bit = 0; bit < 8; bit++) {
                crc = (crc & 1) != 0
                    ? (crc >> 1) ^ 0xEDB88320
                    : crc >> 1;
            }
        }

        return ~crc;
    }

    private static int ReadUInt32BigEndian(byte[] value, int offset) =>
        (value[offset] << 24) |
        (value[offset + 1] << 16) |
        (value[offset + 2] << 8) |
        value[offset + 3];

    private static void WriteUInt32BigEndian(byte[] value, int offset, uint number) {
        value[offset] = (byte)(number >> 24);
        value[offset + 1] = (byte)(number >> 16);
        value[offset + 2] = (byte)(number >> 8);
        value[offset + 3] = (byte)number;
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using OfficeIMO.Drawing;
using Xunit;
using PlaygroundImage = global::ImagePlayground.Image;

namespace ImagePlayground.Tests;

public partial class ImagePlayground {
    [Fact]
    public void HeifProtectedExifRejectsRequestedReadsAndClearsButPreservesIndependentXmpEdits() {
        string source = Path.Combine(_directoryWithTests, "heif-protected-exif.heic");
        string rejected = Path.Combine(_directoryWithTests, "heif-protected-exif-rejected.heic");
        string output = Path.Combine(_directoryWithTests, "heif-protected-exif-xmp-edited.heic");
        byte[] exif = CreateExifPayload("Opaque protected EXIF");
        byte[] original = CreateHeifMetadataSiblings(exif, "<x:xmpmeta xmlns:x=\"adobe:ns:meta/\" />", 0, 0, 7);
        byte[] sentinel = Encoding.ASCII.GetBytes("Existing output remains intact");
        File.WriteAllBytes(source, original);
        File.WriteAllBytes(rejected, sentinel);

        Assert.Throws<NotSupportedException>(() => PlaygroundImage.GetExifValues(source));
        Assert.Throws<NotSupportedException>(() => PlaygroundImage.ClearExifValues(source, rejected));
        Assert.Equal(sentinel, File.ReadAllBytes(rejected));
        Assert.Throws<NotSupportedException>(() => global::ImagePlayground.ImageHelper.RemoveMetadata(new ImageMetadataRemovalOptions(source, rejected) { MetadataTypes = ImageMetadataType.Exif }));
        Assert.Equal(sentinel, File.ReadAllBytes(rejected));
        const string xmp = "<x:xmpmeta xmlns:x=\"adobe:ns:meta/\">Updated XMP</x:xmpmeta>";

        PlaygroundImage.SetHeifXmp(source, output, xmp);

        Assert.Equal(xmp, PlaygroundImage.GetHeifXmp(output));
        Assert.Equal(original, File.ReadAllBytes(source));
        var info = PlaygroundImage.GetHeifInfo(output);
        Assert.True(info.HasExif);
        Assert.Equal((ushort)7, info.ExifItem!.ItemProtectionIndex);
        Assert.True(ContainsSequence(File.ReadAllBytes(output), Combine(UInt32BigEndian(6), Encoding.ASCII.GetBytes("Exif\0\0"), exif)));
        Assert.Throws<NotSupportedException>(() => PlaygroundImage.GetExifValues(output));
    }

    [Fact]
    public void HeifInfoRetainsOpaqueMetadataDeclarationsWithoutInterpretingTheirPayload() {
        string source = Path.Combine(_directoryWithTests, "heif-info-opaque-xmp.heic");
        File.WriteAllBytes(source, CreateMinimalHeifWithPrimaryImageExifAndXmp(640, 480, CreateExifPayload("Readable EXIF"), "Opaque item bytes", 7, "utf-8"));

        var info = PlaygroundImage.GetHeifInfo(source);

        Assert.True(info.HasXmp);
        Assert.Equal((ushort)7, info.XmpItem!.ItemProtectionIndex);
        Assert.Equal("utf-8", info.XmpItem.ContentEncoding);
        Assert.Equal("application/rdf+xml", info.XmpItem.MimeType);
        Assert.Equal(640U, info.Width);
        Assert.Equal(480U, info.Height);
        Assert.NotNull(info.XmpItem.Location);
        Assert.Equal(Encoding.UTF8.GetByteCount("Opaque item bytes"), Assert.Single(info.XmpItem.Location!.Extents).Length);
    }

    [Theory]
    [InlineData(7, "")]
    [InlineData(0, "gzip")]
    public void HeifOpaqueXmpRejectsRequestedReadsAndClearsButPreservesIndependentExifEdits(ushort protectionIndex, string contentEncoding) {
        string suffix = protectionIndex + "-" + contentEncoding;
        string source = Path.Combine(_directoryWithTests, "heif-opaque-xmp-" + suffix + ".heic");
        string rejected = Path.Combine(_directoryWithTests, "heif-opaque-xmp-rejected-" + suffix + ".heic");
        string edited = Path.Combine(_directoryWithTests, "heif-opaque-xmp-edited-" + suffix + ".heic");
        const string payload = "Opaque sibling payload";
        byte[] original = CreateMinimalHeifWithPrimaryImageExifAndXmp(640, 480, CreateExifPayload("Original"), payload, protectionIndex, contentEncoding);
        byte[] sentinel = Encoding.ASCII.GetBytes("Existing output remains intact");
        File.WriteAllBytes(source, original);
        File.WriteAllBytes(rejected, sentinel);

        Assert.Throws<NotSupportedException>(() => PlaygroundImage.GetHeifXmp(source));
        Assert.Throws<NotSupportedException>(() => PlaygroundImage.RemoveHeifXmp(source, rejected));
        Assert.Equal(sentinel, File.ReadAllBytes(rejected));
        Assert.Throws<NotSupportedException>(() => global::ImagePlayground.ImageHelper.RemoveMetadata(new ImageMetadataRemovalOptions(source, rejected) { MetadataTypes = ImageMetadataType.Xmp }));
        Assert.Equal(sentinel, File.ReadAllBytes(rejected));

        PlaygroundImage.SetExifValue(source, edited, OfficeExifTag.Software, "Updated");

        Assert.Equal("Updated", Assert.Single(PlaygroundImage.GetExifValues(edited), value => value.Tag.Equals(OfficeExifTag.Software)).Value);
        Assert.Equal(original, File.ReadAllBytes(source));
        var info = PlaygroundImage.GetHeifInfo(edited);
        Assert.True(info.HasXmp);
        Assert.Equal(protectionIndex, info.XmpItem!.ItemProtectionIndex);
        Assert.Equal(contentEncoding, info.XmpItem.ContentEncoding);
        Assert.True(ContainsSequence(File.ReadAllBytes(edited), Encoding.UTF8.GetBytes(payload)));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void ExifOnlyHeifEditPreservesAnUnwritableOrUnreadableXmpSibling(int xmpStorage) {
        string source = Path.Combine(_directoryWithTests, "heif-exif-only-" + xmpStorage + ".heic");
        string output = Path.Combine(_directoryWithTests, "heif-exif-only-output-" + xmpStorage + ".heic");
        const string xmp = "<x:xmpmeta xmlns:x=\"adobe:ns:meta/\">Unrequested XMP</x:xmpmeta>";
        byte[] original = CreateHeifMetadataSiblings(CreateExifPayload("Original"), xmp, 0, xmpStorage);
        File.WriteAllBytes(source, original);

        Assert.Equal("Original", Assert.Single(PlaygroundImage.GetExifValues(source), value => value.Tag.Equals(OfficeExifTag.Software)).Value);
        PlaygroundImage.SetExifValue(source, output, OfficeExifTag.Software, "Updated");

        Assert.Equal("Updated", Assert.Single(PlaygroundImage.GetExifValues(output), value => value.Tag.Equals(OfficeExifTag.Software)).Value);
        Assert.Equal(original, File.ReadAllBytes(source));
        var info = PlaygroundImage.GetHeifInfo(output);
        Assert.True(info.HasXmp);
        if (xmpStorage == 1 || xmpStorage == 3) {
            Assert.Equal(xmp, PlaygroundImage.GetHeifXmp(output));
            Assert.Equal((ushort)(xmpStorage == 1 ? 1 : 0), info.XmpItem!.Location!.ConstructionMethod);
            Assert.Equal(xmpStorage == 1 ? 1 : 2, info.XmpItem.Location.Extents.Count);
            Assert.True(ContainsSequence(File.ReadAllBytes(output), xmpStorage == 1 ? Box("idat", Encoding.UTF8.GetBytes(xmp)) : Encoding.UTF8.GetBytes(xmp)));
        } else {
            Assert.Null(info.XmpItem!.Location);
            Assert.Throws<NotSupportedException>(() => PlaygroundImage.GetHeifXmp(output));
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void XmpOnlyHeifRemovalPreservesAnOpaqueOrUnwritableExifSibling(int exifStorage) {
        string source = Path.Combine(_directoryWithTests, "heif-xmp-only-" + exifStorage + ".heic");
        string output = Path.Combine(_directoryWithTests, "heif-xmp-only-output-" + exifStorage + ".heic");
        byte[] exif = exifStorage == 1 || exifStorage == 3 ? CreateExifPayload("Preserved") : new byte[] { 1, 2, 3 };
        byte[] original = CreateHeifMetadataSiblings(exif, "<x:xmpmeta xmlns:x=\"adobe:ns:meta/\" />", exifStorage, 0);
        File.WriteAllBytes(source, original);

        var result = global::ImagePlayground.ImageHelper.RemoveMetadata(new ImageMetadataRemovalOptions(source, output) { MetadataTypes = ImageMetadataType.Xmp });

        Assert.Equal(ImageMetadataType.Xmp, result.RemovedMetadataTypes);
        Assert.False(result.WasReencoded);
        Assert.Equal(string.Empty, PlaygroundImage.GetHeifXmp(output));
        Assert.Equal(original, File.ReadAllBytes(source));
        var info = PlaygroundImage.GetHeifInfo(output);
        Assert.True(info.HasExif);
        if (exifStorage == 2) {
            Assert.Null(info.ExifItem!.Location);
        } else {
            byte[] itemBytes = Combine(UInt32BigEndian(6), Encoding.ASCII.GetBytes("Exif\0\0"), exif);
            Assert.True(ContainsSequence(File.ReadAllBytes(output), itemBytes));
            Assert.Equal((ushort)(exifStorage == 1 ? 1 : 0), info.ExifItem!.Location!.ConstructionMethod);
            Assert.Equal(exifStorage == 3 ? 2 : 1, info.ExifItem.Location.Extents.Count);
        }
        if (exifStorage == 1 || exifStorage == 3) {
            Assert.Equal("Preserved", Assert.Single(PlaygroundImage.GetExifValues(output), value => value.Tag.Equals(OfficeExifTag.Software)).Value);
        } else {
            Assert.Throws<NotSupportedException>(() => PlaygroundImage.GetExifValues(output));
        }
    }

    [Fact]
    public void HeifProvenanceReadsXmpWithoutReadingAnUnlocatedExifSibling() {
        string source = Path.Combine(_directoryWithTests, "heif-provenance-unlocated-exif.heic");
        const string xmp = "<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\" xmlns:Iptc4xmpExt=\"http://iptc.org/std/Iptc4xmpExt/2008-02-29/\"><rdf:Description Iptc4xmpExt:DigitalSourceType=\"http://cv.iptc.org/newscodes/digitalsourcetype/trainedAlgorithmicMedia\" /></rdf:RDF>";
        File.WriteAllBytes(source, CreateHeifMetadataSiblings(new byte[] { 1, 2, 3 }, xmp, 2, 0));

        var result = global::ImagePlayground.ImageHelper.InspectProvenance(source);

        Assert.True(result.XmpDeclaresAiGenerated);
        Assert.False(result.XmpDeclaresAiEdited);
        Assert.Contains(result.Evidence, item => item.Source == ImageProvenanceSource.Xmp);
        Assert.Throws<NotSupportedException>(() => PlaygroundImage.GetExifValues(source));
    }

    [Fact]
    public void HeifMetadataSnapshotLeavesUnavailableDensityFieldsNull() {
        string source = Path.Combine(_directoryWithTests, "heif-density-absent.heic");
        File.WriteAllBytes(source, CreateMinimalHeifWithPrimaryImageAndExif(320, 240, CreateExifPayload("Snapshot")));

        var snapshot = global::ImagePlayground.ImageHelper.InspectMetadata(source);

        Assert.Null(snapshot.HorizontalResolution);
        Assert.Null(snapshot.VerticalResolution);
        Assert.Null(snapshot.ResolutionUnits);
        Assert.Equal("Snapshot", Assert.Single(snapshot.ExifValues, value => value.Tag.Equals(OfficeExifTag.Software)).Value);
    }

    [Theory]
    [InlineData("IccProfile")]
    [InlineData("IptcProfile")]
    public void HeifImportRejectsUnsupportedSuppliedProfilesWithoutChangingFiles(string profileName) {
        string source = Path.Combine(_directoryWithTests, "heif-import-unsupported-" + profileName + ".heic");
        string metadataPath = Path.Combine(_directoryWithTests, "heif-import-unsupported-" + profileName + ".json");
        string output = Path.Combine(_directoryWithTests, "heif-import-unsupported-output-" + profileName + ".heic");
        byte[] original = CreateMinimalHeifWithPrimaryImageExifAndXmp(320, 240, CreateExifPayload("Original"), "<x:xmpmeta xmlns:x=\"adobe:ns:meta/\" />");
        byte[] sentinel = Encoding.ASCII.GetBytes("Existing destination");
        File.WriteAllBytes(source, original);
        File.WriteAllBytes(output, sentinel);
        var metadata = new Dictionary<string, object?> {
            ["HorizontalResolution"] = 96,
            ["VerticalResolution"] = 96,
            ["ResolutionUnits"] = "PixelsPerInch",
            ["ExifProfile"] = CreateExifPayload("Replacement"),
            ["XmpProfile"] = Encoding.UTF8.GetBytes("<x:xmpmeta xmlns:x=\"adobe:ns:meta/\" />"),
            [profileName] = new byte[] { 1, 2, 3 }
        };
        File.WriteAllText(metadataPath, JsonSerializer.Serialize(metadata));

        Assert.Throws<NotSupportedException>(() => global::ImagePlayground.ImageHelper.ImportMetadata(source, metadataPath, output));

        Assert.Equal(original, File.ReadAllBytes(source));
        Assert.Equal(sentinel, File.ReadAllBytes(output));
    }

    // Storage 0 is a single absolute mdat extent; 1 is idat-relative; 2 declares no location; 3 splits mdat into two extents.
    private static byte[] CreateHeifMetadataSiblings(byte[] exif, string xmp, int exifStorage, int xmpStorage, ushort exifProtectionIndex = 0) {
        byte[][] payloads = { Combine(UInt32BigEndian(6), Encoding.ASCII.GetBytes("Exif\0\0"), exif), Encoding.UTF8.GetBytes(xmp) };
        int[] storage = { exifStorage, xmpStorage };
        byte[] ftyp = Box("ftyp", Combine(Encoding.ASCII.GetBytes("heic"), UInt32BigEndian(0), Encoding.ASCII.GetBytes("mif1heic")));
        byte[] idatPayload = Combine(payloads.Where((_, index) => storage[index] == 1).ToArray());
        byte[] mdatPayload = Combine(payloads.Where((_, index) => storage[index] == 0 || storage[index] == 3).ToArray());
        byte[] BuildMeta(uint dataOffset) {
            byte[] exifInfo = FullBox("infe", 2, Combine(UInt16BigEndian(1), UInt16BigEndian(exifProtectionIndex), Encoding.ASCII.GetBytes("Exif"), new byte[] { 0 }));
            byte[] xmpInfo = FullBox("infe", 2, Combine(UInt16BigEndian(2), UInt16BigEndian(0), Encoding.ASCII.GetBytes("mime"), new byte[] { 0 }, Encoding.ASCII.GetBytes("application/rdf+xml"), new byte[] { 0, 0 }));
            var locations = new List<byte[]>();
            uint mdatOffset = dataOffset, idatOffset = 0;
            for (int index = 0; index < payloads.Length; index++) {
                if (storage[index] == 2) { continue; }
                bool usesMdat = storage[index] == 0 || storage[index] == 3;
                uint offset = usesMdat ? mdatOffset : idatOffset;
                uint length = (uint)payloads[index].Length;
                byte[] extents = storage[index] == 3
                    ? Combine(UInt32BigEndian(offset), UInt32BigEndian(length / 2), UInt32BigEndian(offset + length / 2), UInt32BigEndian(length - length / 2))
                    : Combine(UInt32BigEndian(offset), UInt32BigEndian(length));
                locations.Add(Combine(UInt16BigEndian((ushort)(index + 1)), UInt16BigEndian((ushort)(usesMdat ? 0 : 1)), UInt16BigEndian(0), UInt16BigEndian((ushort)(storage[index] == 3 ? 2 : 1)), extents));
                if (usesMdat) { mdatOffset += length; } else { idatOffset += length; }
            }
            byte[] iinf = FullBox("iinf", 0, Combine(UInt16BigEndian(2), exifInfo, xmpInfo));
            byte[] iloc = FullBox("iloc", 1, Combine(new byte[] { 0x44, 0x00 }, UInt16BigEndian((ushort)locations.Count), Combine(locations.ToArray())));
            byte[] idat = idatPayload.Length == 0 ? Array.Empty<byte>() : Box("idat", idatPayload);
            return FullBox("meta", 0, Combine(iinf, iloc, idat));
        }
        byte[] placeholder = BuildMeta(0);
        byte[] meta = BuildMeta((uint)(ftyp.Length + placeholder.Length + 8));
        return Combine(ftyp, meta, Box("mdat", mdatPayload));
    }
}

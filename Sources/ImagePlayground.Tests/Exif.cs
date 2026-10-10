using OfficeIMO.Drawing;
using Color = OfficeIMO.Drawing.OfficeColor;
using ExifTag = OfficeIMO.Drawing.OfficeExifTag;
using Rgba32 = OfficeIMO.Drawing.OfficeColor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Xunit;
using PlaygroundImage = global::ImagePlayground.Image;

namespace ImagePlayground.Tests;

/// <summary>
/// Tests for Exif.
/// </summary>
public partial class ImagePlayground {
    [Fact]
    public void Test_ReadExif() {
        string filePath = Path.Combine(_directoryWithImages, "exif-read.jpg");
        using (var img = new PlaygroundImage()) {
            img.Create(filePath, 20, 20);
            img.SetExifValue(ExifTag.Software, "ImagePlayground");
            img.Save();
        }

        using var loaded = Image.Load(filePath);
        var values = loaded.GetExifValues();
        Assert.Contains(values, v => v.Tag.Equals(ExifTag.Software) && v.Value?.ToString() == "ImagePlayground");
    }

    [Fact]
    public void Test_SetAndRemoveExif() {
        string dest = Path.Combine(_directoryWithTests, "exif-edit.jpg");
        if (File.Exists(dest)) File.Delete(dest);

        using (var img = new PlaygroundImage()) {
            img.Create(dest, 20, 20);
            img.SetExifValue(ExifTag.Software, "ImagePlayground");
            img.Save();
        }

        using (var img = Image.Load(dest)) {
            img.SetExifValue(ExifTag.Software, "Modified");
            img.Save(dest);
        }

        using (var img = Image.Load(dest)) {
            Assert.Equal("Modified", Assert.Single(img.GetExifValues(), value => value.Tag.Equals(ExifTag.Software)).Value);
            img.RemoveExifValues(ExifTag.Software);
            img.Save();
        }

        using var check = Image.Load(dest);
        Assert.DoesNotContain(check.GetExifValues(), value => value.Tag.Equals(ExifTag.Software));
    }

    [Fact]
    public void Test_SetExifValue_InvalidType_Throws() {
        using var img = new PlaygroundImage();
        img.Create(Path.Combine(_directoryWithTests, "invalid.jpg"), 10, 10);

        Assert.Throws<ArgumentException>(() => img.SetExifValue(ExifTag.Software, 123));
    }

    [Fact]
    public void Test_SetExifValue_NumberTag_Works() {
        using var img = new PlaygroundImage();
        img.Create(Path.Combine(_directoryWithTests, "number-tag.jpg"), 10, 10);

        const uint imageWidth = 123;
        img.SetExifValue(OfficeExifTag.ImageWidth, imageWidth);

        var exifValue = Assert.Single(img.GetExifValues(), v => v.Tag.Equals(OfficeExifTag.ImageWidth));
        Assert.Equal(imageWidth, exifValue.Value);
    }

    [Fact]
    public void Test_ReadHeifExif() {
        string filePath = Path.Combine(_directoryWithTests, "exif-read.heic");
        var profile = new OfficeImageMetadata();
        profile.SetExifValue(ExifTag.Software, "ImagePlayground");

        File.WriteAllBytes(filePath, CreateMinimalHeifWithExif(Assert.IsType<byte[]>(profile.EncodeExifProfile())));

        IReadOnlyList<OfficeExifValue> values = PlaygroundImage.GetExifValues(filePath);

        Assert.Contains(values, v => v.Tag.Equals(ExifTag.Software) && v.Value?.ToString() == "ImagePlayground");
    }

    [Fact]
    public void Test_ReadHeifExifWithoutExif_ReturnsEmpty() {
        string filePath = Path.Combine(_directoryWithTests, "exif-empty.heic");
        File.WriteAllBytes(filePath, CreateMinimalHeifWithoutExif());

        IReadOnlyList<OfficeExifValue> values = PlaygroundImage.GetExifValues(filePath);

        Assert.Empty(values);
    }

    [Fact]
    public void Test_ReadHeifExifWithUnreadableExif_Throws() {
        string filePath = Path.Combine(_directoryWithTests, "exif-read-unreadable.heic");
        File.WriteAllBytes(filePath, CreateMinimalHeifWithExif(Array.Empty<byte>()));

        Assert.Throws<NotSupportedException>(() => PlaygroundImage.GetExifValues(filePath));
    }

    [Fact]
    public void Test_SetHeifExifValue() {
        string filePath = Path.Combine(_directoryWithTests, "exif-set.heic");
        File.WriteAllBytes(filePath, CreateMinimalHeifWithExif(CreateExifPayload("Original")));

        PlaygroundImage.SetExifValue(filePath, null, ExifTag.Software, "Changed");

        IReadOnlyList<OfficeExifValue> values = PlaygroundImage.GetExifValues(filePath);
        Assert.Contains(values, v => v.Tag.Equals(ExifTag.Software) && v.Value?.ToString() == "Changed");
        Assert.False(ContainsSequence(File.ReadAllBytes(filePath), Encoding.ASCII.GetBytes("Original")));
    }

    [Fact]
    public void Test_RemoveHeifExifValue() {
        string filePath = Path.Combine(_directoryWithTests, "exif-remove.heic");
        File.WriteAllBytes(filePath, CreateMinimalHeifWithExif(CreateExifPayload("ImagePlayground")));

        PlaygroundImage.RemoveExifValues(filePath, null, ExifTag.Software);

        IReadOnlyList<OfficeExifValue> values = PlaygroundImage.GetExifValues(filePath);
        Assert.Empty(values);
    }

    [Fact]
    public void Test_ClearHeifExifValues() {
        string filePath = Path.Combine(_directoryWithTests, "exif-clear.heic");
        File.WriteAllBytes(filePath, CreateMinimalHeifWithExif(CreateExifPayload("ImagePlayground")));

        PlaygroundImage.ClearExifValues(filePath, null);

        IReadOnlyList<OfficeExifValue> values = PlaygroundImage.GetExifValues(filePath);
        Assert.Empty(values);
    }

    [Fact]
    public void Test_SetHeifExifValueAfterClear() {
        string filePath = Path.Combine(_directoryWithTests, "exif-set-after-clear.heic");
        File.WriteAllBytes(filePath, CreateMinimalHeifWithExif(CreateExifPayload("ImagePlayground")));

        PlaygroundImage.ClearExifValues(filePath, null);
        PlaygroundImage.SetExifValue(filePath, null, ExifTag.Software, "Recreated");

        IReadOnlyList<OfficeExifValue> values = PlaygroundImage.GetExifValues(filePath);
        Assert.Contains(values, v => v.Tag.Equals(ExifTag.Software) && v.Value?.ToString() == "Recreated");
    }

    [Fact]
    public void Test_SetHeifExifValueWithoutExistingExif_Throws() {
        string filePath = Path.Combine(_directoryWithTests, "exif-set-empty.heic");
        File.WriteAllBytes(filePath, CreateMinimalHeifWithoutExif());

        Assert.Throws<NotSupportedException>(() => PlaygroundImage.SetExifValue(filePath, null, ExifTag.Software, "ImagePlayground"));
    }

    [Fact]
    public void Test_SetHeifExifValueWithUnreadableExif_Throws() {
        string filePath = Path.Combine(_directoryWithTests, "exif-set-unreadable.heic");
        File.WriteAllBytes(filePath, CreateMinimalHeifWithExif(Array.Empty<byte>()));

        Assert.Throws<NotSupportedException>(() => PlaygroundImage.SetExifValue(filePath, null, ExifTag.Software, "ImagePlayground"));
    }

    [Fact]
    public void Test_RemoveHeifExifValueWithUnreadableExif_Throws() {
        string filePath = Path.Combine(_directoryWithTests, "exif-remove-unreadable.heic");
        File.WriteAllBytes(filePath, CreateMinimalHeifWithExif(Array.Empty<byte>()));

        Assert.Throws<NotSupportedException>(() => PlaygroundImage.RemoveExifValues(filePath, null, ExifTag.Software));
    }

    [Fact]
    public void Test_ClearHeifExifValuesWithNonWritableExif_Throws() {
        string filePath = Path.Combine(_directoryWithTests, "exif-clear-idat.heic");
        File.WriteAllBytes(filePath, CreateMinimalHeifWithIdatExif(CreateExifPayload("ImagePlayground")));

        Assert.Throws<NotSupportedException>(() => PlaygroundImage.ClearExifValues(filePath, null));
    }

    [Fact]
    public void Test_ReadHeifInfo() {
        string filePath = Path.Combine(_directoryWithTests, "info.heic");
        File.WriteAllBytes(filePath, CreateMinimalHeifWithPrimaryImageAndExif(320, 240, CreateExifPayload("ImagePlayground")));

        OfficeHeifImageInfo info = PlaygroundImage.GetHeifInfo(filePath);

        Assert.Equal("heic", info.MajorBrand);
        Assert.Contains("mif1", info.CompatibleBrands);
        Assert.Equal((uint)1, info.PrimaryItemId);
        Assert.NotNull(info.PrimaryItem);
        Assert.Equal("hvc1", info.PrimaryItem!.ItemType);
        Assert.Equal((uint)320, info.Width);
        Assert.Equal((uint)240, info.Height);
        Assert.Equal((uint)320, info.PrimaryItem.Width);
        Assert.Equal((uint)240, info.PrimaryItem.Height);
        Assert.Null(info.PrimaryItem.RotationDegrees);
        Assert.False(info.PrimaryItem.IsMirrored);
        Assert.Null(info.PrimaryItem.PixelAspectRatioHorizontalSpacing);
        Assert.Null(info.PrimaryItem.PixelAspectRatioVerticalSpacing);
        Assert.True(info.HasExif);
        Assert.NotNull(info.ExifItem);
        Assert.NotNull(info.ExifItem!.Location);
        Assert.Equal((ushort)0, info.ExifItem.Location!.ConstructionMethod);
        Assert.Equal((ushort)0, info.ExifItem.Location.DataReferenceIndex);
        Assert.True(info.ExifItem.Location.IsFileBacked);
        Assert.True(info.ExifItem.Location.CanWriteSingleFileExtent);
        OfficeHeifItemExtentInfo exifExtent = Assert.Single(info.ExifItem.Location.Extents);
        Assert.True(exifExtent.Offset > 0);
        Assert.True(exifExtent.Length > 0);
        Assert.Contains(info.Items, item => item.HasExif && item.ItemType == "Exif");
    }

    [Fact]
    public void Test_ReadHeifInfoWithTransformProperties() {
        string filePath = Path.Combine(_directoryWithTests, "info-transform.heic");
        File.WriteAllBytes(filePath, CreateMinimalHeifWithPrimaryImageTransformProperties(320, 240, CreateExifPayload("ImagePlayground")));

        OfficeHeifImageInfo info = PlaygroundImage.GetHeifInfo(filePath);

        Assert.NotNull(info.PrimaryItem);
        Assert.Equal((uint)320, info.Width);
        Assert.Equal((uint)240, info.Height);
        Assert.Equal(90, info.RotationDegrees);
        Assert.True(info.IsMirrored);
        Assert.Equal((uint)4, info.PixelAspectRatioHorizontalSpacing);
        Assert.Equal((uint)3, info.PixelAspectRatioVerticalSpacing);
        Assert.Equal(new byte[] { 8, 8, 8 }, info.PixelBitDepths);
        Assert.Equal("nclx", info.ColorType);
        Assert.Equal((ushort)1, info.ColorPrimaries);
        Assert.Equal((ushort)13, info.TransferCharacteristics);
        Assert.Equal((ushort)6, info.MatrixCoefficients);
        Assert.True(info.FullRangeFlag);
        Assert.Equal("hvcC", info.CodecConfigurationType);
        Assert.Equal(new byte[] { 1, 2, 3, 4 }, info.CodecConfigurationBytes);
        Assert.Equal(90, info.PrimaryItem!.RotationDegrees);
        Assert.True(info.PrimaryItem.IsMirrored);
        Assert.Equal((uint)4, info.PrimaryItem.PixelAspectRatioHorizontalSpacing);
        Assert.Equal((uint)3, info.PrimaryItem.PixelAspectRatioVerticalSpacing);
        Assert.Equal(new byte[] { 8, 8, 8 }, info.PrimaryItem.PixelBitDepths);
        Assert.Equal("nclx", info.PrimaryItem.ColorType);
        Assert.Equal("hvcC", info.PrimaryItem.CodecConfigurationType);
        Assert.Equal(new byte[] { 1, 2, 3, 4 }, info.PrimaryItem.CodecConfigurationBytes);
        Assert.Equal(7, info.PrimaryItem.PropertyAssociations.Count);
        Assert.Contains(info.PrimaryItem.PropertyAssociations, association => association.PropertyIndex == 1 && association.PropertyType == "ispe" && association.IsEssential);
        Assert.Contains(info.PrimaryItem.PropertyAssociations, association => association.PropertyIndex == 2 && association.PropertyType == "irot" && !association.IsEssential);
        Assert.Contains(info.PrimaryItem.PropertyAssociations, association => association.PropertyIndex == 6 && association.PropertyType == "colr" && !association.IsEssential);
        Assert.Contains(info.PrimaryItem.PropertyAssociations, association => association.PropertyIndex == 7 && association.PropertyType == "hvcC" && !association.IsEssential);
    }

    [Fact]
    public void Test_ReadHeifInfoWithXmpAndReferences() {
        string filePath = Path.Combine(_directoryWithTests, "info-xmp.heic");
        string xmp = "<?xpacket begin=\"\"?><x:xmpmeta xmlns:x=\"adobe:ns:meta/\"><rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\" /></x:xmpmeta>";
        File.WriteAllBytes(filePath, CreateMinimalHeifWithPrimaryImageExifAndXmp(640, 480, CreateExifPayload("ImagePlayground"), xmp));

        OfficeHeifImageInfo info = PlaygroundImage.GetHeifInfo(filePath);

        Assert.True(info.HasXmp);
        Assert.NotNull(info.XmpItem);
        Assert.Equal("mime", info.XmpItem!.ItemType);
        Assert.Equal("application/rdf+xml", info.XmpItem.MimeType);
        Assert.Equal(string.Empty, info.XmpItem.ContentEncoding);
        Assert.True(info.XmpItem.IsHidden);
        Assert.Equal((ushort)0, info.XmpItem.ItemProtectionIndex);
        Assert.NotNull(info.XmpItem.Location);
        Assert.True(info.XmpItem.Location!.CanWriteSingleFileExtent);
        OfficeHeifItemExtentInfo xmpExtent = Assert.Single(info.XmpItem.Location.Extents);
        Assert.Equal(Encoding.UTF8.GetByteCount(xmp), xmpExtent.Length);
        Assert.Contains(info.References, reference => reference.ReferenceType == "cdsc" && reference.FromItemId == 2 && reference.ToItemIds.Contains(1u));
        Assert.Contains(info.References, reference => reference.ReferenceType == "cdsc" && reference.FromItemId == 3 && reference.ToItemIds.Contains(1u));
        Assert.Equal(xmp, PlaygroundImage.GetHeifXmp(filePath));

        string json = global::ImagePlayground.ImageHelper.ExportMetadata(filePath);
        using JsonDocument document = JsonDocument.Parse(json);
        byte[]? exportedXmp = document.RootElement.GetProperty("XmpProfile").Deserialize<byte[]>();
        Assert.NotNull(exportedXmp);
        Assert.Equal(xmp, Encoding.UTF8.GetString(exportedXmp!));
    }

    [Fact]
    public void Test_ReadHeifInfoWithIdatBackedXmp() {
        string filePath = Path.Combine(_directoryWithTests, "info-idat-xmp.heic");
        string xmp = "<x:xmpmeta xmlns:x=\"adobe:ns:meta/\" />";
        File.WriteAllBytes(filePath, CreateMinimalHeifWithIdatXmp(xmp));

        OfficeHeifImageInfo info = PlaygroundImage.GetHeifInfo(filePath);

        Assert.True(info.HasXmp);
        Assert.NotNull(info.XmpItem);
        Assert.Equal(xmp, PlaygroundImage.GetHeifXmp(filePath));
        Assert.NotNull(info.XmpItem!.Location);
        Assert.Equal((ushort)1, info.XmpItem.Location!.ConstructionMethod);
        Assert.False(info.XmpItem.Location.IsFileBacked);
        Assert.True(info.XmpItem.Location.IsItemDataBoxBacked);
        Assert.False(info.XmpItem.Location.CanWriteSingleFileExtent);
        OfficeHeifItemExtentInfo extent = Assert.Single(info.XmpItem.Location.Extents);
        Assert.Equal(Encoding.UTF8.GetByteCount(xmp), extent.Length);
        Assert.True(extent.Offset > 0);
        Assert.Equal((byte)'<', File.ReadAllBytes(filePath)[extent.Offset]);
    }

    [Fact]
    public void Test_ReadHeifXmpWithUnreadableXmp_Throws() {
        string filePath = Path.Combine(_directoryWithTests, "xmp-read-unreadable.heic");
        File.WriteAllBytes(filePath, CreateMinimalHeifWithUnlocatedXmp());

        Assert.Throws<NotSupportedException>(() => PlaygroundImage.GetHeifXmp(filePath));
    }

    [Fact]
    public void Test_ExportHeifMetadataWithUnreadableXmp_Throws() {
        string filePath = Path.Combine(_directoryWithTests, "metadata-export-unreadable-xmp.heic");
        File.WriteAllBytes(filePath, CreateMinimalHeifWithUnlocatedXmp());

        Assert.Throws<NotSupportedException>(() => global::ImagePlayground.ImageHelper.ExportMetadata(filePath));
    }

    [Fact]
    public void Test_ReadHeifInfoWithAuxiliaryImage() {
        string filePath = Path.Combine(_directoryWithTests, "info-auxiliary.heic");
        File.WriteAllBytes(filePath, CreateMinimalHeifWithAuxiliaryImage());

        OfficeHeifImageInfo info = PlaygroundImage.GetHeifInfo(filePath);

        OfficeHeifItemInfo auxiliaryItem = Assert.Single(info.Items, item => item.AuxiliaryType is not null);
        Assert.Equal((uint)2, auxiliaryItem.ItemId);
        Assert.Equal("urn:mpeg:hevc:2015:auxid:1", auxiliaryItem.AuxiliaryType);
        Assert.Equal(new byte[] { 0x10, 0x20 }, auxiliaryItem.AuxiliarySubtypes);
        Assert.Contains(info.References, reference => reference.ReferenceType == "auxl" && reference.FromItemId == 2 && reference.ToItemIds.Contains(1u));
    }

    [Fact]
    public void Test_ImportHeifMetadataUpdatesExistingExifAndXmp() {
        string filePath = Path.Combine(_directoryWithTests, "metadata-import.heic");
        string metadataPath = Path.Combine(_directoryWithTests, "metadata-import.json");
        string outputPath = Path.Combine(_directoryWithTests, "metadata-import-output.heic");
        string originalXmp = "<x:xmpmeta />";
        string updatedXmp = "<?xpacket begin=\"\"?><x:xmpmeta xmlns:x=\"adobe:ns:meta/\"><rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"><rdf:Description /></rdf:RDF></x:xmpmeta>";
        File.WriteAllBytes(filePath, CreateMinimalHeifWithPrimaryImageExifAndXmp(640, 480, CreateExifPayload("Original"), originalXmp));
        WriteHeifMetadataJson(filePath, metadataPath, CreateExifPayload("Imported"), updatedXmp);

        global::ImagePlayground.ImageHelper.ImportMetadata(filePath, metadataPath, outputPath);

        IReadOnlyList<OfficeExifValue> values = PlaygroundImage.GetExifValues(outputPath);
        Assert.Contains(values, v => v.Tag.Equals(ExifTag.Software) && v.Value?.ToString() == "Imported");
        Assert.Equal(updatedXmp, PlaygroundImage.GetHeifXmp(outputPath));
        byte[] outputBytes = File.ReadAllBytes(outputPath);
        Assert.False(ContainsSequence(outputBytes, Encoding.ASCII.GetBytes("Original")));
        Assert.False(ContainsSequence(outputBytes, Encoding.UTF8.GetBytes(originalXmp)));
    }

    [Fact]
    public void Test_RemoveHeifMetadataClearsExistingExifAndXmp() {
        string filePath = Path.Combine(_directoryWithTests, "metadata-remove.heic");
        string outputPath = Path.Combine(_directoryWithTests, "metadata-remove-output.heic");
        string xmp = "<x:xmpmeta />";
        File.WriteAllBytes(filePath, CreateMinimalHeifWithPrimaryImageExifAndXmp(640, 480, CreateExifPayload("ImagePlayground"), xmp));

        global::ImagePlayground.ImageHelper.RemoveMetadata(filePath, outputPath);

        Assert.Empty(PlaygroundImage.GetExifValues(outputPath));
        Assert.Equal(string.Empty, PlaygroundImage.GetHeifXmp(outputPath));
        Assert.False(ContainsSequence(File.ReadAllBytes(outputPath), Encoding.UTF8.GetBytes(xmp)));
    }

    [Fact]
    public void Test_RemoveHeifMetadataWithNonWritableExif_Throws() {
        string filePath = Path.Combine(_directoryWithTests, "metadata-remove-idat-exif.heic");
        string outputPath = Path.Combine(_directoryWithTests, "metadata-remove-idat-exif-output.heic");
        File.WriteAllBytes(filePath, CreateMinimalHeifWithIdatExif(CreateExifPayload("ImagePlayground")));

        Assert.Throws<NotSupportedException>(() => global::ImagePlayground.ImageHelper.RemoveMetadata(filePath, outputPath));
    }

    [Fact]
    public void Test_RemoveHeifMetadataWithNonWritableXmp_Throws() {
        string filePath = Path.Combine(_directoryWithTests, "metadata-remove-idat-xmp.heic");
        string outputPath = Path.Combine(_directoryWithTests, "metadata-remove-idat-xmp-output.heic");
        File.WriteAllBytes(filePath, CreateMinimalHeifWithIdatXmp("<x:xmpmeta xmlns:x=\"adobe:ns:meta/\" />"));

        Assert.Throws<NotSupportedException>(() => global::ImagePlayground.ImageHelper.RemoveMetadata(filePath, outputPath));
    }

    [Fact]
    public void Test_SetHeifXmp() {
        string filePath = Path.Combine(_directoryWithTests, "xmp-set.heic");
        string originalXmp = "<x:xmpmeta />";
        string updatedXmp = "<?xpacket begin=\"\"?><x:xmpmeta xmlns:x=\"adobe:ns:meta/\"><rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\" /></x:xmpmeta>";
        File.WriteAllBytes(filePath, CreateMinimalHeifWithPrimaryImageExifAndXmp(640, 480, CreateExifPayload("ImagePlayground"), originalXmp));

        PlaygroundImage.SetHeifXmp(filePath, null, updatedXmp);

        Assert.Equal(updatedXmp, PlaygroundImage.GetHeifXmp(filePath));
        Assert.False(ContainsSequence(File.ReadAllBytes(filePath), Encoding.UTF8.GetBytes(originalXmp)));
    }

    [Fact]
    public void Test_RemoveHeifXmp() {
        string filePath = Path.Combine(_directoryWithTests, "xmp-remove.heic");
        string xmp = "<x:xmpmeta />";
        File.WriteAllBytes(filePath, CreateMinimalHeifWithPrimaryImageExifAndXmp(640, 480, CreateExifPayload("ImagePlayground"), xmp));

        PlaygroundImage.RemoveHeifXmp(filePath, null);

        Assert.Equal(string.Empty, PlaygroundImage.GetHeifXmp(filePath));
        Assert.False(ContainsSequence(File.ReadAllBytes(filePath), Encoding.UTF8.GetBytes(xmp)));
    }

    [Fact]
    public void Test_SetHeifXmpWithoutExistingXmp_Throws() {
        string filePath = Path.Combine(_directoryWithTests, "xmp-set-empty.heic");
        File.WriteAllBytes(filePath, CreateMinimalHeifWithPrimaryImageAndExif(320, 240, CreateExifPayload("ImagePlayground")));

        Assert.Throws<NotSupportedException>(() => PlaygroundImage.SetHeifXmp(filePath, null, "<x:xmpmeta />"));
    }

    private static byte[] CreateMinimalHeifWithExif(byte[] tiffPayload) {
        byte[] exifItemData = Combine(
            UInt32BigEndian(6),
            Encoding.ASCII.GetBytes("Exif\0\0"),
            tiffPayload);

        byte[] ftyp = Box("ftyp", Combine(
            Encoding.ASCII.GetBytes("heic"),
            UInt32BigEndian(0),
            Encoding.ASCII.GetBytes("heic"),
            Encoding.ASCII.GetBytes("mif1")));

        byte[] metaWithPlaceholder = CreateMetaBox(0, exifItemData.Length);
        uint exifOffset = (uint)(ftyp.Length + metaWithPlaceholder.Length + 8);
        byte[] meta = CreateMetaBox(exifOffset, exifItemData.Length);
        byte[] mdat = Box("mdat", exifItemData);

        return Combine(ftyp, meta, mdat);
    }

    private static byte[] CreateMinimalHeifWithPrimaryImageExifAndXmp(uint width, uint height, byte[] tiffPayload, string xmp, ushort xmpProtectionIndex = 0, string xmpContentEncoding = "") {
        byte[] exifItemData = Combine(
            UInt32BigEndian(6),
            Encoding.ASCII.GetBytes("Exif\0\0"),
            tiffPayload);
        byte[] xmpItemData = Encoding.UTF8.GetBytes(xmp);

        byte[] ftyp = Box("ftyp", Combine(
            Encoding.ASCII.GetBytes("heic"),
            UInt32BigEndian(0),
            Encoding.ASCII.GetBytes("heic"),
            Encoding.ASCII.GetBytes("mif1")));

        byte[] metaWithPlaceholder = CreateMetaBoxWithPrimaryImageExifAndXmp(0, exifItemData.Length, 0, xmpItemData.Length, width, height, xmpProtectionIndex, xmpContentEncoding);
        uint exifOffset = (uint)(ftyp.Length + metaWithPlaceholder.Length + 8);
        uint xmpOffset = (uint)(exifOffset + exifItemData.Length);
        byte[] meta = CreateMetaBoxWithPrimaryImageExifAndXmp(exifOffset, exifItemData.Length, xmpOffset, xmpItemData.Length, width, height, xmpProtectionIndex, xmpContentEncoding);
        byte[] mdat = Box("mdat", Combine(exifItemData, xmpItemData));

        return Combine(ftyp, meta, mdat);
    }

    private static byte[] CreateMinimalHeifWithPrimaryImageTransformProperties(uint width, uint height, byte[] tiffPayload) {
        byte[] exifItemData = Combine(
            UInt32BigEndian(6),
            Encoding.ASCII.GetBytes("Exif\0\0"),
            tiffPayload);

        byte[] ftyp = Box("ftyp", Combine(
            Encoding.ASCII.GetBytes("heic"),
            UInt32BigEndian(0),
            Encoding.ASCII.GetBytes("heic"),
            Encoding.ASCII.GetBytes("mif1")));

        byte[] metaWithPlaceholder = CreateMetaBoxWithPrimaryImageTransformProperties(0, exifItemData.Length, width, height);
        uint exifOffset = (uint)(ftyp.Length + metaWithPlaceholder.Length + 8);
        byte[] meta = CreateMetaBoxWithPrimaryImageTransformProperties(exifOffset, exifItemData.Length, width, height);
        byte[] mdat = Box("mdat", exifItemData);

        return Combine(ftyp, meta, mdat);
    }

    private static byte[] CreateMinimalHeifWithPrimaryImageAndExif(uint width, uint height, byte[] tiffPayload) {
        byte[] exifItemData = Combine(
            UInt32BigEndian(6),
            Encoding.ASCII.GetBytes("Exif\0\0"),
            tiffPayload);

        byte[] ftyp = Box("ftyp", Combine(
            Encoding.ASCII.GetBytes("heic"),
            UInt32BigEndian(0),
            Encoding.ASCII.GetBytes("heic"),
            Encoding.ASCII.GetBytes("mif1")));

        byte[] metaWithPlaceholder = CreateMetaBoxWithPrimaryImage(0, exifItemData.Length, width, height);
        uint exifOffset = (uint)(ftyp.Length + metaWithPlaceholder.Length + 8);
        byte[] meta = CreateMetaBoxWithPrimaryImage(exifOffset, exifItemData.Length, width, height);
        byte[] mdat = Box("mdat", exifItemData);

        return Combine(ftyp, meta, mdat);
    }

    private static byte[] CreateMinimalHeifWithAuxiliaryImage() {
        byte[] ftyp = Box("ftyp", Combine(
            Encoding.ASCII.GetBytes("heic"),
            UInt32BigEndian(0),
            Encoding.ASCII.GetBytes("heic"),
            Encoding.ASCII.GetBytes("mif1")));
        byte[] meta = CreateMetaBoxWithAuxiliaryImage();

        return Combine(ftyp, meta);
    }

    private static byte[] CreateMinimalHeifWithIdatXmp(string xmp) {
        byte[] xmpItemData = Encoding.UTF8.GetBytes(xmp);
        byte[] ftyp = Box("ftyp", Combine(
            Encoding.ASCII.GetBytes("heic"),
            UInt32BigEndian(0),
            Encoding.ASCII.GetBytes("heic"),
            Encoding.ASCII.GetBytes("mif1")));
        byte[] meta = CreateMetaBoxWithIdatXmp(xmpItemData);

        return Combine(ftyp, meta);
    }

    private static byte[] CreateMinimalHeifWithUnlocatedXmp() {
        byte[] ftyp = Box("ftyp", Combine(
            Encoding.ASCII.GetBytes("heic"),
            UInt32BigEndian(0),
            Encoding.ASCII.GetBytes("heic"),
            Encoding.ASCII.GetBytes("mif1")));
        byte[] meta = CreateMetaBoxWithUnlocatedXmp();

        return Combine(ftyp, meta);
    }

    private static byte[] CreateMinimalHeifWithIdatExif(byte[] tiffPayload) {
        byte[] exifItemData = Combine(
            UInt32BigEndian(6),
            Encoding.ASCII.GetBytes("Exif\0\0"),
            tiffPayload);
        byte[] ftyp = Box("ftyp", Combine(
            Encoding.ASCII.GetBytes("heic"),
            UInt32BigEndian(0),
            Encoding.ASCII.GetBytes("heic"),
            Encoding.ASCII.GetBytes("mif1")));
        byte[] meta = CreateMetaBoxWithIdatExif(exifItemData);

        return Combine(ftyp, meta);
    }

    private static byte[] CreateExifPayload(string software) {
        var profile = new OfficeImageMetadata();
        profile.SetExifValue(ExifTag.Software, software);
        return Assert.IsType<byte[]>(profile.EncodeExifProfile());
    }

    private static void WriteHeifMetadataJson(string sourceFilePath, string metadataPath, byte[]? exifProfile, string? xmp) {
        string exported = global::ImagePlayground.ImageHelper.ExportMetadata(sourceFilePath);
        using JsonDocument document = JsonDocument.Parse(exported);
        JsonElement root = document.RootElement;
        var metadata = new Dictionary<string, object?> {
            ["HorizontalResolution"] = root.GetProperty("HorizontalResolution").GetDouble(),
            ["VerticalResolution"] = root.GetProperty("VerticalResolution").GetDouble(),
            ["ResolutionUnits"] = root.GetProperty("ResolutionUnits").GetString(),
            ["ExifProfile"] = exifProfile,
            ["XmpProfile"] = xmp != null ? Encoding.UTF8.GetBytes(xmp) : null,
            ["IccProfile"] = null,
            ["IptcProfile"] = null
        };

        File.WriteAllText(metadataPath, JsonSerializer.Serialize(metadata));
    }

    private static byte[] CreateMinimalHeifWithoutExif() {
        byte[] ftyp = Box("ftyp", Combine(
            Encoding.ASCII.GetBytes("heic"),
            UInt32BigEndian(0),
            Encoding.ASCII.GetBytes("heic"),
            Encoding.ASCII.GetBytes("mif1")));

        byte[] iinf = FullBox("iinf", 0, UInt16BigEndian(0));
        byte[] meta = FullBox("meta", 0, iinf);
        return Combine(ftyp, meta);
    }

    private static byte[] CreateMetaBox(uint exifOffset, int exifLength) {
        byte[] infe = FullBox("infe", 2, Combine(
            UInt16BigEndian(1),
            UInt16BigEndian(0),
            Encoding.ASCII.GetBytes("Exif"),
            new byte[] { 0 }));

        byte[] iinf = FullBox("iinf", 0, Combine(
            UInt16BigEndian(1),
            infe));

        byte[] iloc = FullBox("iloc", 1, Combine(
            new byte[] { 0x44, 0x00 },
            UInt16BigEndian(1),
            UInt16BigEndian(1),
            UInt16BigEndian(0),
            UInt16BigEndian(0),
            UInt16BigEndian(1),
            UInt32BigEndian(exifOffset),
            UInt32BigEndian((uint)exifLength)));

        return FullBox("meta", 0, Combine(iinf, iloc));
    }

    private static byte[] CreateMetaBoxWithPrimaryImage(uint exifOffset, int exifLength, uint width, uint height) {
        byte[] imageInfe = FullBox("infe", 2, Combine(
            UInt16BigEndian(1),
            UInt16BigEndian(0),
            Encoding.ASCII.GetBytes("hvc1"),
            new byte[] { 0 }));
        byte[] exifInfe = FullBox("infe", 2, Combine(
            UInt16BigEndian(2),
            UInt16BigEndian(0),
            Encoding.ASCII.GetBytes("Exif"),
            new byte[] { 0 }));

        byte[] iinf = FullBox("iinf", 0, Combine(
            UInt16BigEndian(2),
            imageInfe,
            exifInfe));

        byte[] pitm = FullBox("pitm", 0, UInt16BigEndian(1));
        byte[] iloc = FullBox("iloc", 1, Combine(
            new byte[] { 0x44, 0x00 },
            UInt16BigEndian(1),
            UInt16BigEndian(2),
            UInt16BigEndian(0),
            UInt16BigEndian(0),
            UInt16BigEndian(1),
            UInt32BigEndian(exifOffset),
            UInt32BigEndian((uint)exifLength)));
        byte[] ispe = FullBox("ispe", 0, Combine(UInt32BigEndian(width), UInt32BigEndian(height)));
        byte[] ipco = Box("ipco", ispe);
        byte[] ipma = FullBox("ipma", 0, Combine(
            UInt32BigEndian(1),
            UInt16BigEndian(1),
            new byte[] { 1, 1 }));
        byte[] iprp = Box("iprp", Combine(ipco, ipma));

        return FullBox("meta", 0, Combine(pitm, iinf, iloc, iprp));
    }

    private static byte[] CreateMetaBoxWithPrimaryImageTransformProperties(uint exifOffset, int exifLength, uint width, uint height) {
        byte[] imageInfe = FullBox("infe", 2, Combine(
            UInt16BigEndian(1),
            UInt16BigEndian(0),
            Encoding.ASCII.GetBytes("hvc1"),
            new byte[] { 0 }));
        byte[] exifInfe = FullBox("infe", 2, Combine(
            UInt16BigEndian(2),
            UInt16BigEndian(0),
            Encoding.ASCII.GetBytes("Exif"),
            new byte[] { 0 }));

        byte[] iinf = FullBox("iinf", 0, Combine(
            UInt16BigEndian(2),
            imageInfe,
            exifInfe));

        byte[] pitm = FullBox("pitm", 0, UInt16BigEndian(1));
        byte[] iloc = FullBox("iloc", 1, Combine(
            new byte[] { 0x44, 0x00 },
            UInt16BigEndian(1),
            UInt16BigEndian(2),
            UInt16BigEndian(0),
            UInt16BigEndian(0),
            UInt16BigEndian(1),
            UInt32BigEndian(exifOffset),
            UInt32BigEndian((uint)exifLength)));
        byte[] ispe = FullBox("ispe", 0, Combine(UInt32BigEndian(width), UInt32BigEndian(height)));
        byte[] irot = Box("irot", new byte[] { 1 });
        byte[] imir = Box("imir", new byte[] { 0 });
        byte[] pasp = Box("pasp", Combine(UInt32BigEndian(4), UInt32BigEndian(3)));
        byte[] pixi = FullBox("pixi", 0, new byte[] { 3, 8, 8, 8 });
        byte[] colr = Box("colr", Combine(
            Encoding.ASCII.GetBytes("nclx"),
            UInt16BigEndian(1),
            UInt16BigEndian(13),
            UInt16BigEndian(6),
            new byte[] { 0x80 }));
        byte[] hvcC = Box("hvcC", new byte[] { 1, 2, 3, 4 });
        byte[] ipco = Box("ipco", Combine(ispe, irot, imir, pasp, pixi, colr, hvcC));
        byte[] ipma = FullBox("ipma", 0, Combine(
            UInt32BigEndian(1),
            UInt16BigEndian(1),
            new byte[] { 7, 0x81, 2, 3, 4, 5, 6, 7 }));
        byte[] iprp = Box("iprp", Combine(ipco, ipma));

        return FullBox("meta", 0, Combine(pitm, iinf, iloc, iprp));
    }

    private static byte[] CreateMetaBoxWithAuxiliaryImage() {
        byte[] imageInfe = FullBox("infe", 2, Combine(
            UInt16BigEndian(1),
            UInt16BigEndian(0),
            Encoding.ASCII.GetBytes("hvc1"),
            new byte[] { 0 }));
        byte[] auxiliaryInfe = FullBox("infe", 2, Combine(
            UInt16BigEndian(2),
            UInt16BigEndian(0),
            Encoding.ASCII.GetBytes("hvc1"),
            new byte[] { 0 }));

        byte[] iinf = FullBox("iinf", 0, Combine(
            UInt16BigEndian(2),
            imageInfe,
            auxiliaryInfe));
        byte[] pitm = FullBox("pitm", 0, UInt16BigEndian(1));
        byte[] auxReference = Box("auxl", Combine(
            UInt16BigEndian(2),
            UInt16BigEndian(1),
            UInt16BigEndian(1)));
        byte[] iref = FullBox("iref", 0, auxReference);
        byte[] ispe = FullBox("ispe", 0, Combine(UInt32BigEndian(320), UInt32BigEndian(240)));
        byte[] auxC = FullBox("auxC", 0, Combine(
            Encoding.ASCII.GetBytes("urn:mpeg:hevc:2015:auxid:1"),
            new byte[] { 0, 0x10, 0x20 }));
        byte[] ipco = Box("ipco", Combine(ispe, auxC));
        byte[] ipma = FullBox("ipma", 0, Combine(
            UInt32BigEndian(2),
            UInt16BigEndian(1),
            new byte[] { 1, 1 },
            UInt16BigEndian(2),
            new byte[] { 1, 2 }));
        byte[] iprp = Box("iprp", Combine(ipco, ipma));

        return FullBox("meta", 0, Combine(pitm, iinf, iref, iprp));
    }

    private static byte[] CreateMetaBoxWithIdatXmp(byte[] xmpItemData) {
        byte[] xmpInfe = FullBoxWithFlags("infe", 2, 1, Combine(
            UInt16BigEndian(1),
            UInt16BigEndian(0),
            Encoding.ASCII.GetBytes("mime"),
            new byte[] { 0 },
            Encoding.ASCII.GetBytes("application/rdf+xml"),
            new byte[] { 0 },
            Array.Empty<byte>(),
            new byte[] { 0 }));
        byte[] iinf = FullBox("iinf", 0, Combine(
            UInt16BigEndian(1),
            xmpInfe));
        byte[] iloc = FullBox("iloc", 1, Combine(
            new byte[] { 0x44, 0x00 },
            UInt16BigEndian(1),
            UInt16BigEndian(1),
            UInt16BigEndian(1),
            UInt16BigEndian(0),
            UInt16BigEndian(1),
            UInt32BigEndian(0),
            UInt32BigEndian((uint)xmpItemData.Length)));
        byte[] idat = Box("idat", xmpItemData);

        return FullBox("meta", 0, Combine(iinf, iloc, idat));
    }

    private static byte[] CreateMetaBoxWithUnlocatedXmp() {
        byte[] xmpInfe = FullBoxWithFlags("infe", 2, 1, Combine(
            UInt16BigEndian(1),
            UInt16BigEndian(0),
            Encoding.ASCII.GetBytes("mime"),
            new byte[] { 0 },
            Encoding.ASCII.GetBytes("application/rdf+xml"),
            new byte[] { 0 },
            Array.Empty<byte>(),
            new byte[] { 0 }));
        byte[] iinf = FullBox("iinf", 0, Combine(
            UInt16BigEndian(1),
            xmpInfe));

        return FullBox("meta", 0, iinf);
    }

    private static byte[] CreateMetaBoxWithIdatExif(byte[] exifItemData) {
        byte[] exifInfe = FullBox("infe", 2, Combine(
            UInt16BigEndian(1),
            UInt16BigEndian(0),
            Encoding.ASCII.GetBytes("Exif"),
            new byte[] { 0 }));
        byte[] iinf = FullBox("iinf", 0, Combine(
            UInt16BigEndian(1),
            exifInfe));
        byte[] iloc = FullBox("iloc", 1, Combine(
            new byte[] { 0x44, 0x00 },
            UInt16BigEndian(1),
            UInt16BigEndian(1),
            UInt16BigEndian(1),
            UInt16BigEndian(0),
            UInt16BigEndian(1),
            UInt32BigEndian(0),
            UInt32BigEndian((uint)exifItemData.Length)));
        byte[] idat = Box("idat", exifItemData);

        return FullBox("meta", 0, Combine(iinf, iloc, idat));
    }

    private static byte[] CreateMetaBoxWithPrimaryImageExifAndXmp(uint exifOffset, int exifLength, uint xmpOffset, int xmpLength, uint width, uint height, ushort xmpProtectionIndex = 0, string xmpContentEncoding = "") {
        byte[] imageInfe = FullBox("infe", 2, Combine(
            UInt16BigEndian(1),
            UInt16BigEndian(0),
            Encoding.ASCII.GetBytes("hvc1"),
            new byte[] { 0 }));
        byte[] exifInfe = FullBox("infe", 2, Combine(
            UInt16BigEndian(2),
            UInt16BigEndian(0),
            Encoding.ASCII.GetBytes("Exif"),
            new byte[] { 0 }));
        byte[] xmpInfe = FullBoxWithFlags("infe", 2, 1, Combine(
            UInt16BigEndian(3),
            UInt16BigEndian(xmpProtectionIndex),
            Encoding.ASCII.GetBytes("mime"),
            new byte[] { 0 },
            Encoding.ASCII.GetBytes("application/rdf+xml"),
            new byte[] { 0 },
            Encoding.ASCII.GetBytes(xmpContentEncoding),
            new byte[] { 0 }));

        byte[] iinf = FullBox("iinf", 0, Combine(
            UInt16BigEndian(3),
            imageInfe,
            exifInfe,
            xmpInfe));

        byte[] pitm = FullBox("pitm", 0, UInt16BigEndian(1));
        byte[] exifLocation = Combine(
            UInt16BigEndian(2),
            UInt16BigEndian(0),
            UInt16BigEndian(0),
            UInt16BigEndian(1),
            UInt32BigEndian(exifOffset),
            UInt32BigEndian((uint)exifLength));
        byte[] xmpLocation = Combine(
            UInt16BigEndian(3),
            UInt16BigEndian(0),
            UInt16BigEndian(0),
            UInt16BigEndian(1),
            UInt32BigEndian(xmpOffset),
            UInt32BigEndian((uint)xmpLength));
        byte[] iloc = FullBox("iloc", 1, Combine(
            new byte[] { 0x44, 0x00 },
            UInt16BigEndian(2),
            exifLocation,
            xmpLocation));
        byte[] exifReference = Box("cdsc", Combine(
            UInt16BigEndian(2),
            UInt16BigEndian(1),
            UInt16BigEndian(1)));
        byte[] xmpReference = Box("cdsc", Combine(
            UInt16BigEndian(3),
            UInt16BigEndian(1),
            UInt16BigEndian(1)));
        byte[] iref = FullBox("iref", 0, Combine(exifReference, xmpReference));
        byte[] ispe = FullBox("ispe", 0, Combine(UInt32BigEndian(width), UInt32BigEndian(height)));
        byte[] ipco = Box("ipco", ispe);
        byte[] ipma = FullBox("ipma", 0, Combine(
            UInt32BigEndian(1),
            UInt16BigEndian(1),
            new byte[] { 1, 1 }));
        byte[] iprp = Box("iprp", Combine(ipco, ipma));

        return FullBox("meta", 0, Combine(pitm, iinf, iref, iloc, iprp));
    }

    private static byte[] FullBox(string type, byte version, byte[] payload) =>
        Box(type, Combine(new byte[] { version, 0, 0, 0 }, payload));

    private static byte[] FullBoxWithFlags(string type, byte version, uint flags, byte[] payload) =>
        Box(type, Combine(new[] { version, (byte)(flags >> 16), (byte)(flags >> 8), (byte)flags }, payload));

    private static byte[] Box(string type, byte[] payload) =>
        Combine(UInt32BigEndian((uint)(8 + payload.Length)), Encoding.ASCII.GetBytes(type), payload);

    private static byte[] UInt16BigEndian(ushort value) =>
        new[] {
            (byte)(value >> 8),
            (byte)value
        };

    private static byte[] UInt32BigEndian(uint value) =>
        new[] {
            (byte)(value >> 24),
            (byte)(value >> 16),
            (byte)(value >> 8),
            (byte)value
        };

    private static bool ContainsSequence(byte[] data, byte[] sequence) {
        if (sequence.Length == 0) {
            return true;
        }

        for (int dataIndex = 0; dataIndex <= data.Length - sequence.Length; dataIndex++) {
            bool matched = true;
            for (int sequenceIndex = 0; sequenceIndex < sequence.Length; sequenceIndex++) {
                if (data[dataIndex + sequenceIndex] != sequence[sequenceIndex]) {
                    matched = false;
                    break;
                }
            }

            if (matched) {
                return true;
            }
        }

        return false;
    }

    private static byte[] Combine(params byte[][] arrays) {
        var result = new byte[arrays.Sum(static a => a.Length)];
        int offset = 0;
        foreach (byte[] array in arrays) {
            Buffer.BlockCopy(array, 0, result, offset, array.Length);
            offset += array.Length;
        }

        return result;
    }
}
